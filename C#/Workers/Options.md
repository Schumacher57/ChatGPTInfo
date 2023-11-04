# Содержание
1. [Example№1](#Part1)
2. [Example№2](#Part2)


## Example№1 <a name="Par1"></a>
Просто 🌟! `Microsoft.Extensions.Options` - это часть пакета `Microsoft.Extensions.Configuration`, который предоставляет удобный способ управления настройками приложения в ASP.NET Core. Пакет `Microsoft.Extensions.Options` позволяет вам связать настройки (settings) вашего приложения с классами C#, чтобы получить доступ к этим настройкам в удобном и типобезопасном формате.

Вот пример, чтобы это прояснить:

1. Для начала, добавьте `appsettings.json` файл в ваш проект и определите там требуемые настройки. Например, следующий файл `appsettings.json` содержит настройки подключения к базе данных:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=MyDatabase;User Id=myUsername;Password=myPassword;"
  },
  "AppSettings": {
    "MaxItemCount": 100,
    "EnableFeatureA": true
  }
}
```

2. Создайте класс для связывания настроек. Например, для настроек подключения к базе данных:

```csharp
public class ConnectionStrings
{
    public string DefaultConnection { get; set; }
}
```

3. В классе Startup вашего приложения, в методе `ConfigureServices`, добавьте связывание настроек с классами:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    // ...
    
    // Регистрируем настройки подключения к базе данных
    services.Configure<ConnectionStrings>(Configuration.GetSection("ConnectionStrings"));

    // ...
}
```

4. Теперь вы можете получить доступ к настройкам в любом классе, в котором внедрен экземпляр `IOptions<T>`, где `T` - класс, связанный с настройками. Например, в сервисе или контроллере:

```csharp
public class MyService
{
    private readonly string _connectionString;

    public MyService(IOptions<ConnectionStrings> options)
    {
        _connectionString = options.Value.DefaultConnection;
    }

    public void DoSomething()
    {
        // Используем _connectionString для работы с базой данных
        // ...
    }
}
```

Таким образом, путем внедрения `IOptions<ConnectionStrings>` в `MyService`, мы можем легко получить доступ к настройкам подключения к базе данных и использовать их внутри метода `DoSomething()`.

Использование `Microsoft.Extensions.Options` позволяет упростить управление настройками вашего приложения, обеспечивая типобезопасную и удобную конфигурацию. Это особенно полезно в ASP.NET Core, где настройки приложения могут быть легко связаны с соответствующими классами и использованы в различных компонентах приложения.


## Example №2 <a name="Par2"></a>

Привет! 😊 Чтобы подключить и использовать `IOptions` в C#, вам необходимо выполнить следующие шаги:

1. Установите пакет `Microsoft.Extensions.Options` из NuGet в вашем проекте. Вы можете сделать это через менеджер пакетов NuGet в Visual Studio или используя консольный интерфейс командной строки с помощью команды `dotnet add package Microsoft.Extensions.Options`.

2. Создайте класс для хранения настроек, например, `AppSettings`:

```csharp
public class AppSettings
{
    public string SomeSetting { get; set; }
    public int AnotherSetting { get; set; }
    // Добавьте другие необходимые настройки
}
```

3. В файле `appsettings.json` вашего проекта определите соответствующие элементы настроек:

```json
{
  "AppSettings": {
    "SomeSetting": "Значение1",
    "AnotherSetting": 42
  }
}
```

4. В Startup.cs добавьте следующий код для регистрации настроек:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    // Регистрируем конфигурацию
    services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

    // Остальные регистрации служб...
}
```

5. Теперь вы можете внедрить `IOptions<AppSettings>` в ваши сервисы или контроллеры:

```csharp
public class MyService
{
    private readonly AppSettings _appSettings;

    public MyService(IOptions<AppSettings> appSettings)
    {
        _appSettings = appSettings.Value;
    }

    public void DoSomething()
    {
        string someSetting = _appSettings.SomeSetting;
        int anotherSetting = _appSettings.AnotherSetting;

        // Используйте полученные настройки...
    }
}
```

Убедитесь, что вы правильно настроили имя секции в `GetSection` (в данном случае "AppSettings") и что оно соответствует имени секции в вашем файле `appsettings.json`. Если элемент не найден, убедитесь, что он правильно указан в файле конфигурации.

Если вы используете ASP.NET Core, регистрация и конфигурация настроек может отличаться. В таком случае дополнительно убедитесь, что вы добавили настройки в раздел `appsettings.json` и правильно настроили `IConfiguration` в `Startup.cs`.

Для создания экземпляра `MyService` из примера выше вам понадобится использовать внедрение зависимостей (Dependency Injection, DI). Вот как это можно сделать в зависимости от среды, в которой вы работаете:

ASP.NET Core:
Если вы разрабатываете веб-приложение на ASP.NET Core, то DI настроен автоматически. Вы можете внедрить `MyService` в контроллер или другой сервис, добавив его в конструктор. Например:

```csharp
public class MyController : ControllerBase
{
    private readonly MyService _myService;

    public MyController(MyService myService)
    {
        _myService = myService;
    }

    // Далее идут действия контроллера...
}
```

Когда ASP.NET Core обнаруживает, что контроллер требует `MyService`, он автоматически создаст экземпляр и передаст его в конструктор.

Консольное приложение или другие среды:
Если вы разрабатываете консольное приложение или работаете в другой среде, где DI не настроен автоматически, то вам нужно будет настроить DI вручную. Вот пример того, как это можно сделать:

```csharp
// В точке входа вашего приложения (например, в методе Main) создайте экземпляр IServiceCollection:
var services = new ServiceCollection();

// Зарегистрируйте необходимые службы, включая MyService:
services.AddScoped<MyService>();

// Строим провайдер служб:
var serviceProvider = services.BuildServiceProvider();

// Теперь вы можете получить экземпляр MyService через провайдер:
var myService = serviceProvider.GetRequiredService<MyService>();
```

В этом примере мы используем `ServiceCollection` для регистрации служб. `AddScoped` используется для зарегистрированного сервиса в области (scoped), что означает, что для каждой области (обычно это запрос) будет создан новый экземпляр `MyService`.

Пожалуйста, учтите, что код для настройки DI может различаться в зависимости от выбранного вами фреймворка или библиотеки DI. Вышеуказанные примеры относятся к стандартным шаблонам ASP.NET Core и .NET Core.

