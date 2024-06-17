Конечно, мы можем считать настройки с использованием классов `ConfigurationSection`, `ConfigurationElement`, и `ConfigurationElementCollection`. Вот пример, как это можно сделать.

### Шаг 1: Обновление App.config

Убедитесь, что ваш файл App.config имеет следующую структуру:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
    <configSections>
        <sectionGroup name="applicationSettings" type="System.Configuration.ApplicationSettingsGroup, System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089">
            <section name="ConfigAppNetFramework.Properties.Settings" type="System.Configuration.ClientSettingsSection, System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" requirePermission="false" />
            <section name="MyArrayParametr" type="ConfigAppNetFramework.MyArrayParametrSection, ConfigAppNetFramework" requirePermission="false" />
        </sectionGroup>
    </configSections>
    <startup>
        <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.8" />
    </startup>
    <applicationSettings>
        <ConfigAppNetFramework.Properties.Settings>
            <setting name="MyParametr" serializeAs="String">
                <value>SomeValue</value>
            </setting>
        </ConfigAppNetFramework.Properties.Settings>
    </applicationSettings>
    <MyArrayParametr>
        <SomeParametrString1>Value1</SomeParametrString1>
        <SomeParametrInt2>1</SomeParametrInt2>
        <SomeParametrBool3>true</SomeParametrBool3>
    </MyArrayParametr>
</configuration>
```

### Шаг 2: Создание классов для чтения конфигурации

Создайте файл `ConfigSettings.cs` и добавьте следующие классы:

```csharp
using System;
using System.Configuration;

namespace ConfigAppNetFramework
{
    public class MyArrayParametrSection : ConfigurationSection
    {
        [ConfigurationProperty("SomeParametrString1", IsRequired = true)]
        public string SomeParametrString1
        {
            get { return (string)this["SomeParametrString1"]; }
            set { this["SomeParametrString1"] = value; }
        }

        [ConfigurationProperty("SomeParametrInt2", IsRequired = true)]
        public int SomeParametrInt2
        {
            get { return (int)this["SomeParametrInt2"]; }
            set { this["SomeParametrInt2"] = value; }
        }

        [ConfigurationProperty("SomeParametrBool3", IsRequired = true)]
        public bool SomeParametrBool3
        {
            get { return (bool)this["SomeParametrBool3"]; }
            set { this["SomeParametrBool3"] = value; }
        }
    }
}
```

### Шаг 3: Основная программа

Создайте или обновите файл `Program.cs`:

```csharp
using System;

namespace ConfigAppNetFramework
{
    class Program
    {
        static void Main(string[] args)
        {
            MyArrayParametrSection myArrayParametr = (MyArrayParametrSection)ConfigurationManager.GetSection("MyArrayParametr");

            if (myArrayParametr != null)
            {
                Console.WriteLine($"SomeParametrString1: {myArrayParametr.SomeParametrString1}");
                Console.WriteLine($"SomeParametrInt2: {myArrayParametr.SomeParametrInt2}");
                Console.WriteLine($"SomeParametrBool3: {myArrayParametr.SomeParametrBool3}");
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

1. **App.config**: Обновлен, чтобы включить секцию `MyArrayParametr` с параметрами `SomeParametrString1`, `SomeParametrInt2` и `SomeParametrBool3`.
2. **ConfigSettings.cs**: Включает класс `MyArrayParametrSection`, который представляет конфигурационные параметры и их свойства.
3. **Program.cs**: Основная программа, которая считывает и выводит значения конфигурационных параметров из секции `MyArrayParametr`.

### Запуск

Соберите и запустите проект. Программа выведет значения `SomeParametrString1`, `SomeParametrInt2` и `SomeParametrBool3` из конфигурации.

Если будут вопросы или потребуется дополнительная помощь, обращайтесь!