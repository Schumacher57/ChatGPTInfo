Привет! В PowerShell можно запускать другие скрипты (как PowerShell, так и batch) и передавать в них параметры. Я покажу несколько примеров, как это сделать.

### 1. Запуск других PowerShell скриптов с параметрами

#### Пример:
Предположим, у вас есть два скрипта:

- **`Script1.ps1`**: основной скрипт, который вызывает другие скрипты.
- **`Script2.ps1`**: вспомогательный скрипт, который принимает параметры.

```powershell
# Script2.ps1
param (
    [string]$Name,
    [int]$Age
)

Write-Host "Hello, $Name! You are $Age years old."
```

Теперь в **`Script1.ps1`** вы можете запустить `Script2.ps1` и передать в него параметры:

```powershell
# Script1.ps1
$scriptPath = ".\Script2.ps1"  # Укажите путь к другому скрипту
$name = "Alice"
$age = 30

# Запуск другого PowerShell скрипта с параметрами
& $scriptPath -Name $name -Age $age
```

Здесь:
- Символ **`&`** используется для выполнения скрипта.
- Параметры передаются с помощью `-Name` и `-Age`.

### 2. Запуск batch-файлов с параметрами

PowerShell также позволяет запускать **batch**-файлы (.bat или .cmd) и передавать в них параметры.

#### Пример batch-файла:
Предположим, у вас есть batch-файл **`Script.bat`**, который принимает два параметра:

```batch
@echo off
echo Parameter 1: %1
echo Parameter 2: %2
pause
```

Теперь в **`Script1.ps1`** вы можете запустить этот batch-файл и передать параметры:

```powershell
# Script1.ps1
$batchPath = ".\Script.bat"  # Укажите путь к batch-файлу
$param1 = "Value1"
$param2 = "Value2"

# Запуск batch-файла с параметрами
& $batchPath $param1 $param2
```

Здесь PowerShell передает параметры **`Value1`** и **`Value2`** в batch-файл как `%1` и `%2`.

### 3. Использование вызова скриптов через `Start-Process` (если требуется запуск в новом процессе)

Если нужно запустить PowerShell или batch-скрипт в новом процессе (например, в отдельном окне), вы можете использовать команду **`Start-Process`**:

#### Пример для PowerShell скрипта:

```powershell
# Script1.ps1
$scriptPath = ".\Script2.ps1"
$name = "Bob"
$age = 25

# Запуск PowerShell скрипта в новом процессе
Start-Process powershell.exe -ArgumentList "-File $scriptPath -Name $name -Age $age"
```

#### Пример для batch-файла:

```powershell
# Script1.ps1
$batchPath = ".\Script.bat"
$param1 = "ValueA"
$param2 = "ValueB"

# Запуск batch-файла в новом процессе
Start-Process cmd.exe -ArgumentList "/c $batchPath $param1 $param2"
```

### Объяснение:
- **`&`**: Используется для запуска PowerShell и batch-скриптов внутри одного процесса.
- **`Start-Process`**: Запускает процесс отдельно от текущей сессии, что полезно для выполнения задач в фоновом режиме или открытия нового окна командной строки.

### Заключение

Таким образом, PowerShell позволяет легко запускать как PowerShell, так и batch-скрипты, передавая параметры, используя символ **`&`** или команду **`Start-Process`**, если требуется отдельный процесс.