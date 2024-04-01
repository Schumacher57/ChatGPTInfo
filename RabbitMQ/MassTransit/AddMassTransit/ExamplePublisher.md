Конечно! Вот пример кода, демонстрирующий, как реализовать Publisher в отдельном классе с использованием MassTransit и RabbitMQ в C#:

```csharp
using System;
using MassTransit;
using Messages;

namespace PublisherApp
{
    public class MyMessagePublisher
    {
        private readonly IBusControl _busControl;

        public MyMessagePublisher()
        {
            _busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host(new Uri("rabbitmq://localhost"), h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });
            });
        }

        public async Task PublishMessage(string messageText)
        {
            await _busControl.StartAsync();

            try
            {
                var message = new MyMessage
                {
                    Text = messageText
                };

                await _busControl.Publish(message);

                Console.WriteLine("Сообщение успешно отправлено.");
            }
            finally
            {
                await _busControl.StopAsync();
            }
        }
    }

    public class MyMessage
    {
        public string Text { get; set; }
    }

    class Program
    {
        static async Task Main(string[] args)
        {
            var publisher = new MyMessagePublisher();
            await publisher.PublishMessage("Hello, RabbitMQ!");
        }
    }
}
```

В этом примере мы создали отдельный класс `MyMessagePublisher`, который инкапсулирует логику отправки сообщений. Класс инициализирует эк
