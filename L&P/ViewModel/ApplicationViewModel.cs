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
using System.Windows.Threading;

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
            get { return SelectedAccords; }
            set { SelectedAccords = value; OnPropertyChanged("SelectedAccords"); }
        }
        #endregion
        public ObservableCollection<Music> Music { get; set; }
        public ObservableCollection<Accords> Accords { get; set; }
        public ObservableCollection<Podcast> Podcasts { get; set; }

        public ApplicationViewModel()
        {
            Music = new ObservableCollection<Music>();
            Podcasts = new ObservableCollection<Podcast>();
            Accords = new ObservableCollection<Accords>();
            ContentSwitch = new ContentSwitch();

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += (sender, e) => CurrentPosition = mediaPlayer.Position;
            timer.Start();
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
                    openDialog.Filter = "Аккорд файлы (.midi) | *.midi";
                    openDialog.Title = "Выберите аккорд файлы....";
                    bool? succses = openDialog.ShowDialog();
                    openDialog.Multiselect = true;
                    if (succses == true)
                    {
                        foreach (string fileName in openDialog.FileNames)
                        {
                            try
                            {
                                TagLib.File file = TagLib.File.Create(fileName);

                                Accords accords = new Accords
                                {
                                    AccordName = file.Tag.Title,
                                    AccorderName = string.Join(", ", file.Tag.Performers),
                                    Album = file.Tag.Album,
                                    Date = (int)file.Tag.Year,
                                    Durations = TimeSpan.FromSeconds(file.Properties.Duration.TotalSeconds),
                                    AccordFile = new FileStream(fileName, FileMode.Open)
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
