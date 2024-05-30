В контексте консольного приложения или приложения службы Windows вы также можете использовать библиотеку `Microsoft.Extensions.Configuration` для работы с конфигурационными файлами, такими как `appsettings.json`. Это позволяет вам гибко настраивать и управлять настройками даже вне ASP.NET Core. Рассмотрим, как это можно сделать пошагово.

### Шаги по настройке конфигурации в консольном приложении

1. **Добавление необходимых пакетов**:
    - Убедитесь, что у вас установлены нужные пакеты NuGet:
        - `Microsoft.Extensions.Configuration`
        - `Microsoft.Extensions.Configuration.Json`
        - `Microsoft.Extensions.DependencyInjection` (для использования DI, если нужно)

```bash
dotnet add package Microsoft.Extensions.Configuration
dotnet add package Microsoft.Extensions.Configuration.Json
dotnet add package Microsoft.Extensions.DependencyInjection
```

2. **Создание класса настроек**:
    - Создайте POCO (Plain Old CLR Object) класс для хранения настроек.

```csharp
public class MySettings
{
    public string Setting1 { get; set; }
    public int Setting2 { get; set; }
}
```

3. **Создание файла конфигурации**:
    - Добавьте файл `appsettings.json` в проект с нужными настройками.

```json
{
  "MySettings": {
    "Setting1": "Value1",
    "Setting2": 10
  }
}
```

4. **Настройка конфигурации в `Program.cs`**:
    - Настройте чтение конфигурации из файла `appsettings.json`.

```csharp
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

class Program
{
    static void Main(string[] args)
    {
        // Создание конфигурации
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory) // Устанавливаем базовый путь для поиска файлов
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true) // Добавляем файл конфигурации
            .Build();

        // Чтение настроек в POCO класс
        var mySettings = new MySettings();
        configuration.GetSection("MySettings").Bind(mySettings);

        // Вывод настроек
        Console.WriteLine($"Setting1: {mySettings.Setting1}");
        Console.WriteLine($"Setting2: {mySettings.Setting2}");

        // Настройка DI контейнера
        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection, configuration);

        var serviceProvider = serviceCollection.BuildServiceProvider();
        
        // Использование сервиса с настройками
        var myService = serviceProvider.GetService<MyService>();
        myService.DoSomething();
    }

    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        // Регистрация настроек в DI
        services.Configure<MySettings>(configuration.GetSection("MySettings"));

        // Регистрация других сервисов
        services.AddTransient<MyService>();
    }
}

public class MyService
{
    private readonly MySettings _settings;

    public MyService(IOptions<MySettings> options)
    {
        _settings = options.Value;
    }

    public void DoSomething()
    {
        Console.WriteLine($"MyService is using Setting1: {_settings.Setting1} and Setting2: {_settings.Setting2}");
    }
}
```

### Пояснения

1. **Создание конфигурации**:
    - `ConfigurationBuilder` используется для построения конфигурации, добавляя провайдеры конфигурации, такие как JSON файл.
    - `SetBasePath(AppContext.BaseDirectory)` задает базовый путь для поиска файлов конфигурации.
    - `AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)` добавляет провайдер конфигурации для чтения из `appsettings.json`.

2. **Чтение настроек**:
    - `configuration.GetSection("MySettings").Bind(mySettings)` связывает секцию "MySettings" из файла конфигурации с объектом `MySettings`.

3. **Настройка DI контейнера**:
    - `services.Configure<MySettings>(configuration.GetSection("MySettings"))` регистрирует настройки в DI контейнере.
    - Регистрируются другие сервисы, которые могут использовать эти настройки.

4. **Использование сервиса**:
    - `MyService` получает настройки через DI, используя интерфейс `IOptions<MySettings>`.

Таким образом, даже в консольном приложении или приложении службы Windows вы можете использовать мощную систему конфигурации и Dependency Injection, аналогичную той, что используется в ASP.NET Core, для управления настройками приложения.