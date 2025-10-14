Отлично, разберём полностью процесс репликации в твоём случае и как с этим работать, шаг за шагом. Сделаю разбор “по полочкам”, чтобы всё было понятно, включая агентов, джобы и статус ошибок.

---

## 1. Общая структура репликации SQL Server (Transactional + Snapshot)

Ты работаешь с **Transactional Replication** (репликация транзакций) с использованием **Snapshot Agent**. Основные компоненты:

| Компонент                      | Что делает                                                                                                                       |
| ------------------------------ | -------------------------------------------------------------------------------------------------------------------------------- |
| **Publisher (CrmExtensions)**  | База, где исходные данные. Создаётся публикация (`CRMTODWHACRM2015`) с набором таблиц.                                           |
| **Publication**                | Логическая единица репликации. Определяет, какие таблицы/объекты публикуются и как.                                              |
| **Snapshot Agent**             | Формирует “снимок” данных публикации. Берёт все строки таблиц и складывает их в `distribution` database.                         |
| **Distribution database**      | Хранилище промежуточного состояния репликации. Там Snapshot Agent кладёт данные, а Distribution Agent забирает их на подписчика. |
| **Distribution Agent**         | Берёт данные из `distribution` и синхронизирует их с **Subscriber** (`DWH_aCRM_2015`).                                           |
| **Subscriber (DWH_aCRM_2015)** | База, куда доставляются таблицы и данные.                                                                                        |

---

## 2. Джобы SQL Server Agent

Каждый агент репликации запускается как **джоба SQL Server Agent**:

| Джоба                                                 | Агент                                                                                      | Назначение                                                        |
| ----------------------------------------------------- | ------------------------------------------------------------------------------------------ | ----------------------------------------------------------------- |
| `S-T-CRM31-SQL01-CrmExtensions-CRMTODWHACRM2015-1612` | Snapshot Agent                                                                             | Создаёт полные копии таблиц публикации.                           |
| Distribution Agent (непосредственно)                  | Джоба обычно называется `S-T-CRM31-SQL01-CrmExtensions-CRMTODWHACRM2015-1612-Distribution` | Доставляет данные подписчику.                                     |
| Log Reader Agent                                      | Джоба, если нужна транзакционная репликация                                                | Читает журнал транзакций Publisher и готовит их для Distribution. |

**Важно:** Джобы запускаются через `sp_start_job` или по расписанию через SQL Server Agent.

---

## 3. Статусы агентов (runstatus в MSsnapshot_history)

В таблице `distribution.dbo.MSsnapshot_history` есть колонка `runstatus`. Значения:

| runstatus | Значение                                                |
| --------- | ------------------------------------------------------- |
| 0         | Not yet run (ещё не запускался)                         |
| 1         | Succeeded (успешно завершён)                            |
| 2         | Retry (агент перепробует)                               |
| 3         | In progress (выполняется)                               |
| 4         | Succeeded with warnings (завершился с предупреждениями) |
| 5         | Failed (ошибка выполнения)                              |

Пример из твоей истории:

* **runstatus = 3** — агент выполняется, например копирует часть таблицы.
* **runstatus = 5** — агент упал. В твоём случае причина была: `There is not enough space on the disk`.

Каждая запись хранит также:

* `comments` — что конкретно делает агент на момент записи.
* `delivered_commands`, `delivered_transactions` — количество команд/строк обработанных на этот момент.
* `error_id` — код ошибки SQL Server (например, нехватка места на диске).

---

## 4. Как посмотреть текущие джобы и их статус

### Проверка джобов Snapshot Agent

```sql
USE msdb;
EXEC sp_help_job @job_name = 'S-T-CRM31-SQL01-CrmExtensions-CRMTODWHACRM2015-1612';
```

Колонки важные:

* `current_execution_status`

  * 1 = Idle
  * 2 = Executing
  * 4 = Waiting for thread
  * 5 = Between retries
* `last_run_outcome`

  * 0 = Failed
  * 1 = Succeeded
  * 3 = Canceled

### История выполнения Snapshot Agent

```sql
SELECT TOP 200 *
FROM distribution.dbo.MSsnapshot_history
WHERE agent_id = 1612
ORDER BY time DESC;
```

Используется, чтобы видеть, на каком шаге агент падал или успешно выполнялся.

---

## 5. Как запускаются агенты

### Snapshot Agent

```sql
USE msdb;
EXEC sp_start_job @job_name = 'S-T-CRM31-SQL01-CrmExtensions-CRMTODWHACRM2015-1612';
```

* Инициирует создание полного снимка данных.
* Если агент уже выполняется, SQL Server выдаст ошибку:

```
The job is already running from a request by User ...
```

### Distribution Agent

* Обычно настраивается вместе с подпиской.
* Если Snapshot Agent завершился успешно, Distribution Agent забирает данные на подписчика.
* Можно проверить статус Distribution Agent в `MSdistribution_agents` и истории `MSdistribution_history`.

---

## 6. Частые ошибки и их интерпретация

| Ошибка                                                    | Что значит                                  | Как исправить                                             |
| --------------------------------------------------------- | ------------------------------------------- | --------------------------------------------------------- |
| `Msg 22022 ... job is already running`                    | Джоба уже выполняется                       | Подождать или отменить текущий запуск через `sp_stop_job` |
| `There is not enough space on the disk`                   | На диске distribution или tempdb мало места | Освободить место, увеличить диски, перезапустить джоб     |
| `Could not find stored procedure 'sp_helpsnapshot_agent'` | Вызов из неправильной базы                  | Запускать в базе публикации или `distribution`            |

---

## 7. Как убедиться, что данные дошли до Subscriber

```sql
-- Проверка таблиц в подписчике
USE DWH_aCRM_2015;
SELECT TOP 10 * FROM dbo.[Имя_таблицы];
```

Если таблицы появились и строки совпадают с Publisher — репликация прошла успешно.

---

## 8. Алгоритм восстановления и старта репликации

1. Проверить дисковое пространство (важно для Snapshot Agent).
2. Убедиться, что старые подписки удалены, если нужно.
3. Запустить Snapshot Agent:

```sql
USE msdb;
EXEC sp_start_job @job_name = 'S-T-CRM31-SQL01-CrmExtensions-CRMTODWHACRM2015-1612';
```

4. Дождаться успешного завершения (`runstatus = 1` в MSsnapshot_history).
5. Проверить, что Distribution Agent доставил данные (`MSdistribution_history`).
6. Проверить таблицы в подписчике (`DWH_aCRM_2015`).

---
---

Да, примерно так, но с уточнениями. Разложим по пунктам:

---

## 1. Агенты и публикации

Для каждой **публикации** (`Publication`) создаются свои агенты:

| Агент                  | Что делает                                                                                       | Создаётся ли отдельный для каждой публикации? |
| ---------------------- | ------------------------------------------------------------------------------------------------ | --------------------------------------------- |
| **Snapshot Agent**     | Создаёт снимок всех таблиц и объектов публикации                                                 | Да, отдельный на каждую публикацию            |
| **Log Reader Agent**   | Берёт транзакции из журнала и готовит их для Distribution (только для транзакционной репликации) | Да, отдельный на каждую публикацию            |
| **Distribution Agent** | Переносит данные из Distribution на Subscriber                                                   | Да, отдельный на каждую публикацию + подписку |

---

## 2. Subscriber и Distribution

* На каждый **Subscriber** создаётся свой **Distribution Agent**, даже если одна и та же публикация доставляется в несколько подписчиков.
* Если подписчиков несколько, есть несколько агентов Distribution, но **Snapshot Agent остаётся один** (для одной публикации).
* **Важно:** Snapshot Agent генерирует данные один раз для публикации, а потом каждый Distribution Agent берёт их для своего подписчика.

---

## 3. Влияние количества баз и таблиц

* **Количество таблиц в публикации**: один Snapshot Agent копирует все таблицы публикации за один запуск.
* **Количество баз публикации**: для каждой базы публикации создаётся отдельная публикация → отдельный Snapshot Agent и свои Distribution/Log Reader агенты.

То есть:

* Публикация = связка **Publisher DB + набор таблиц**
* На каждую публикацию свои агенты
* Для одного подписчика на одну публикацию — один Distribution Agent
* Для нескольких подписчиков одной публикации — несколько Distribution Agents, но один Snapshot Agent

---

Пример:

```
Publisher DB: CrmExtensions
Publication1 (таблицы A,B,C) → Subscriber1
Publication2 (таблицы D,E)  → Subscriber1
Publication1 → Subscriber2
```

Агенты будут такие:

| Публикация   | Snapshot Agent | Distribution Agent            |
| ------------ | -------------- | ----------------------------- |
| Publication1 | 1              | 2 (Subscriber1 + Subscriber2) |
| Publication2 | 1              | 1 (Subscriber1)               |

---
---
Да, через запросы можно полностью обойтись без GUI. Разложим по шагам и по таблицам/процедурам.

---

## 1. Snapshot Agent

### Таблица:

`distribution.dbo.MSsnapshot_agents`

Пример запроса для публикации:

```sql
USE distribution;
SELECT name AS snapshot_agent_name,
       publisher_db,
       publication,
       publisher_id,
       agent_id
FROM dbo.MSsnapshot_agents
WHERE publication = 'CRMTODWHACRM2015';
```

* `name` → имя агента, совпадает с именем джобы в SQL Server Agent
* `agent_id` → используется для истории и журналов

### История работы агента:

`distribution.dbo.MSsnapshot_history`

```sql
USE distribution;
SELECT TOP 100 *
FROM dbo.MSsnapshot_history
WHERE agent_id = 1612
ORDER BY time DESC;
```

* `runstatus` = статус выполнения (0=запланирован, 1=успешно, 3=в процессе, 5=ошибка)
* `comments` → текст логов (важно при ошибках)

---

## 2. Log Reader Agent

Для транзакционной репликации:

```sql
USE distribution;
SELECT name AS logreader_agent_name,
       publication,
       publisher_db,
       agent_id
FROM dbo.MSlogreader_agents
WHERE publication = 'CRMTODWHACRM2015';
```

* `agent_id` → аналогично для истории
* История: `distribution.dbo.MSlogreader_history`

---

## 3. Distribution Agent (для подписчика)

```sql
USE distribution;
SELECT name AS dist_agent_name,
       publisher_db,
       publication,
       subscriber_server,
       subscriber_db,
       agent_id
FROM dbo.MSdistribution_agents
WHERE publication = 'CRMTODWHACRM2015';
```

* Каждый подписчик получает свой Distribution Agent

История работы:
`distribution.dbo.MSdistribution_history`

---

## 4. Джобы SQL Server Agent

Все агенты создаются как **SQL Server Agent Job**. Таблицы находятся в `msdb`:

```sql
USE msdb;
-- По имени агента
EXEC sp_help_job @job_name = 'S-T-CRM31-SQL01-CrmExtensions-CRMTODWHACRM2015-1612';

-- По статусу выполнения
EXEC sp_help_job @execution_status = 4; -- 4 = выполняется
```

* `current_execution_status` → 1=выполняется, 4=не выполняется, 5=завершено с ошибкой
* `last_run_outcome` → 0=не запускался, 1=успех, 3=ошибка

---

## 5. Связь между публикацией и джобой

* `distribution.dbo.MSsnapshot_agents.name` = имя джобы в `msdb.dbo.sysjobs.name`
* То же для Log Reader и Distribution Agents

---





