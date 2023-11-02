using L_P.Model;
using L_P.Model.Event;
using L_P.View;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace L_P
{
    public class ApplicationViewModel : Notify
    {
        #region Selected
        private object? selectedAudio;
        public object? SelectedAudio
        {
            get { return selectedAudio; }
            set
            {
                selectedAudio = value;
                OnPropertyChanged("SelectedAudio");
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
            Music = new ObservableCollection<Music>()
            {
                new Music{SongName = "Кукла Колдуна", SongerName = "КиШ", Album = "Акустический альбом", Date = 1998, Durations = new TimeSpan(0,3,23)}
            };
            Podcasts = new ObservableCollection<Podcast>();
            Accords = new ObservableCollection<Accords>();

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1); 
            timer.Tick += (sender, e) => CurrentPosition = mediaPlayer.Position;
            timer.Start();
        }
        #region ContentSwitch
        private ContentControl? myContentControl;
        public ContentControl MyContentControl
        {
            get
            {
                if (myContentControl == null) { myContentControl = new MainView(); }
                return myContentControl;
            }
            set
            {
                myContentControl = value;
                OnPropertyChanged("MyContentControl");
            }
        }
        public RelayCommand? setObject;

        public RelayCommand SetObject
        {
            get
            {
                return setObject ?? (setObject = new RelayCommand(obj =>
                {
                    if ((obj as string) == "MainView") MyContentControl = new MainView();
                    if ((obj as string) == "MusicView") MyContentControl = new MusicView();
                    if ((obj as string) == "PodcastView") MyContentControl = new PodcastView();
                    if ((obj as string) == "AccordsView") MyContentControl = new AccordsView();
                    if ((obj as string) == "AddView") MyContentControl = new AddView();
                    if ((obj as string) == "UserView") MyContentControl = new UserView();
                    if ((obj as string) == "SettingView") MyContentControl = new SettingView();
                    if ((obj as string) == "UserAgreementView") MyContentControl = new UserAgreementView();
                    if ((obj as string) == "AboutAplView") MyContentControl = new AboutAplView();
                }));
            }
        }
        #endregion
        #region SmthCommand
        public RelayCommand ExitCommand
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    System.Windows.Application.Current.Shutdown();
                });
            }
        }

        public RelayCommand ContactCommand
        {
            get
            {
                return (new RelayCommand(obj =>
                {
                    string url = "https://sun6-22.userapi.com/s/v1/if2/N8gfbNFwrAPxOwsrvN8RQ-8IoxiMX_jy8N7zLUf4itGzIzR_6yXNwcfnF7TVALYa_GSL6-99tcshKtpidnDzB9zL.jpg?size=1686x1686&quality=96&crop=203,152,1686,1686&ava=1";
                    System.Diagnostics.Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
                }));
            }
        }
        #endregion
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
        #region Player
        MediaPlayer mediaPlayer = new MediaPlayer();
        private bool isPlaying = false;
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
                            mediaPlayer.Play();
                            isPlaying = true;
                        }
                    }
                }));
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
    }
}
