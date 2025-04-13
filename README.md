# iserv_test

### Запуск
1. Склонируйте репозиторий 
```
git clone https://github.com/dimankan/iserv_test.git
```
2. Запустите локально у себя докер
3. Откройте в powershell / cmd по адресу куда склонировали текущий репозиторий
4. Выполните команду 
```
docker-compose up
```
5. Перейдите по адресу предложенного порта в программе Docker Desktop. Необходимо выбрать запущенный контейнер и кликнуть по порту образа 'app-1'.
```
http://localhost:8080/index.html
```
6. Должен выйти Swagger. Тут можно протестировать работоспособность.

### Скрипт и БД.
Явно создавать объекты БД не нужно.
При запуске контейнера, будет использована миграция. И создаться таблица.
Фактически отработает следующий запрос 
```
CREATE TABLE "Universities" (
"Id" integer GENERATED BY DEFAULT AS IDENTITY,
"Country" text NOT NULL,
"Name" text NOT NULL,
"Websites" text NOT NULL,
CONSTRAINT "PK_Universities" PRIMARY KEY ("Id")
);
```

Также обращу внимание, что при каждом запуске контейнера БД будет удаляться и создаваться заново.
