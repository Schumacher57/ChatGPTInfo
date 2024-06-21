Конечно! В PowerShell существует несколько различных способов создания и использования функций. Вот несколько примеров:

### Пример 1: Создание простой функции без параметров
```powershell
function SayHello {
    Write-Host "Привет, мир!"
}

# Вызов функции
SayHello
```

### Пример 2: Функция с параметрами
```powershell
function Greet {
    param(
        [string]$name
    )
    Write-Host "Привет, $name!"
}

# Вызов функции с передачей аргумента
Greet -name "Алексей"
```

### Пример 3: Использование параметров по умолчанию
```powershell
function Get-User {
    param(
        [string]$name = "Гость"
    )
    Write-Host "Привет, $name!"
}

# Вызов функции с и без параметров
Get-User
Get-User -name "Екатерина"
```

### Пример 4: Возврат значения из функции
```powershell
function Add-Numbers {
    param(
        [int]$num1,
        [int]$num2
    )
    $sum = $num1 + $num2
    return $sum
}

# Вызов функции и использование возвращаемого значения
$result = Add-Numbers -num1 5 -num2 3
Write-Host "Сумма: $result"
```

### Пример 5: Использование именованных параметров
```powershell
function Create-User {
    param(
        [string]$name,
        [string]$role
    )
    Write-Host "Создан пользователь $name с ролью $role"
}

# Вызов функции с именованными параметрами
Create-User -name "Иван" -role "Администратор"
```

### Пример 6: Создание анонимной функции (скрипт-блока)
```powershell
$sayHello = {
    Write-Host "Привет, анонимная функция!"
}

# Вызов анонимной функции
& $sayHello
```

### В PowerShell аргументы функции могут быть получены без использования ключевого слова `param`. Вот пример определения функции без явного использования ключевого слова `param`:

```powershell
function Get-UserDetails($username, $email) {
    Write-Host "Username: $username"
    Write-Host "Email: $email"
}

# Вызов функции с передачей аргументов
Get-UserDetails "john_doe" "john@example.com"
```

При вызове этого кода вы увидите вывод, отображающий переданные аргументы функции:

```
Username: john_doe
Email: john@example.com
```

В этом примере аргументы функции `Get-UserDetails` (`$username` и `$email`) получаются без использования ключевого слова `param`.

---
Конечно! Вот ещё несколько примеров для работы с функциями и аргументами в PowerShell без использования ключевого слова `param`:

1. Объявление функции с использованием скобок для указания аргументов:

```powershell
function Show-Message {
    [CmdletBinding()]
    param (
        [string]$message
    )
    Write-Host $message
}

# Вызов функции
Show-Message "Hello, World!"
```

2. Использование встроенной переменной `args` для доступа к переданным аргументам:

```powershell
function Show-Arguments {
    Write-Host "Received arguments:"
    foreach ($arg in $args) {
        Write-Host $arg
    }
}

# Вызов функции с аргументами
Show-Arguments "arg1" "arg2" "arg3"
```

3. Использование специальных переменных `$args` для доступа ко всем переданным аргументам:

```powershell
function Show-AllArguments {
    Write-Host "Received arguments:"
    foreach ($arg in $args) {
        Write-Host $arg
    }
}

# Вызов функции с аргументами
Show-AllArguments "arg1" "arg2" "arg3" "arg4"
```

Надеюсь, это поможет вам лучше понять разнообразные способы работы с функциями и аргументами в PowerShell!

