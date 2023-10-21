CancellationToken в C# является механизмом для отмены операции. Он используется для нежелательного прерывания длительных или асинхронных задач. CancellationToken предоставляет возможность уведомления других частей кода о необходимости отмены операции и позволяет им выполнять соответствующие действия.

Вот пример использования CancellationToken:

```csharp
using System;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;

        // Запуск асинхронной задачи с использованием CancellationToken
        Task longRunningTask = LongRunningOperation(cancellationToken);

        // Ждем некоторое время (например, 3 секунды)
        await Task.Delay(TimeSpan.FromSeconds(3));

        // Отменяем операцию, вызывая Cancel() на CancellationTokenSource
        cancellationTokenSource.Cancel();

        try
        {
            // Ждем завершения задачи
            await longRunningTask;
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Операция была отменена.");
        }

        Console.WriteLine("Программа завершена.");
    }

    static async Task LongRunningOperation(CancellationToken cancellationToken)
    {
        while (true)
        {
            // Проверяем, была ли запрошена отмена операции
            if (cancellationToken.IsCancellationRequested)
            {
                // Выполняем действия по очистке или завершению операции
                // ...

                cancellationToken.ThrowIfCancellationRequested();
            }

            // Делаем работу на протяжении продолжительного времени
            await Task.Delay(TimeSpan.FromSeconds(1));
        }
    }
}
```

В приведенном выше примере создается CancellationTokenSource, который предоставляет CancellationToken. Затем асинхронная задача LongRunningOperation() запускается с использованием этого CancellationToken. Через некоторое время (3 секунды) операция отменяется, вызывая метод Cancel() на CancellationTokenSource. Задача перехватывает исключение OperationCanceledException и выполняет необходимые действия при отмене операции.
