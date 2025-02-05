### **Что такое Mock-объекты?**  
Mock-объекты (или просто "моки") — это поддельные (фейковые) объекты, которые позволяют тестировать код **без реальных зависимостей**. Они заменяют настоящие объекты, чтобы:  

✅ **Изолировать тестируемый код** (Unit-тесты должны проверять только одну вещь)  
✅ **Задать предсказуемое поведение** (например, всегда возвращать определённое значение)  
✅ **Избежать работы с реальными базами данных, API, файлами и т. д.**  

---

## **1. Базовый пример Mock-объекта**
Допустим, у нас есть интерфейс `ICalculator`, который умеет складывать числа:  
```csharp
public interface ICalculator
{
    int Add(int a, int b);
}
```
В реальном коде он мог бы быть сложным, но для тестов мы **подменим его мок-объектом**.  

### **Создаём мок и задаём поведение**
```csharp
using Moq;
using Xunit;

public class CalculatorTests
{
    [Fact]
    public void Test_Add_Method()
    {
        // Создаём мок объекта ICalculator
        var mockCalculator = new Mock<ICalculator>();

        // Настраиваем метод Add: пусть он возвращает 10, если переданы (5, 5)
        mockCalculator.Setup(calc => calc.Add(5, 5)).Returns(10);

        // Используем mock как настоящий объект
        int result = mockCalculator.Object.Add(5, 5);

        // Проверяем, что метод работает как ожидалось
        Assert.Equal(10, result);
    }
}
```
📌 **Что тут происходит?**  
- `Mock<ICalculator>` — создаём мок объекта `ICalculator`.  
- `Setup(calc => calc.Add(5, 5)).Returns(10);` — говорим моку, что если передать `5, 5`, он должен вернуть `10`.  
- `mockCalculator.Object` — это поддельный объект, который можно использовать.  
- `Assert.Equal(10, result);` — проверяем, что результат соответствует ожиданиям.  

---

## **2. Проверяем вызов метода**
Допустим, у нас есть метод, который должен вызвать `Add`, но мы не знаем, как он работает внутри:  
```csharp
public class MathService
{
    private readonly ICalculator _calculator;

    public MathService(ICalculator calculator)
    {
        _calculator = calculator;
    }

    public int ComputeSum(int a, int b)
    {
        return _calculator.Add(a, b);
    }
}
```
Мы можем **не только задать поведение, но и проверить, был ли метод вызван**:
```csharp
public class MathServiceTests
{
    [Fact]
    public void Test_ComputeSum_Calls_Add()
    {
        var mockCalculator = new Mock<ICalculator>();

        // Создаём сервис, передавая в него мок-калькулятор
        var mathService = new MathService(mockCalculator.Object);

        // Вызываем метод
        mathService.ComputeSum(2, 3);

        // Проверяем, что Add(2,3) действительно был вызван
        mockCalculator.Verify(calc => calc.Add(2, 3), Times.Once);
    }
}
```
📌 **Что тут проверяем?**  
- `mockCalculator.Verify(calc => calc.Add(2, 3), Times.Once);` — проверяем, что `Add(2,3)` вызывался **ровно один раз**.  

✅ **Если метод не вызывался или вызывался с другими параметрами, тест упадёт.**  

---

## **3. Возвращаем разные значения в зависимости от параметров**
Моки можно настраивать так, чтобы они по-разному реагировали на разные аргументы:
```csharp
mockCalculator.Setup(calc => calc.Add(It.Is<int>(x => x > 0), It.IsAny<int>()))
             .Returns(100);
```
📌 **Разбираем**:  
- `It.Is<int>(x => x > 0)` — метод `Add` должен сработать, **если первый аргумент больше 0**.  
- `It.IsAny<int>()` — второй аргумент может быть **любым числом**.  
- Возвращаем `100`, если условие срабатывает.  

#### **Пример теста с разными аргументами**
```csharp
[Fact]
public void Test_Add_Different_Inputs()
{
    var mockCalculator = new Mock<ICalculator>();

    // Если первый аргумент больше 0, возвращаем 100
    mockCalculator.Setup(calc => calc.Add(It.Is<int>(x => x > 0), It.IsAny<int>())).Returns(100);

    Assert.Equal(100, mockCalculator.Object.Add(1, 5));  // ✅ Работает, потому что 1 > 0
    Assert.Equal(100, mockCalculator.Object.Add(10, 20)); // ✅ Работает, потому что 10 > 0
    Assert.NotEqual(100, mockCalculator.Object.Add(-1, 5)); // ❌ Не работает, потому что -1 не больше 0
}
```
---

## **4. Выбрасываем исключения в моке**
Допустим, наш `ICalculator` должен вызывать исключение при делении на 0:
```csharp
public interface ICalculator
{
    int Divide(int a, int b);
}
```
Настроим мок так, чтобы он выбрасывал исключение:
```csharp
var mockCalculator = new Mock<ICalculator>();

mockCalculator.Setup(calc => calc.Divide(It.IsAny<int>(), 0))
              .Throws(new DivideByZeroException());
```
📌 Теперь, если кто-то вызовет `Divide` с `0` во втором аргументе, он получит ошибку.  

#### **Проверяем, что исключение действительно выбрасывается**
```csharp
[Fact]
public void Test_Divide_By_Zero()
{
    var mockCalculator = new Mock<ICalculator>();
    mockCalculator.Setup(calc => calc.Divide(It.IsAny<int>(), 0))
                  .Throws(new DivideByZeroException());

    var calculator = mockCalculator.Object;

    Assert.Throws<DivideByZeroException>(() => calculator.Divide(10, 0));
}
```
✅ **Этот тест гарантирует, что при делении на 0 метод вызовет исключение.**

---

## **5. Callbacks: выполнение кода при вызове метода**
Если нам нужно, чтобы мок **не просто возвращал значения, а выполнял какое-то действие**, можно использовать `Callback`:
```csharp
mockCalculator.Setup(calc => calc.Add(It.IsAny<int>(), It.IsAny<int>()))
              .Callback<int, int>((x, y) => Console.WriteLine($"Add вызван с параметрами {x} и {y}"))
              .Returns(0);
```
Теперь при вызове метода `Add` будет выводиться текст.

---

## **Заключение**
Моки — мощный инструмент для написания изолированных тестов.  
Мы разобрали:  
✅ **Как задать поведение мока** (`Setup(...).Returns(...)`)  
✅ **Как проверить вызов метода** (`Verify(...)`)  
✅ **Как задать разные реакции в зависимости от параметров**  
✅ **Как выбрасывать исключения** (`Throws(...)`)  
✅ **Как выполнить код при вызове мока** (`Callback(...)`)  

**Если тебе нужны примеры сложнее — спрашивай!** 😃