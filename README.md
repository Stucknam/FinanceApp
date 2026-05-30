# FinanceApp - Мои финансы
Минималистичный, но мощный менеджер личных финансов с красивым консольным интерфейсом.  
Проект создан как учебный + портфолио, но архитектура полностью продакшен‑уровня.  

## Функциональность
- Управление счетами (создание, редактирование, удаление)  
- Учёт доходов и расходов  
- Переводы между счетами (отдельная сущность Transfer)  
- Просмотр последних операций и переводов  
- Цветной консольный UI с таблицами и форматированием  
- Чистая архитектура и репозитории с поддержкой Query()  
- Полноценный REST API
- Возможность расшмрения до полноценного UI (UI в разработке)  

## Архитектура проекта
- **Domain** — сущности, enum'ы, контракты  
- **Data** — EF Core, репозитории, миграции, DTO, мапперы  
- **Application** — прикладная логика, работа с доменом  
- **ConsoleUI** — консольный интерфейс, меню, вывод таблиц  
- **API** - Minimal API
- **WPF** — Windows-клиент с расширенным функционалом (змаорожено, планируется)  

## Технологии
- .NET 10  
- EF Core  
- PostgreSQL
- Minimal API
- SQLite (частично)  
- MAUI (планируется)  
- WPF (заморожено)  
- Репозиторный паттерн + Query()  

## Roadmap
- [x] Консольный UI  
- [x] Введение DTO  
- [x] REST API
- [ ] Добавление Unit тестов
- [ ] MAUI
- [x] SQLite хранение
- [ ] WPF 
- [ ] Импорт/Экспорт данных


## WPF Status

WPF‑клиент временно заморожен.  
Основная разработка ведётся в консольном UI и MAUI.

## Запуск проекта (Console UI)
1. Клонировать репозиторий
git clone https://github.com/Stucknam/FinanceApp.git  
cd FinanceApp  

2. Перейти в консольный проект  
cd FinanceApp.ConsoleUI  

3. Восстановить зависимости  
dotnet restore  

### Настройка PostgreSQL
1. Установить PostgreSQL
Скачать можно с официального сайта:  
https://www.postgresql.org/download/  

2. Создать базу данных
После установки открой psql или PgAdmin и создай БД:  

CREATE DATABASE financeapp;  

3. Создать пользователя (если нужно)

CREATE USER finance_user WITH PASSWORD 'your_password';  
GRANT ALL PRIVILEGES ON DATABASE financeapp TO finance_user;  

### Настройка строки подключения
Открой файл:  
FinanceApp.ConsoleUI/appsettings.json  
И укажи строку подключения:

json  
{  
  "ConnectionStrings": {  
    "DefaultConnection": "Host=localhost;Port=5432;Database=financeapp;Username=finance_user;Password=your_password"  
  }  
}  

Важно:  

Имя БД, пользователя и пароль должны совпадать с тем, что ты создал в PostgreSQL.  

Если PostgreSQL работает в Docker — укажи Host=postgres или имя контейнера.  

### Применение миграций
Если миграции уже созданы:  

dotnet ef database update  

Если миграций нет — создать:  

dotnet ef migrations add InitialCreate  
dotnet ef database update  
### Запуск приложения  
dotnet run  
После запуска откроется главное меню консольного интерфейса.  

### Запуск через Visual Studio
Открыть FinanceApp.sln  

Выбрать проект FinanceApp.ConsoleUI  

ПКМ → Set as Startup Project  

Нажать F5 или Ctrl + F5  

### Запуск проекта (API)
1. Перейти в папку API

cd FinanceApp.API

2. Восстановить зависимости

dotnet restore

3. Настроить строку подключения (аналогично ConsoleUI)

4. Применить миграции (аналогично ConsoleUI)

5. Запустить API

dotnet run

После запуска API будет доступен по адресу:
http://localhost:5245
https://localhost:7049

Для открытия swagger нужно перейти по адресу:
http://localhost:5245/swagger
https://localhost:7049/swagger

6. Запуск через Visual Studio / Rider
Открыть FinanceApp.sln

Выбрать проект FinanceApp.Api

ПКМ → Set as Startup Project

Запустить (F5 или Ctrl+F5)

## Лицензия

MIT

