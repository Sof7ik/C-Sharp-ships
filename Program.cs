using System;

namespace TankiOnline
{
    // Перечисление действий игроков.
    enum Actions
    {
        Огонь,
        Починка,
        Купить_патроны
    }

    // Описание класса техники, на которой игроки будут играть.
    interface ITechnic
    {
        int armor { get; set; }
        double currentHP { get; set; }
        float damage { get; set; }
        double critDamage { get; set; }
        private int totalHP { get { return totalHP; } }
        public int bullets { get; set; }
        void Shoot(Ship enemy);
        void Repair(int HPToHealth);
        int BuyBullets(int newBullets);
    }

    // Класс техники, который применяет вышенаписанный интерфейс.
    class Ship : ITechnic
    {
        // Броня корабля.
        public int armor { get; set; }

        // Текущее количество жизней корабля.
        public double currentHP { get; set; }

        // Урон, наносимый кораблем.
        public float damage { get; set; }

        // Калькуляция критического урона корабля.
        public double critDamage { get; set; }

        // Количество патронов.
        public int bullets { get; set; }

        // Максимально возможное количество жизней корабля.
        private int totalHP { get; set; }

        // Конструктор.
        public Ship (int armor, int healthPoints, int damage, int bullets)
        {
            this.armor = armor;
            this.totalHP = healthPoints;
            this.currentHP = totalHP;
            this.damage = damage;
            this.critDamage = this.damage * 0.20;
            this.bullets = bullets;
        }

        // Метод "Выстрел".
        public void Shoot(Ship enemy)
        {
            // Отнимаем 1 пулю
            this.bullets -= 1;
            // вероятности
            Random probability = new Random();
            int probabilityValue = probability.Next(10);

            // 1 из 10 = 10% - вероятность крита.
            if (probabilityValue == 1)
            {
                Console.WriteLine("Критическое попадание!");
                enemy.currentHP -= this.damage + critDamage;
            }
            // 2 из 10 = 20% - вероятность промаха.
            else if (probabilityValue == 2 || probabilityValue == 3)
            {
                Console.WriteLine("Промах");
            }
            // Обычный выстрел.
            else
            {
                enemy.currentHP -= this.damage;
            }
            // Увеличиваем броню
            enemy.armor += 10;
        }

        // Метод "Починка"
        public void Repair(int HPToHeath = 10)
        {
            // Обычное восстановление здоровья, все нормально.
            if ((this.currentHP + HPToHeath) <= this.totalHP)
            {
                this.currentHP += HPToHeath;
            }
            // Если при восстановлении жизней их количество превысит максимально возможное.
            else if ((this.currentHP + HPToHeath) > this.totalHP && this.currentHP != this.totalHP)
            {
                this.currentHP = this.totalHP;
            }
            // Если у игрока максимальное количество жизней.
            else if (this.currentHP == this.totalHP)
            {
                Console.WriteLine("Вы не можете починиться, потому у вас Full HP");
            }
        }

        // Метод "Купить патроны"
        public int BuyBullets (int newBullets = 5)
        {
            this.bullets += newBullets;
            return this.bullets;
        }

        // Получить текущее количество жизней
        public double GetHP ()
        {
            return this.currentHP;
        }

        // Получить максимально возможное количество жизней
        public int GetTotalHP() 
        {
            return this.totalHP;
        }

        // Получить текущее количество пуль
        public int GetBullets()
        {
            return this.bullets;
        }
    }

    class Program
    {
        // функция для отображения боевой инфорации
        static void GetBothHP(Ship user, Ship computer) 
        {
            Console.WriteLine($"Количество жизней компьютера = {computer.GetHP()}, количество патронов = {computer.GetBullets()}");
            Console.WriteLine($"Количество жизней игрока = {user.GetHP()}, количество патронов = {user.GetBullets()} \n");
        }

        static void Main(string[] args)
        {
            // Активный игрок. 0 - человек, 1 - компьютер.
            byte currentPlayer = 0;

            // Индикатор порядкового номера хода.
            int hod = 1;

            // Создаем корабль для пользователя.
            Ship user = new Ship(armor:100, healthPoints:250, damage:10, bullets:20);

            // Создаем корабль для компьютера.
            Ship computer = new Ship(armor:120, healthPoints:250, damage:15, bullets:12);

            // Главный экран приветствия.
            Console.WriteLine("Добро пожаловать в Морской Бой! \n");

            // Играем постоянно
            while (true)
            {
                Console.WriteLine($"====ХОД №{hod}==== {(currentPlayer == 0 ? "ИГРОК" : "КОМПЬЮТЕР")} \n");

                GetBothHP(user, computer);

                // выбор игрока - компьютер или человек.
                switch (currentPlayer)
                {
                    // Ходит живой человек.
                    case 0:
                        Console.WriteLine($"Выберите требуемое действие:");
                        byte listNumber = 1;
                        foreach (Actions action in Enum.GetValues(typeof(Actions)))
                        {
                            Console.WriteLine($"{listNumber}. {action}");
                            listNumber++;
                        }

                        string act = Console.ReadLine();

                        switch (act)
                        {
                            case "1":
                                Console.WriteLine("ИГРОК СТРЕЛЯЕТ");

                                if(user.GetBullets() > 0)
                                {
                                    user.Shoot(computer);
                                }
                                else
                                {
                                    Console.WriteLine("Необходимо купить патроны для выстрела");
                                }
                                break;
                            case "2":
                                Console.WriteLine("ИГРОК ХИЛЛИТСЯ");
                                user.Repair();
                                break;
                            case "3":
                                Console.WriteLine("ИГРОК ПОКУПАЕТ ПАТРОНЫ");
                                user.BuyBullets();
                                break;
                            default:
                                Console.WriteLine("Неизвестное действие!");
                                break;
                        }
                        currentPlayer = 1;
                        break;
                    // Ходит компьютер
                    case 1:
                        // Выбор действия.
                        Random randElem = new Random();
                        int randValue = randElem.Next(3);

                        // Стреляем.
                        if (randValue == 0)
                        {
                            if (computer.GetBullets() > 0)
                            {
                                Console.WriteLine("КОМПЬЮТЕР СТРЕЛЯЕТ");
                                computer.Shoot(user);
                            }
                            else
                            {
                                Console.WriteLine("У КОМПЬЮТЕРА НЕТ ПУЛЬ. ПОКУПАЕТ");
                                computer.BuyBullets();
                            }
                        }
                        // Лечимся.
                        else if (randValue == 1)
                        {
                            Console.WriteLine("КОМПЬЮТЕР ХИЛЛИТСЯ");
                            if (computer.GetHP() == computer.GetTotalHP())
                            {
                                computer.Shoot(user);
                            }
                            else
                            {
                                computer.Repair();
                            }
                        }
                        // Покупаем пули.
                        else if(randValue == 2)
                        {
                            Console.WriteLine("КОМПЬЮТЕР ПОКУПАЕТ ПАТРОНЫ");
                            computer.BuyBullets();
                        }
                        currentPlayer = 0;
                        break;
                    default:
                        Console.WriteLine("Неизвестный игрок!");
                        break;
                }

                GetBothHP(user, computer);

                // Вывод сводной информации по игре
                if (user.GetHP() <= 0 || computer.GetHP() <= 0)
                {
                    Console.WriteLine($"Игра окончена! {(user.GetHP() > 0 ? "Победитель - человек" : "Победитель компьютер")}");

                    Console.WriteLine($"Количество жизней компьютера = {computer.GetHP()}");
                    Console.WriteLine($"Количество жизней игрока = {user.GetHP()} \n");
                    break;
                }
                else
                {
                    hod++;
                }
            }
        }
    }
}
