Привет!

**Паттерн Transaction Outbox** решает проблему согласованности данных между базой данных и брокером сообщений, например, RabbitMQ. Основная цель этого паттерна — гарантировать, что изменения в базе данных и отправка сообщения в брокер происходят атомарно (т.е. либо оба действия выполняются, либо ни одно).

## Описание:
- **Transaction Outbox** заключается в использовании таблицы "Outbox" в базе данных для хранения сообщений, которые затем будут отправлены в брокер сообщений. 
- Сначала транзакция в базе данных записывает изменения и добавляет сообщение в таблицу Outbox. После завершения транзакции другой процесс (или фоновый сервис) берёт сообщения из Outbox и отправляет их в RabbitMQ.
- Это позволяет избежать несогласованности, когда изменения в базе данных выполнены, но сообщение не отправлено.

## Основные шаги:
1. **Транзакция сохраняет данные в основной таблице и таблице Outbox.**
2. **Фоновый процесс проверяет таблицу Outbox и отправляет сообщения в RabbitMQ.**
3. **После успешной отправки сообщения удаляются из таблицы Outbox.**

### Пример реализации паттерна Transaction Outbox на C#

### 1. Настройка таблицы Outbox в базе данных:

```sql
CREATE TABLE OutboxMessages (
    Id UNIQUEIDENTIFIER PRIMARY KEY,  -- Уникальный идентификатор сообщения
    OccurredOn DATETIME2,             -- Время создания сообщения
    Processed BIT,                    -- Флаг обработки сообщения
    Payload NVARCHAR(MAX)             -- Тело сообщения (например, в JSON)
);
```

### 2. Сервис, который записывает в Outbox и базу данных в рамках одной транзакции

```csharp
using System;
using System.Data.SqlClient;
using Dapper; // Установите пакет Dapper через NuGet
using Newtonsoft.Json; // Установите пакет Newtonsoft.Json через NuGet

public class OrderService
{
    private readonly string _connectionString;

    public OrderService(string connectionString)
    {
        _connectionString = connectionString;
    }

    public void CreateOrderAndSendEvent(Order order)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    // 1. Сохраняем заказ в основной таблице
                    string insertOrderSql = "INSERT INTO Orders (Id, CustomerId, Total) VALUES (@Id, @CustomerId, @Total)";
                    connection.Execute(insertOrderSql, new { order.Id, order.CustomerId, order.Total }, transaction);

                    // 2. Создаем сообщение для Outbox
                    var message = new OutboxMessage
                    {
                        Id = Guid.NewGuid(),
                        OccurredOn = DateTime.UtcNow,
                        Processed = false,
                        Payload = JsonConvert.SerializeObject(new OrderCreatedEvent { OrderId = order.Id })
                    };

                    // 3. Записываем сообщение в таблицу Outbox
                    string insertOutboxSql = "INSERT INTO OutboxMessages (Id, OccurredOn, Processed, Payload) VALUES (@Id, @OccurredOn, @Processed, @Payload)";
                    connection.Execute(insertOutboxSql, message, transaction);

                    // 4. Подтверждаем транзакцию
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}

public class Order
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public decimal Total { get; set; }
}

public class OrderCreatedEvent
{
    public Guid OrderId { get; set; }
}

public class OutboxMessage
{
    public Guid Id { get; set; }
    public DateTime OccurredOn { get; set; }
    public bool Processed { get; set; }
    public string Payload { get; set; }
}
```

### 3. Фоновый сервис, который обрабатывает сообщения из Outbox и отправляет их в RabbitMQ

```csharp
using System;
using System.Data.SqlClient;
using Dapper;
using Newtonsoft.Json;
using RabbitMQ.Client; // Установите пакет RabbitMQ.Client через NuGet

public class OutboxProcessor
{
    private readonly string _connectionString;
    private readonly IConnection _rabbitConnection;

    public OutboxProcessor(string connectionString, IConnection rabbitConnection)
    {
        _connectionString = connectionString;
        _rabbitConnection = rabbitConnection;
    }

    public void ProcessOutboxMessages()
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    // 1. Выбираем сообщения из Outbox, которые еще не были обработаны
                    var messages = connection.Query<OutboxMessage>("SELECT * FROM OutboxMessages WHERE Processed = 0", transaction);

                    foreach (var message in messages)
                    {
                        // 2. Отправляем сообщение в RabbitMQ
                        SendToRabbitMQ(message.Payload);

                        // 3. Обновляем статус Processed на true
                        connection.Execute("UPDATE OutboxMessages SET Processed = 1 WHERE Id = @Id", new { message.Id }, transaction);
                    }

                    // 4. Подтверждаем транзакцию
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }

    private void SendToRabbitMQ(string messagePayload)
    {
        using (var channel = _rabbitConnection.CreateModel())
        {
            // Объявляем очередь (если она еще не создана)
            channel.QueueDeclare(queue: "order_created_queue",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            // Преобразуем сообщение в байты
            var body = System.Text.Encoding.UTF8.GetBytes(messagePayload);

            // Публикуем сообщение в RabbitMQ
            channel.BasicPublish(exchange: "",
                                 routingKey: "order_created_queue",
                                 basicProperties: null,
                                 body: body);

            Console.WriteLine($" [x] Sent {messagePayload}");
        }
    }
}
```

### 4. Запуск Outbox Processor в фоновом режиме

```csharp
using RabbitMQ.Client;
using System;
using System.Threading;

class Program
{
    static void Main(string[] args)
    {
        var connectionString = "Your SQL Server Connection String";

        // Создание подключения к RabbitMQ
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using (var rabbitConnection = factory.CreateConnection())
        {
            var outboxProcessor = new OutboxProcessor(connectionString, rabbitConnection);

            // Обрабатываем сообщения в цикле (имитируем фоновый сервис)
            while (true)
            {
                outboxProcessor.ProcessOutboxMessages();
                Thread.Sleep(5000); // Ждём 5 секунд перед следующей итерацией
            }
        }
    }
}
```

### Объяснение:
1. **OrderService** — это сервис, который создаёт заказы и записывает сообщение в таблицу Outbox в одной транзакции. Это гарантирует, что если база данных обновлена, сообщение для RabbitMQ будет сохранено в таблице Outbox.
   
2. **OutboxProcessor** — это сервис, который периодически проверяет таблицу Outbox, извлекает необработанные сообщения и отправляет их в RabbitMQ. После успешной отправки сообщения статус Processed устанавливается в `true`.

3. **Таблица Outbox** — используется для хранения сообщений, которые нужно отправить в RabbitMQ. Мы не отправляем сообщения напрямую из транзакции, чтобы избежать несогласованности.

### Как это работает:
- Когда создаётся новый заказ, сообщение записывается в таблицу Outbox.
- Фоновый процесс `OutboxProcessor` берёт сообщения из таблицы Outbox и отправляет их в очередь RabbitMQ.
- Если сообщение успешно отправлено в RabbitMQ, оно помечается как обработанное (Processed = 1).

Этот подход гарантирует, что изменения в базе данных и отправка сообщений в очередь будут синхронизированы и атомарны, даже если отправка в RabbitMQ может быть отложена.