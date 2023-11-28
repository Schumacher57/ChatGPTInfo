## Подключение к локальной БД MS SQL ##

Чтобы подключиться к локальной базе данных MS SQL с использованием Entity Framework Core, вам потребуется выполнить следующие шаги:

1️⃣ Установите пакет NuGet:
Убедитесь, что в вашем проекте установлен пакет NuGet `Microsoft.EntityFrameworkCore.SqlServer`. Если пакет отсутствует, вы можете установить его через менеджер пакетов NuGet в Visual Studio или с помощью инструмента командной строки `dotnet add package Microsoft.EntityFrameworkCore.SqlServer`.

2️⃣ Определите контекст данных:
Создайте класс, который наследуется от `DbContext`. В этом классе вы определите свои модели данных (классы) и настройки подключения к базе данных. Пример кода для контекста данных может выглядеть следующим образом:

```csharp
using Microsoft.EntityFrameworkCore;

public class YourDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=YourDatabaseName;Trusted_Connection=True;");
    }

    // Определите свои сущности (таблицы) как свойства DbSet<T>:
    // public DbSet<YourEntity> YourEntities { get; set; }
}
```

Обратите внимание, что в методе `OnConfiguring` мы указываем строку подключения к базе данных. Здесь `"Server=(localdb)\\MSSQLLocalDB;Database=YourDatabaseName;Trusted_Connection=True;"` представляет строку подключения к базе данных MS SQL на локальном сервере. Вы можете заменить `YourDatabaseName` на имя вашей базы данных.

3️⃣ Зарегистрируйте контекст данных:
В классе `Startup` вашего приложения добавьте регистрацию контекста данных в методе `ConfigureServices`. Пример:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    // Другие регистрации служб...

    services.AddDbContext<YourDbContext>();
}
```

Теперь ваше приложение должно быть подключено к локальной базе данных MS SQL через Entity Framework Core. Вы можете добавить модели данных и использовать их в своем приложении с помощью контекста данных.

Обратите внимание, что в приведенном выше примере используется подключение с доверенным соединением (`Trusted_Connection=True`). Если вам нужно указать имя пользователя и пароль, вы можете изменить строку подключения в методе `OnConfiguring` с использованием `UserId` и `Password`.

Также убедитесь, что у вас установлен SQL Server LocalDB или другая версия MS SQL Server, к которой вы хотите подключиться.
