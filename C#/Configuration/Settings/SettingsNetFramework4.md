Да, конечно! Чтобы использовать настройки через `Properties.Settings.Default`, нам нужно воспользоваться механизмом автоматической генерации классов для настроек, доступных в Visual Studio.

### Шаг 1: Обновление App.config

Обновим файл App.config, чтобы включить раздел настроек:

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
            <setting name="Value1" serializeAs="String">
                <value>1</value>
            </setting>
            <setting name="Value2" serializeAs="String">
                <value>2</value>
            </setting>
            <setting name="Value3" serializeAs="String">
                <value>3</value>
            </setting>
        </ConfigAppNetFramework.Properties.Settings>
    </applicationSettings>
</configuration>
```

### Шаг 2: Настройка файла Settings.settings

1. Откройте проект в Visual Studio.
2. Перейдите в **Project** -> **Properties**.
3. Перейдите на вкладку **Settings**.
4. Добавьте три новых параметра: `Value1`, `Value2`, и `Value3`, как строки (string), и задайте их значения согласно App.config (1, 2 и 3 соответственно).

Это создаст `Settings.settings` файл и автоматически сгенерирует `Settings.Designer.cs` файл.

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
            var value1 = Properties.Settings.Default.Value1;
            var value2 = Properties.Settings.Default.Value2;
            var value3 = Properties.Settings.Default.Value3;

            Console.WriteLine($"Value1: {value1}");
            Console.WriteLine($"Value2: {value2}");
            Console.WriteLine($"Value3: {value3}");

            Console.ReadLine();
        }
    }
}
```

### Запуск

Соберите и запустите проект. Программа выведет значения `Value1`, `Value2` и `Value3` из конфигурационных параметров, используя `Properties.Settings.Default`.

### Пояснение

1. **App.config**: Определяет конфигурационные параметры, включая секцию `ConfigAppNetFramework.Properties.Settings` с параметрами `Value1`, `Value2` и `Value3`.
2. **Settings.settings**: Создает и управляет настройками, доступными через `Properties.Settings.Default`.
3. **Program.cs**: Основная программа, которая считывает и выводит значения конфигурационных параметров через `Properties.Settings.Default`.

Если будут вопросы или потребуется дополнительная помощь, обращайтесь!