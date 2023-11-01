using System.IO;

namespace L_P.Model
{
    public class Music : Notify
    {
        private string? songName;
        private string? songerName;
        private string? album;
        private int date;
        private double duration;
        //private FileStream? musicFile;

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
        public double Duration
        {
            get { return duration; }
            set { duration = value; OnPropertyChanged("Duration"); }
        }
        //public FileStream? MusicFile
        //{
        //    get { return musicFile; }
        //    set { musicFile = value; OnPropertyChanged("MusicFile"); }
        //}
    }
}
