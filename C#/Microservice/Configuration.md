# Содержание
1. [Конфигурация микросервиса в C#](#Part1)
2. [Пример реализации](#Part2)
3. [Пример реализации с добавлением DI `Microsoft.Extensions.DependencyInjection`](#Part3)
## Конфигурация микросервиса в C# <a name="Part1"></a>

Конфигурация микросервиса в C# может быть реализована с использованием класса `ConfigurationBuilder` из пакета `Microsoft.Extensions.Configuration`. Этот класс позволяет загружать и объединять несколько источников конфигурации, таких как файлы, переменные окружения, командная строка и другие.

Вот пример реализации конфигурации микросервиса на C#:

1. Установите пакет `Microsoft.Extensions.Configuration` через NuGet или добавьте соответствующую ссылку на проект.
2. Создайте файл конфигурации (например, `appsettings.json`), в котором определите необходимые настройки. Например:

```json
{
  "Database": {
    "ConnectionString": "Server=example.com;Database=mydatabase;User=root;Password=pa$$w0rd;"
  },
  "ApiKeys": {
    "Stripe": "your-stripe-api-key",
    "SendGrid": "your-sendgrid-api-key"
  }
}
```

3. В коде вашего микросервиса добавьте следующий код для загрузки конфигурации:

```csharp
using Microsoft.Extensions.Configuration;

// ...

var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory()) // Укажите базовый путь для поиска файла конфигурации
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true); // Загрузите файл конфигурации

var configuration = builder.Build();
```

4. Теперь вы можете получить значения конфигурации в коде вашего микросервиса, используя объект `configuration`. Например, чтобы получить строку подключения к базе данных и ключ Stripe:

```csharp
string connectionString = configuration.GetSection("Database:ConnectionString").Value;
string stripeApiKey = configuration.GetSection("ApiKeys:Stripe").Value;
```

Обратите внимание, что ключи конфигурации указываются в формате "Раздел:Подраздел:...:Параметр". Вы можете использовать разные источники конфигурации, такие как переменные окружения (`AddEnvironmentVariables()`), командная строка (`AddCommandLine()`), XML-файлы (`AddXmlFile()`) и др. с помощью методов `AddXxx`.

Таким образом, вы можете легко настроить ваш микросервис, изменяя значения в файле конфигурации или других источниках, без необходимости изменения кода. Это удобно для развертывания и настройки микросервисов в различных средах.

---

## Пример реализации <a name="Part2"></a>

Конечно! Вот полная реализация примера с консольным приложением на C# с комментариями:

1. Создайте новый проект "Console Application" в вашей IDE.

2. Установите пакет `Microsoft.Extensions.Configuration` через NuGet или добавьте соответствующую ссылку на проект.

3. Создайте файл `appsettings.json` в корневой папке проекта и добавьте следующий код в него:

```json
{
  "Database": {
    "ConnectionString": "Server=example.com;Database=mydatabase;User=root;Password=pa$$w0rd;"
  },
  "ApiKeys": {
    "Stripe": "your-stripe-api-key",
    "SendGrid": "your-sendgrid-api-key"
  }
}
```

4. В `Program.cs` добавьте следующий код:

```csharp
using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Создание конфигурационного объекта
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // Указываем текущую директорию как базовый путь
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true) // Загрузка файла конфигурации
                .Build();

            // Получение значения строки подключения к базе данных
            string connectionString = configuration.GetSection("Database:ConnectionString").Value;
            Console.WriteLine($"Database ConnectionString: {connectionString}");

            // Получение значения ключа Stripe
            string stripeApiKey = configuration.GetSection("ApiKeys:Stripe").Value;
            Console.WriteLine($"Stripe API Key: {stripeApiKey}");

            Console.ReadLine();
        }
    }
}
```

5. Запустите консольное приложение. Вы должны увидеть значения из файла `appsettings.json`, выведенные в консоль.

Обратите внимание, что файл `appsettings.json` должен находиться в той же директории, где находится выполняемый файл вашего консольного приложения.

---

## Пример реализации с добавлением DI `Microsoft.Extensions.DependencyInjection` <a name="Part3"></a>
Конечно! Вот полная реализация примера с консольным приложением на C# с использованием DI контейнера из `Microsoft.Extensions.DependencyInjection`:

1. Создайте новый проект "Console Application" в вашей IDE.

2. Установите пакет `Microsoft.Extensions.Configuration` и `Microsoft.Extensions.DependencyInjection` через NuGet или добавьте соответствующие ссылки на проект.

3. Создайте файл `appsettings.json` в корневой папке проекта и добавьте следующий код в него:

```json
{
  "Database": {
    "ConnectionString": "Server=example.com;Database=mydatabase;User=root;Password=pa$$w0rd;"
  },
  "ApiKeys": {
    "Stripe": "your-stripe-api-key",
    "SendGrid": "your-sendgrid-api-key"
  }
}
```

4. В `Program.cs` добавьте следующий код:

```csharp
using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Создание конфигурационного объекта
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // Указываем текущую директорию как базовый путь
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true) // Загрузка файла конфигурации
                .Build();

            // Создание и настройка контейнера DI
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IConfiguration>(configuration) // Регистрация конфигурации как синглтон
                .AddScoped<MyService>() // Регистрация сервиса MyService в Scoped режиме
                .BuildServiceProvider();

            using (var scope = serviceProvider.CreateScope())
            {
                var myService = scope.ServiceProvider.GetRequiredService<MyService>();
                myService.Execute();
            }

            Console.ReadLine();
        }
    }

    public class MyService
    {
        private readonly IConfiguration _configuration;

        public MyService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Execute()
        {
            // Получение значения строки подключения к базе данных
            string connectionString = _configuration.GetSection("Database:ConnectionString").Value;
            Console.WriteLine($"Database ConnectionString: {connectionString}");

            // Получение значения ключа Stripe
            string stripeApiKey = _configuration.GetSection("ApiKeys:Stripe").Value;
            Console.WriteLine($"Stripe API Key: {stripeApiKey}");
        }
    }
}
```

5. Запустите консольное приложение. Вы должны увидеть значения из файла `appsettings.json`, выведенные в консоль.

Обратите внимание, что файл `appsettings.json` должен находиться в той же директории, где находится выполняемый файл вашего консольного приложения.
