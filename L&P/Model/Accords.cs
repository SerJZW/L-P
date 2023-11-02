using System;
using System.IO;

namespace L_P.Model
{
    public class Accords : Notify
    {
        private string? accordName;
        private string? accorderName;
        private string? album;
        private int date;
        private TimeSpan durations;
        private FileStream? accordFile;

        public string? AccordName
        {
            get { return accordName; }
            set { accordName = value; OnPropertyChanged("AccordName"); }
        }
        public string? AccorderName
        {
            get { return accorderName; }
            set { accorderName = value; OnPropertyChanged("AccorderName"); }
        }
        public string? Album
        {
            get { return album; }
            set { album = value; OnPropertyChanged("Album"); }
        }
        public int Date
        {
            get { return date; }
            set { date = value; OnPropertyChanged("Date"); }
        }
        public TimeSpan Durations
        {
            get { return durations; }
            set { durations = value; OnPropertyChanged("Duration"); }
        }
        public FileStream? AccordFile
        {
            get { return accordFile; }
            set { accordFile = value; OnPropertyChanged("AccordFile"); }
        }
    }
}
