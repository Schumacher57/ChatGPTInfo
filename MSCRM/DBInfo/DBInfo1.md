Привет! В базе данных Microsoft Dynamics CRM 2015 (обычно называется **`MSCRM`**, если используется имя по умолчанию) создается множество системных таблиц, которые используются для работы платформы. Вот основные группы таблиц, которые присутствуют в стандартной установке:

---

### **1. Таблицы метаданных (Metadata Tables)**
Эти таблицы содержат информацию о структуре и настройках CRM, включая сущности, поля, формы и представления.

- **Entity** — информация обо всех сущностях CRM (настраиваемых и системных).
- **Attribute** — описания атрибутов (полей) всех сущностей.
- **LocalizedLabel** — локализованные названия сущностей, полей и других объектов для разных языков.
- **Relationship** — описания связей между сущностями (1:N, N:N).
- **OptionSet** — информация о наборах опций (значения для выпадающих списков).

---

### **2. Основные таблицы данных (Data Tables)**
Таблицы, которые содержат данные стандартных сущностей CRM, таких как пользователи, контакты, счета и т.д.

- **AccountBase** — таблица для хранения данных о "Счетах" (Account).
- **ContactBase** — таблица для хранения данных о "Контактах" (Contact).
- **LeadBase** — таблица для хранения данных о "Потенциальных клиентах" (Lead).
- **OpportunityBase** — таблица для хранения данных о "Сделках" (Opportunity).
- **IncidentBase** — таблица для хранения данных о "Обращениях" (Case).
- **TaskBase** — таблица для хранения данных о "Задачах" (Task).
- **ActivityPointerBase** — основная таблица для всех видов активности (письма, звонки и т.д.).

Каждая сущность имеет пару таблиц:
- **ИмяСущностиBase** — основная таблица с данными.
- **ИмяСущностиExtensionBase** — таблица для хранения пользовательских (кастомных) полей.

---

### **3. Таблицы пользователей и ролей**
Эти таблицы содержат данные о пользователях системы, их ролях и безопасности.

- **SystemUserBase** — информация о пользователях CRM.
- **RoleBase** — роли безопасности.
- **TeamBase** — данные о командах пользователей.
- **PrivilegeBase** — привилегии, определенные в системе.
- **SystemUserRoles** — связь между пользователями и ролями.

---

### **4. Таблицы аудита (Audit Tables)**
Эти таблицы используются для хранения данных об изменениях в системе, если включен аудит.

- **AuditBase** — основная таблица аудита.
- **AttributeChangeBase** — информация об изменении атрибутов (полей).

---

### **5. Таблицы очередей и маршрутизации**
Используются для управления очередями и распределения задач.

- **QueueBase** — данные о очередях.
- **QueueItemBase** — записи, находящиеся в очередях.

---

### **6. Таблицы активностей (Activities)**
Таблицы для различных видов активностей:

- **EmailBase** — письма.
- **PhoneCallBase** — телефонные звонки.
- **AppointmentBase** — встречи.
- **FaxBase** — факсы.
- **LetterBase** — письма.
- **CampaignActivityBase** — маркетинговые активности.

---

### **7. Таблицы конфигурации и системы**
Эти таблицы управляют системными настройками и конфигурацией.

- **OrganizationBase** — информация о текущей организации (в CRM может быть одна или несколько организаций).
- **PluginAssemblyBase** — зарегистрированные сборки плагинов.
- **SdkMessageBase** — сообщения SDK (используются для выполнения операций через плагины и SDK).
- **AsyncOperationBase** — фоновые процессы и задания (например, Workflows).

---

### **8. Таблицы отчетов**
Используются для управления отчетами, хранящимися в CRM.

- **ReportBase** — данные об отчетах.
- **ReportCategoryBase** — категории отчетов.
- **ReportVisibilityBase** — информация о доступности отчетов.

---

### **9. Таблицы для интеграций**
Эти таблицы управляют синхронизацией и интеграцией данных с внешними системами.

- **SubscriptionBase** — данные о подписках на изменения.
- **SyncEntryBase** — данные о синхронизации.
- **IntegrationStatusBase** — статус интеграции.

---

### Особенности именования
- Большинство таблиц имеют суффикс **Base**, который указывает на основную таблицу сущности.
- Каждая сущность имеет две таблицы: **Base** для системных полей и **ExtensionBase** для пользовательских (кастомных) полей.
- Некоторые таблицы включают версии данных для управления связями, аудитом или историей изменений.

---

Если у тебя есть конкретный запрос по таблице или ты хочешь подробнее рассмотреть структуру данных (например, SQL-запрос для просмотра полей конкретной таблицы), дай знать! 😊