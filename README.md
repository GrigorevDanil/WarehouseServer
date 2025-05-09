# Сервер Склад

Сервер предоставляет REST API для управления складскими запасами и решает транспортную задачу методом потенциалов для минимизации затрат на перевозки.

# Этот блог пишу после проверки!

Пишу это находясь на 2-м стриме

## О хакатоне

Было очень приятно в нем поучавствовать и было интересно посмотреть на реализацию других участников, в будущем некоторые моменты можно использовать. Понравилось что Кирилл останавливается на каждом участнике и рассказывает как стоит делать, что лучше не делать, то что не забивает на чат и общается с каждым. А также что даже если просто поучаствовал получаешь бонус в виде скидки. Для меня это конечно очень приятный опыт и буду учавствовать еще!

## Обо мне

Я студент, мне 19 лет. Сейчас прохожу заключительный 4 курс. Немного пришлось разорваться на этом хакатоне, так как не успеваю писать дипломную работу и готовиться к демонстрационному экзамену. Но я очень хотел в нем поучавствовать. Начал свой путь программиста в 17 лет, точнее на 2-м курсе нас обучали C++, позже перешли на C#. Писали проекты WPF и WinForm, работали с бд MySql и SQLite, еще занимались мобильной разработкой на андроид. Нас не учили архитектуре, работой с EF и серверной разработкой. Все что я написал, я узнал как это делать только от Кирилла за что я ему благодарен.

## Если выйграю курс

Я бы все равно не смог им воспользоваться после победы, так как нужно отдать долг родине), так что я надеялся вспомнить и подтянуть знания после армии (Боюсь я убью все свои знания за это время).

## Почему не написал сразу?

Просто не успел. Хотел больше описать функционала что бы было видно мою работу, а не то что я из себя представляю)

## Если интересно что за диплом

Меня попросили преподаватели написать им мобильное приложение расписание занятий которое работает с 1С сервером, а также подсистему с журналом пропуском. Пишу на React Native

# Транспортная задача

Транспортная задача — это классическая задача в области линейного программирования и оптимизации, которая заключается в нахождении наиболее эффективного способа распределения ресурсов (например, товаров, материалов) между несколькими источниками (поставщиками) и несколькими пунктами назначения (потребителями) с целью минимизации общих затрат на транспортировку.

Для проверки правильности решения был использован сервис по ссылке https://math.semestr.ru/transp/index.php

# О предметной области

1. Продукты – Хранение информации о товарах, которые производятся или продаются.

2. Ресурсы – Учет материалов или компонентов, из которых состоят продукты.

3. Склады – Управление местами хранения товаров и ресурсов.

4. Магазины – Учет точек продаж, куда поставляется продукция.

5. Состав продуктов – Определение, какие ресурсы и в каком количестве входят в продукт.

6. Остатки на складах – Контроль количества каждого товара на каждом складе.

7. Расстояния – Хранение информации о дистанциях между складами и магазинами для логистики.

# О базе данных

В качестве СУБД была выбрана MySql.

## Импорт данных в бд

Для того что бы появились данные в бд необходимо сделать импорт. 

1. Переходим на адрес http://localhost:8080/

2. Нажимаем на нашу базу данных (слева в проводнике) (также после выполнения миграции)

3. Необходимо удалить все таблицы. Как на скрине

![image](https://github.com/user-attachments/assets/43a98f8a-f44a-4086-a1ae-781731ad91fc)

4. Переходим в импорт

![image](https://github.com/user-attachments/assets/ef9787c4-9e7a-4413-88a8-ec353f667e83)


5. В корне этого проекта есть файлик backup с расширением .sql. его помещаем в "Выбрать файл" и нажимаем кнопку "Импорт"

6. Вуаля!!!

## Cхема базы данных

![image](https://github.com/user-attachments/assets/7bef1724-360e-412c-b4b7-e7b38f1ee805)

# Документация API

Все скриншоты были взяты со Swagger

## Товары (Products)

### `GET /Product/List`
- Получение списка всех товаров без ресурсов
  
![image](https://github.com/user-attachments/assets/771964c4-6017-4ee0-a8f3-c93fec2b1845)


### `GET /Product`
- Получение списка всех товаров с ресурсами
  
![image](https://github.com/user-attachments/assets/85c22026-59c8-490e-87dd-728ecbeda222)


### `POST /Product`
- Добавление нового товара
  
![image](https://github.com/user-attachments/assets/44fc11f3-53a8-43d6-8c98-94eeb5f0157a)
![image](https://github.com/user-attachments/assets/6b475690-db89-45f0-b19c-ba657fa85c3f)

### `GET /Product/{id}`
- Получение конкретного товара с ресурсами по идентификатору

![image](https://github.com/user-attachments/assets/2e0ac8d8-704b-499c-9089-592da40c1f0b)
![image](https://github.com/user-attachments/assets/10432dd8-36b3-4dfb-b6ec-9c235a92a639)


### `PUT /Product/{id}`
- Обновление данных товара

![image](https://github.com/user-attachments/assets/702e231d-2a72-47d6-bbed-89f36f970735)
![image](https://github.com/user-attachments/assets/1e152fb9-27bc-48d5-800a-b23813d1e2b4)


### `DELETE /Product/{id}`
- Удаление товара

![image](https://github.com/user-attachments/assets/8bec9636-161e-4ada-9e7d-92dd1e0beb6a)
![image](https://github.com/user-attachments/assets/f837f518-e130-4c41-806f-5f8d9ad5c32c)


### `POST /Product/{id}/Resources`
- Добавление ресурса к товару

![image](https://github.com/user-attachments/assets/de08b0c8-eeb1-4639-b18d-98548d1f534e)
![image](https://github.com/user-attachments/assets/9302665c-eae2-45d0-8306-78c41d6e085c)


### `PUT /Product/{id}/Resources/{resourceId}`
- Изменение количества ресурса в товаре

![image](https://github.com/user-attachments/assets/00b8cd2f-daf0-485e-9c85-5613f05e0983)
![image](https://github.com/user-attachments/assets/58954abc-7003-4e6c-a5cd-81ccac6bbc35)


### `DELETE /Product/{id}/Resources/{resourceId}`
- Удаление ресурса из товара

![image](https://github.com/user-attachments/assets/3c388c0b-dacf-4fe4-bd2c-ad92408527fd)
![image](https://github.com/user-attachments/assets/79d98436-47a5-411d-8cb1-c243ed506690)


## Ресурсы (Resources)

### `GET /Resource`
- Получение списка всех ресурсов

![image](https://github.com/user-attachments/assets/e6b6fdb5-9280-420d-9c1c-de9d6f4634e9)

### `POST /Resource`
- Добавление нового ресурса

![image](https://github.com/user-attachments/assets/f82a4ed4-7edd-4cb8-87aa-fbcd2beff445)
![image](https://github.com/user-attachments/assets/386b9a5b-4283-42aa-ac17-d104ec875ecb)

### `GET /Resource/{id}`
- Получение конкретного ресурса по идентификатору

![image](https://github.com/user-attachments/assets/826b9964-2715-49f1-9b7d-973ca57e672d)
![image](https://github.com/user-attachments/assets/c7a3f78a-8c17-47f7-841b-ea3d034e5d0a)

### `PUT /Resource/{id}`
- Обновление данных ресурса

![image](https://github.com/user-attachments/assets/aaad77d7-59d3-4399-a72b-c2758652ab4b)
![image](https://github.com/user-attachments/assets/c046b303-03df-433d-acc7-572ccd0426c0)


### `DELETE /Resource/{id}`
- Удаление ресурса

![image](https://github.com/user-attachments/assets/78292f15-c105-4a27-be81-fed8af9168dd)
![image](https://github.com/user-attachments/assets/292b3c5b-2188-4468-8c7b-9253a609c7fb)

## Магазины (Shops)

### `GET /Shop/List`
- Получение списка магазинов без дистанций

![image](https://github.com/user-attachments/assets/6a9903c8-5b0b-4b24-a3bf-902d04723341)


### `GET /Shop`
- Получение списка магазинов с дистанциями

![image](https://github.com/user-attachments/assets/5dada80c-e2b5-4963-9c05-a3a769771568)


### `POST /Shop`
- Добавление нового магазина

![image](https://github.com/user-attachments/assets/cb5c3d51-aa98-4c1e-9f7a-bb818502cc6d)
![image](https://github.com/user-attachments/assets/c5870130-a322-426c-a598-39385a4412b0)


### `GET /Shop/{id}`
- Получение конкретного магазина по идентификатору

![image](https://github.com/user-attachments/assets/5d435392-3a72-4eeb-887c-7a7176b6e51d)
![image](https://github.com/user-attachments/assets/911af8d2-03bd-4d76-a9e9-38afbc119253)


### `PUT /Shop/{id}`
- Обновление данных магазина

![image](https://github.com/user-attachments/assets/7c005ba2-9eaf-40c1-9b87-8d63ead8c70b)
![image](https://github.com/user-attachments/assets/30d8a228-85d4-4542-b370-10da6d7c066b)


### `DELETE /Shop/{id}`
- Удаление магазина

![image](https://github.com/user-attachments/assets/c53cfd53-bf1b-4d4d-b8b0-ab6159fef83e)
![image](https://github.com/user-attachments/assets/cc81f681-bcd7-42c8-b57c-d078091aa4cf)


### `POST /Shop/{id}/Distances`
- Добавление дистанции до склада

![image](https://github.com/user-attachments/assets/80697552-0027-43a7-9ea7-d19aa1990ef5)
![image](https://github.com/user-attachments/assets/1222e972-6683-4e0d-b8c8-50e654150a2d)


### `PUT /Shop/{id}/Distances/{warehouseId}`
- Изменение расстояния до склада

![image](https://github.com/user-attachments/assets/161a8e45-5d59-4b48-b6ab-5c83a166d700)
![image](https://github.com/user-attachments/assets/05536a18-8d1b-49e1-b5c8-717e88b11cdc)


### `DELETE /Shop/{id}/Distances/{warehouseId}`
- Удаление расстояния до склада

![image](https://github.com/user-attachments/assets/4b80881b-ed7d-4a69-989c-a1fefd0f1ac7)
![image](https://github.com/user-attachments/assets/18b374d8-b452-495f-8fd8-85e84b6ed1d8)


### `POST /Shop/TransportMethod`
- Расчет оптимального плана поставок (транспортная задача)

![image](https://github.com/user-attachments/assets/4836bdb2-52ac-4c9b-9886-37a274ee3d70)
![image](https://github.com/user-attachments/assets/1b4a6849-291d-4fe5-a16c-21189d0826f6)
![image](https://github.com/user-attachments/assets/fcd961f9-26c8-4e5c-95f8-da806120f44b)
![image](https://github.com/user-attachments/assets/41e12aba-49b8-4bb6-8f80-c5fb7bc4ee20)


## Склады (Warehouses)

### `GET /Warehouse/List`
- Получение списка складов без товаров

![image](https://github.com/user-attachments/assets/f80cd2e3-e137-45aa-b944-411c8b144187)

### `GET /Warehouse`
- Получение списка складов с товарами

![image](https://github.com/user-attachments/assets/daa5168f-558c-4be1-ba80-0ff40ecc82fe)


### `POST /Warehouse`
- Добавление нового склада

![image](https://github.com/user-attachments/assets/a8f21365-d31b-4da3-9851-b51d42fd876d)
![image](https://github.com/user-attachments/assets/244070bc-4a69-4319-b638-53ecc291ae6e)


### `GET /Warehouse/{id}`
- Получение конкретного склада по идентификатору

![image](https://github.com/user-attachments/assets/4949f928-4fa0-40d5-bb7f-298164ae7aaa)
![image](https://github.com/user-attachments/assets/267503c3-e443-4af6-a793-10565e79a420)


### `PUT /Warehouse/{id}`
- Обновление данных склада

![image](https://github.com/user-attachments/assets/1aa2cda3-a746-4f0a-8dde-2e669b98e6dc)
![image](https://github.com/user-attachments/assets/097487a4-8ac4-48eb-866f-378a21f09f80)


### `DELETE /Warehouse/{id}`
- Удаление склада

![image](https://github.com/user-attachments/assets/5e5bcd94-de58-4b6e-9a28-a361dfa9b32a)
![image](https://github.com/user-attachments/assets/f6b705c4-7171-4ace-a23d-20b04be6e827)

### `POST /Warehouse/{id}/Products`
- Добавление товара на склад

![image](https://github.com/user-attachments/assets/407a394d-00bb-4dec-a56d-2af04426d28a)
![image](https://github.com/user-attachments/assets/647ab31b-707a-41a2-b4a8-9a6c505be7bb)


### `PUT /Warehouse/{id}/Products/{productId}`
- Изменение количества товара на складе

![image](https://github.com/user-attachments/assets/05a5f3df-ea6d-4d96-b6fd-c143d3faf9a8)
![image](https://github.com/user-attachments/assets/26fbbe66-c9e6-4a9a-b1d1-da358e7ffe39)


### `DELETE /Warehouse/{id}/Products/{productId}`
- Удаление товара со склада

![image](https://github.com/user-attachments/assets/57472cac-d234-4048-9f38-bf9ec7fc53bc)
![image](https://github.com/user-attachments/assets/2972e01a-b87a-4e17-a000-39f7786f6171)





