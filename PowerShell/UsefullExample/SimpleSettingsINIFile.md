Конечно! Давайте создадим простой файл конфигурации в формате INI и затем считываем его в PowerShell для использования настроек в скрипте.

### Шаг 1: Создание файла конфигурации

Создайте файл с именем `config.ini` и добавьте в него следующие строки:

```ini
[Settings]
Setting1=Value1
Setting2=Value2
Setting3=Value3
```

### Шаг 2: Считывание файла конфигурации в PowerShell

Для считывания файла конфигурации можно использовать следующий скрипт на PowerShell:

```powershell
# Определяем функцию для чтения INI файла
function Read-IniFile {
    param (
        [string]$Path
    )

    $ini = @{}
    $currentSection = $null

    foreach ($line in Get-Content -Path $Path) {
        $line = $line.Trim()
        if ($line -match '^\[(.+)\]$') {
            $currentSection = $matches[1]
            $ini[$currentSection] = @{}
        } elseif ($line -match '^(.*?)=(.*)$') {
            $key = $matches[1].Trim()
            $value = $matches[2].Trim()
            $ini[$currentSection][$key] = $value
        }
    }

    return $ini
}

# Считываем настройки из конфигурационного файла
$config = Read-IniFile -Path "config.ini"

# Используем настройки в скрипте
$setting1 = $config['Settings']['Setting1']
$setting2 = $config['Settings']['Setting2']
$setting3 = $config['Settings']['Setting3']

# Выводим настройки на экран
Write-Output "Setting1: $setting1"
Write-Output "Setting2: $setting2"
Write-Output "Setting3: $setting3"
```

### Объяснение кода:

1. **Определение функции `Read-IniFile`**:
   - Функция принимает путь к файлу INI в качестве параметра.
   - Используется `Get-Content` для чтения файла построчно.
   - Если строка соответствует шаблону секции (`[Section]`), создается новый раздел в хэш-таблице `$ini`.
   - Если строка соответствует шаблону ключ-значение (`key=value`), добавляется пара ключ-значение в текущую секцию.

2. **Считывание настроек**:
   - Настройки считываются из файла конфигурации `config.ini` и сохраняются в хэш-таблице `$config`.

3. **Использование настроек в скрипте**:
   - Настройки извлекаются из хэш-таблицы `$config` и используются в скрипте.

4. **Вывод настроек**:
   - Настройки выводятся на экран с помощью `Write-Output`.

### Запуск скрипта

Сохраните PowerShell скрипт в файл, например, `script.ps1`, и выполните его:

```powershell
.\script.ps1
```

Вывод будет таким:

```plaintext
Setting1: Value1
Setting2: Value2
Setting3: Value3
```

Этот пример показывает, как создать простой файл конфигурации и использовать его настройки в скрипте на PowerShell.