Давайте разберем код, который вы привели. Это тестовый класс для проверки плагина Dynamics CRM, который отправляет идентификатор CRM субъекта в систему Eva при обновлении определенных полей контакта.

### Общий обзор

Класс `ContactUpdateSendSubjectCrmIdTest` наследуется от `TestPluginBase`, что означает, что он использует какую-то базовую функциональность для настройки и выполнения тестов плагина.

### Важные моменты

1. **Инициализация Kernel:**
   ```csharp
   protected override void KernelInitialize()
   {
       base.KernelInitialize();
       KernelBindCache();
   }
   ```
   Метод `KernelInitialize` вызывается для инициализации зависимостей. `KernelBindCache` связывает зависимости для тестирования.

2. **Тестовые методы:**
   Каждый тестовый метод помечен атрибутом `[TestMethod]`, что делает его тестом, который будет выполняться тестовым фреймворком (в данном случае, MSTest).

### Объяснение тестов

1. **EvaIdUpdate:**
   ```csharp
   public void EvaIdUpdate()
   ```
   Этот тест проверяет, что метод `SendSubjectCrmId` вызывается один раз при обновлении `new_id_eva` поля контакта.
   
   - **Mock объектов:**
     ```csharp
     var evaDataAccess = new Mock<IEvaDataAccess>();
     var integrationDataAccess = new Mock<IIntegrationDataAccess>();
     var dataAccess = new Mock<IDataAccess>();
     ```
     Создаются mock объекты для зависимостей, таких как `IEvaDataAccess`, `IIntegrationDataAccess` и `IDataAccess`.

   - **Настройка mock объектов:**
     ```csharp
     integrationDataAccess.Setup(a => a.Eva).Returns(() => evaDataAccess.Object);
     dataAccess.Setup(a => a.Integration).Returns(() => integrationDataAccess.Object);
     ```
     Настраивается возвращаемое значение методов `Eva` и `Integration` для mock объектов.

   - **Создание и настройка target и preImage:**
     ```csharp
     var target = EntityExt.Gen("contact", new { Id = contactId, new_id_eva = Guid.NewGuid().ToString() });
     var preImage = EntityExt.Gen("contact", new { Id = contactId, new_id_eva = Guid.NewGuid().ToString() });
     ```
     Создаются объекты target и preImage, представляющие контакт до и после обновления.

   - **Настройка контекста:**
     ```csharp
     Crm.Context.SetTarget(target.Clone());
     Crm.Context.SetPreImage(preImage.Clone());
     ```
     Настраивается контекст CRM с target и preImage.

   - **Выполнение плагина:**
     ```csharp
     Crm.Execute<ContactUpdateSendSubjectCrmId>(StepStage.PreOperation, StepMessage.Update);
     ```
     Выполняется плагин с указанным этапом и сообщением.

   - **Проверка:**
     ```csharp
     evaDataAccess.Verify(e => e.SendSubjectCrmId(It.IsAny<Guid>()), Times.Once);
     ```
     Проверяется, что метод `SendSubjectCrmId` был вызван один раз.

2. **PfpIdUpdate:**
   Этот тест аналогичен `EvaIdUpdate`, но проверяет поле `new_id_pfp`.

3. **NewStateCodeUpdate:**
   ```csharp
   public void NewStateCodeUpdate()
   ```
   Этот тест проверяет, что метод `SendSubjectCrmId` вызывается один раз при изменении поля `new_statecode`.

4. **SamePfpUpdate:**
   ```csharp
   public void SamePfpUpdate()
   ```
   Этот тест проверяет, что метод `SendSubjectCrmId` не вызывается, если значение `new_id_pfp` не изменилось.

5. **SameEvaUpdate:**
   ```csharp
   public void SameEvaUpdate()
   ```
   Этот тест проверяет, что метод `SendSubjectCrmId` не вызывается, если значение `new_id_eva` не изменилось.

6. **SameStateCodeUpdate:**
   ```csharp
   public void SameStateCodeUpdate()
   ```
   Этот тест проверяет, что метод `SendSubjectCrmId` не вызывается, если значение `new_statecode` не изменилось.

### Пояснение по коду

- **Mocks:** Используются библиотеки Moq для создания mock объектов, что позволяет тестировать поведение без необходимости реального вызова зависимостей.
- **EntityExt.Gen:** Вероятно, метод для создания экземпляров `Entity` с заданными атрибутами, используемый для упрощения тестирования.
- **Crm.Context:** Похоже на контекст CRM, используемый для установки и проверки состояния объектов перед и после выполнения плагина.
- **KernelBind:** Метод для связывания зависимостей, вероятно, часть базового класса `TestPluginBase`.

Этот код является частью тестового пакета, проверяющего корректность выполнения плагинов Dynamics CRM в различных сценариях обновления контактных данных.