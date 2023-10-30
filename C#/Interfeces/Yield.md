## Список
1. [Подробный пример работы с `IEnumerable<T>` и использованием ключевого слова `yield` в C#](#IenumirableExample)
2. [Усложнённый пример с использованием ключевого слова `yield` в C#](#HardExample)

### 1. Подробный пример работы с `IEnumerable<T>` и использованием ключевого слова `yield` в C#: <a name="IenumirableExample"></a> ###

---

Конечно! Вот подробный пример работы с `IEnumerable<T>` и использованием ключевого слова `yield` в C#:

```csharp
using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        // Пример 1: Простая реализация с использованием yield
        IEnumerable<int> countToFive = CountToFive();

        Console.WriteLine("Пример 1: Простая реализация с использованием yield");
        foreach (int number in countToFive)
        {
            Console.WriteLine(number);
        }
        Console.WriteLine();

        // Пример 2: Условная логика в yield
        IEnumerable<int> evenNumbers = GetEvenNumbers(10);

        Console.WriteLine("Пример 2: Условная логика в yield");
        foreach (int number in evenNumbers)
        {
            Console.WriteLine(number);
        }
        Console.WriteLine();

        // Пример 3: Использование yield с объектами
        IEnumerable<Person> people = GetPeople();

        Console.WriteLine("Пример 3: Использование yield с объектами");
        foreach (Person person in people)
        {
            Console.WriteLine($"{person.Name} ({person.Age} years old)");
        }
    }

    // Пример 1: Простая реализация с использованием yield
    static IEnumerable<int> CountToFive()
    {
        yield return 1;
        yield return 2;
        yield return 3;
        yield return 4;
        yield return 5;
    }

    // Пример 2: Условная логика в yield
    static IEnumerable<int> GetEvenNumbers(int count)
    {
        for (int i = 0; i < count; i++)
        {
            if (i % 2 == 0)
            {
                yield return i;
            }
        }
    }

    // Пример 3: Использование yield с объектами
    static IEnumerable<Person> GetPeople()
    {
        yield return new Person { Name = "John", Age = 25 };
        yield return new Person { Name = "Jane", Age = 30 };
        yield return new Person { Name = "Alex", Age = 40 };
    }

    // Пример класса Person
    class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
```

Этот пример демонстрирует использование `yield` вместе с `IEnumerable<T>`:

1. Пример 1 показывает простую реализацию `IEnumerable<T>` с использованием `yield` для возврата значений от 1 до 5.
2. Пример 2 демонстрирует условную логику в `yield`, где только четные числа возвращаются из метода.
3. Пример 3 показывает использование `yield` с объектами. В этом примере метод `GetPeople` возвращает серию объектов типа `Person`.

В каждом из примеров `yield` используется для "ленивого" генерирования элементов по мере необходимости. Это позволяет эффективно работать с большими наборами данных или потенциально бесконечными последовательностями.

Надеюсь, это поможет вам понять, как использовать `yield` вместе с `IEnumerable<T>` в C#! 😊

---

### 2. Усложнённый пример с использованием ключевого слова `yield` в C#: <a name="HardExample"></a> ###

Конечно, я могу показать вам более сложный пример использования `yield` в C#. Рассмотрим следующий пример:

```csharp
using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        IEnumerable<int> fibonacciSequence = GetFibonacciSequence(10);

        Console.WriteLine("Fibonacci Sequence:");
        foreach (int number in fibonacciSequence)
        {
            Console.WriteLine(number);
        }
    }

    static IEnumerable<int> GetFibonacciSequence(int count)
    {
        if (count <= 0)
        {
            yield break;
        }

        int current = 0;
        int next = 1;

        for (int i = 0; i < count; i++)
        {
            yield return current;

            int temp = current;
            current = next;
            next = temp + next;
        }
    }
}
```

В этом примере используется `yield` для генерации последовательности чисел Фибоначчи. Вместо того, чтобы вычислять все числа сразу и хранить их в памяти, мы используем `yield return`, чтобы возвращать числа по мере необходимости.

Здесь есть некоторые особенности этого примера:

- Метод `GetFibonacciSequence` возвращает `IEnumerable<int>`, что позволяет использовать его в цикле `foreach`.
- Если `count` меньше или равен нулю, мы используем `yield break`, чтобы немедленно выйти из последовательности. Это обеспечивает обработку случаев, когда загружать последовательность Фибоначчи не нужно.
- Внутри цикла `for` мы используем `yield return`, чтобы вернуть текущее число каждый раз, когда итерируемся по последовательности Фибоначчи.
- Цикл `foreach` позволяет нам итерироваться по значениям, генерируемым с помощью `yield return`, без явного управления индексами или состоянием.

В результате, когда мы вызываем `GetFibonacciSequence(10)`, получаем последовательность из 10 чисел Фибоначчи и выводим их на консоль.

Этот пример показывает мощь и гибкость `yield` в C#, когда мы хотим генерировать последовательности значений по требованию без лишней нагрузки на память.

✨🐇💫 
