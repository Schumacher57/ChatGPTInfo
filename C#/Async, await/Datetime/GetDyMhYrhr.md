### C# DateTime.UtcNow.ToString() получить только День, Месяц, Год, Час. ###

---

Для получения только дня, месяца, года и часа из текущего времени в формате строки в C#, вы можете использовать следующий код:

```csharp
DateTime currentTime = DateTime.UtcNow;
string formattedDateTime = currentTime.ToString("dd-MM-yyyy HH");
```

В результате вы получите строку, содержащую день (dd), месяц (MM), год (yyyy) и час (HH) текущего времени.
