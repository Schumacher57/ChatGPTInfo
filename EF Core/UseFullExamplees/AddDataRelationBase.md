Для добавления данных в таблицу с использованием Entity Framework Core (EF Core) вам нужно выполнить следующие шаги. Предполагается, что у вас уже настроены модели, контекст базы данных и имеется связь между таблицами.

1. Создайте экземпляр вашей модели данных (сущности) и установите нужные значения свойств:

```csharp
var entity = new YourEntity
{
    Property1 = value1,
    Property2 = value2,
    // ...
    RelatedEntity = relatedEntity // Если имеется связь с другой таблицей
};
```

2. Получите экземпляр контекста базы данных (`DbContext`):

```csharp
using (var context = new YourDbContext())
{
    // ...
}
```

3. Добавьте экземпляр вашей модели данных в контекст базы данных:

```csharp
context.YourEntities.Add(entity);
```

4. Если у вас есть связь с другой таблицей, создайте и добавьте экземпляр связанной модели:

```csharp
var relatedEntity = new RelatedEntity
{
    PropertyX = valueX,
    PropertyY = valueY,
    // ...
};

context.RelatedEntities.Add(relatedEntity);
```

5. Если вы хотите сохранить изменения в базе данных, вызовите метод `SaveChanges()` контекста базы данных:

```csharp
context.SaveChanges();
```

После выполнения вышеуказанных шагов, данные будут добавлены в соответствующую таблицу, а при необходимости связи будут установлены между таблицами.
