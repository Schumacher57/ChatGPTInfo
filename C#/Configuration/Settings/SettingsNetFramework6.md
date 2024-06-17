Да, мы можем использовать этот класс, чтобы считывать настройки из файла конфигурации. Придется внести некоторые изменения в ваш класс и файл конфигурации, чтобы все работало правильно.

### Шаг 1: Обновление App.config

Обновите файл App.config, чтобы включить секцию `MySection1` с коллекцией значений:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
    <configSections>
        <sectionGroup name="applicationSettings" type="System.Configuration.ApplicationSettingsGroup, System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089">
            <section name="ConfigAppNetFramework.Properties.Settings" type="System.Configuration.ClientSettingsSection, System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" requirePermission="false" />
            <section name="MySection1" type="ConfigAppNetFramework.MySection1, ConfigAppNetFramework" requirePermission="false" />
        </sectionGroup>
    </configSections>
    <startup>
        <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.8" />
    </startup>
    <applicationSettings>
        <ConfigAppNetFramework.Properties.Settings>
            <setting name="MyParametr" serializeAs="String">
                <value>123</value>
            </setting>
        </ConfigAppNetFramework.Properties.Settings>
        <MySection1>
            <servers>
                <add Value1="1" Value2="2" Value3="3" />
            </servers>
        </MySection1>
    </applicationSettings>
</configuration>
```

### Шаг 2: Обновление классов для чтения конфигурации

Создайте или обновите файл `ConfigSettings.cs`, чтобы включить корректные определения классов:

```csharp
using System;
using System.Configuration;
using System.ComponentModel;

namespace ConfigAppNetFramework
{
    public class MySection1 : ConfigurationSection
    {
        [ConfigurationProperty("servers", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(ServerCollection), AddItemName = "add")]
        public ServerCollection Servers
        {
            get { return (ServerCollection)this["servers"]; }
            set { this["servers"] = value; }
        }
    }

    public class ServerCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ServerElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ServerElement)element).Value1;
        }
    }

    public class ServerElement : ConfigurationElement
    {
        [ConfigurationProperty("Value1", IsRequired = true)]
        public string Value1
        {
            get { return (string)this["Value1"]; }
            set { this["Value1"] = value; }
        }

        [ConfigurationProperty("Value2", IsRequired = true)]
        public string Value2
        {
            get { return (string)this["Value2"]; }
            set { this["Value2"] = value; }
        }

        [ConfigurationProperty("Value3", IsRequired = true)]
        public string Value3
        {
            get { return (string)this["Value3"]; }
            set { this["Value3"] = value; }
        }
    }
}
```

### Шаг 3: Основная программа

Создайте или обновите файл `Program.cs`:

```csharp
using System;
using System.Configuration;

namespace ConfigAppNetFramework
{
    class Program
    {
        static void Main(string[] args)
        {
            MySection1 mySection = (MySection1)ConfigurationManager.GetSection("applicationSettings/MySection1");

            if (mySection != null)
            {
                foreach (ServerElement element in mySection.Servers)
                {
                    Console.WriteLine($"Value1: {element.Value1}");
                    Console.WriteLine($"Value2: {element.Value2}");
                    Console.WriteLine($"Value3: {element.Value3}");
                }
            }
            else
            {
                Console.WriteLine("Section not found.");
            }

            Console.ReadLine();
        }
    }
}
```

### Пояснение

1. **App.config**: Обновлен, чтобы включить секцию `MySection1` с коллекцией значений `servers`.
2. **ConfigSettings.cs**: Обновлен, чтобы включать классы `MySection1`, `ServerCollection` и `ServerElement` для представления конфигурационной секции и её элементов.
3. **Program.cs**: Основная программа, которая считывает и выводит значения конфигурационных параметров из секции `MySection1`.

### Запуск

Соберите и запустите проект. Программа выведет значения `Value1`, `Value2` и `Value3` из секции `MySection1`.

Если будут вопросы или потребуется дополнительная помощь, обращайтесь!