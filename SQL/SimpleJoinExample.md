Конечно! Вот пример простого `JOIN` в T-SQL:

У нас есть две таблицы - `Customers` (клиенты) и `Orders` (заказы). Мы хотим получить список клиентов и соответствующих им заказов. Связь между таблицами устанавливается по полю `CustomerID`.

```sql
SELECT Customers.CustomerID, Customers.FirstName, Customers.LastName, Orders.OrderID, Orders.OrderDate
FROM Customers
JOIN Orders ON Customers.CustomerID = Orders.CustomerID;
```

Этот запрос выбирает поля `CustomerID`, `FirstName` и `LastName` из таблицы `Customers`, а также поля `OrderID` и `OrderDate` из таблицы `Orders`. Затем он объединяет (JOIN) эти таблицы, указывая, что `CustomerID` в таблице `Customers` должен совпадать с `CustomerID` в таблице `Orders`.

Используя этот запрос `JOIN`, вы получите результат, содержащий информацию о клиентах и соответствующих заказах.