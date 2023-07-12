using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
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
            //setNotiTray();
            /*base.OnStartup(e);
            setNotiTray();
            if (noti == null)
            {
                setNotiTray();
            }*/
        }

        private void setNotiTray()
        {
            noti = new Winforms.NotifyIcon();
            //noti.Icon = new System.Drawing.Icon("icon.png");
            var stream = GetResourceStream(new Uri("pack://application:,,,/window_disaster_noti;component/icon.png")).Stream;
            var bitmap = new Bitmap(stream);
            noti.Icon = Icon.FromHandle(bitmap.GetHicon());
            noti.Visible = true;
            noti.Text = "NotiTest";

            
        }
    }



}
