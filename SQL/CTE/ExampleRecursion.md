Рекурсивные Common Table Expressions (CTE) в SQL Server позволяют выполнять рекурсивные операции, такие как вычисление иерархий, последовательностей и т.д. Примером такого использования может быть вычисление иерархии сотрудников, где каждый сотрудник имеет менеджера.

### Пример использования рекурсии с CTE в MS SQL

#### Пример 1: Иерархия сотрудников
Рассмотрим таблицу `Employees` с такой структурой:
- `EmployeeID` - уникальный идентификатор сотрудника
- `EmployeeName` - имя сотрудника
- `ManagerID` - идентификатор менеджера, который является `EmployeeID` другого сотрудника

```sql
CREATE TABLE Employees (
    EmployeeID INT PRIMARY KEY,
    EmployeeName NVARCHAR(50),
    ManagerID INT NULL
);
```

Заполним таблицу данными:

```sql
INSERT INTO Employees (EmployeeID, EmployeeName, ManagerID)
VALUES
    (1, 'Alice', NULL),
    (2, 'Bob', 1),
    (3, 'Charlie', 1),
    (4, 'David', 2),
    (5, 'Eve', 2),
    (6, 'Frank', 3);
```

Теперь используем рекурсивный CTE, чтобы вычислить иерархию сотрудников:

```sql
WITH EmployeeHierarchy AS (
    -- Начальная точка рекурсии (менеджер верхнего уровня)
    SELECT 
        EmployeeID, 
        EmployeeName, 
        ManagerID, 
        CAST(EmployeeName AS NVARCHAR(MAX)) AS HierarchyPath
    FROM Employees
    WHERE ManagerID IS NULL

    UNION ALL

    -- Рекурсивный запрос
    SELECT 
        e.EmployeeID, 
        e.EmployeeName, 
        e.ManagerID, 
        CAST(h.HierarchyPath + ' -> ' + e.EmployeeName AS NVARCHAR(MAX)) AS HierarchyPath
    FROM Employees e
    INNER JOIN EmployeeHierarchy h ON e.ManagerID = h.EmployeeID
)
SELECT * FROM EmployeeHierarchy;
```

Этот запрос создаст иерархию сотрудников, показывая путь от корневого менеджера до каждого сотрудника.

#### Пример 2: Последовательность чисел

Рассмотрим задачу генерации последовательности чисел от 1 до 10 с помощью рекурсивного CTE.

```sql
WITH Numbers AS (
    -- Начальная точка рекурсии
    SELECT 1 AS Number
    UNION ALL
    -- Рекурсивный запрос
    SELECT Number + 1
    FROM Numbers
    WHERE Number < 10
)
SELECT Number FROM Numbers;
```

Этот запрос создаст таблицу с числами от 1 до 10.

### Объяснение

- **Начальная точка рекурсии**: В части `SELECT` перед `UNION ALL` выбираются начальные значения.
- **Рекурсивный запрос**: В части `SELECT` после `UNION ALL` идет рекурсивный запрос, который ссылается на CTE. Условие в `WHERE` гарантирует, что рекурсия завершится.

### Примечания

1. **Ограничение уровня рекурсии**: По умолчанию, максимальная глубина рекурсии для CTE в SQL Server составляет 100. Вы можете изменить это значение, используя опцию `OPTION (MAXRECURSION n)`:
    ```sql
    WITH Numbers AS (
        SELECT 1 AS Number
        UNION ALL
        SELECT Number + 1
        FROM Numbers
        WHERE Number < 1000
    )
    SELECT Number FROM Numbers
    OPTION (MAXRECURSION 1000);
    ```
2. **Зацикливание**: Будьте осторожны с рекурсивными CTE, чтобы избежать бесконечных циклов.

Эти примеры демонстрируют, как можно использовать рекурсивные CTE для решения различных задач в SQL Server.