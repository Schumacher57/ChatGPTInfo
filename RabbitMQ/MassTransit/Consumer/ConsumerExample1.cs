using System;
using MassTransit;
using MassTransit.RabbitMqTransport;
using MassTransit.Util;

class Program
{
    static void Main()
    {
        var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
        {
            var host = cfg.Host(new Uri("rabbitmq://localhost/"), h =>
            {
                h.Username("guest");
                h.Password("guest");
            });

            cfg.ReceiveEndpoint(host, "my-queue", e =>
            {
                e.Bind("my-exchange");
                e.Consumer<MyConsumer>();
            });
        });

        busControl.Start();

        Console.WriteLine("Consumer started. Press any key to exit.");
        Console.ReadKey();

        busControl.Stop();
    }
}

class MyConsumer : IConsumer<MyMessage>
{
    public Task Consume(ConsumeContext<MyMessage> context)
    {
        // Обработка сообщения
        return Task.CompletedTask;
    }
}

class MyMessage
{
    public string Text { get; set; }
}