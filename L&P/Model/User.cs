using System.Windows.Controls;

namespace L_P.Model
{
    public class User : Notify
    {
        private string? name;
        private string? email;
        private string? password;
        private Image? image;

        public string? Name
        {
            get { return name; }
            set { name = value; OnPropertyChanged("Name"); }
        }
        public string? Email
        {
            get { return email; }
            set { email = value; OnPropertyChanged("Email"); }
        }
        public string? Password
        {
            get { return password; }
            set { password = value; OnPropertyChanged("Password"); }
        }
        public Image? Image
        {
            get { return image; }
            set { image = value; OnPropertyChanged("Image"); }
        }
    }
}
