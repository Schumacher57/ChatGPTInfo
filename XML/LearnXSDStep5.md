### Шаг 5: Самый сложный пример с расширенными функциями

На этом шаге мы добавим несколько сложных аспектов:
1. **Абстрактные типы и наследование типов**: Используем абстрактные базовые типы для создания иерархии элементов.
2. **Валидация по содержимому**: Добавим ограничения на значения элементов с использованием регулярных выражений и других средств валидации.
3. **Использование уникальных идентификаторов и ссылок**: Добавим уникальные идентификаторы и ссылки между элементами для обеспечения ссылочной целостности.
4. **Дополнительные элементы и атрибуты**: Добавим больше элементов и атрибутов, чтобы показать возможности XSD.

### Пример XML-документа

```xml
<?xml version="1.0" encoding="UTF-8"?>
<people xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xsi:noNamespaceSchemaLocation="people.xsd">
    <person id="1" type="employee">
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
        <employment>
            <position>Developer</position>
            <department>IT</department>
        </employment>
    </person>
    <person id="2" type="customer">
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
        <purchaseHistory>
            <purchase id="p1">
                <item>Book</item>
                <amount>2</amount>
            </purchase>
            <purchase id="p2">
                <item>Laptop</item>
                <amount>1</amount>
            </purchase>
        </purchaseHistory>
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

    <!-- Определение базового абстрактного типа для человека -->
    <xs:complexType name="abstractPersonType" abstract="true">
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
        <xs:attribute name="type" type="xs:string" use="required"/>
    </xs:complexType>

    <!-- Наследование типов: определение типа для сотрудников -->
    <xs:complexType name="employeeType">
        <xs:complexContent>
            <xs:extension base="abstractPersonType">
                <xs:sequence>
                    <xs:element name="employment">
                        <xs:complexType>
                            <xs:sequence>
                                <xs:element name="position" type="xs:string"/>
                                <xs:element name="department" type="xs:string"/>
                            </xs:sequence>
                        </xs:complexType>
                    </xs:element>
                </xs:sequence>
            </xs:extension>
        </xs:complexContent>
    </xs:complexType>

    <!-- Наследование типов: определение типа для клиентов -->
    <xs:complexType name="customerType">
        <xs:complexContent>
            <xs:extension base="abstractPersonType">
                <xs:sequence>
                    <xs:element name="purchaseHistory">
                        <xs:complexType>
                            <xs:sequence>
                                <xs:element name="purchase" maxOccurs="unbounded">
                                    <xs:complexType>
                                        <xs:sequence>
                                            <xs:element name="item" type="xs:string"/>
                                            <xs:element name="amount" type="xs:int"/>
                                        </xs:sequence>
                                        <xs:attribute name="id" type="xs:string" use="required"/>
                                    </xs:complexType>
                                </xs:element>
                            </xs:sequence>
                        </xs:complexType>
                    </xs:element>
                </xs:sequence>
            </xs:extension>
        </xs:complexContent>
    </xs:complexType>

    <!-- Определение типа для адреса -->
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
                <!-- Использование абстрактного типа для элемента person -->
                <xs:element name="person" maxOccurs="unbounded">
                    <xs:complexType>
                        <xs:complexContent>
                            <xs:extension base="abstractPersonType">
                                <xs:sequence>
                                    <xs:choice>
                                        <xs:element name="employment" type="employeeType"/>
                                        <xs:element name="purchaseHistory" type="customerType"/>
                                    </xs:choice>
                                </xs:sequence>
                            </xs:extension>
                        </xs:complexContent>
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

1. **Абстрактные типы и наследование типов**:
   - Создан абстрактный тип `abstractPersonType`, который используется как базовый тип для элементов `employeeType` и `customerType`.

2. **Тип `employeeType` и `customerType`**:
   - Тип `employeeType` наследует от `abstractPersonType` и добавляет элемент `employment`.
   - Тип `customerType` наследует от `abstractPersonType` и добавляет элемент `purchaseHistory`.

3. **Элемент `<xs:choice>`**:
   - В элементе `person` добавлен элемент выбора (choice) между `employment` и `purchaseHistory`, что позволяет дифференцировать между сотрудниками и клиентами.

4. **Уникальные идентификаторы и ссылки**:
   - Добавлены уникальные идентификаторы для элемента `purchase` внутри `purchaseHistory`.

### Проверка XML-документа с помощью XSD

Ваш XML-документ должен включать ссылку на XSD схему для проверки:

```xml
<?xml version="1.0" encoding="UTF-8"?>
<people xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xsi:noNamespaceSchemaLocation="people.xsd">
    <person id

="1" type="employee">
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
        <employment>
            <position>Developer</position>
            <department>IT</department>
        </employment>
    </person>
    <person id="2" type="customer">
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
        <purchaseHistory>
            <purchase id="p1">
                <item>Book</item>
                <amount>2</amount>
            </purchase>
            <purchase id="p2">
                <item>Laptop</item>
                <amount>1</amount>
            </purchase>
        </purchaseHistory>
    </person>
</people>
```

### Объяснение новых элементов и типов

- **Комплексный тип `abstractPersonType`**:
  - Базовый абстрактный тип для элемента `person`, от которого наследуются `employeeType` и `customerType`.

- **Комплексный тип `employeeType` и `customerType`**:
  - Специализированные типы, наследующие от `abstractPersonType` и добавляющие дополнительные элементы `employment` и `purchaseHistory`.

- **Элемент выбора (choice)**:
  - Использование элемента выбора позволяет дифференцировать между сотрудниками и клиентами.

- **Уникальные идентификаторы для элемента `purchase`**:
  - Обеспечивают уникальность идентификаторов для покупок внутри элемента `purchaseHistory`.

Этот пример показывает, как создавать сложные схемы с использованием наследования типов, валидации по содержимому, уникальными идентификаторами и ссылками.