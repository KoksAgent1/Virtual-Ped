using Figgle;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Virtual_Ped
{
    internal class Program
    {
        
        static void Main(string[] args)
        {
            List<Virtual_Ped> virtualPeds = LoadState();
            Header();
            while (true)
            {
                Console.Clear();
                Console.WriteLine(FiggleFonts.Standard.Render("Virtual Pet"));
                if (virtualPeds.Count > 0)
                {
                    Console.WriteLine("Menu");
                    Console.WriteLine("--------------------------");
                    Console.WriteLine("Wählen Sie eine Option:");
                    Console.WriteLine("1: Neues Pet hinzufügen");
                    Console.WriteLine("2: Pet auswählen");
                    Console.WriteLine("3: Beenden");
                    var choice = Console.ReadKey();

                    switch (choice.Key)
                    {
                        case ConsoleKey.D1:
                            AddNewVirtualPed(virtualPeds);
                            SaveState(virtualPeds);
                            break;
                        case ConsoleKey.D2:
                            SelectAndStartVirtualPed(virtualPeds);
                            break;
                        case ConsoleKey.D3:
                            SaveState(virtualPeds);
                            return;
                        default:
                            Console.WriteLine("Ungültige Auswahl. Bitte versuchen Sie es erneut.");
                            break;
                    }
                }
                else
                {
                    AddNewVirtualPed(virtualPeds);
                }
            }
        }

        public static void SaveState(List<Virtual_Ped> virtualPeds)
        {
            string json = JsonConvert.SerializeObject(virtualPeds, Formatting.Indented);
            File.WriteAllText("virtualPeds.json", json);
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

        static void AddNewVirtualPed(List<Virtual_Ped> virtualPeds)
        {
            Header();
            Console.WriteLine("Name des neuen Pets eingeben:");
            var name = Console.ReadLine();
            if (!string.IsNullOrEmpty(name))
            {
                virtualPeds.Add(new Virtual_Ped(name));
            }
            else
            {
                Console.WriteLine("Bitte einen gültigen Namen eingeben");
            }
        }
        static void SelectAndStartVirtualPed(List<Virtual_Ped> virtualPeds)
        {
            Header();
            for (int i = 0; i < virtualPeds.Count; i++)
            {
                Console.WriteLine($"{i + 1}: {virtualPeds[i].Name}");
            }
            Console.WriteLine("Wählen Sie ein Pet aus (Nummer):");
            if (int.TryParse(Console.ReadLine(), out int index) && index >= 1 && index <= virtualPeds.Count)
            {
                bool exited = virtualPeds[index - 1].Start(); // Startet die Interaktion und prüft, ob sie beendet wurde
                if (exited)
                {
                    SaveState(virtualPeds); // Speichert den Zustand, wenn die Interaktion beendet wurde
                }
            }
            else
            {
                Console.WriteLine("Ungültige Auswahl. Bitte versuchen Sie es erneut.");
            }
        }

        static void Header()
        {
            Console.Clear();
            Console.WriteLine(FiggleFonts.Standard.Render("Virtual Pet"));
            Console.WriteLine("----------------------------");
        }

    }
}
