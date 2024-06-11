### Шаг 4: Наследование типов и использование ключей

На этом шаге мы будем использовать наследование типов и добавим уникальные ключи для обеспечения уникальности элементов. Мы также добавим элемент для хранения нескольких адресов и предоставим возможность выбора из различных типов адресов.

### Пример XML-документа

```xml
<?xml version="1.0" encoding="UTF-8"?>
<people>
    <person id="1">
        <name>John Doe</name>
        <age>30</age>
        <addresses>
            <address type="home">
                <street>Main St</street>
                <city>Springfield</city>
                <zipcode>12345</zipcode>
            </address>
            <address type="work">
                <street>Second St</street>
                <city>Shelbyville</city>
                <zipcode>54321</zipcode>
            </address>
        </addresses>
        <contact>
            <phone>123-456-7890</phone>
            <email>john@example.com</email>
        </contact>
    </person>
    <person id="2">
        <name>Jane Smith</name>
        <age>25</age>
        <addresses>
            <address type="home">
                <street>Third St</street>
                <city>Ogdenville</city>
                <zipcode>67890</zipcode>
            </address>
        </addresses>
        <contact>
            <phone>098-765-4321</phone>
            <email>jane@example.com</email>
        </contact>
    </person>
</people>
```

### Пример XSD схемы

```xml
<?xml version="1.0" encoding="UTF-8"?>
<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">

    <!-- Определение типа для почтового индекса -->
    <xs:simpleType name="zipcodeType">
        <xs:restriction base="xs:string">
            <xs:pattern value="\d{5}"/>
        </xs:restriction>
    </xs:simpleType>

    <!-- Определение типа для телефонного номера -->
    <xs:simpleType name="phoneType">
        <xs:restriction base="xs:string">
            <xs:pattern value="\d{3}-\d{3}-\d{4}"/>
        </xs:restriction>
    </xs:simpleType>

    <!-- Определение типа для email -->
    <xs:simpleType name="emailType">
        <xs:restriction base="xs:string">
            <xs:pattern value="[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}"/>
        </xs:restriction>
    </xs:simpleType>

    <!-- Определение базового типа для адреса -->
    <xs:complexType name="addressType">
        <xs:sequence>
            <xs:element name="street" type="xs:string"/>
            <xs:element name="city" type="xs:string"/>
            <xs:element name="zipcode" type="zipcodeType"/>
        </xs:sequence>
        <xs:attribute name="type" type="xs:string"/>
    </xs:complexType>

    <!-- Определение типа для контактной информации -->
    <xs:complexType name="contactType">
        <xs:sequence>
            <xs:element name="phone" type="phoneType"/>
            <xs:element name="email" type="emailType"/>
        </xs:sequence>
    </xs:complexType>

    <!-- Определение корневого элемента -->
    <xs:element name="people">
        <xs:complexType>
            <xs:sequence>
                <!-- Определение элемента person -->
                <xs:element name="person" maxOccurs="unbounded">
                    <xs:complexType>
                        <xs:sequence>
                            <xs:element name="name" type="xs:string"/>
                            <xs:element name="age" type="xs:int"/>
                            <xs:element name="addresses">
                                <xs:complexType>
                                    <xs:sequence>
                                        <xs:element name="address" type="addressType" maxOccurs="unbounded"/>
                                    </xs:sequence>
                                </xs:complexType>
                            </xs:element>
                            <xs:element name="contact" type="contactType"/>
                        </xs:sequence>
                        <xs:attribute name="id" type="xs:int" use="required"/>
                    </xs:complexType>
                </xs:element>
            </xs:sequence>
        </xs:complexType>
        <!-- Определение уникальных ключей для элемента person по атрибуту id -->
        <xs:key name="uniquePersonID">
            <xs:selector xpath="person"/>
            <xs:field xpath="@id"/>
        </xs:key>
    </xs:element>

</xs:schema>
```

### Объяснение изменений

1. **Базовый тип `addressType`**:
   - Определен комплексный тип для адреса с элементами `street`, `city`, и `zipcode`, а также атрибутом `type`.

2. **Тип `contactType`**:
   - Уже определенный комплексный тип для контактной информации.

3. **Элемент `<xs:element name="addresses">`**:
   - Вложенный элемент для хранения нескольких адресов. Используется тип `addressType` с `maxOccurs="unbounded"` для возможности иметь несколько адресов.

4. **Элемент `<xs:key>`**:
   - Определяет уникальный ключ для атрибута `id` в элементе `person`. Это гарантирует, что каждый `person` элемент имеет уникальный `id`.

### Проверка XML-документа с помощью XSD

Ваш XML-документ должен включать ссылку на XSD схему для проверки:

```xml
<?xml version="1.0" encoding="UTF-8"?>
<people xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xsi:noNamespaceSchemaLocation="people.xsd">
    <person id="1">
        <name>John Doe</name>
        <age>30</age>
        <addresses>
            <address type="home">
                <street>Main St</street>
                <city>Springfield</city>
                <zipcode>12345</zipcode>
            </address>
            <address type="work">
                <street>Second St</street>
                <city>Shelbyville</city>
                <zipcode>54321</zipcode>
            </address>
        </addresses>
        <contact>
            <phone>123-456-7890</phone>
            <email>john@example.com</email>
        </contact>
    </person>
    <person id="2">
        <name>Jane Smith</name>
        <age>25</age>
        <addresses>
            <address type="home">
                <street>Third St</street>
                <city>Ogdenville</city>
                <zipcode>67890</zipcode>
            </address>
        </addresses>
        <contact>
            <phone>098-765-4321</phone>
            <email>jane@example.com</email>
        </contact>
    </person>
</people>
```

### Объяснение новых элементов и типов

- **Комплексный тип `addressType`**:
  - Базовый тип для адреса, включающий элементы `street`, `city`, и `zipcode`, а также атрибут `type`.

- **Элемент `addresses`**:
  - Вложенный элемент для хранения нескольких адресов, каждый из которых должен соответствовать `addressType`.

- **Уникальные ключи**:
  - Определение ключа для уникальности `id` элемента `person` гарантирует, что каждый `person` имеет уникальный идентификатор.

Этот пример показывает, как использовать наследование типов и уникальные ключи для создания более сложных XML-схем с ограничениями и возможностью выбора различных типов элементов.