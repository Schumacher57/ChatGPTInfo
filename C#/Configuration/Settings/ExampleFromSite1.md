## [Go to the site](https://teonotebook.wordpress.com/2019/08/10/custom-configuration-section-and-collection-in-net/)

### Exmaple 1
```xml
<configSections>
  <section  name="Location" type="System.Configuration.NameValueSectionHandler" />
</configSections>
```
### Exmaple 2
```xml
<configSections>
  <section name="Person" type="ConfigDemo.PersonConfigurationSection, ConfigDemo "/>
</configSections>
```

### Common Exaple:
```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
 
  <!-- NOTE: configSections tag must be the first tag after the root -->
  <!-- NOTE: a section must appear just once per config file, or an exception is thrown -->
  <configSections>
    <!-- for 2 -->
    <section name="Location" type="System.Configuration.NameValueSectionHandler" />
    <!-- for 3 -->
    <sectionGroup name="InfoGroup">
      <section name="PersonInfo" type="System.Configuration.NameValueSectionHandler" />
      <section name="AddressInfo" type="System.Configuration.NameValueSectionHandler"/>
    </sectionGroup>
    <!-- for 4 -->
    <section name="Person" type="ConfigDemo.PersonConfigurationSection, ConfigDemo "/>
    <!-- for 5 -->
    <section name="Developers" type="ConfigDemo.DevelopersConfigurationSection, ConfigDemo "/>
  </configSections>
 
  <!-- 1. Using the preset appSettings section tag -->
  <appSettings>
    <add key="FirstName" value="Theo" />
    <add key="LastName" value="Bebekis" />
  </appSettings>
 
  <!-- 2. Using a custom section -->
  <Location>
    <add key="Country" value="Greece" />
    <add key="City" value="Thessaloniki" />
  </Location>
 
  <!-- 3. Using a custom section group -->
  <InfoGroup>
    <PersonInfo>
      <add key="FirstName" value="Theo" />
      <add key="LastName" value="Bebekis" />
    </PersonInfo>
    <AddressInfo>
      <add key="Country" value="Greece" />
      <add key="City" value="Thessaloniki" />
    </AddressInfo>    
  </InfoGroup>
 
  <!-- 4. Using a custom ConfigurationSection class and a custom ConfigurationElement class -->
  <Person>
    <Person1 Code="THB" FirstName="Theo" LastName="Bebekis" Country="Greece" City="Thessaloniki" ></Person1>
    <Person2 Code="JOD" FirstName="John" LastName="Doe" Country="Greece" City="Thessaloniki" ></Person2>
  </Person>
 
  <!-- 5. Using a custom ConfigurationElementCollection to have a section that simulates a collection of elements -->
  <Developers>
    <Developer Code="THB" FirstName="Theo" LastName="Bebekis" Country="Greece" City="Thessaloniki" ></Developer>
    <Developer Code="JOD" FirstName="John" LastName="Doe" Country="Greece" City="Thessaloniki" ></Developer>
  </Developers>
 
  <startup>
    <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.6.1" />
  </startup>
 
</configuration>
```


### 1. Using the preset appSettings section tag
```csharp
void Case1()
{           
    string FirstName = ConfigurationManager.AppSettings["FirstName"];
    string LastName = ConfigurationManager.AppSettings["LastName"];
}
```

### 2. Using a custom section
```csharp
<!-- for 2 -->
<section name="Location" type="System.Configuration.NameValueSectionHandler" />
```
---
```csharp
void Case2()
{            
    NameValueCollection Section = ConfigurationManager.GetSection("Location") as NameValueCollection;
    if (Section != null)
    {
        string Country = Section["Country"];
        string City = Section["City"];
    }
}
```
### 3. Using a custom section group
```csharp
void Case3()
{
    NameValueCollection Section = ConfigurationManager.GetSection("InfoGroup/PersonInfo") as NameValueCollection;
    if (Section != null)
    {
        string FirstName = Section["FirstName"];
        string LastName = Section["LastName"];
    }
 
    Section = ConfigurationManager.GetSection("InfoGroup/AddressInfo") as NameValueCollection;
    if (Section != null)
    {
        string Country = Section["Country"];
        string City = Section["City"];
    }
}
```
### 4. Using a custom ConfigurationSection class and a custom ConfigurationElement class
```csharp
namespace ConfigDemo
{
    public class PersonConfigurationElement : ConfigurationElement
    {
        public PersonConfigurationElement()
        {
        }
 
        [ConfigurationProperty("Code")]
        public string Code { get { return this["Code"] as string; } }
 
        [ConfigurationProperty("FirstName", DefaultValue = "John")]
        public string FirstName { get { return this["FirstName"] as string; } }
 
        [ConfigurationProperty("LastName", DefaultValue = "Doe")]
        public string LastName { get { return this["LastName"] as string; } }
 
        [ConfigurationProperty("Country")]
        public string Country { get { return this["Country"] as string; } }
 
        [ConfigurationProperty("City")]
        public string City { get { return this["City"] as string; } }
    }
 
    public class PersonConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("Person1")]
        public PersonConfigurationElement Person1
        {
            get { return this["Person1"] as PersonConfigurationElement; }
            set { this["Person1"] = value; }
        }
        [ConfigurationProperty("Person2")]
        public PersonConfigurationElement Person2
        {
            get { return this["Person2"] as PersonConfigurationElement; }
            set { this["Person2"] = value; }
        }
    }
}
```
Использование пользовательского раздела конфигурации требует соответствующего объявления в файле конфигурации, включающего имя раздела и тип пользовательского класса ConfigurationSection. Имя сборки необходимо в объявлении типа вместе с полным именем типа.
```xml
<!-- for 4 -->
<section name="Person" type="ConfigDemo.PersonConfigurationSection, ConfigDemo "/>
```
Для чтения из пользовательского раздела вам просто нужно передать имя раздела в методе GetSection и использовать пользовательский класс ConfigurationSection в качестве возвращаемого типа.
```csharp
void Case4()
{
    PersonConfigurationSection Section = ConfigurationManager.GetSection("Person") as PersonConfigurationSection;
    if (Section != null && Section.Person2 != null)
    {
        string FirstName = Section.Person2.FirstName;
        string LastName = Section.Person2.LastName;
        string Country = Section.Person2.Country;
        string City = Section.Person2.City;                
    }
}
```