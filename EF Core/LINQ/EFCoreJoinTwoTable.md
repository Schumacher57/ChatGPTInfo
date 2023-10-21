Для выполнения JOIN между двумя DbSet в Entity Framework Core можно использовать методы LINQ, такие как `Join`, `GroupJoin`, `SelectMany` и т.д. Вот пример выполнения INNER JOIN между двумя DbSet:

Предположим, у нас есть две сущности: `Order` и `Customer`, и мы хотим выполнить JOIN по полю `CustomerId`. 

```csharp
var result = dbContext.Orders
    .Join(dbContext.Customers,
          order => order.CustomerId,
          customer => customer.CustomerId,
          (order, customer) => new
          {
              OrderId = order.OrderId,
              OrderDate = order.OrderDate,
              CustomerName = customer.CustomerName
          })
    .ToList();
```

В этом примере мы используем метод `Join`, который принимает следующие параметры:

- Первый DbSet (`dbContext.Orders`) - левая сторона JOIN операции.
- Второй DbSet (`dbContext.Customers`) - правая сторона JOIN операции.
- Лямбда-выражение, где `order => order.CustomerId` указывает, какое поле из левой стороны должно соответствовать полю из правой стороны (`customer => customer.CustomerId`).
- Второе лямбда-выражение `(order, customer) => new { ... }` определяет, какие поля двух DbSet должны быть выбраны в результирующий объект.

В этом примере результатом будет список объектов, содержащий `OrderId`, `OrderDate` и `CustomerName`. Обратите внимание, что в примере я использовал метод `ToList()`, чтобы выполнить фактический SQL-запрос и получить результаты. Можете также использовать другие методы расширений LINQ, такие как `FirstOrDefault()`, `SingleOrDefault()` и т.д., в зависимости от вашего конкретного случая.

---
Другой пример
---
Конечно! Вот пример выполнения JOIN между двумя DbSet с использованием LINQ в Entity Framework Core:

```csharp
var result = dbContext.Orders
    .Join(
        dbContext.Customers,
        order => order.CustomerId,
        customer => customer.CustomerId,
        (order, customer) => new 
        {
            OrderId = order.OrderId,
            OrderDate = order.OrderDate,
            CustomerName = customer.CustomerName
        })
    .ToList();
```

В этом примере мы объединяем два DbSet (`Orders` и `Customers`) по полю `CustomerId` и проецируем результат в новый анонимный тип с полями `OrderId`, `OrderDate` и `CustomerName`. Результат JOIN сохраняется в переменной `result`.

Обратите внимание, что мы используем метод `Join` при вызове `Orders`, передавая ему три аргумента:
1. Правый DbSet (`Customers`).
2. Лямбда-выражение, определяющее поле из левого DbSet (`order => order.CustomerId`).
3. Лямбда-выражение, определяющее поле из правого DbSet (`customer => customer.CustomerId`).
4. Лямбда-выражение, определяющее проекцию результатов JOIN в новый тип.

Затем мы вызываем метод `ToList()` для выполнения фактического запроса и получения результатов в виде списка.

Помните, что вам может понадобиться настроить отношения между сущностями в модели Entity Framework Core, чтобы включить связи между DbSet и использовать их для JOIN операций.

