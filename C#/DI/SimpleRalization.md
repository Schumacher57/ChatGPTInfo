Конечно! Вот простой и подробный пример использования метода `AddTransient` для регистрации зависимости в `ServiceCollection`:

1. Создайте интерфейс `ISomeInterface`:

```csharp
public interface ISomeInterface
{
    void DoSomething();
}
```

2. Создайте класс `SomeRealization`, который реализует `ISomeInterface`:

```csharp
public class SomeRealization : ISomeInterface
{
    public void DoSomething()
    {
        Console.WriteLine("Doing something...");
    }
}
```

3. В методе `Main` вашего приложения или любом другом месте, где вы хотите использовать внедрение зависимостей, добавьте следующий код:

```csharp
using Microsoft.Extensions.DependencyInjection;
using System;

class Program
{
    static void Main(string[] args)
    {
        // Создание контейнера сервисов
        var services = new ServiceCollection();

        // Регистрация зависимости ISomeInterface -> SomeRealization
        services.AddTransient<ISomeInterface, SomeRealization>();

        // Построение провайдера сервисов
        var serviceProvider = services.BuildServiceProvider();

        // Получение экземпляра зависимости
        var someInterface = serviceProvider.GetService<ISomeInterface>();

        // Использование зависимости
        someInterface.DoSomething();

        // Ожидание нажатия клавиши, чтобы консольное окно не закрылось сразу
        Console.ReadKey();
    }
}
```

В этом примере мы создаем контейнер сервисов (`ServiceCollection`), регистрируем зависимость между интерфейсом `ISomeInterface` и его реализацией `SomeRealization` с помощью метода `AddTransient`. Затем мы строим провайдер сервисов (`serviceProvider`) и используем его для получения экземпляра `ISomeInterface`. Наконец, мы вызываем метод `DoSomething` на экземпляре зависимости.

Обратите внимание, что в примере мы добавили директивы `using` для `Microsoft.Extensions.DependencyInjection` и `System`, чтобы использовать соответствующие классы и пространства имен.

После выполнения этого кода в консоль будет выведено "Doing something...".

Надеюсь, этот пример поможет вам лучше понять использование метода `AddTransient` и внедрение зависимостей в C#! 😊
