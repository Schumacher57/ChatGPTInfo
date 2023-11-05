Конечно! Вот пример использования `Microsoft.Extensions.DependencyInjection` для регистрации и внедрения зависимостей:

```csharp
using Microsoft.Extensions.DependencyInjection;

// Создаем контейнер сервисов
var serviceProvider = new ServiceCollection()
    .AddTransient<ISomeInterfaces, SomeImplementation1>() // Регистрируем первую реализацию
    .AddTransient<ISomeInterfaces, SomeImplementation2>() // Регистрируем вторую реализацию
    .AddTransient<ISomeInterfaces, SomeImplementation3>() // Регистрируем третью реализацию
    .BuildServiceProvider();

// Получаем нужную реализацию через DI
var implementation1 = serviceProvider.GetService<ISomeInterfaces>();
var implementation2 = serviceProvider.GetService<ISomeInterfaces>();
var implementation3 = serviceProvider.GetService<ISomeInterfaces>();
```

В данном примере мы создаем `ServiceCollection`, регистрируем различные реализации интерфейса `ISomeInterfaces` с помощью метода `AddTransient`, который указывает на создание нового экземпляра при каждом запросе. Затем мы вызываем метод `BuildServiceProvider()` для создания провайдера служб.

При помощи метода `GetService<>()` мы можем получить нужную реализацию из контейнера.

Обратите внимание, что код предоставлен для ознакомления и может потребовать настройки и адаптации в зависимости от ваших потребностей и структуры проекта.

### Example2 ###

Извините за путаницу. Чтобы выбрать реализацию ISomeInterfaces через Dependency Injection (DI) контейнер, вам потребуется настроить контейнер для внедрения зависимостей. Вот как это можно сделать на примере популярного DI контейнера, такого как Unity или Autofac:

Unity:
```csharp
var container = new UnityContainer();

// Регистрируем различные реализации интерфейса
container.RegisterType<ISomeInterfaces, SomeImplementation1>("Implementation1");
container.RegisterType<ISomeInterfaces, SomeImplementation2>("Implementation2");
container.RegisterType<ISomeInterfaces, SomeImplementation3>("Implementation3");

// Получаем нужную реализацию из контейнера
var implementation1 = container.Resolve<ISomeInterfaces>("Implementation1");
var implementation2 = container.Resolve<ISomeInterfaces>("Implementation2");
var implementation3 = container.Resolve<ISomeInterfaces>("Implementation3");
```

Autofac:
```csharp
var builder = new ContainerBuilder();

// Регистрируем различные реализации интерфейса
builder.RegisterType<SomeImplementation1>().As<ISomeInterfaces>().Named<ISomeInterfaces>("Implementation1");
builder.RegisterType<SomeImplementation2>().As<ISomeInterfaces>().Named<ISomeInterfaces>("Implementation2");
builder.RegisterType<SomeImplementation3>().As<ISomeInterfaces>().Named<ISomeInterfaces>("Implementation3");

// Строим контейнер
var container = builder.Build();

// Получаем нужную реализацию из контейнера
var implementation1 = container.ResolveNamed<ISomeInterfaces>("Implementation1");
var implementation2 = container.ResolveNamed<ISomeInterfaces>("Implementation2");
var implementation3 = container.ResolveNamed<ISomeInterfaces>("Implementation3");
```

В данных примерах мы зарегистрировали различные реализации интерфейса ISomeInterfaces с разными именами ("Implementation1", "Implementation2", "Implementation3"). Затем, при помощи методов `Resolve` или `ResolveNamed`, мы получаем нужную реализацию из контейнера.

Пожалуйста, обратите внимание, что код предоставлен для ознакомления и может потребовать настройки и адаптации в соответствии с выбранным вами DI контейнером.
