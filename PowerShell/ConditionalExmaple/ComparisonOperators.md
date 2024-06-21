В PowerShell существует несколько операторов сравнения, которые позволяют сравнивать значения. Вот некоторые из них:

1. `-eq` : Оператор эквивалентности (Equal). Проверяет, равны ли два объекта.
2. `-ne` : Оператор неравенства (Not Equal). Проверяет, не равны ли два объекта.
3. `-gt` : Оператор больше (Greater Than). Проверяет, является ли один объект больше другого.
4. `-lt` : Оператор меньше (Less Than). Проверяет, является ли один объект меньше другого.
5. `-ge` : Оператор больше или равно (Greater Than or Equal). Проверяет, является ли один объект больше или равен другому.
6. `-le` : Оператор меньше или равно (Less Than or Equal). Проверяет, является ли один объект меньше или равен другому.
7. `-like` : Оператор подобия (Like). Проверяет, соответствует ли строка шаблону.
8. `-notlike` : Оператор не подобия (Not Like). Проверяет, не соответствует ли строка шаблону.

Примеры использования:
```powershell
# Проверка эквивалентности
if ($a -eq $b) {
    Write-Host "a equals b"
}

# Проверка больше
if ($x -gt $y) {
    Write-Host "x is greater than y"
}
```

Эти операторы сравнения могут быть использованы в условных выражениях `IF`, циклах `WHILE` и `FOR`, а также в других местах, где требуется выполнение сравнения. 📊

---

Конечно! Вот примеры использования каждого оператора сравнения в PowerShell:

1. Оператор эквивалентности (`-eq`):
```powershell
$a = 10
$b = 10
if ($a -eq $b) {
    Write-Host "a equals b"
}
```

2. Оператор неравенства (`-ne`):
```powershell
$name1 = "Alice"
$name2 = "Bob"
if ($name1 -ne $name2) {
    Write-Host "Names are not equal"
}
```

3. Оператор больше (`-gt`):
```powershell
$age1 = 25
$age2 = 30
if ($age2 -gt $age1) {
    Write-Host "age2 is greater than age1"
}
```

4. Оператор меньше (`-lt`):
```powershell
$price1 = 50
$price2 = 70
if ($price1 -lt $price2) {
    Write-Host "price1 is less than price2"
}
```

5. Оператор больше или равно (`-ge`):
```powershell
$number1 = 100
$number2 = 100
if ($number1 -ge $number2) {
    Write-Host "number1 is greater than or equal to number2"
}
```

6. Оператор меньше или равно (`-le`):
```powershell
$score1 = 80
$score2 = 85
if ($score1 -le $score2) {
    Write-Host "score1 is less than or equal to score2"
}
```

7. Оператор подобия (`-like`):
```powershell
$name = "John"
if ($name -like "J*") {
    Write-Host "Name starts with letter J"
}
```

8. Оператор не подобия (`-notlike`):
```powershell
$fruit = "apple"
if ($fruit -notlike "*orange*") {
    Write-Host "The fruit is not orange"
}
```

Эти примеры демонстрируют основные способы использования каждого оператора сравнения в PowerShell. 🚀
