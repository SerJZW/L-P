using L_P.Model;
using L_P.ViewModel;
using System;
using System.Collections.ObjectModel;


namespace L_P
{
    public class ApplicationViewModel : Notify
    {
        #region ViewModels
        public ContentSwitch ContentSwitch { get; set; }
        public StyleSwitcher StyleSwitch { get; set; }
        public AddViewModel AddCommand { get; set; }
        public AudioPlayer AudioPlayer { get; set; }
        public SearchViewModel SearchViewModel { get; set; }
        #endregion
        public ObservableCollection<Music> Music { get; set; }
        public ObservableCollection<Accords> Accords { get; set; }
        public ObservableCollection<Podcast> Podcasts { get; set; }
       


        public ApplicationViewModel()
        {
            Music = new ObservableCollection<Music>()
            {
                new Music{SongName = "Кукла Колдуна", SongerName = "Король и Шут", Album = "Акустический альбом", Date = 1998, Durations = TimeSpan.FromMinutes(3.23)},
                new Music{SongName = "МАЛИНОВАЯ ЛАДА", SongerName = "GAYAZOV$ BROTHER$", Album = "МАЛИНОВАЯ ЛАДА", Date = 2021, Durations = TimeSpan.FromMinutes(3.33)},
            };
            Music[0].SetMusicFile("C:\\Users\\zemzh\\source\\repos\\L&P\\L&P\\Source\\Musics\\Korol_i_SHut_-_Kukla_kolduna_62570545.mp3");
            Music[1].SetMusicFile("C:\\Users\\zemzh\\source\\repos\\L&P\\L&P\\Source\\Musics\\GAYAZOV_BROTHER_-_MALINOVAYA_LADA_73214200.mp3");    
            Podcasts = new ObservableCollection<Podcast>()
            {
                new Podcast{PodcastName = "Danza Kuduro", PodcasterName = "Don Lore V", Date = 2014, Duration = TimeSpan.FromMinutes(3.19)},
            };
            Podcasts[0].SetPodcastFile("C:\\Users\\zemzh\\source\\repos\\L&P\\L&P\\Source\\Podcasts\\Don_Omar_-_Danza_Kuduro_28587730.mp3");
            Accords = new ObservableCollection<Accords>()
            {
                new Accords{AccordName = "Король и шут - Кукла колдуна"},
                new Accords{AccordName = "Imagine Dragons - Demons"},
            };
            Accords[0].SetAccordFile("C:\\Users\\zemzh\\Source\\Repos\\L&P\\L&P\\Source\\Accords\\Король и шут - Кукла Колдуна.txt");
            Accords[1].SetAccordFile("C:\\Users\\zemzh\\source\\repos\\L&P\\L&P\\Source\\Accords\\Imagine Dragons - Demons.txt");
            AudioPlayer = new AudioPlayer(this);
            AddCommand = new AddViewModel(this);
            SearchViewModel = new SearchViewModel(this);
            ContentSwitch = new ContentSwitch();
            StyleSwitch = new StyleSwitcher();

        }
    }
}
