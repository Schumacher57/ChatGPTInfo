Отлично! Давайте сделаем шаг посложнее. В этом примере мы расширим нашу XML-схему для описания списка людей, добавим атрибуты и элементы с ограничениями.

### Шаг 2: Пример посложнее

Мы будем описывать список людей с именем, возрастом и адресом. Адрес будет состоять из улицы, города и почтового индекса.

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
    </person>
    <person id="2">
        <name>Jane Smith</name>
        <age>25</age>
        <address>
            <street>Second St</street>
            <city>Shelbyville</city>
            <zipcode>54321</zipcode>
        </address>
    </person>
</people>
```

### Пример XSD схемы

```xml
<?xml version="1.0" encoding="UTF-8"?>
<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">

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
                                        <xs:element name="zipcode" type="xs:string" pattern="\d{5}"/>
                                    </xs:sequence>
                                </xs:complexType>
                            </xs:element>
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

1. **Корневой элемент `<xs:element name="people">`**:
   - Это новый корневой элемент, содержащий список людей (`person`).

2. **Элемент `<xs:element name="person" maxOccurs="unbounded">`**:
   - Элемент `person` может встречаться неограниченное количество раз.

3. **Элемент `<xs:complexType>` внутри `person`**:
   - Каждый `person` содержит последовательность элементов: `name`, `age` и `address`.
   - Введен атрибут `id` типа `xs:int`, который обязателен (`use="required"`).

4. **Элемент `<xs:element name="address">`**:
   - Новый элемент `address`, который является сложным типом и содержит последовательность элементов: `street`, `city` и `zipcode`.

5. **Элемент `<xs:element name="zipcode" type="xs:string" pattern="\d{5}"/>`**:
   - Элемент `zipcode` типа `xs:string` с ограничением по шаблону (5 цифр).

### Проверка XML-документа с помощью XSD

Вот как связать ваш XML-документ с XSD схемой для проверки:

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
    </person>
    <person id="2">
        <name>Jane Smith</name>
        <age>25</age>
        <address>
            <street>Second St</street>
            <city>Shelbyville</city>
            <zipcode>54321</zipcode>
        </address>
    </person>
</people>
```

### Объяснение новых элементов и атрибутов

- **Атрибут `id`**: Уникальный идентификатор для каждого человека.
- **Элемент `address`**: Включает элементы `street`, `city` и `zipcode`.
- **Ограничение по шаблону для `zipcode`**: Почтовый индекс должен состоять из 5 цифр.

Этот пример усложняет структуру XML-документа, добавляя вложенные элементы и ограничения, что является следующим шагом в изучении XSD схем.