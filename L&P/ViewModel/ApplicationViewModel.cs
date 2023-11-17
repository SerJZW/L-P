using L_P.Model;
using L_P.Model.Event;
using L_P.View;
using L_P.ViewModel;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace L_P
{
    public class ApplicationViewModel : Notify
    {
        #region ViewModels
        private ContentSwitch? contentSwitch;
        public ContentSwitch? ContentSwitch
        {
            get { return contentSwitch; }
            set { contentSwitch = value; OnPropertyChanged("ContentSwitch"); }
        }

        #endregion

        #region Selected
        private int currentTrackIndex = 0;
        private object? selectedAudio;
        public object? SelectedAudio
        {
            get { return selectedAudio; }
            set
            {
                selectedAudio = value;
                OnPropertyChanged("SelectedAudio");
                if (selectedAudio is Music music)
                {
                    currentTrackIndex = Music.IndexOf(music);
                }
                else if (selectedAudio is Podcast podcast)
                {
                    currentTrackIndex = Podcasts.IndexOf(podcast);
                }
            }
        }
        private Accords? selectedAccords;
        public Accords? SelectedAccords
        {
            get { return selectedAccords; }
            set { selectedAccords = value; OnPropertyChanged("SelectedAccords"); }
        }
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
            Accords[0].SetAccordFile("C:\\Users\\zemzh\\source\\repos\\L&P\\L&P\\Source\\Accords\\Король и шут - Кукла Колдуна.txt");
            Accords[1].SetAccordFile("C:\\Users\\zemzh\\source\\repos\\L&P\\L&P\\Source\\Accords\\Imagine Dragons - Demons.txt");

            ContentSwitch = new ContentSwitch();

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += (sender, e) => CurrentPosition = mediaPlayer.Position;        
            timer.Start();
            DispatcherTimer musicTimer = new DispatcherTimer();
            musicTimer.Interval = TimeSpan.FromSeconds(1);
            musicTimer.Tick += (sender, e) => _CurrentPosition = mediaPlayer.Position.TotalSeconds / 2;
            musicTimer.Start();
        }

        #region AddCommandRealisations
        private OpenFileDialog openDialog = new OpenFileDialog();
        public RelayCommand SearchMusicCommand
        {
            get
            {
                return (new RelayCommand(obj =>
                {
                    openDialog.Filter = "Музыкальные файлы (.mp3) | *.mp3";
                    openDialog.Title = "Выберите музыкальные файлы....";
                    bool? succses = openDialog.ShowDialog();
                    openDialog.Multiselect = true;
                    if (succses == true)
                    {
                        foreach (string fileName in openDialog.FileNames)
                        {
                            try
                            {
                                TagLib.File file = TagLib.File.Create(fileName);

                                Music music = new Music
                                {
                                    SongName = file.Tag.Title,
                                    SongerName = string.Join(", ", file.Tag.Performers),
                                    Album = file.Tag.Album,
                                    Date = (int)file.Tag.Year,
                                    Durations = TimeSpan.FromSeconds(file.Properties.Duration.TotalSeconds),
                                    MusicFile = new FileStream(fileName, FileMode.Open)
                                };

                                Music.Add(music);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Ошибка при обработке файла: {ex.Message}");
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Ни один элемент не был добавлен");
                    }
                }));
            }
        }
        public RelayCommand SearchPodcastCommand
        {
            get
            {
                return (new RelayCommand(obj =>
                {
                    openDialog.Filter = "Подкаст файлы (.mp3) | *.mp3";
                    openDialog.Title = "Выберите подкаст файлы....";
                    bool? succses = openDialog.ShowDialog();
                    openDialog.Multiselect = true;
                    if (succses == true)
                    {
                        foreach (string fileName in openDialog.FileNames)
                        {
                            try
                            {
                                TagLib.File file = TagLib.File.Create(fileName);

                                Podcast podcast = new Podcast
                                {
                                    PodcastName = file.Tag.Title,
                                    PodcasterName = string.Join(", ", file.Tag.Performers),
                                    Date = (int)file.Tag.Year,
                                    Duration = TimeSpan.FromSeconds(file.Properties.Duration.TotalSeconds),
                                    PodcastFile = new FileStream(fileName, FileMode.Open)
                                };

                                Podcasts.Add(podcast);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Ошибка при обработке файла: {ex.Message}");
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Ни один элемент не был добавлен");
                    }
                }));
            }
        }
        public RelayCommand SearchAccordsCommand
        {
            get
            {
                return (new RelayCommand(obj =>
                {
                    openDialog.Filter = "Аккорд файлы (.txt) | *.txt";
                    openDialog.Title = "Выберите аккорд файлы....";
                    openDialog.Multiselect = true;

                    bool? success = openDialog.ShowDialog();

                    if (success == true)
                    {
                        foreach (string fileName in openDialog.FileNames)
                        {
                            try
                            {
                                string title = File.ReadAllText(fileName);
                                Accords accords = new Accords
                                {
                                    AccordName = Path.GetFileNameWithoutExtension(fileName),
                                    AccordFileText = title
                                };
                                Accords.Add(accords);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Ошибка при обработке файла: {ex.Message}");
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Ни один элемент не был добавлен");
                    }
                }));
            }
        }

        #endregion
        #region AudioPlayer
        MediaPlayer mediaPlayer = new MediaPlayer();
        private bool isPlaying = false;
        private TimeSpan? lastPosition;

        public RelayCommand PlayCommand
        {
            get
            {
                return (new RelayCommand(obj =>
                {
                    if (SelectedAudio != null)
                    {
                        if (isPlaying)
                        {
                            lastPosition = mediaPlayer.Position;
                            mediaPlayer.Pause();
                            isPlaying = false;
                        }
                        else
                        {
                            if (SelectedAudio is Music)
                            {
                                mediaPlayer.Open(new Uri((SelectedAudio as Music).MusicFile.Name));
                            }
                            else if (SelectedAudio is Podcast)
                            {
                                mediaPlayer.Open(new Uri((SelectedAudio as Podcast).PodcastFile.Name));
                            }

                            if (lastPosition.HasValue)
                            {
                                mediaPlayer.Position = lastPosition.Value;
                            }

                            mediaPlayer.Play();

                            isPlaying = true;

                        }
                    }
                }));
            }
        }

        public RelayCommand NextTrackCommand
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    if (currentTrackIndex < Music.Count)
                    {
                        currentTrackIndex++;
                        if (currentTrackIndex >= 0 && currentTrackIndex < Music.Count)
                        {
                            mediaPlayer.Open(new Uri(Music[currentTrackIndex].MusicFile.Name));
                            mediaPlayer.Play();
                            isPlaying = true;
                        }
                    }
                });
            }
        }

        public RelayCommand PreviousTrackCommand
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    if (currentTrackIndex > 0)
                    {
                        currentTrackIndex--;
                        if (currentTrackIndex >= 0 && currentTrackIndex < Music.Count)
                        {
                            mediaPlayer.Open(new Uri(Music[currentTrackIndex].MusicFile.Name));
                            mediaPlayer.Play();
                            isPlaying = true;
                        }
                    }
                });
            }
        }

        #endregion
        #region Sliders
        private double volume = 1;
        public double Volume
        {
            get { return volume; }
            set
            {
                volume = value;
                OnPropertyChanged("Volume");
                mediaPlayer.Volume = volume;
            }
        }

        private TimeSpan currentPosition;
        public TimeSpan CurrentPosition
        {
            get { return currentPosition; }
            set
            {
                currentPosition = value;
                OnPropertyChanged("CurrentPosition");
            }
        }

        private double _currentPosition = 0;

        public double _CurrentPosition
        {
            get { return _currentPosition; }
            set
            {
                if (_currentPosition != value)
                {
                    _currentPosition = value;
                    OnPropertyChanged("_CurrentPosition");
                }
            }
        }
        #endregion
        #region StyleSwitcher
        private bool _isChecked;
        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                _isChecked = value;
                OnPropertyChanged("IsChecked");
            }
        }

        private Style? windowStyle;

        public Style? WindowStyle
        {
            get { return windowStyle; }
            set
            {
                if (windowStyle != value)
                {
                    windowStyle = value;
                    OnPropertyChanged("WindowStyle");
                }
            }
        }
        private Style? userControlStyle;

        public Style? UserControlStyle
        {
            get { return userControlStyle; }
            set
            {
                if (userControlStyle != value)
                {
                    userControlStyle = value;
                    OnPropertyChanged("UserControlStyle");
                }
            }
        }

        public RelayCommand ThemeChange
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    IsDarkTheme = !IsDarkTheme;
                });
            }
        }
       private bool isDarkTheme;

        public bool IsDarkTheme
        {
            get { return isDarkTheme; }
            set
            {
                if (isDarkTheme != value)
                {
                    isDarkTheme = value;
                    OnPropertyChanged("IsDarkTheme");
                    WindowStyle = IsDarkTheme ? Application.Current.FindResource("DarkWindowStyle") as Style : Application.Current.FindResource("LightWindowStyle") as Style;
                    UserControlStyle = IsDarkTheme ? Application.Current.FindResource("DarkUC") as Style : Application.Current.FindResource("LightUC") as Style;
                }
            }
        }
        #endregion
    }
}
