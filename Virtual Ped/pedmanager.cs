using System;
using System.Collections.Generic;
using System.IO;

namespace Virtual_Ped
{
    internal class pedmanager
    {
        private string FilePath;
        private System.Timers.Timer Timer;
        private static int Interval = 10000;
        private List<Virtual_Ped> petList;

        public pedmanager()
        {
            FilePath = LoadFilePath();
            //LoadPets();
            //SetTimer();
        }

        private string LoadFilePath()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            string myAppFolder = Path.Combine(appDataPath, "virtual-pet");
            Directory.CreateDirectory(myAppFolder); // Create file if not exists

            string fileName = "pets.json";
            return Path.Combine(myAppFolder, fileName);
        }
    }
}