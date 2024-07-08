Чтобы получить значения всех настроек из приведенного примера, мы можем добавить методы и свойства, которые позволяют извлекать эти значения. Мы модифицируем классы, чтобы можно было получить доступ к настроенным значениям.

### Измененный код

```csharp
using System;

namespace LambdaExample
{
    // Класс Server для демонстрации конфигурации с использованием лямбда-выражений
    class Server
    {
        public ServerConfig Config { get; private set; }

        public void Configure(Action<ServerConfig> configure)
        {
            var config = new ServerConfig();
            configure(config);
            Config = config; // Сохраняем конфигурацию для последующего доступа
        }
    }

    // Класс ServerConfig, который будет настраиваться с помощью лямбда-выражений
    class ServerConfig
    {
        public HostConfig HostConfig { get; private set; }
        public EndpointConfig EndpointConfig { get; private set; }

        public void Host(Uri uri, Action<HostConfig> configureHost)
        {
            var hostConfig = new HostConfig(uri);
            configureHost(hostConfig);
            HostConfig = hostConfig; // Сохраняем конфигурацию хоста для последующего доступа
        }

        public void ReceiveEndpoint(string name, Action<EndpointConfig> configureEndpoint)
        {
            var endpointConfig = new EndpointConfig(name);
            configureEndpoint(endpointConfig);
            EndpointConfig = endpointConfig; // Сохраняем конфигурацию конечной точки для последующего доступа
        }
    }

    // Класс HostConfig для конфигурации хоста
    class HostConfig
    {
        public Uri Uri { get; }

        public string Username { get; set; }
        public string Password { get; set; }

        public HostConfig(Uri uri)
        {
            Uri = uri;
        }
    }

    // Класс EndpointConfig для конфигурации конечной точки
    class EndpointConfig
    {
        public string Name { get; }

        public string BoundQueue { get; private set; }
        public bool ConfigureConsumeTopology { get; set; }
        public Type ConsumerType { get; private set; }

        public EndpointConfig(string name)
        {
            Name = name;
        }

        public void Bind(string queueName)
        {
            BoundQueue = queueName; // Сохраняем имя очереди
            Console.WriteLine($"Binding to queue: {queueName}");
        }

        public void Consumer<TConsumer>()
        {
            ConsumerType = typeof(TConsumer); // Сохраняем тип потребителя
            Console.WriteLine($"Configuring consumer: {typeof(TConsumer).Name}");
        }
    }

    // Пример потребителя
    class MessageConsumer
    {
        // Здесь могла бы быть логика потребления сообщений
    }

    class Program
    {
        static void Main(string[] args)
        {
            var server = new Server();

            server.Configure(configurator =>
            {
                configurator.Host(new Uri("http://localhost"), h =>
                {
                    h.Username = "guest";
                    h.Password = "guest";
                });

                configurator.ReceiveEndpoint("my_queue", ep =>
                {
                    ep.Bind("my_queue");
                    ep.ConfigureConsumeTopology = false;
                    ep.Consumer<MessageConsumer>();
                });
            });

            // Получение и отображение значений настроек
            var hostConfig = server.Config.HostConfig;
            var endpointConfig = server.Config.EndpointConfig;

            Console.WriteLine($"Host Uri: {hostConfig.Uri}");
            Console.WriteLine($"Host Username: {hostConfig.Username}");
            Console.WriteLine($"Host Password: {hostConfig.Password}");

            Console.WriteLine($"Endpoint Name: {endpointConfig.Name}");
            Console.WriteLine($"Bound Queue: {endpointConfig.BoundQueue}");
            Console.WriteLine($"Configure Consume Topology: {endpointConfig.ConfigureConsumeTopology}");
            Console.WriteLine($"Consumer Type: {endpointConfig.ConsumerType.Name}");

            Console.WriteLine("Configuration complete. Press any key to exit...");
            Console.ReadKey();
        }
    }
}
```

### Объяснение изменений

1. **Server**:
   - Добавлено свойство `Config`, которое сохраняет конфигурацию после вызова метода `Configure`.

2. **ServerConfig**:
   - Добавлены свойства `HostConfig` и `EndpointConfig`, которые сохраняют конфигурации хоста и конечной точки соответственно.

3. **HostConfig**:
   - Без изменений, поскольку свойства `Username`, `Password` и `Uri` уже доступны.

4. **EndpointConfig**:
   - Добавлены свойства `BoundQueue`, `ConfigureConsumeTopology` и `ConsumerType` для хранения настроек конечной точки.

5. **Program**:
   - После конфигурирования серверного объекта извлекаются и отображаются значения всех настроек.

Эти изменения позволяют сохранять и извлекать значения настроек, что делает код более гибким и удобным для использования в реальных приложениях.