Конечно, я могу показать примеры использования условных конструкций в PowerShell с комментариями для каждого примера. Вот несколько примеров:

### IF...ELSE:
```powershell
# Пример использования IF...ELSE
$number = 10

if ($number -gt 5) {
    Write-Host "$number is greater than 5"
} 
else {
    Write-Host "$number is less than or equal to 5"
}
```
Комментарий:
- В данном примере мы используем оператор `-gt` для сравнения числа с 5. Если число больше 5, то выводится соответствующее сообщение, иначе - другое.

### WHILE:
```powershell
# Пример использования WHILE
$i = 1

while ($i -le 5) {
    Write-Host "Current value of i is: $i"
    $i++
}
```
Комментарий:
- Этот пример создает цикл, который будет выполняться, пока `$i` будет меньше или равно 5. На каждой итерации цикла значение `$i` увеличивается на 1.

### FOR:
```powershell
# Пример использования FOR
for ($i = 1; $i -le 5; $i++) {
    Write-Host "Current value of i is: $i"
}
```
Комментарий:
- Здесь используется цикл `for`, который выполняется от `$i = 1` до `$i` станет больше 5. На каждой итерации значение `$i` увеличивается на 1, и выводится текущее значение.

### SWITCH:
Вот пример максимально развёрнутого использования оператора `switch` в PowerShell:

```powershell
# Создадим переменную с именем $fruit и установим ее значение
$fruit = "apple"

# Используем оператор switch для сравнения значения переменной $fruit с различными случаями
switch ($fruit) {
    "apple" {
        Write-Host "You selected an apple 🍎"
        # Дополнительные действия, если $fruit равно "apple"
    }
    "banana" {
        Write-Host "You selected a banana 🍌"
        # Дополнительные действия, если $fruit равно "banana"
    }
    "orange" {
        Write-Host "You selected an orange 🍊"
        # Дополнительные действия, если $fruit равно "orange"
    }
    default {
        Write-Host "You selected something else"
        # Действия, если $fruit не соответствует ни одному из остальных случаев
    }
}
```

В этом примере переменная `$fruit` сравнивается с различными случаями с помощью оператора `switch`. Если значение переменной соответствует какому-либо из случаев, выполняются соответствующие действия. Если нет соответствия, выполняются действия включенные в блок `default`.

Также можно добавить условия с использованием операторов `-or` и `-and`:

```powershell
switch ($fruit) {
    "apple" {
        Write-Host "You selected an apple 🍎"
    }
    "banana" {
        Write-Host "You selected a banana 🍌"
    }
    "orange" {
        Write-Host "You selected an orange 🍊"
    }
    default {
        if ($fruit -eq "pear" -or $fruit -eq "grapefruit") {
            Write-Host "You selected a pear or a grapefruit"
        }
        elseif ($fruit -like "*berry") {
            Write-Host "You selected a berry"
        }
        else {
            Write-Host "You selected something else"
        }
    }
}
```

Такой подход позволяет более гибко управлять логикой сравнения и выполнять соответствующие действия в зависимости от результатов сравнения.



