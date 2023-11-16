using System;
using System.IO;

namespace L_P.Model
{
    public class Music : Notify
    {
        private string? songName;
        private string? songerName;
        private string? album;
        private int date;
        private TimeSpan durations;
        private FileStream? musicFile;

        public string? SongName
        {
            get { return songName; }
            set { songName = value; OnPropertyChanged("SongName"); }
        }
        public string? SongerName
        {
            get { return songerName; }
            set { songerName = value; OnPropertyChanged("SongerName"); }
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
            set { durations = value; OnPropertyChanged("Durations"); }
        }
        public FileStream? MusicFile
        {
            get { return musicFile; }
            set { musicFile = value; OnPropertyChanged("MusicFile"); }
        }

        public void SetMusicFile(string filePath)
        {
            try
            {
                FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                MusicFile = fileStream;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при открытии музыкального файла: {ex.Message}");
            }
        }
    }
}
