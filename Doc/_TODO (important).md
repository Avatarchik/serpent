- Мигрировать JIRA из Attlassian Cloud на AWS
    - **[CURRENT]** Дочитать о [Meteor & AWS case study](https://aws.amazon.com/solutions/case-studies/meteor-development-group/)
    - Оплатить JIRA Server
    - Следовать [гайду](https://confluence.atlassian.com/adminjiraserver071/migrating-jira-applications-to-another-server-802592269.html)
    - Настроить Docker Compose
    - Поставить на Elastic Compute Cloud с помощью [EC2 Container Service](https://aws.amazon.com/ecs/)
        - Попробовать сперва t2.micro, если не подойдёт - выбрать spot instance покрупнее
        - Оптимизировать для дешевизны
            - Посмотреть советы в официальной документации и в гугле
            - Настроить уход в спящий режим при простое инстанса в течение 10 минут
            - Использовать managed DB
- Поискать музыку
    - На last.fm: outer echo, utkowski, enjoii

# Минимальный мультиплеер
- ~~Закоммитить, что есть~~
- ~~Ввод ника в главном меню~~
    - Поддержка Unicode: ```♫♪•∪P∎R∅⏄ᴉY`K∅ↁ∄R°◐‿◑```
- Спаун нескольких змей с разными скинами
- Определение столкновений
- Исправить камерy

# Потом
- Поддержка джойстика на компе
- Обновить third-party библиотеки
- Реорганизация файловой иерархии
    - Реорганизовать /Doc - распределить файлы по подпапкам
    - Собрать на рабочем компе
    - Вынести `/Dev Assets` из репозитория в отдельный репозиторий
        - Перенести также историю git или хотя бы создать таск на будущее
    - Сделать /Doc частью ../Dev Assets
- Обновить лого на то, что я нарисовал
- Анимация лого

# Совсем потом
- Посмотреть UniRx
- Сделать WebGL сборку
