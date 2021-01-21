using System;

namespace TankiOnline
{
    class Ship
    {
        public byte armor;
        public int currentHP;
        public byte damage;
        private int totalHP;

        public Ship (byte armor, int healthPoints, byte damage)
        {
            this.armor = armor;
            this.totalHP = healthPoints;
            this.currentHP = this.totalHP;
            this.damage = damage;
        }

        public void Shoot(Ship enemy)
        {
            enemy.currentHP = (enemy.currentHP - this.damage);
        }

        public void Repair()
        {
            if ((this.currentHP + 10) <= this.totalHP)
            {
                this.currentHP = (this.currentHP + 10);
            }
            else if ((this.currentHP + 10) > this.totalHP && this.currentHP != this.totalHP)
            {
                this.currentHP = this.totalHP;
            }
            else if (this.currentHP == this.totalHP)
            {
                Console.WriteLine("Вы не можете починиться, потому у вас Full HP");
            }
        }

        public int GetHP ()
        {
            return this.currentHP;
        }

        public int GetTotalHP() 
        {
            return this.totalHP;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Активный игрок. 0 - человек, 1 - компьютер.
            byte currentPlayer = 0;

            // Индикатор порядкового номера хода.
            int hod = 1;

            // Создаем корабль для пользователя.
            Ship user = new Ship(armor:100, healthPoints:30, damage:10);

            // Создаем корабль для компьютера.
            Ship computer = new Ship(armor:120, healthPoints:30, damage:15);

            // Главный экран приветствия.
            Console.WriteLine("Добро пожаловать в Морской Бой! \n");

            // Играем постоянно
            while (true)
            {
                Console.WriteLine($"====ХОД №{hod}==== {(currentPlayer == 0 ? "ИГРОК" : "КОМПЬЮТЕР")} \n");
                
                Console.WriteLine($"Количество жизней компьютера = {computer.GetHP()}");
                Console.WriteLine($"Количество жизней игрока = {user.GetHP()} \n");

                switch (currentPlayer)
                {
                    // Ходит живой человек.
                    case 0:
                        Console.WriteLine($"Выберите требуемое действие:");
                        Console.WriteLine($"1. Огонь");
                        Console.WriteLine($"2. Ремонт");

                        string act = Console.ReadLine();

                        switch (act)
                        {
                            case "1":
                                user.Shoot(computer);
                                break;
                            case "2":
                                user.Repair();
                                break;
                            default:
                                Console.WriteLine("Неизвестное действие!");
                                break;
                        }
                        currentPlayer = 1;
                        break;
                    case 1:
                        Random randElem = new Random();
                        int randValue = randElem.Next(2);

                        if (randValue == 0)
                        {
                            //Стреляем
                            Console.WriteLine("КОМПЬЮТЕР СТРЕЛЯЕТ");
                            computer.Shoot(user);
                        }
                        else if (randValue == 1)
                        {
                            //Лечимся
                            Console.WriteLine("КОМПЬЮТЕР ХИЛЛИТСЯ");
                            computer.Repair();
                        }
                        currentPlayer = 0;
                        break;
                    default:
                        Console.WriteLine("Неизвестный игрок!");
                        break;
                }

                if (user.GetHP() <= 0 || computer.GetHP() <= 0)
                {
                    Console.Write($"Игра окончена! {(user.currentHP != 0 ? "Победитель - человек" : "Победитель компьютер")}");
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
