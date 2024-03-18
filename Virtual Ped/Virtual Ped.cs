using System;
using System.Collections.Generic;
using System.IO;
using System.Timers;
using Newtonsoft.Json;

namespace Virtual_Ped
{
    public class Virtual_Ped
    {
        private Timer timer = new Timer();
        public string Name { get; set; }
        public int Count { get; set; } = 0;
        public int Full { get; set; } = 650;
        public int Eating { get; set; } = 0;
        public int Hour { get; set; } = 0;
        public int Poop { get; set; } = 0;
        public int Sick { get; set; } = 0;
        public int boredom { get; set; } = 0;
        public int ChanceCheck { get; set; } = 0;
        public string Space { get; set; } = "Left";
        public string NextSpace { get; set; } = "Left";
        public string Hunger { get; set; } = "Zufrieden";
        public string Stage { get; set; } = "Ei";
        public Random Chance { get; set; } = new Random();
        public bool isSleeping { get; set; } = false;

        public bool isEating { get; set; } = false;

        public bool pooped { get; set; } = false;

        public Virtual_Ped(string name)
        {
            this.Name = name;
            timer.Interval = 1000;
            timer.Elapsed += On_TimerElapsed;

        }

        public bool Start()
        {
            LoadState();
            bool exit = false;
            while (!exit)
            {
                    buildUI();
                    timer.Start();

                var key = Console.ReadKey();
                switch (key.Key)
                {
                    case ConsoleKey.D1:
                        Feed_Click();
                        break;
                    case ConsoleKey.D2:
                        Clean_Click();
                        break;
                    case ConsoleKey.D3:
                        Medicine_Click();
                        break;
                    case ConsoleKey.Escape:
                        exit = true;
                        timer.Stop();
                        break;
                }
            }
            return exit;
        }
        public static List<Virtual_Ped> LoadState()
        {
            if (File.Exists("virtualPeds.json"))
            {
                string json = File.ReadAllText("virtualPeds.json");
                return JsonConvert.DeserializeObject<List<Virtual_Ped>>(json);
            }
            return new List<Virtual_Ped>();
        }

        private void buildUI()
        {
            Console.Clear();
            if (Full < 1 || Full > 1050)
            {
                Dead();
                timer.Stop();
                return;
            }

            switch (Count)
            {
                case 0:
                    Egg();
                    break;
                case 1:
                    Egg();
                    break;

                case 2:
                    Egg2();
                    break;

                case 3:
                    Egg3();
                    break;

                case 4:
                    Egg4();
                    break;

                case 5:
                    Stage = "Baby";
                    break;

                case 30:
                    Stage = "Teen";
                    break;
            }
            
            if (Count > 4)
            {
                if (Hour > 22 || Hour < 8)
                {
                    isSleeping = true;
                }
                else
                {
                    if (isSleeping && Full > 1000)
                    {
                        isSleeping = false;
                    }
                }

                if (isSleeping)
                {
                    Sleep();
                }
                else
                {
                    if (!isEating && !pooped)
                    {
                        Move();
                    }
                    if (isEating && !pooped)
                    {
                        Eat();
                    }

                    if (Eating == 0)
                    {
                        isEating = false;
                    }
                    if (Poop > 10)
                    {
                        pooped = true;
                    }
                    if (Poop < 10)
                    {
                        if (pooped)
                        {
                            pooped = false;
                        }
                    }
                    if (pooped && !isEating)
                    {
                        Pooped();
                    }
                }



                Console.WriteLine($"Status von {Name}:");
                Console.WriteLine($"Hunger: {Hunger}");
                Console.WriteLine($"Poop:{Poop}");
                Console.WriteLine($"Krank: {Sick}");
                Console.WriteLine($"Alter: {Stage}");
                Console.WriteLine($"Langeweile: {boredom}");
                Console.WriteLine("\n[1] Füttern   [2] Sauer machen   [3] Medizin geben");
            }
        }

        private void On_TimerElapsed(Object source, ElapsedEventArgs e)
        {
            Count++;
            Full--;
            Space = NextSpace;
            ChanceCheck = Chance.Next(10);
            Hour = Int32.Parse(DateTime.Now.ToString("HH"));

            switch (ChanceCheck)
            {
                case 0:
                    Full--;
                    break;

                case 1:
                    Full--;
                    break;

                case 2:
                    Poop++;
                    break;

                case 3:
                    Poop++;
                    break;

                case 4:
                    Sick++;
                    break;

                case 5:
                    Sick++;
                    break;
            }

            if (Full < 1000)
            {
                isSleeping = true;
                Hunger = "Im Fresskoma!";
            }
            if (Full < 900)
            {
                isSleeping = false;
                Hunger = "Zu voll";
            }
            if (Full < 800)
            {
                Hunger = "Sehr voll";
            }
            if (Full < 700)
            {
                Hunger = "Voll";
            }
            if (Full < 600)
            {
                Hunger = "Zufrieden";
            }
            if (Full < 500)
            {
                Hunger = "Nicht so Hungrig";
            }
            if (Full < 400)
            {
                Hunger = "Ein bischen Hungrig";
            }
            if (Full < 300)
            {
                Hunger = "Sehr Hungrig";
            }
            if (Full < 200)
            {
                Hunger = "Unangenehm Hungrig";
            }
            if (Full < 100)
            {
                Hunger = "Am Verhungern!";
            }

            buildUI();
        }

        public void Sleep()
        {
            if (isSleeping)
            {
                if (Count % 2 == 0)
                {
                    BabySleep1();
                }
                else
                {
                    BabySleep2();
                }
            }
        }

        public void Eat()
        {
            switch (Eating)
            {
                case 1:
                    BabyEat2();
                    Eating--;
                    break;

                case 2:
                    BabyEat1();
                    Eating--;
                    break;

                case 3:
                    BabyEat2();
                    Eating--;
                    break;
            }

            if (Eating == 0)
            {
                isEating = false;
            }
        }

        public void Pooped()
        {
            if (Poop > 10)
            {
                if (Count % 2 == 0)
                {
                    BabyPoop1();
                }
                else
                {
                    BabyPoop2();
                }
                Sick += 2;
            }
            if (Poop > 20)
            {
                if (Count % 2 == 0)
                {
                    Baby2Poop1();
                }
                else
                {
                    Baby2Poop2();
                }
                Sick += 4;
            }

            if (Poop < 10)
            {
                pooped = false;
            }
        }

        public void Move()
        {
            switch (Space)
            {
                case "Left":
                    MoveLeft();
                    NextSpace = "GoRight";
                    break;

                case "Right":
                    MoveRight();
                    NextSpace = "GoLeft";
                    break;

                case "GoLeft":
                    MoveMiddle();
                    NextSpace = "Left";
                    break;

                case "GoRight":
                    MoveMiddle();
                    NextSpace = "Right";
                    break;
            }
        }

        public void MoveLeft()
        {
            switch (Stage)
            {
                case "Baby":
                    BabyLeft();
                    break;

                case "Teen":
                    TeenLeft();
                    break;
            }
        }

        public void MoveRight()
        {
            switch (Stage)
            {
                case "Baby":
                    BabyRight();
                    break;

                case "Teen":
                    TeenRight();
                    break;
            }
        }

        public void MoveMiddle()
        {
            switch (Stage)
            {
                case "Baby":
                    BabyMiddle();
                    break;

                case "Teen":
                    TeenMiddle();
                    break;
            }
        }

        #region Pixel Art
        public void BabyLeft()
        {
            Console.WriteLine("");
            Console.WriteLine("    ■ ■■■■ ■                  ");
            Console.WriteLine("  ■          ■                ");
            Console.WriteLine(" ■  ■      ■  ■               ");
            Console.WriteLine(" ■    ■■■■    ■               ");
            Console.WriteLine(" ■            ■               ");
            Console.WriteLine("  ■          ■                ");
            Console.WriteLine("    ■ ■■■■ ■                  ");
            Console.WriteLine("");
            Console.WriteLine("");

        }

        public void BabyRight()
        {
            Console.WriteLine("");
            Console.WriteLine("                  ■ ■■■■ ■    ");
            Console.WriteLine("                ■          ■  ");
            Console.WriteLine("               ■  ■      ■  ■ ");
            Console.WriteLine("               ■    ■■■■    ■ ");
            Console.WriteLine("               ■            ■ ");
            Console.WriteLine("                ■          ■  ");
            Console.WriteLine("                  ■ ■■■■ ■    ");
            Console.WriteLine("");
            Console.WriteLine("");
        }

        public void BabyMiddle()
        {
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("           ■ ■■■■ ■           ");
            Console.WriteLine("         ■          ■         ");
            Console.WriteLine("        ■  ■■    ■■  ■        ");
            Console.WriteLine("        ■    ■■■■    ■        ");
            Console.WriteLine("         ■          ■         ");
            Console.WriteLine("           ■ ■■■■ ■           ");
            Console.WriteLine("");
            Console.WriteLine("");
        }

        public void BabySick()
        {
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("           ■ ■■■■ ■           ");
            Console.WriteLine("         ■          ■         ");
            Console.WriteLine("        ■  ■■■  ■■■  ■        ");
            Console.WriteLine("        ■    ■■■■    ■        ");
            Console.WriteLine("         ■          ■         ");
            Console.WriteLine("           ■ ■■■■ ■           ");
            Console.WriteLine("");
            Console.WriteLine("");
        }

        public void BabySick2()
        {
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("           ■ ■■■■ ■           ");
            Console.WriteLine("        ■  ■■■  ■■■  ■        ");
            Console.WriteLine("        ■    ■■■■    ■        ");
            Console.WriteLine("           ■ ■■■■ ■           ");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");
        }

        public void BabyPoop1()
        {
            Console.WriteLine("                              ");
            Console.WriteLine("                              ");
            Console.WriteLine("           ■ ■■■■ ■           ");
            Console.WriteLine("         ■          ■         ");
            Console.WriteLine("        ■   ■■    ■■ ■        ");
            Console.WriteLine("        ■    ■■■■    ■        ");
            Console.WriteLine("         ■          ■         ");
            Console.WriteLine("           ■ ■■■■ ■       @ ! ");
            Console.WriteLine("                        !@@@  ");
            Console.WriteLine("                        @@@@@ ");
        }

        public void BabyPoop2()
        {
            Console.WriteLine("                              ");
            Console.WriteLine("                              ");
            Console.WriteLine("           ■ ■■■■ ■           ");
            Console.WriteLine("         ■          ■         ");
            Console.WriteLine("        ■ ■■    ■■   ■        ");
            Console.WriteLine("        ■    ■■■■    ■        ");
            Console.WriteLine("         ■          ■         ");
            Console.WriteLine("           ■ ■■■■ ■     ! @   ");
            Console.WriteLine("                         @@@! ");
            Console.WriteLine("                        @@@@@ ");
        }

        public void Baby2Poop1()
        {
            Console.WriteLine(" ! @                          ");
            Console.WriteLine("  @@@!                        ");
            Console.WriteLine(" @@@@@     ■ ■■■■ ■           ");
            Console.WriteLine("         ■          ■         ");
            Console.WriteLine("        ■ ■■    ■■   ■        ");
            Console.WriteLine("        ■    ■■■■    ■        ");
            Console.WriteLine("         ■          ■         ");
            Console.WriteLine("           ■ ■■■■ ■       @ ! ");
            Console.WriteLine("                        !@@@  ");
            Console.WriteLine("                        @@@@@ ");
        }

        public void Baby2Poop2()
        {
            Console.WriteLine("   @ !                        ");
            Console.WriteLine(" !@@@                         ");
            Console.WriteLine(" @@@@@     ■ ■■■■ ■           ");
            Console.WriteLine("         ■          ■         ");
            Console.WriteLine("        ■   ■■    ■■ ■        ");
            Console.WriteLine("        ■    ■■■■    ■        ");
            Console.WriteLine("         ■          ■         ");
            Console.WriteLine("           ■ ■■■■ ■     ! @   ");
            Console.WriteLine("                         @@@! ");
            Console.WriteLine("                        @@@@@ ");
        }

        public void BabySleep1()
        {
            Console.WriteLine(" ■■■■■                        ");
            Console.WriteLine("    ■                         ");
            Console.WriteLine("   ■       ■ ■■■■ ■           ");
            Console.WriteLine("  ■      ■          ■         ");
            Console.WriteLine(" ■■■■■  ■  ■■    ■■  ■        ");
            Console.WriteLine("        ■    ■■■■    ■        ");
            Console.WriteLine("         ■          ■         ");
            Console.WriteLine("           ■ ■■■■ ■           ");
            Console.WriteLine("                              ");
            Console.WriteLine("                              ");
        }

        public void BabySleep2()
        {

            Console.WriteLine("                              ");
            Console.WriteLine("                              ");
            Console.WriteLine("           ■ ■■■■ ■           ");
            Console.WriteLine("         ■          ■         ");
            Console.WriteLine("        ■  ■■    ■■  ■        ");
            Console.WriteLine("        ■    ■■■■    ■  ■■■■■ ");
            Console.WriteLine("         ■          ■      ■  ");
            Console.WriteLine("           ■ ■■■■ ■       ■   ");
            Console.WriteLine("                         ■    ");
            Console.WriteLine("                        ■■■■■ ");
        }

        public void BabyEat1()
        {

            Console.WriteLine("");
            Console.WriteLine("           ■ ■■■■ ■           ");
            Console.WriteLine("         ■          ■         ");
            Console.WriteLine("        ■  ■      ■  ■        ");
            Console.WriteLine("        ■    ■■■■    ■        ");
            Console.WriteLine("        ■            ■        ");
            Console.WriteLine("         ■   ■■■■   ■         ");
            Console.WriteLine("           ■ ■■■■ ■           ");
            Console.WriteLine("");
            Console.WriteLine("");
        }

        public void BabyEat2()
        {

            Console.WriteLine("");
            Console.WriteLine("           ■ ■■■■ ■           ");
            Console.WriteLine("         ■          ■         ");
            Console.WriteLine("        ■  ■      ■  ■        ");
            Console.WriteLine("        ■    ■■■■    ■        ");
            Console.WriteLine("        ■    ■■■■    ■        ");
            Console.WriteLine("         ■          ■         ");
            Console.WriteLine("           ■ ■■■■ ■           ");
            Console.WriteLine("");
            Console.WriteLine("");
        }

        public void TeenLeft()
        {

            Console.WriteLine("    ■ ■■■■ ■■                 ");
            Console.WriteLine("  ■           ■               ");
            Console.WriteLine(" ■  ■       ■  ■              ");
            Console.WriteLine(" ■    ■■■■■    ■              ");
            Console.WriteLine("■■             ■■             ");
            Console.WriteLine("  ■           ■               ");
            Console.WriteLine("   ■    ■    ■                ");
            Console.WriteLine("    ■  ■ ■  ■                 ");
            Console.WriteLine("     ■     ■                  ");
            Console.WriteLine("                              ");
        }

        public void TeenRight()
        {

            Console.WriteLine("                 ■ ■■■■ ■■    ");
            Console.WriteLine("               ■           ■  ");
            Console.WriteLine("              ■  ■       ■  ■ ");
            Console.WriteLine("              ■    ■■■■■    ■ ");
            Console.WriteLine("             ■■             ■■");
            Console.WriteLine("               ■           ■  ");
            Console.WriteLine("                ■    ■    ■   ");
            Console.WriteLine("                 ■  ■ ■  ■    ");
            Console.WriteLine("                  ■     ■     ");
            Console.WriteLine("                              ");
        }

        public void TeenMiddle()
        {

            Console.WriteLine("                              ");
            Console.WriteLine("           ■ ■■■■ ■■          ");
            Console.WriteLine("         ■           ■        ");
            Console.WriteLine("        ■  ■       ■  ■       ");
            Console.WriteLine("        ■    ■■■■■    ■       ");
            Console.WriteLine("       ■■             ■■      ");
            Console.WriteLine("          ■    ■    ■         ");
            Console.WriteLine("           ■  ■ ■  ■          ");
            Console.WriteLine("            ■     ■           ");
            Console.WriteLine("                              ");
        }

        public void Egg()
        {

            Console.WriteLine("              ■■              ");
            Console.WriteLine("            ■    ■            ");
            Console.WriteLine("          ■        ■          ");
            Console.WriteLine("         ■          ■         ");
            Console.WriteLine("        ■            ■        ");
            Console.WriteLine("       ■              ■       ");
            Console.WriteLine("       ■              ■       ");
            Console.WriteLine("       ■              ■       ");
            Console.WriteLine("         ■          ■         ");
            Console.WriteLine("           ■ ■■■■ ■           ");
        }

        public void Egg2()
        {

            Console.WriteLine("             ■■               ");
            Console.WriteLine("           ■    ■             ");
            Console.WriteLine("         ■        ■           ");
            Console.WriteLine("        ■          ■          ");
            Console.WriteLine("       ■             ■        ");
            Console.WriteLine("      ■               ■       ");
            Console.WriteLine("      ■               ■       ");
            Console.WriteLine("       ■              ■       ");
            Console.WriteLine("         ■          ■         ");
            Console.WriteLine("           ■ ■■■■ ■           ");
        }

        public void Egg3()
        {

            Console.WriteLine("               ■■             ");
            Console.WriteLine("             ■    ■           ");
            Console.WriteLine("           ■        ■         ");
            Console.WriteLine("         ■           ■        ");
            Console.WriteLine("        ■             ■       ");
            Console.WriteLine("       ■               ■      ");
            Console.WriteLine("       ■               ■      ");
            Console.WriteLine("       ■              ■       ");
            Console.WriteLine("         ■          ■         ");
            Console.WriteLine("           ■ ■■■■ ■           ");
        }

        public void Egg4()
        {

            Console.WriteLine("              ■■              ");
            Console.WriteLine("            ■    ■            ");
            Console.WriteLine("          ■        ■          ");
            Console.WriteLine("       ■  ■ ■ ■■ ■ ■  ■       ");
            Console.WriteLine("        ■  ■      ■  ■        ");
            Console.WriteLine("        ■    ■■■■    ■        ");
            Console.WriteLine("       ■  ■ ■ ■■ ■ ■  ■       ");
            Console.WriteLine("       ■              ■       ");
            Console.WriteLine("         ■          ■         ");
            Console.WriteLine("           ■ ■■■■ ■           ");
        }


        public void Dead()
        {

            Console.WriteLine("            ■ ■ ■ ■           ");
            Console.WriteLine("            ■     ■           ");
            Console.WriteLine("      ■ ■ ■ ■     ■ ■ ■ ■     ");
            Console.WriteLine("      ■      R.I.P.     ■     ");
            Console.WriteLine("      ■ ■ ■ ■     ■ ■ ■ ■     ");
            Console.WriteLine("            ■     ■           ");
            Console.WriteLine("            ■     ■           ");
            Console.WriteLine("            ■     ■           ");
            Console.WriteLine("            ■     ■           ");
            Console.WriteLine("            ■ ■ ■ ■           ");
        }
        #endregion

        private void Feed_Click()
        {
            Full += 50;
            Poop++;
            Eating = 3;
            isEating = true;
        }

        private void Clean_Click()
        {
            Poop = 0;
            if (Sick < 5)
            {
                Sick = 0;
            }
            else
            {
                Sick -= 5;
            }
            
        }

        private void Medicine_Click()
        {
            Sick = 0;
        }



    }
}