Для того чтобы прочитать настройки из вашего `app.config` файла с использованием `ConfigurationSection`, вам нужно выполнить следующие шаги:

1. **Создание класса настроек:**
   Создайте класс, который будет представлять структуру вашей секции настроек. В вашем случае, это будет класс `ServerSection`, который будет содержать список серверов.

```csharp
using System.Configuration;

namespace YourNamespace
{
    public class ServerSection : ConfigurationSection
    {
        [ConfigurationProperty("servers")]
        public ServerCollection Servers
        {
            get { return (ServerCollection)this["servers"]; }
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
            return ((ServerElement)element).Name;
        }
    }

    public class ServerElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return (string)this["name"]; }
        }

        [ConfigurationProperty("ip", IsRequired = true)]
        public string IP
        {
            get { return (string)this["ip"]; }
        }
    }
}
```

2. **Изменение секции настроек в файле app.config:**
   В вашем файле `app.config` замените объявление секции `serverSection` следующим образом:

```xml
<section name="serverSection" type="YourNamespace.ServerSection, YourAssemblyName" />
```

3. **Чтение настроек в коде:**
   Теперь вы можете прочитать настройки из вашего `app.config` файла, используя созданный класс `ServerSection`:

```csharp
var serverConfig = ConfigurationManager.GetSection("serverSection") as ServerSection;

if (serverConfig != null)
{
    foreach (ServerElement server in serverConfig.Servers)
    {
        Console.WriteLine($"Server Name: {server.Name}, IP: {server.IP}");
    }
}
```

Таким образом, с помощью подхода с `ConfigurationSection`, вы можете удобно читать настройки из вашего конфигурационного файла в структурированной форме. 🛠️ ⚙️
