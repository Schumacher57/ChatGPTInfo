Конечно, давай создадим простое консольное приложение на C#, которое будет использовать лямбда-выражения аналогично примеру с RabbitMQ. Для демонстрации мы создадим класс `Server` с методом `Configure`, который принимает лямбда-выражения для настройки, и класс `Endpoint` для настройки конечной точки.

### Пример кода с лямбда-выражениями

1. **Создаем проект консольного приложения:**
   - Открой Visual Studio.
   - Создай новый проект типа "Console App (.NET Framework)".
   - Назови проект, например, `LambdaExample`.

2. **Создаем необходимые классы:**

```csharp
using System;

namespace LambdaExample
{
    // Класс Server для демонстрации конфигурации с использованием лямбда-выражений
    class Server
    {
        public void Configure(Action<ServerConfig> configure)
        {
            var config = new ServerConfig();
            configure(config);
        }
    }

    // Класс ServerConfig, который будет настраиваться с помощью лямбда-выражений
    class ServerConfig
    {
        public void Host(Uri uri, Action<HostConfig> configureHost)
        {
            var hostConfig = new HostConfig(uri);
            configureHost(hostConfig);
        }

        public void ReceiveEndpoint(string name, Action<EndpointConfig> configureEndpoint)
        {
            var endpointConfig = new EndpointConfig(name);
            configureEndpoint(endpointConfig);
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

        public EndpointConfig(string name)
        {
            Name = name;
        }

        public void Bind(string queueName)
        {
            Console.WriteLine($"Binding to queue: {queueName}");
        }

        public bool ConfigureConsumeTopology { get; set; }
        public void Consumer<TConsumer>()
        {
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

            Console.WriteLine("Configuration complete. Press any key to exit...");
            Console.ReadKey();
        }
    }
}
```

### Объяснение кода:

1. **Server**:
   - Метод `Configure` принимает лямбда-выражение `Action<ServerConfig>`, которое позволяет настраивать объект `ServerConfig`.

2. **ServerConfig**:
   - Метод `Host` принимает `Uri` и лямбда-выражение `Action<HostConfig>` для настройки конфигурации хоста.
   - Метод `ReceiveEndpoint` принимает имя конечной точки и лямбда-выражение `Action<EndpointConfig>` для настройки конфигурации конечной точки.

3. **HostConfig**:
   - Содержит свойства для настройки имени пользователя и пароля.

4. **EndpointConfig**:
   - Содержит метод `Bind` для привязки к очереди.
   - Свойство `ConfigureConsumeTopology` и метод `Consumer<TConsumer>` для настройки потребителя.

5. **MessageConsumer**:
   - Пример класса потребителя (здесь могла бы быть логика обработки сообщений).

6. **Program**:
   - Создаем экземпляр `Server` и вызываем метод `Configure`, передавая ему лямбда-выражение для настройки хоста и конечной точки.

Этот пример демонстрирует, как использовать лямбда-выражения для настройки различных аспектов системы в стиле конфигурации, подобной той, что используется в RabbitMQ.