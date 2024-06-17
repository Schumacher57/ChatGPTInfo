Привет!

Вот пример консольного приложения на C#, которое читает настройки из App.config, используя классы ConfigurationSection, ConfigurationElementCollection и ConfigurationElement:

### Структура проекта

1. **ConfigAppNetFramework.csproj** (файл проекта)
2. **Program.cs** (основной файл программы)
3. **App.config** (файл конфигурации)

### Шаг 1: Создание App.config

Создайте файл App.config в корне проекта и добавьте следующую структуру:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
    <configSections>
        <sectionGroup name="applicationSettings" type="System.Configuration.ApplicationSettingsGroup, System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089">
            <section name="ConfigAppNetFramework.Properties.Settings" type="System.Configuration.ClientSettingsSection, System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" requirePermission="false" />
        </sectionGroup>
    </configSections>
    <startup>
        <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.8" />
    </startup>
    <applicationSettings>
        <ConfigAppNetFramework.Properties.Settings>
            <setting name="MyParametr" serializeAs="String">
                <value />
            </setting>
            <setting name="MyArrayParametr" serializeAs="Xml">
                <value>
                    <Array>
                        <Value1>1</Value1>
                        <Value2>2</Value2>
                        <Value3>3</Value3>
                    </Array>
                </value>
            </setting>
        </ConfigAppNetFramework.Properties.Settings>
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
    public class MyArrayParametrConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("Array", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(ArrayValueCollection), AddItemName = "add")]
        public ArrayValueCollection ArrayValues
        {
            get { return (ArrayValueCollection)this["Array"]; }
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
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var section = (ClientSettingsSection)config.GetSection("applicationSettings/ConfigAppNetFramework.Properties.Settings");
            var myParametr = section.Settings.Get("MyParametr").Value.ValueXml.InnerText;

            Console.WriteLine("MyParametr: " + myParametr);

            var myArrayParametrSection = (ClientSettingsSection)config.GetSection("applicationSettings/ConfigAppNetFramework.Properties.Settings");
            var myArrayParametrXml = myArrayParametrSection.Settings.Get("MyArrayParametr").Value.ValueXml.InnerXml;

            var xmlDoc = new System.Xml.XmlDocument();
            xmlDoc.LoadXml(myArrayParametrXml);

            foreach (System.Xml.XmlNode node in xmlDoc.DocumentElement.ChildNodes)
            {
                Console.WriteLine($"{node.Name}: {node.InnerText}");
            }

            Console.ReadLine();
        }
    }
}
```

### Шаг 4: Настройка проекта

Убедитесь, что ваш проект настроен на использование .NET Framework 4.8.

### Пояснение

1. **App.config**: Определяет конфигурационные параметры, включая секцию с именем `MyArrayParametr`.
2. **ConfigSettings.cs**: Содержит классы для представления конфигурационной секции и её элементов.
3. **Program.cs**: Основная программа, которая считывает и выводит значения конфигурационных параметров.

### Запуск

Соберите и запустите проект. Программа выведет значения `Value1`, `Value2` и `Value3` из секции `MyArrayParametr`.

Надеюсь, это поможет! Если будут вопросы, обращайтесь.