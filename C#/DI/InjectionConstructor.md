Из предоставленной ошибки видно, что сервис `MyServiceNotification` не был зарегистрирован в контейнере зависимостей `IServiceCollection`. Это означает, что вы забыли добавить регистрацию этого сервиса.

Вам необходимо добавить регистрацию `MyServiceNotification` в контейнер `ServiceCollection` перед построением `ServiceProvider`. Вот исправленный пример:

```csharp
using System;
using Microsoft.Extensions.DependencyInjection;

// Интерфейс зависимости
public interface IMessageService
{
    void SendMessage(string message);
}

// Реализация зависимости
public class EmailService : IMessageService
{
    public void SendMessage(string message)
    {
        Console.WriteLine($"Sending email: {message}");
    }
}

// Класс, использующий зависимость
public class NotificationService
{
    private readonly IMessageService _messageService;

    public NotificationService(IMessageService messageService)
    {
        _messageService = messageService;
    }

    public void SendNotification(string notification)
    {
        _messageService.SendMessage(notification);
    }
}

// Новый сервис
public class MyServiceNotification
{
    private readonly IMessageService _messageService;

    public MyServiceNotification(IMessageService messageService)
    {
        _messageService = messageService;
    }

    public void DoSomething()
    {
        _messageService.SendMessage("Doing something...");
    }
}

class Program
{
    static void Main(string[] args)
    {
        var serviceProvider = new ServiceCollection()
            .AddScoped<IMessageService, EmailService>()
            .AddScoped<NotificationService>() // Добавляем регистрацию NotificationService
            .AddScoped<MyServiceNotification>() // Добавляем регистрацию MyServiceNotification
            .BuildServiceProvider();

        using (var scope = serviceProvider.CreateScope())
        {
            var notificationService = scope.ServiceProvider.GetRequiredService<NotificationService>();
            notificationService.SendNotification("Hello, world!");

            var myService = scope.ServiceProvider.GetRequiredService<MyServiceNotification>();
            myService.DoSomething();
        }
    }
}
```

Теперь `MyServiceNotification` добавлен в контейнер с помощью `.AddScoped<MyServiceNotification>()`. В методе `Main` теперь вы можете разрешить зависимость `MyServiceNotification` с помощью `scope.ServiceProvider.GetRequiredService<MyServiceNotification>()`.

Теперь при запуске программы вы должны увидеть вывод с обоих сервисов: `Sending email: Hello, world!` и `Sending email: Doing something...`.

Прошу прощения за пропущенную регистрацию в исходном ответе. Надеюсь, это помогло исправить проблему! 🙇‍♀️



---

### Example Microsoft.Extensions.DependencyInjection ###
Конечно! Вот пример регистрации зависимости через конструктор с использованием `Microsoft.Extensions.DependencyInjection`:

```csharp
using System;
using Microsoft.Extensions.DependencyInjection;

// Интерфейс зависимости
public interface IMessageService
{
    void SendMessage(string message);
}

// Реализация зависимости
public class EmailService : IMessageService
{
    public void SendMessage(string message)
    {
        Console.WriteLine($"Sending email: {message}");
    }
}

// Класс, использующий зависимость
public class NotificationService
{
    private readonly IMessageService _messageService;

    public NotificationService(IMessageService messageService)
    {
        _messageService = messageService;
    }

    public void SendNotification(string notification)
    {
        _messageService.SendMessage(notification);
    }
}

class Program
{
    static void Main(string[] args)
    {
        // Создание контейнера DI
        var serviceProvider = new ServiceCollection()
            .AddScoped<IMessageService, EmailService>()
            .BuildServiceProvider();

        // Разрешение зависимости
        using (var scope = serviceProvider.CreateScope())
        {
            var notificationService = scope.ServiceProvider.GetRequiredService<NotificationService>();
            notificationService.SendNotification("Hello, world!");
        }
    }
}
```

В этом примере мы используем `Microsoft.Extensions.DependencyInjection` для регистрации и разрешения зависимости.

Мы создаем `ServiceCollection`, добавляем регистрацию зависимости с помощью `AddScoped<IMessageService, EmailService>()` (вместо `AddScoped` можно использовать `AddSingleton` или `AddTransient`, в зависимости от требуемого времени жизни экземпляра).

Затем мы строим `ServiceProvider` с помощью `BuildServiceProvider` и разрешаем зависимость класса `NotificationService` с помощью `scope.ServiceProvider.GetRequiredService<NotificationService>()`.

При запуске консольного приложения вы увидите вывод `Sending email: Hello, world!`, что означает, что сообщение было отправлено с помощью `EmailService`.

Таким образом, этот пример демонстрирует регистрацию и использование зависимости через конструктор с помощью `Microsoft.Extensions.DependencyInjection`.

Надеюсь, это помогло! 🤗

---

### Exampple №2 ###
Конечно! Вот простой пример регистрации зависимости через конструктор в обычном консольном приложении на C#:

```csharp
using System;

// Интерфейс зависимости
public interface IMessageService
{
    void SendMessage(string message);
}

// Реализация зависимости
public class EmailService : IMessageService
{
    public void SendMessage(string message)
    {
        Console.WriteLine($"Sending email: {message}");
    }
}

// Класс, использующий зависимость
public class NotificationService
{
    private readonly IMessageService _messageService;

    public NotificationService(IMessageService messageService)
    {
        _messageService = messageService;
    }

    public void SendNotification(string notification)
    {
        _messageService.SendMessage(notification);
    }
}

class Program
{
    static void Main(string[] args)
    {
        // Создание контейнера DI
        var serviceProvider = new ServiceContainer();

        // Регистрация зависимостей
        serviceProvider.AddScoped<IMessageService, EmailService>();
        // Можно зарегистрировать и другие реализации IMessageService, используя разные методы расширений,
        // например, AddSingleton для синглтона или AddTransient для создания нового экземпляра при каждом запросе

        // Построение и разрешение зависимости
        using (var scope = serviceProvider.CreateScope())
        {
            var notificationService = scope.ServiceProvider.GetRequiredService<NotificationService>();
            notificationService.SendNotification("Hello, world!");
        }
    }
}
```

В этом примере мы создаем контейнер DI (`ServiceContainer`), регистрируем зависимость `IMessageService` с реализацией `EmailService`, а затем создаем и разрешаем зависимость класса `NotificationService`, используя `scope.ServiceProvider.GetRequiredService<NotificationService>()`.

При выполнении кода будет создан экземпляр `EmailService`, который будет внедрен в конструктор `NotificationService`. Когда вызывается метод `SendNotification` у `NotificationService`, он вызывает метод `SendMessage` зависимости `IMessageService`.

Таким образом, при запуске консольного приложения вы увидите вывод `Sending email: Hello, world!`, что означает, что сообщение было отправлено с помощью `EmailService`.

Надеюсь, этот пример помог вам понять, как зарегистрировать и использовать зависимость через конструктор в консольном приложении на C#.
