using L_P.Model;
using L_P.Model.Event;
using L_P.View;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace L_P
{
    public class ApplicationViewModel : Notify
    {
        public ObservableCollection<Music>? Music { get; set; }
        public ObservableCollection<Accords>? Accords { get; set; }
        public ObservableCollection<Podcast>? Podcasts { get; set; }
       
        public ApplicationViewModel()
        {
            Music = new ObservableCollection<Music>()
            {
                new Music{SongName = "Кукла Колдуна", SongerName = "КиШ", Album = "Акустический альбом", Date = 1998, Duration = 3.23}
            };
        }
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

                    }
                    else
                    {
                        MessageBox.Show("Ни один элемент не был добавлен");
                    }
                }));
            }
        }
    }
}
