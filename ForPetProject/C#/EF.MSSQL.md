Конечно! Вот пример большого задания для пет-проекта, который включает использование MS SQL, C#, EF Core и требует создания хранимых процедур и/или джобов. Проект будет представлять собой консольное приложение для управления библиотекой.

### Проект: Консольное приложение для управления библиотекой

#### Цель
Разработать консольное приложение для управления библиотекой, где пользователи могут добавлять, удалять и обновлять книги, а также управлять выдачей и возвратом книг.

#### Требования

1. **Использование C# и .NET Core (или .NET Framework).**
2. **База данных MS SQL для хранения информации о книгах, пользователях и транзакциях.**
3. **Entity Framework Core для доступа к базе данных.**
4. **Хранимые процедуры и/или джобы для выполнения сложных операций в базе данных.**

### Функциональные требования

#### Управление книгами

- Добавление новой книги.
- Удаление книги.
- Обновление информации о книге (название, автор, год издания и т.д.).
- Получение списка всех книг.
- Поиск книг по названию или автору.

#### Управление пользователями

- Добавление нового пользователя.
- Удаление пользователя.
- Обновление информации о пользователе (имя, фамилия, электронная почта и т.д.).
- Получение списка всех пользователей.

#### Управление транзакциями

- Выдача книги пользователю.
- Возврат книги.
- Получение истории выдачи и возврата книг.

### Нефункциональные требования

1. **Консольный интерфейс:** Приложение должно иметь удобный и интуитивно понятный интерфейс для взаимодействия с пользователем через консоль.
2. **Логирование:** Все действия пользователя должны быть залогированы.
3. **Ошибки и исключения:** Приложение должно корректно обрабатывать ошибки и исключения.

### База данных

#### Таблицы

1. **Books**
   - BookId (INT, PRIMARY KEY, IDENTITY)
   - Title (NVARCHAR(100))
   - Author (NVARCHAR(100))
   - YearPublished (INT)
   - ISBN (NVARCHAR(20))

2. **Users**
   - UserId (INT, PRIMARY KEY, IDENTITY)
   - FirstName (NVARCHAR(50))
   - LastName (NVARCHAR(50))
   - Email (NVARCHAR(100))

3. **Transactions**
   - TransactionId (INT, PRIMARY KEY, IDENTITY)
   - BookId (INT, FOREIGN KEY)
   - UserId (INT, FOREIGN KEY)
   - IssueDate (DATETIME)
   - ReturnDate (DATETIME, NULLABLE)

#### Хранимые процедуры

1. **sp_AddBook**
   - Входные параметры: Title, Author, YearPublished, ISBN
   - Операция: Добавление новой книги в таблицу Books.

2. **sp_AddUser**
   - Входные параметры: FirstName, LastName, Email
   - Операция: Добавление нового пользователя в таблицу Users.

3. **sp_IssueBook**
   - Входные параметры: BookId, UserId
   - Операция: Запись о выдаче книги пользователю в таблицу Transactions.

4. **sp_ReturnBook**
   - Входные параметры: TransactionId
   - Операция: Обновление записи в таблице Transactions о возврате книги.

#### Джобы

1. **Job_CheckOverdueBooks**
   - Описание: Джоб, который выполняется ежедневно и проверяет книги, которые были выданы и не возвращены в течение 30 дней. Отправка уведомлений пользователям по электронной почте (можно просто залогировать).

### Пример структуры проекта

#### 1. Создание базы данных

```sql
CREATE DATABASE LibraryDB;
GO

USE LibraryDB;
GO

CREATE TABLE Books (
    BookId INT PRIMARY KEY IDENTITY,
    Title NVARCHAR(100),
    Author NVARCHAR(100),
    YearPublished INT,
    ISBN NVARCHAR(20)
);

CREATE TABLE Users (
    UserId INT PRIMARY KEY IDENTITY,
    FirstName NVARCHAR(50),
    LastName NVARCHAR(50),
    Email NVARCHAR(100)
);

CREATE TABLE Transactions (
    TransactionId INT PRIMARY KEY IDENTITY,
    BookId INT FOREIGN KEY REFERENCES Books(BookId),
    UserId INT FOREIGN KEY REFERENCES Users(UserId),
    IssueDate DATETIME,
    ReturnDate DATETIME
);

-- Примеры хранимых процедур

CREATE PROCEDURE sp_AddBook
    @Title NVARCHAR(100),
    @Author NVARCHAR(100),
    @YearPublished INT,
    @ISBN NVARCHAR(20)
AS
BEGIN
    INSERT INTO Books (Title, Author, YearPublished, ISBN)
    VALUES (@Title, @Author, @YearPublished, @ISBN);
END;

CREATE PROCEDURE sp_AddUser
    @FirstName NVARCHAR(50),
    @LastName NVARCHAR(50),
    @Email NVARCHAR(100)
AS
BEGIN
    INSERT INTO Users (FirstName, LastName, Email)
    VALUES (@FirstName, @LastName, @Email);
END;

CREATE PROCEDURE sp_IssueBook
    @BookId INT,
    @UserId INT
AS
BEGIN
    INSERT INTO Transactions (BookId, UserId, IssueDate)
    VALUES (@BookId, @UserId, GETDATE());
END;

CREATE PROCEDURE sp_ReturnBook
    @TransactionId INT
AS
BEGIN
    UPDATE Transactions
    SET ReturnDate = GETDATE()
    WHERE TransactionId = @TransactionId;
END;
```

#### 2. Консольное приложение на C#

Создайте проект консольного приложения в .NET Core (или .NET Framework) и настройте его для использования Entity Framework Core.

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace LibraryConsoleApp
{
    public class LibraryContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=your_server;Database=LibraryDB;Trusted_Connection=True;");
        }
    }

    public class Book
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int YearPublished { get; set; }
        public string ISBN { get; set; }
    }

    public class User
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }

    public class Transaction
    {
        public int TransactionId { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new LibraryContext())
            {
                // Ваш код для взаимодействия с пользователем через консоль.
            }
        }

        static void AddBook(string title, string author, int yearPublished, string isbn)
        {
            using (var context = new LibraryContext())
            {
                var book = new Book { Title = title, Author = author, YearPublished = yearPublished, ISBN = isbn };
                context.Books.Add(book);
                context.SaveChanges();
            }
        }

        // Другие методы для управления пользователями и транзакциями.
    }
}
```

#### 3. Джоб для проверки просроченных книг

Создайте джоб в MS SQL для проверки просроченных книг.

```sql
USE LibraryDB;
GO

CREATE PROCEDURE sp_CheckOverdueBooks
AS
BEGIN
    DECLARE @OverdueBooks TABLE (
        BookId INT,
        UserId INT,
        IssueDate DATETIME
    );

    INSERT INTO @OverdueBooks
    SELECT BookId, UserId, IssueDate
    FROM Transactions
    WHERE ReturnDate IS NULL AND DATEDIFF(DAY, IssueDate, GETDATE()) > 30;

    -- Логирование или отправка уведомлений пользователям
    SELECT * FROM @OverdueBooks;
END;

-- Создание джоба с помощью SQL Server Agent (нужно настроить через SQL Server Management Studio)
```

### Дополнительные улучшения

- Реализуйте валидацию ввода данных пользователем.
- Добавьте возможность экспорта данных (например, в CSV).
- Реализуйте дополнительные отчеты (например, самые популярные книги, активные пользователи и т.д.).

Этот проект предоставляет отличную возможность для изучения и практики использования C#, EF Core и MS SQL. Вы можете расширять его функциональность по мере необходимости.