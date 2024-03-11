using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Virtual_Ped
{
    internal class Program
    {
        
        static void Main(string[] args)
        {
            bool validinput = false;
            string name = "Lona";
            while (!validinput)
            {
                Console.Clear();
                Console.WriteLine("Virtual Ped");
                Console.WriteLine("--------------------------");
                Console.WriteLine("Name des Pets eingeben: ");
                name = Console.ReadLine();

                if (string.IsNullOrEmpty(name))
                {
                    Console.WriteLine("Bitte einen gültigen Namen eingeben");
                }
                else
                {
                    validinput = true;
                }
            }
            var ui = new Virtual_Ped(name);
            ui.Start();
        }
    }
}
