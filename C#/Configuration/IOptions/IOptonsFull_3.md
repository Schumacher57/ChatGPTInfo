Отличный вопрос! Код понимает, что надо читать настройки из файла `appsettings.json`, благодаря встроенному механизму конфигурации в ASP.NET Core. Важную роль в этом процессе играют несколько ключевых компонентов: система конфигурации ASP.NET Core, метод `CreateDefaultBuilder` и конфигурация сервисов в `Startup` классе.

### Шаги и компоненты, отвечающие за чтение настроек из `appsettings.json`

1. **Файл конфигурации**:
    - В корне проекта находится файл `appsettings.json`, который содержит настройки в формате JSON.

2. **Метод `CreateDefaultBuilder` в `Program.cs`**:
    - Этот метод автоматически настраивает приложение для чтения конфигураций из нескольких источников, включая `appsettings.json`.
    - По умолчанию, этот метод добавляет файл `appsettings.json` в конфигурационный провайдер.

```csharp
public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}
```

3. **Host.CreateDefaultBuilder(args)**:
    - Этот метод делает множество вещей за кулисами, включая настройку конфигурации. Он добавляет несколько конфигурационных источников, таких как:
        - `appsettings.json`
        - `appsettings.{Environment}.json` (если существует, например, `appsettings.Development.json`)
        - переменные окружения
        - аргументы командной строки

4. **Метод `ConfigureAppConfiguration`**:
    - Если вам нужно настроить конфигурацию более детально, можно использовать метод `ConfigureAppConfiguration` в `CreateDefaultBuilder`.

```csharp
public static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration((context, config) =>
        {
            // Добавляем конфигурационные источники вручную (если необходимо)
            config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            config.AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true);
            config.AddEnvironmentVariables();
            if (args != null)
            {
                config.AddCommandLine(args);
            }
        })
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup<Startup>();
        });
```

5. **Конфигурация в `Startup.cs`**:
    - В классе `Startup` можно использовать объект конфигурации (`IConfiguration`), который автоматически настроен для чтения из этих источников.

```csharp
public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        // Здесь настройки из appsettings.json связываются с POCO классом
        services.Configure<MySettings>(Configuration.GetSection("MySettings"));
        
        // Другие сервисы...
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // Конфигурация Middleware...
    }
}
```

### Резюме

- **Файл конфигурации** `appsettings.json` находится в корне проекта.
- **Метод `CreateDefaultBuilder`** в `Program.cs` добавляет `appsettings.json` в конфигурацию приложения.
- **Метод `ConfigureServices`** в `Startup.cs` связывает настройки из конфигурационного файла с POCO классами.

Эти компоненты работают вместе, чтобы приложение автоматически читало настройки из `appsettings.json` и других источников конфигурации, обеспечивая удобный способ управления настройками через Dependency Injection и другие механизмы ASP.NET Core.