Конечно! Для этого мы изменим подход, чтобы не использовать XML сериализаторы. Мы будем работать с конфигурацией напрямую через `ConfigurationSection`, `ConfigurationElement`, и `ConfigurationElementCollection`, как было показано ранее. Но мы также создадим свойство, аналогичное автоматически генерируемым, для доступа к настройкам.

### Шаг 1: Обновление App.config

Убедитесь, что ваш файл App.config имеет следующую структуру:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
    <configSections>
        <sectionGroup name="applicationSettings" type="System.Configuration.ApplicationSettingsGroup, System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089">
            <section name="ConfigAppNetFramework.Properties.Settings" type="System.Configuration.ClientSettingsSection, System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" requirePermission="false" />
            <section name="MyArrayParametrSection" type="ConfigAppNetFramework.MyArrayParametrSection, ConfigAppNetFramework" requirePermission="false" />
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
    <MyArrayParametrSection>
        <SomeParametrString1>Value1</SomeParametrString1>
        <SomeParametrInt2>1</SomeParametrInt2>
        <SomeParametrBool3>true</SomeParametrBool3>
    </MyArrayParametrSection>
</configuration>
```

### Шаг 2: Создание классов для чтения конфигурации

Создайте файл `ConfigSettings.cs` и добавьте следующие классы:

```csharp
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

    public sealed partial class Settings : ApplicationSettingsBase
    {
        private static readonly Settings defaultInstance = (Settings)Synchronized(new Settings());

        public static Settings Default
        {
            get { return defaultInstance; }
        }

        [ApplicationScopedSetting()]
        public MyArrayParametrSection MyArrayParametr
        {
            get
            {
                return (MyArrayParametrSection)ConfigurationManager.GetSection("MyArrayParametrSection");
            }
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
            var settings = Settings.Default.MyArrayParametr;

            if (settings != null)
            {
                Console.WriteLine($"SomeParametrString1: {settings.SomeParametrString1}");
                Console.WriteLine($"SomeParametrInt2: {settings.SomeParametrInt2}");
                Console.WriteLine($"SomeParametrBool3: {settings.SomeParametrBool3}");
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

1. **App.config**: Обновлен, чтобы включить секцию `MyArrayParametrSection` с параметрами `SomeParametrString1`, `SomeParametrInt2` и `SomeParametrBool3`.
2. **ConfigSettings.cs**: Включает классы `MyArrayParametrSection` и `Settings`. Класс `MyArrayParametrSection` представляет конфигурационные параметры и их свойства. Класс `Settings` используется для доступа к настройкам через свойство `MyArrayParametr`.
3. **Program.cs**: Основная программа использует свойство `MyArrayParametr` из класса `Settings` для доступа к настройкам и вывода их значений.

Теперь, при запуске программы, она будет считывать и выводить значения параметров из конфигурационной секции `MyArrayParametrSection`, используя свойство `MyArrayParametr` в классе `Settings`.