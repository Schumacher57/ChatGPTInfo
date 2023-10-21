# Data base first model

Если у вас уже есть существующая модель базы данных, и вы хотите использовать Entity Framework Core без автогенерации кода моделей, вы можете вручную создать классы моделей, которые соответствуют вашей базе данных.

Шаг 1: Создайте классы моделей
-----------------------------
Начните создавать классы моделей, которые соответствуют таблицам в вашей базе данных. Каждый класс должен представлять одну таблицу и иметь свойства, соответствующие столбцам таблицы.

Например, предположим, у вас есть таблица "Customers" с колонками "Id", "Name" и "Email". Создайте класс `Customer`, который отображает эту таблицу:

```csharp
public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}
```

Шаг 2: Создайте класс контекста базы данных
----------------------------------------
Создайте класс контекста базы данных, который наследуется от `DbContext`. Этот класс предоставляет доступ к вашим моделям и устанавливает соединение с базой данных.

```csharp
public class YourDbContext : DbContext
{
    public DbSet<Customer> Customers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Data Source=your-db-server;Initial Catalog=your-db;Integrated Security=True");
        }
    }
}
```

В приведенном выше коде `DbSet<Customer> Customers` указывает на созданный вами класс `Customer`, а метод `OnConfiguring` предоставляет строку подключения к вашей базе данных.

Шаг 3: Используйте вашу модель
------------------------------
Теперь вы можете использовать вашу модель в коде для работы с базой данных. Как пример, в методе `Main` класса `Program`:

```csharp
using System;
using System.Linq;

namespace YourNamespace
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using (var dbContext = new YourDbContext())
            {
                // Пример запроса к базе данных
                var results = dbContext.Customers.Where(c => c.Name.StartsWith("A")).ToList();

                foreach (var customer in results)
                {
                    Console.WriteLine($"Id: {customer.Id}, Name: {customer.Name}, Email: {customer.Email}");
                }
            }
        }
    }
}
```

В приведенном выше коде мы создаем экземпляр `YourDbContext` и выполняем запрос к таблице `Customers`, выбирая все записи, где имя клиента начинается с символа "A". Результаты выводятся в консоль.

Обратите внимание, что вам нужно заменить `YourNamespace`, `YourDbContext`, `Customer` и их свойства на соответствующие имена в вашем проекте.

Таким образом, вы можете использовать существующую модель базы данных без автогенерации кода с помощью Entity Framework Core!