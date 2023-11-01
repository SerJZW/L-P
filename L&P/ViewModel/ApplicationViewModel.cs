using L_P.Model.Event;
using L_P.View;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Input;

namespace L_P
{
    public class ApplicationViewModel : Notify
    {
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
    }
}
