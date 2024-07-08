Понимание того, как настройки, заданные через лямбда-выражения, читаются и применяются, требует понимания внутренних механизмов конфигурации и запуска в MassTransit и подобных библиотеках. Давайте разберем, как это может работать под капотом на более простом примере, который поможет вам понять общий механизм.

### Пример с внутренним хранением конфигураций

Мы создадим простой аналог конфигурации, где будем сохранять настройки, заданные через лямбда-выражения, и затем читать их при запуске.

#### Шаг 1: Создание классов для конфигурации и запуска

1. **Создаем класс `BusControl` для управления настройками и запуском.**

```csharp
using System;
using System.Threading.Tasks;

namespace LambdaExample
{
    // Класс для управления настройками и запуском
    class BusControl
    {
        private readonly Action<Configurator> _configuratorAction;

        // Конструктор принимает лямбда-выражение для конфигурации
        public BusControl(Action<Configurator> configuratorAction)
        {
            _configuratorAction = configuratorAction;
        }

        // Метод для запуска
        public async Task StartAsync()
        {
            var configurator = new Configurator();
            _configuratorAction(configurator);

            // Здесь можно получить доступ к настроенным значениям
            Console.WriteLine($"Host: {configurator.HostConfig.Uri}");
            Console.WriteLine($"Username: {configurator.HostConfig.Username}");
            Console.WriteLine($"Password: {configurator.HostConfig.Password}");
            Console.WriteLine($"Endpoint: {configurator.EndpointConfig.Name}");
            Console.WriteLine($"Bound Queue: {configurator.EndpointConfig.BoundQueue}");
            Console.WriteLine($"Consumer Type: {configurator.EndpointConfig.ConsumerType?.Name}");

            // Симуляция запуска
            await Task.Delay(1000);
            Console.WriteLine("Bus started.");
        }
    }

    // Класс для конфигурации
    class Configurator
    {
        public HostConfig HostConfig { get; private set; }
        public EndpointConfig EndpointConfig { get; private set; }

        public void Host(Uri uri, Action<HostConfig> configureHost)
        {
            var hostConfig = new HostConfig(uri);
            configureHost(hostConfig);
            HostConfig = hostConfig; // Сохраняем конфигурацию хоста
        }

        public void ReceiveEndpoint(string name, Action<EndpointConfig> configureEndpoint)
        {
            var endpointConfig = new EndpointConfig(name);
            configureEndpoint(endpointConfig);
            EndpointConfig = endpointConfig; // Сохраняем конфигурацию конечной точки
        }
    }

    // Класс для конфигурации хоста
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

    // Класс для конфигурации конечной точки
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
        }

        public void Consumer<TConsumer>()
        {
            ConsumerType = typeof(TConsumer); // Сохраняем тип потребителя
        }
    }

    // Пример потребителя
    class MessageConsumer
    {
    }

    class Program
    {
        static async Task Main(string[] args)
        {
            var busControl = new BusControl(configurator =>
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

            await busControl.StartAsync();
        }
    }
}
```

### Объяснение

1. **BusControl**:
   - Класс `BusControl` принимает лямбда-выражение, которое используется для настройки.
   - В методе `StartAsync` создается экземпляр `Configurator`, который передается в лямбда-выражение для настройки.
   - После настройки значения сохраняются в свойствах `Configurator`.
   - Значения конфигурации затем читаются и выводятся в консоль.

2. **Configurator**:
   - Этот класс содержит методы для настройки хоста и конечной точки.
   - В методах `Host` и `ReceiveEndpoint` создаются и настраиваются объекты `HostConfig` и `EndpointConfig`, которые затем сохраняются в соответствующие свойства.

3. **HostConfig** и **EndpointConfig**:
   - Эти классы содержат свойства для хранения настроек, которые могут быть изменены через лямбда-выражения.

### Как это связано с вашим примером с MassTransit

В MassTransit происходит что-то похожее. Когда вы вызываете методы конфигурации, такие как `Host` и `ReceiveEndpoint`, они создают и настраивают внутренние объекты, которые сохраняются в состоянии конфигуратора. Когда вы вызываете `StartAsync`, библиотека использует эти сохраненные настройки для инициализации и запуска инфраструктуры RabbitMQ.

### Вывод

Таким образом, настройки, заданные через лямбда-выражения, сохраняются во внутренние объекты конфигурации. При старте `await busControl.StartAsync()` эти объекты используются для настройки и запуска сервиса.