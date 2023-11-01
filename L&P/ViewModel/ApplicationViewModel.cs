using L_P.Model.Event;
using L_P.View;
using System.Windows.Controls;

namespace L_P
{
    public class ApplicationViewModel : Notify
    {
        private ContentControl? myContentControl;
        /// <summary>
        /// Content controls for loading content
        /// </summary>
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
    }
}
