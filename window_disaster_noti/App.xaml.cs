using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Winforms = System.Windows.Forms;

namespace window_disaster_noti
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        Winforms.NotifyIcon noti;

        private void Onstartup(object sender, StartupEventArgs e)
        {
            /*base.OnStartup(e);
            setNotiTray();*/
        }

        private void setNotiTray()
        {
            noti = new Winforms.NotifyIcon();
            //noti.Icon = new System.Drawing.Icon("icon.png");
            noti.Visible = true;
            noti.Text = "NotiTest";

            
        }
    }



}
