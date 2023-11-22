using L_P.Model.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace L_P.ViewModel
{
    public class StyleSwitcher : Notify
    {
        public StyleSwitcher() 
        {
            OnPropertyChanged("IsDarkTheme");
            WindowStyle = IsDarkTheme ? Application.Current.FindResource("DarkWindowStyle") as Style : Application.Current.FindResource("LightWindowStyle") as Style;
            UserControlStyle = IsDarkTheme ? Application.Current.FindResource("DarkUC") as Style : Application.Current.FindResource("LightUC") as Style;
        }
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
