В C# можно использовать классы ConfigurationSection, ConfigurationElementCollection и ConfigurationElement для работы с пользовательскими настройками конфигурации в приложениях. Давайте рассмотрим, как создать и использовать пользовательские настройки конфигурации в простом консольном приложении.

### Шаг 1: Создание модели конфигурации

#### Определение ConfigurationElement

ConfigurationElement представляет собой один элемент конфигурации. Создадим класс, который будет представлять отдельный элемент.

```csharp
public class ServerElement : ConfigurationElement
{
    [ConfigurationProperty("name", IsRequired = true)]
    public string Name
    {
        get { return (string)this["name"]; }
        set { this["name"] = value; }
    }

    [ConfigurationProperty("ip", IsRequired = true)]
    public string IP
    {
        get { return (string)this["ip"]; }
        set { this["ip"] = value; }
    }
}
```

#### Определение ConfigurationElementCollection

ConfigurationElementCollection представляет коллекцию элементов конфигурации. Создадим класс, который будет содержать коллекцию элементов.

```csharp
[ConfigurationCollection(typeof(ServerElement))]
public class ServerElementCollection : ConfigurationElementCollection
{
    protected override ConfigurationElement CreateNewElement()
    {
        return new ServerElement();
    }

    protected override object GetElementKey(ConfigurationElement element)
    {
        return ((ServerElement)element).Name;
    }

    public ServerElement this[int index]
    {
        get { return (ServerElement)BaseGet(index); }
    }
}
```

#### Определение ConfigurationSection

ConfigurationSection представляет секцию конфигурации. Создадим класс, который будет представлять секцию конфигурации и содержать коллекцию элементов.

```csharp
public class ServerSection : ConfigurationSection
{
    [ConfigurationProperty("servers", IsDefaultCollection = false)]
    public ServerElementCollection Servers
    {
        get { return (ServerElementCollection)this["servers"]; }
        set { this["servers"] = value; }
    }
}
```

### Шаг 2: Настройка конфигурационного файла

Теперь добавим секцию в файл конфигурации `App.config`.

```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
  <configSections>
    <section name="serverSection" type="YourNamespace.ServerSection, YourAssemblyName" />
  </configSections>

  <serverSection>
    <servers>
      <server name="Server1" ip="192.168.1.1" />
      <server name="Server2" ip="192.168.1.2" />
    </servers>
  </serverSection>
</configuration>
```

Замените `YourNamespace` и `YourAssemblyName` на фактические значения для вашего проекта.

### Шаг 3: Чтение конфигурации в коде

Теперь прочитаем эту конфигурацию в нашем консольном приложении.

```csharp
using System;
using System.Configuration;

namespace YourNamespace
{
    class Program
    {
        static void Main(string[] args)
        {
            ServerSection serverSection = ConfigurationManager.GetSection("serverSection") as ServerSection;

            if (serverSection != null)
            {
                foreach (ServerElement server in serverSection.Servers)
                {
                    Console.WriteLine($"Name: {server.Name}, IP: {server.IP}");
                }
            }
            else
            {
                Console.WriteLine("Failed to load configuration section.");
            }
        }
    }
}
```

### Подведение итогов

1. **Определили классы для модели конфигурации**:
   - `ServerElement` для отдельных элементов.
   - `ServerElementCollection` для коллекции элементов.
   - `ServerSection` для секции конфигурации.

2. **Добавили секцию конфигурации в `App.config`**.

3. **Прочитали и использовали конфигурацию в коде консольного приложения**.

Этот пример демонстрирует основные принципы работы с пользовательскими настройками конфигурации в C#. Вы можете усложнять модель конфигурации по мере необходимости, добавляя дополнительные свойства и элементы.