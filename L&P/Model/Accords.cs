using System;
using System.IO;

namespace L_P.Model
{
    public class Accords : Notify
    {
        private string? accordName;
        private FileStream? accordFile;
        private string? accordFileText;

        public string? AccordName
        {
            get { return accordName; }
            set { accordName = value; OnPropertyChanged("AccordName"); }
        }

        public FileStream? AccordFile
        {
            get { return accordFile; }
            set { accordFile = value; OnPropertyChanged("AccordFile"); }
        }

        public string? AccordFileText
        {
            get { return accordFileText; }
            set { accordFileText = value; OnPropertyChanged("AccordFileText"); }
        }

        public void SetAccordFile(string filePath)
        {
            try
            {
                string fileContent = File.ReadAllText(filePath);
                AccordFileText = fileContent;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при открытии файла: {ex.Message}");
            }
        }

    }
}
