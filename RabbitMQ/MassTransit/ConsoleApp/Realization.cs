using System;
using System.Threading.Tasks;
using MassTransit;

class Program
{
    static async Task Main()
    {
        var busControl = Bus.Factory.CreateUsingRabbitMq(configurator =>
        {
            configurator.Host("rabbitmq://localhost");
            configurator.ReceiveEndpoint("my_queue", endpointConfigurator =>
            {
                endpointConfigurator.Consumer<MessageConsumer>();
            });
        });

        await busControl.StartAsync();

        Console.WriteLine("Ожидание сообщений. Нажмите любую клавишу для выхода...");
        Console.ReadKey();

        await busControl.StopAsync();
    }
}

class MessageConsumer : IConsumer<Message>
{
    public async Task Consume(ConsumeContext<Message> context)
    {
        Console.WriteLine($"Получено сообщение: {context.Message.Text}");
    }
}

class Message
{
    public string Text { get; set; }
}