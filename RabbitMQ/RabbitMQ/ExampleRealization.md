Конечно! Вот пример реализации Publisher и Consumer через RabbitMQ на языке C# с использованием фреймворка RabbitMQ:

```csharp
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

public class RabbitMqPublisher
{
    private readonly string _exchangeName;
    private readonly string _routingKey;

    public RabbitMqPublisher(string exchangeName, string routingKey)
    {
        _exchangeName = exchangeName;
        _routingKey = routingKey;
    }

    public void PublishMessage(string message)
    {
        var factory = new ConnectionFactory()
        {
            HostName = "localhost", // Замените на ваше имя хоста RabbitMQ
            UserName = "guest", // Замените на ваше имя пользователя
            Password = "guest" // Замените на ваш пароль
        };

        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.ExchangeDeclare(_exchangeName, ExchangeType.Direct);

            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: _exchangeName, routingKey: _routingKey, basicProperties: null, body: body);
            Console.WriteLine($"Message sent: {message}");
        }
    }
}

public class RabbitMqConsumer
{
    private readonly string _exchangeName;
    private readonly string _queueName;
    private readonly string _routingKey;

    public RabbitMqConsumer(string exchangeName, string queueName, string routingKey)
    {
        _exchangeName = exchangeName;
        _queueName = queueName;
        _routingKey = routingKey;
    }

    public void StartConsuming()
    {
        var factory = new ConnectionFactory()
        {
            HostName = "localhost", // Замените на ваше имя хоста RabbitMQ
            UserName = "guest", // Замените на ваше имя пользователя
            Password = "guest" // Замените на ваш пароль
        };

        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.ExchangeDeclare(_exchangeName, ExchangeType.Direct);
            channel.QueueDeclare(_queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            channel.QueueBind(_queueName, _exchangeName, _routingKey, null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"Received message: {message}");

                // Подтверждение обработки сообщения
                channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            };

            channel.BasicConsume(queue: _queueName, autoAck: false, consumer: consumer);

            Console.WriteLine("Consumer started. Press any key to exit.");
            Console.ReadKey();
        }
    }
}
```

Для использования классов Publisher и Consumer, вы можете создать новый класс и добавить следующий код:

```csharp
public class Program
{
    static void Main(string[] args)
    {
        // Создание и запуск Consumer
        RabbitMqConsumer consumer = new RabbitMqConsumer("my_exchange", "my_queue", "my_routing_key");
        consumer.StartConsuming();

        // Создание и отправка сообщений с помощью Publisher
        RabbitMqPublisher publisher = new RabbitMqPublisher("my_exchange", "my_routing_key");
        publisher.PublishMessage("Hello, RabbitMQ!");
    }
}
```

Обратите внимание, что в приведенном примере используются параметры подключения к локальному экземпляру RabbitMQ (имя хоста, имя пользователя и пароль). Замените их соответствующими значениями для вашей конфигурации RabbitMQ.

Также замените значения "my_exchange", "my_queue" и "my_routing_key" на соответствующие значения для ваших целей.

Надеюсь, что этот пример окажется полезным! 😊
