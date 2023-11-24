using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace L_P.Model
{
    public class Podcast : Notify
    {
        private string? podcastName;
        private string? podcasterName;
        private int date;
        private TimeSpan duration;
        private FileStream? podcastFile;
        private BitmapImage? podcastImage;

        public BitmapImage? PodcastImage
        {
            get { return podcastImage; }
            set { podcastImage = value; OnPropertyChanged("PodcastImage"); }
        }
        public string? PodcastName
        {
            get { return podcastName; }
            set { podcastName = value; OnPropertyChanged("PodcastName"); }
        }
        public string? PodcasterName
        {
            get { return podcasterName; }
            set { podcasterName = value; OnPropertyChanged("PodcasterName"); }
        }
        public int Date
        {
            get { return date; }
            set { date = value; OnPropertyChanged("Date"); }
        }
        public TimeSpan Duration
        {
            get { return duration; }
            set { duration = value; OnPropertyChanged("Duration"); }
        }
        public FileStream? PodcastFile
        {
            get { return podcastFile; }
            set { podcastFile = value; OnPropertyChanged("PodcastFile"); }
        }
    }
}
