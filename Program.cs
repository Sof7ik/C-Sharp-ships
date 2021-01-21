using System;
//using System.Reflection;

namespace TankiOnline
{
    enum Actions
    {
        Огонь,
        Починка
    }

    interface ITechnic
    {
        int Armor { get; set; }
        int CurrentHP { get; set; }
        int Damage { get; set; }
        private int TotalHP { get { return TotalHP; } }
        void Shoot(Ship enemy);
        void Repair();
    }

    class Ship : ITechnic
    {
        public int Armor { get; set; }
        public int CurrentHP { get; set; }
        public int Damage { get; set; }
        private int TotalHP { get; set; }

        public Ship (int armor, int healthPoints, int damage)
        {
            Armor = armor;
            TotalHP = healthPoints;
            CurrentHP = TotalHP;
            Damage = damage;
        }

        public void Shoot(Ship enemy)
        {
            enemy.CurrentHP -= this.Damage;
            enemy.Armor += 10;
        }

        public void Repair()
        {
            if ((this.CurrentHP + 10) <= this.TotalHP)
            {
                this.CurrentHP += 10;
            }
            else if ((this.CurrentHP + 10) > this.TotalHP && this.CurrentHP != this.TotalHP)
            {
                this.CurrentHP = this.TotalHP;
            }
            else if (this.CurrentHP == this.TotalHP)
            {
                Console.WriteLine("Вы не можете починиться, потому у вас Full HP");
            }
        }

        public int GetHP ()
        {
            return this.CurrentHP;
        }

        public int GetTotalHP() 
        {
            return this.TotalHP;
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
                        byte listNumber = 1;
                        foreach (Actions action in Enum.GetValues(typeof(Actions)))
                        {
                            Console.WriteLine($"{listNumber}. {action}");
                            listNumber++;
                        }
                        //Console.WriteLine($"1. Огонь");
                        //Console.WriteLine($"2. Ремонт");

                        //Type myType = Type.GetType("TankiOnline.Ship", false, true);
                        //MethodInfo[] methods = myType.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

                        //Console.WriteLine($"{methods.Length}");

                        //foreach (MemberInfo mi in methods)
                        //{
                            //Console.WriteLine($"{mi}");
                        //}

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
                            //Console.WriteLine("КОМПЬЮТЕР СТРЕЛЯЕТ");
                            computer.Shoot(user);
                        }
                        else if (randValue == 1)
                        {
                            //Лечимся
                            //Console.WriteLine("КОМПЬЮТЕР ХИЛЛИТСЯ");
                            if (computer.GetHP() == computer.GetTotalHP())
                            {
                                computer.Shoot(user);
                            }
                            else
                            {
                                computer.Repair();
                            }
                        }
                        currentPlayer = 0;
                        break;
                    default:
                        Console.WriteLine("Неизвестный игрок!");
                        break;
                }

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
