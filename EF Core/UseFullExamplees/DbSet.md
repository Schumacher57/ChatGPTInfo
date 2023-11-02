# Содержание
1. [Создание констант](#Part1)
2. [InProcess.](#Part2)
---

### 1. Создание констант <a name="Part1"></a>
- <ins>Вариант №1 **ModelBuilder**</ins>
```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<YourEntity>()
        .Property(e => e.TypeEntity)
        .HasDefaultValue("ConstValue");
}
```
- <ins>Вариант №2 **Property**</ins>
```csharp
public string TypeEntity { get; set; } = "ConstValue";
```
---

