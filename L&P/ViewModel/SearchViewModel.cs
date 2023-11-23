using L_P.Model;
using L_P.Model.Event;
using System.Linq;
using System.Diagnostics;
using System;
using System.Windows;
using System.Collections.ObjectModel;

namespace L_P.ViewModel
{
    public class SearchViewModel : Notify
    {
        private ApplicationViewModel app;
        public ObservableCollection<Music> originalMusicList { get; set; }
        public ObservableCollection<Podcast> originalPodcastList { get; set; }
        public ObservableCollection<Accords> originalAccordsList { get; set; }
        public SearchViewModel(ApplicationViewModel applicationViewModel)
        {
            app = applicationViewModel;
            originalMusicList = new ObservableCollection<Music>(app.Music);
            originalPodcastList = new ObservableCollection<Podcast>(app.Podcasts);
            originalAccordsList = new ObservableCollection<Accords>(app.Accords);
        }
        private string? searchText;

        public string? SearchText
        {
            get { return searchText; }
            set
            {
                if (searchText != value)
                {
                    searchText = value;
                    OnPropertyChanged("SearchText");
                    SearchCommandMusic.Execute(null);
                }
            }

        }
        private string? searchTextPodcast;

        public string? SearchTextPodcast
        {
            get { return searchTextPodcast; }
            set
            {
                if (searchTextPodcast != value)
                {
                    searchTextPodcast = value;
                    OnPropertyChanged("SearchTextPodcast");
                    SearchCommandPodcast.Execute(null);
                }
            }

        }
        private string? searchTextAccords;

        public string? SearchTextAccords
        {
            get { return searchTextAccords; }
            set
            {
                if (searchTextAccords != value)
                {
                    searchTextAccords = value;
                    OnPropertyChanged("SearchTextAccords");
                    SearchCommandAccords.Execute(null);
                }
            }

        }

        public RelayCommand SearchCommandMusic
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    if (string.IsNullOrEmpty(SearchText))
                    {
                        
                        app.Music.Clear();
                        foreach (var music in originalMusicList)
                        {
                            app.Music.Add(music);
                        }
                    }
                    else
                    {
                        var filteredMusic = originalMusicList.Where(m => m.SongName.Contains(SearchText)).ToList();
                        app.Music.Clear();
                        foreach (var music in filteredMusic)
                        {
                            app.Music.Add(music);
                        }
                    }
                });
            }
        }
        public RelayCommand SearchCommandPodcast
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    if (string.IsNullOrEmpty(SearchTextPodcast))
                    {

                        app.Podcasts.Clear();
                        foreach (var podcast in originalPodcastList)
                        {
                            app.Podcasts.Add(podcast);
                        }
                    }
                    else
                    {

                        var filteredMusic = originalPodcastList.Where(m => m.PodcastName.Contains(SearchTextPodcast)).ToList();
                        app.Podcasts.Clear();
                        foreach (var podcast in filteredMusic)
                        {
                            app.Podcasts.Add(podcast);
                        }
                    }
                });
            }
        }
        public RelayCommand SearchCommandAccords
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    if (string.IsNullOrEmpty(SearchTextAccords))
                    {

                        app.Accords.Clear();
                        foreach (var accords in originalAccordsList)
                        {
                            app.Accords.Add(accords);
                        }
                    }
                    else
                    {
                        var filteredMusic = originalAccordsList.Where(m => m.AccordName.Contains(SearchTextAccords)).ToList();
                        app.Accords.Clear();
                        foreach (var accords in filteredMusic)
                        {
                            app.Accords.Add(accords);
                        }
                    }
                });
            }
        }

    }
}
