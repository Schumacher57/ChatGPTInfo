Конечно! Давайте создадим класс `MySection1`, который будет использовать `ConfigurationSection`, `ConfigurationElementCollection` и `ConfigurationElement` для хранения значений `Value1`, `Value2` и `Value3`.

### Шаг 1: Обновление App.config

Убедитесь, что ваш файл App.config имеет следующую структуру:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
    <configSections>
        <sectionGroup name="applicationSettings" type="System.Configuration.ApplicationSettingsGroup, System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089">
            <section name="ConfigSettingFrameWork.Properties.Settings" type="System.Configuration.ClientSettingsSection, System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" requirePermission="false" />
            <section name="asdasd" type="System.Configuration.ClientSettingsSection, System, Version=4.0.0.0" requirePermission="false" />
            <section name="MySection1" type="ConfigAppNetFramework.MySection1, ConfigAppNetFramework" requirePermission="false" />
        </sectionGroup>
    </configSections>
    <startup>
        <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.8" />
    </startup>
    <applicationSettings>
        <asdasd>
            <setting name="MyParametr" serializeAs="String">
                <value>123</value>
            </setting>
        </asdasd>
        <MySection1>
            <arrayValues>
                <add name="Value1" value="1" />
                <add name="Value2" value="2" />
                <add name="Value3" value="3" />
            </arrayValues>
        </MySection1>
    </applicationSettings>
</configuration>
```

### Шаг 2: Создание классов для чтения конфигурации

Создайте файл `ConfigSettings.cs` и добавьте следующие классы:

```csharp
using System;
using System.Configuration;

namespace ConfigAppNetFramework
{
    public class MySection1 : ConfigurationSection
    {
        [ConfigurationProperty("arrayValues")]
        public ArrayValueCollection ArrayValues
        {
            get { return (ArrayValueCollection)this["arrayValues"]; }
            set { this["arrayValues"] = value; }
        }
    }

    [ConfigurationCollection(typeof(ArrayValueElement), AddItemName = "add")]
    public class ArrayValueCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ArrayValueElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ArrayValueElement)element).Name;
        }

        public ArrayValueElement this[int index]
        {
            get { return (ArrayValueElement)BaseGet(index); }
        }
    }

    public class ArrayValueElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("value", IsRequired = true)]
        public string Value
        {
            get { return (string)this["value"]; }
            set { this["value"] = value; }
        }
    }
}
```

### Шаг 3: Основная программа

Создайте файл `Program.cs` и добавьте следующий код:

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
                foreach (ArrayValueElement element in mySection.ArrayValues)
                {
                    Console.WriteLine($"{element.Name}: {element.Value}");
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

### Шаг 4: Настройка проекта

Убедитесь, что ваш проект настроен на использование .NET Framework 4.8.

### Пояснение

1. **App.config**: Определяет конфигурационные параметры, включая секцию с именем `MySection1`, которая содержит коллекцию значений.
2. **ConfigSettings.cs**: Содержит классы `MySection1`, `ArrayValueCollection` и `ArrayValueElement` для представления конфигурационной секции и её элементов.
3. **Program.cs**: Основная программа, которая считывает и выводит значения конфигурационных параметров из секции `MySection1`.

### Запуск

Соберите и запустите проект. Программа выведет значения `Value1`, `Value2` и `Value3` из секции `MySection1`.

Если будут вопросы или потребуется дополнительная помощь, обращайтесь!