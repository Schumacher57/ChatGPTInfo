### Шаг 3: Добавление типов и дополнительных ограничений

На этом шаге мы добавим собственные типы и дополнительные ограничения на элементы. Мы также введем новый элемент для контактной информации, который может содержать как телефонные номера, так и email.

### Пример XML-документа

```xml
<?xml version="1.0" encoding="UTF-8"?>
<people>
    <person id="1">
        <name>John Doe</name>
        <age>30</age>
        <address>
            <street>Main St</street>
            <city>Springfield</city>
            <zipcode>12345</zipcode>
        </address>
        <contact>
            <phone>123-456-7890</phone>
            <email>john@example.com</email>
        </contact>
    </person>
    <person id="2">
        <name>Jane Smith</name>
        <age>25</age>
        <address>
            <street>Second St</street>
            <city>Shelbyville</city>
            <zipcode>54321</zipcode>
        </address>
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

    <!-- Определение элемента contact -->
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
                            <xs:element name="address">
                                <xs:complexType>
                                    <xs:sequence>
                                        <xs:element name="street" type="xs:string"/>
                                        <xs:element name="city" type="xs:string"/>
                                        <xs:element name="zipcode" type="zipcodeType"/>
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
    </xs:element>

</xs:schema>
```

### Объяснение изменений

1. **Типы `zipcodeType`, `phoneType`, и `emailType`**:
   - Созданы простые типы для почтового индекса, телефонного номера и email с ограничениями по шаблону (pattern).

2. **Тип `contactType`**:
   - Создан комплексный тип для контактной информации, включающий элементы `phone` и `email`.

3. **Элемент `<xs:element name="contact" type="contactType"/>`**:
   - Добавлен элемент `contact` к элементу `person`, использующий новый тип `contactType`.

### Проверка XML-документа с помощью XSD

Ваш XML-документ должен включать ссылку на XSD схему для проверки:

```xml
<?xml version="1.0" encoding="UTF-8"?>
<people xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xsi:noNamespaceSchemaLocation="people.xsd">
    <person id="1">
        <name>John Doe</name>
        <age>30</age>
        <address>
            <street>Main St</street>
            <city>Springfield</city>
            <zipcode>12345</zipcode>
        </address>
        <contact>
            <phone>123-456-7890</phone>
            <email>john@example.com</email>
        </contact>
    </person>
    <person id="2">
        <name>Jane Smith</name>
        <age>25</age>
        <address>
            <street>Second St</street>
            <city>Shelbyville</city>
            <zipcode>54321</zipcode>
        </address>
        <contact>
            <phone>098-765-4321</phone>
            <email>jane@example.com</email>
        </contact>
    </person>
</people>
```

### Объяснение новых элементов и типов

- **Простые типы**:
  - `zipcodeType` ограничивает формат почтового индекса.
  - `phoneType` ограничивает формат телефонного номера.
  - `emailType` ограничивает формат email-адреса.

- **Комплексный тип `contactType`**:
  - Содержит элементы `phone` и `email`, каждый из которых использует свои простые типы.

- **Элемент `contact`**:
  - Включает контактную информацию для каждого человека, используя комплексный тип `contactType`.

Этот пример показывает, как использовать простые и комплексные типы для создания более сложных XML-схем с ограничениями на значения элементов.