Конечно! Вот пример использования Entity Framework Core (EF Core) совместно с механизмом внедрения зависимостей (Dependency Injection - DI) в ASP.NET Core.

1. Первым шагом установите необходимые пакеты NuGet. Для работы с EF Core и DI в ASP.NET Core, вам потребуется следующее:

- Microsoft.EntityFrameworkCore
- Microsoft.EntityFrameworkCore.SqlServer (или Microsoft.EntityFrameworkCore.Design, если используется другая СУБД)
- Microsoft.Extensions.DependencyInjection

2. Создайте класс контекста базы данных наследуясь от `DbContext`. Укажите модели сущностей и настройте подключение к базе данных. Например:

```csharp
using Microsoft.EntityFrameworkCore;

public class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
    {
    }

    // Определите свои сущности, добавив DbSet<T> для каждой таблицы базы данных
    public DbSet<Customer> Customers { get; set; }
}
```

3. Зарегистрируйте класс контекста базы данных и другие зависимости в DI-контейнере в методе `ConfigureServices` класса `Startup`:

```csharp
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        // Конфигурируем подключение к базе данных
        services.AddDbContext<MyDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

        // Регистрируем другие зависимости или сервисы
        services.AddScoped<IMyService, MyService>();

        // ...
    }
}
```

4. В итоге, вы можете использовать DI для внедрения контекста базы данных или других зависимостей в ваши сервисы или контроллеры. Например:

```csharp
public class MyService : IMyService
{
    private readonly MyDbContext _dbContext;

    public MyService(MyDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void DoSomething()
    {
        // Использование контекста базы данных
        var customers = _dbContext.Customers.ToList();

        // ...
    }
}
```

В этом примере, класс `MyService` внедряет `MyDbContext` через конструктор, благодаря DI. Теперь вы можете использовать контекст базы данных (`_dbContext`) внутри сервиса для выполнения операций с базой данных.

Обратите внимание, что приведенный пример является базовым и не включает все аспекты EF Core и DI в ASP.NET Core, такие как миграции базы данных или настройка репозиториев. Однако, он демонстрирует основной принцип использования EF Core совместно с DI в ASP.NET Core приложении.
