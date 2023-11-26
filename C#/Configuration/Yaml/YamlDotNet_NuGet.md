## Чтение файла формата YAML ##
Чтение файла формата YAML вместо JSON требует некоторых изменений в вашем коде. Вам понадобится использовать пакет NuGet, который обеспечивает поддержку YAML в конфигурации.

Вот как вы можете прочитать файл формата YAML (`AppConfig.yml`) вместо файла формата JSON:

1. Установите пакет NuGet `YamlDotNet` в ваш проект. Вы можете сделать это через менеджер пакетов NuGet или добавить ссылку на пакет в файле `csproj` вашего проекта.

2. Включите необходимые пространства имен:
```csharp
using Microsoft.Extensions.Configuration;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.RepresentationModel;
```

3. Используйте следующий код для чтения файла формата YAML (`AppConfig.yml`):
```csharp
var deserializer = new DeserializerBuilder()
    .WithNamingConvention(CamelCaseNamingConvention.Instance)
    .Build();

var fileContents = File.ReadAllText("AppConfig.yml");

var yamlStream = new YamlStream();
yamlStream.Load(new StringReader(fileContents));

var mappingNode = (YamlMappingNode)yamlStream.Documents[0].RootNode;

var dictionary = deserializer.Deserialize<Dictionary<string, object>>(mappingNode);

var configuration = new ConfigurationBuilder()
    .AddInMemoryCollection(dictionary)
    .Build();
```

В этом коде мы используем библиотеку `YamlDotNet` для десериализации содержимого файла YAML в словарь. Затем мы используем `AddInMemoryCollection` для добавления словаря в построитель конфигурации.

Обратите внимание, что `AddInMemoryCollection` добавляет данные в память, поэтому если вам также нужно использовать другие источники конфигурации, вы можете использовать соответствующие методы `Add...` (например, `AddJsonFile`, `AddEnvironmentVariables`).

После добавления указанного кода вы сможете использовать `configuration` для доступа к значениям из файла `AppConfig.yml`, аналогично тому, как вы это делали ранее с файлом формата JSON.
