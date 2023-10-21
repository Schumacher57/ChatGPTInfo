Привет! Вот пример использования оператора JOIN в LINQ для объединения двух таблиц:

```csharp
// Создаем две таблицы с данными
var orders = new[]
{
    new { OrderId = 1, ProductId = 101, Quantity = 2 },
    new { OrderId = 2, ProductId = 102, Quantity = 1 },
    new { OrderId = 3, ProductId = 103, Quantity = 3 }
};

var products = new[]
{
    new { ProductId = 101, Name = "Апельсины", Price = 2.5 },
    new { ProductId = 102, Name = "Яблоки", Price = 1.8 },
    new { ProductId = 103, Name = "Бананы", Price = 3.2 }
};

// Выполняем объединение таблиц с помощью оператора join
var query = from order in orders
            join product in products on order.ProductId equals product.ProductId
            select new
            {
                order.OrderId,
                product.Name,
                order.Quantity,
                TotalPrice = order.Quantity * product.Price
            };

// Выводим результат
foreach (var item in query)
{
    Console.WriteLine($"Заказ #{item.OrderId}: {item.Name}, Количество: {item.Quantity}, Общая стоимость: {item.TotalPrice}");
}
```

Оператор JOIN используется для объединения двух таблиц на основе определенного условия (в данном случае сравнение полей `ProductId`). Результирующий запрос выбирает нужные поля из обоих таблиц и вычисляет общую стоимость заказа. Результат выводится в консоль.
