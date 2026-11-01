using System;
using System.Threading;

namespace GladiatorArena
{
    class Program
    {
        static void Main(string[] sender)
        {
            Console.WriteLine("=== ДОБРО ПОЖАЛОВАТЬ НА АРЕНУ ГЛАДИАТОРОВ ===\n");

            // Создаем двух бойцов: Имя, Здоровье, Макс. Урон, Броня, Прочность брони
            Gladiator glad1 = new Gladiator("Спартак", 100, 25, 15, 50);
            Gladiator glad2 = new Gladiator("Максимус", 110, 22, 10, 60);

            glad1.ShowStats();
            glad2.ShowStats();
            Console.WriteLine("\nБИТВА НАЧИНАЕТСЯ! Нажмите любую клавишу...");
            Console.ReadKey();
            Console.Clear();

            Random random = new Random();
            int round = 1;

            // Цикл боя, пока оба живы
            while (glad1.IsAlive && glad2.IsAlive)
            {
                Console.WriteLine($"--- РАУНД {round} ---");

                // Случайный выбор, кто бьет первым в этом раунде
                if (random.Next(0, 2) == 0)
                {
                    glad1.Attack(glad2, random);
                    if (glad2.IsAlive) glad2.Attack(glad1, random);
                }
                else
                {
                    glad2.Attack(glad1, random);
                    if (glad1.IsAlive) glad1.Attack(glad2, random);
                }

                Console.WriteLine("\nСостояние бойцов:");
                glad1.ShowStats();
                glad2.ShowStats();
                Console.WriteLine("\nНажмите клавишу для следующего раунда...");
                Console.ReadKey();
                Console.Clear();
                round++;
            }

            // Объявление победителя
            Console.WriteLine("=== БОЙ ОКОНЧЕН ===");
            if (glad1.IsAlive)
                Console.WriteLine($"Победил {glad1.Name}! Слава победителю!");
            else if (glad2.IsAlive)
                Console.WriteLine($"Победил {glad2.Name}! Слава победителю!");
            else
                Console.WriteLine("Оба гладиатора пали в жестоком бою! Ничья!");

            Console.ReadLine();
        }
    }

    class Gladiator
    {
        public string Name { get; private set; }
        public int Health { get; private set; }
        public int MaxDamage { get; private set; }
        public int ArmorValue { get; private set; } // Сколько урона блокирует броня (в %)
        public int ArmorDurability { get; private set; } // Прочность брони

        public bool IsAlive => Health > 0;

        public Gladiator(string name, int health, int maxDamage, int armorValue, int armorDurability)
        {
            Name = name;
            Health = health;
            MaxDamage = maxDamage;
            ArmorValue = armorValue;
            ArmorDurability = armorDurability;
        }

        public void Attack(Gladiator target, Random rand)
        {
            // Сила удара случайная от 5 до MaxDamage
            int rawDamage = rand.Next(5, MaxDamage + 1);
            Console.WriteLine($" {Name} замахивается и наносит удар силой в {rawDamage} единиц!");

            target.TakeDamage(rawDamage);
        }

        public void TakeDamage(int rawDamage)
        {
            int finalDamage = rawDamage;

            // Если броня еще цела, она блокирует часть урона
            if (ArmorDurability > 0)
            {
                // Считаем сколько урона заблокировано (ArmorValue — это процент поглощения)
                int blockedDamage = (rawDamage * ArmorValue) / 100;
                finalDamage = rawDamage - blockedDamage;

                // Прочность брони снижается от силы принятого удара
                int armorWear = Math.Max(1, rawDamage / 4);
                ArmorDurability -= armorWear;

                if (ArmorDurability < 0) ArmorDurability = 0;

                Console.WriteLine($"️Броня {Name} заблокировала {blockedDamage} ед. урона (Прочность брони: {ArmorDurability}).");
            }
            else
            {
                Console.WriteLine($"Броня {Name} полностью сломана! Нанесен чистый урон!");
            }

            // Наносим итоговый урон здоровью
            Health -= finalDamage;
            if (Health < 0) Health = 0;

            Console.WriteLine($"{Name} получает {finalDamage} чистого урона. Осталось здоровья: {Health}.");
        }

        public void ShowStats()
        {
            Console.WriteLine($"[ {Name} ] Здоровье: {Health} | Броня: Поглощение {ArmorValue}%, Прочность {ArmorDurability}");
        }
    }
}
