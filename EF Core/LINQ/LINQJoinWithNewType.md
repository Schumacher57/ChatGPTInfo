Если вы хотите создать новый тип или таблицу на основе объединения двух DbSet, вам необходимо создать класс для этого нового типа и использовать его в операторе `select`.

Вот пример, как это может выглядеть с использованием LINQ в Entity Framework Core:

```csharp
public class CustomOrder
{
    public int CustomOrderId { get; set; }
    public DateTime CustomOrderDate { get; set; }
    public string CustomCustomerName { get; set; }
    public string CustomAdditionalInfo { get; set; }
}

var result = dbContext.Orders
    .Join(
        dbContext.Customers,
        order => order.CustomerId,
        customer => customer.CustomerId,
        (order, customer) => new CustomOrder
        {
            CustomOrderId = order.OrderId,
            CustomOrderDate = order.OrderDate,
            CustomCustomerName = customer.CustomerName,
            CustomAdditionalInfo = "Some additional info"
        })
    .ToList();
```

В этом примере мы определяем класс `CustomOrder`, который представляет новый тип данных, получаемых из объединения. Затем в операторе `Join` указываем создание экземпляра этого класса и заполняем его поля значениями из различных DbSet.

Затем вызывается метод `ToList()` для выполнения фактического запроса и получения результатов в виде списка объектов типа `CustomOrder`.

Таким образом, после выполнения JOIN вы создаете список объектов нового типа `CustomOrder`, каждый из которых содержит данные, соответствующие вашей проекции.
