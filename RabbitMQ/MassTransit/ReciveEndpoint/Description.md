## ReceiveEndpoint ##
В контексте C#, MassTransit и RabbitMQ, `cfg.ReceiveEndpoint` представляет собой метод для настройки конечной точки (endpoint) приема сообщений. Он используется для определения параметров и поведения конечной точки, которая будет прослушивать и обрабатывать сообщения, полученные из очереди RabbitMQ.

Пример использования метода `cfg.ReceiveEndpoint` в конфигурации MassTransit выглядит следующим образом:

```csharp
cfg.ReceiveEndpoint("queue-name", endpointConfigurator =>
{
    // Настройка обработчиков сообщений и других параметров конечной точки
});
```

При использовании `cfg.ReceiveEndpoint` вы указываете имя очереди RabbitMQ (`"queue-name"`), на которую будет подписана конечная точка для приема сообщений. Затем внутри делегата `endpointConfigurator` вы можете настроить обработчики сообщений и другие параметры этой конечной точки.

Примеры настроек, которые можно выполнить внутри `cfg.ReceiveEndpoint`, включают:

1. Установка обработчика сообщений:
```csharp
endpointConfigurator.Handler<MyMessage>(context =>
{
    // Обработка сообщения типа MyMessage
});
```

2. Установка метода обработки исключений:
```csharp
endpointConfigurator.Handler<MyMessage>(context =>
{
    // Обработка сообщения типа MyMessage
}, exceptionHandler =>
{
    // Обработка исключений при обработке сообщений
});
```

3. Установка политики повторной обработки сообщений:
```csharp
endpointConfigurator.UseMessageRetry(r =>
{
    r.Interval(3, TimeSpan.FromSeconds(5));
});
```

4. Установка параметров параллельной обработки сообщений:
```csharp
endpointConfigurator.UseConcurrencyLimit(5);
```

5. Настройка делегата для получения уведомления о получении сообщения:
```csharp
endpointConfigurator.Consumer<MyConsumer>(consumerConfigurator =>
{
    consumerConfigurator.Message<MyMessage>(messageConfigurator =>
    {
        messageConfigurator.Handler<MyMessage>(context =>
        {
            // Обработка сообщения типа MyMessage
        });
    });

    consumerConfigurator.Message<MyOtherMessage>(messageConfigurator =>
    {
        messageConfigurator.Handler<MyOtherMessage>(context =>
        {
            // Обработка сообщения типа MyOtherMessage
        });
    });
});
```

Изменения, выполненные внутри метода `cfg.ReceiveEndpoint`, применяются к заданной конечной точке приема сообщений и влияют на ее поведение при получении и обработке сообщений из указанной очереди RabbitMQ.
