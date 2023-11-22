using L_P.Model;
using L_P.Model.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Threading;

namespace L_P.ViewModel
{
    public class AudioPlayer : Notify
    {
        private ApplicationViewModel app;

        public AudioPlayer(ApplicationViewModel applicationViewModel)
        {
            app = applicationViewModel;
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += (sender, e) => CurrentPosition = mediaPlayer.Position;
            timer.Start();
            DispatcherTimer musicTimer = new DispatcherTimer();
            musicTimer.Interval = TimeSpan.FromSeconds(1);
            musicTimer.Tick += (sender, e) => _CurrentPosition = mediaPlayer.Position.TotalSeconds / 2;
            musicTimer.Start();
        }
        private Accords? selectedAccords;
        public Accords? SelectedAccords
        {
            get { return selectedAccords; }
            set { selectedAccords = value; OnPropertyChanged("SelectedAccords"); }
        }
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
                    currentTrackIndex = app.Music.IndexOf(music);
                    PlayCommand.Execute(selectedAudio);
                }
                else if (selectedAudio is Podcast podcast)
                {
                    currentTrackIndex = app.Podcasts.IndexOf(podcast);
                    PlayCommand.Execute(selectedAudio);
                }
            }
        }
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
                }));
            }
        }
        public RelayCommand PauseCommand
        {
            get
            {
                return (new RelayCommand(obj =>
                {
                    if (isPlaying)
                    {
                        lastPosition = mediaPlayer.Position;

                        mediaPlayer.Pause();
                        isPlaying = false;
                    }
                    else
                    {
                        mediaPlayer.Play();
                        isPlaying = true;
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
                    if (currentTrackIndex < app.Music.Count)
                    {
                        currentTrackIndex++;
                        if (currentTrackIndex >= 0 && currentTrackIndex < app.Music.Count)
                        {
                            mediaPlayer.Open(new Uri(app.Music[currentTrackIndex].MusicFile.Name));
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
                        if (currentTrackIndex >= 0 && currentTrackIndex < app.Music.Count)
                        {
                            mediaPlayer.Open(new Uri(app.Music[currentTrackIndex].MusicFile.Name));
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
    }
}
