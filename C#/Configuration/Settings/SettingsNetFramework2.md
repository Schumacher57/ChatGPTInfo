Для чтения значений `Value1`, `Value2` и `Value3` из файла настроек, используя классы `ConfigurationSection`, `ConfigurationElementCollection` и `ConfigurationElement`, создадим специализированные классы для представления структуры конфигурации. Затем прочитаем и выведем эти значения в консольном приложении.

### Шаг 1: Создание App.config

Создайте файл App.config в корне проекта и добавьте следующую структуру:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
    <configSections>
        <sectionGroup name="applicationSettings" type="System.Configuration.ApplicationSettingsGroup, System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089">
            <section name="ConfigSettingFrameWork.Properties.Settings" type="System.Configuration.ClientSettingsSection, System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" requirePermission="false" />
            <section name="asdasd" type="System.Configuration.ClientSettingsSection, System, Version=4.0.0.0" requirePermission="false" />
            <section name="MySection1" type="ConfigAppNetFramework.MySection, ConfigAppNetFramework" requirePermission="false" />
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
            <Value1>1</Value1>
            <Value2>2</Value2>
            <Value3>3</Value3>
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
    public class MySection : ConfigurationSection
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
            MySection mySection = (MySection)ConfigurationManager.GetSection("applicationSettings/MySection1");

            if (mySection != null)
            {
                Console.WriteLine("Value1: " + mySection.Value1);
                Console.WriteLine("Value2: " + mySection.Value2);
                Console.WriteLine("Value3: " + mySection.Value3);
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

1. **App.config**: Определяет конфигурационные параметры, включая секцию с именем `MySection1`.
2. **ConfigSettings.cs**: Содержит класс `MySection` для представления конфигурационной секции и её элементов.
3. **Program.cs**: Основная программа, которая считывает и выводит значения конфигурационных параметров из секции `MySection1`.

### Запуск

Соберите и запустите проект. Программа выведет значения `Value1`, `Value2` и `Value3` из секции `MySection1`.

Если будут вопросы или потребуется дополнительная помощь, обращайтесь!