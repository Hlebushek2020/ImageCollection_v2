# Image Collection
**Image Collection** - программа, которая поможет распределить фотографии (картинки) по соответствующим коллекциям (папкам).  

В текущей версии программы перемещение файлов в коллекцию происходит сразу.

[Версия для Android](https://github.com/Hlebushek2020/ImageCollection_Android)

## Содержание
- [Меню](#меню)
  - [Файл](#файл)
  - [Коллекция](#коллекция)
  - [Сортировка](#сортировка)
- [Roadmap](#roadmap)

## Меню

### Файл
- **Открыть папку** `Ctrl + O` - открывает окно для выбора папки, которую нужно открыть в программе. Вложенные папки становятся коллекциями.  
- **Удалить** `Delete` - удалить текущие выбранные файлы без возможности восстановления.  
- **Переименовать** `F2` - переименовать выбранные файлы. Если выбрано несколько файлов, то в открывшимся окне вводится шаблон имени файла, где вместо конструкции _{0}_ будет подставлен порядковый номер.
- **Добавить коллекцию** `Ctrl + Shift + A` - открыть окно для выбора коллекции, в которую будут перемещены выбранные файлы.
- **Найти в интернете** `Ctrl + Shift + S` - Выполняет заданную команду поиска в настройках, где конструкция _{0}_ заменяется на путь к выбранному файлу.
- **Настройки** - открывает окно с настройками программы.
### Коллекция
- **Создать** `Ctrl + N` - создать новую коллекцию.
- **Переименовать** `Ctrl + F2` - переименовать текущую коллекцию.
- **Удалить** `Ctrl + Delete` - удалить текущую коллекцию. В зависимости от настроек программы эта функция работает по-разному.
- **Переименовать файлы** `Ctrl + Shift + F2` - переименовать все файлы текущей коллекции. В открывшимся окне вводится шаблон имени файла, где вместо конструкции _{0}_ будет подставлен порядковый номер.
- **Горячие клавиши** `Ctrl + H` - открывает окно для настройки горячих клавиш быстрого перемещения выбранных файлов в ту или иную коллекцию.
### Сортировка
- **Нет** `Alt + Shift + 0` - отображать файлы, так как они были добавлены в коллекцию.
- **Имя** `Alt + Shift + 1` - отображать файлы, отсортировав их по имени.
- **Размер** `Alt + Shift + 2` - отображать файлы, отсортировав их по размеру.
- **Разрешение** `Alt + Shift + 3` - отображать файлы, отсортировав их по разрешению.

## Roadmap
- [x] Распределение файлов по коллекциям в реальном времени (файлы сразу перемещаются в нужные папки) `v2.0.0.0`
- [x] Фильтры (сортировка) `v2.0.0.0`
- [ ] ~~Распределение файлов по коллекциям по кнопке (файлы перемещаются в нужные папки по нажатию кнопки "Распределить")~~
- [x] Сохранение последней сессии `v2.0.1.0`
- [ ] Слияние коллекций
- [ ] Окно свойств (атрибутов) файла

