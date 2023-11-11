using L_P.Model.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace L_P.ViewModel
{
    class SmthCommand
    {
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
