using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace window_disaster_noti
{
    /// <summary>
    /// setting.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class setting : Window
    {
        Info window_info;
        public setting(Info info)
        {
            InitializeComponent();

            window_info = info;

            //설정데이터에서 저장된 데이터 불러오기
            cb_allowNoti.IsChecked = Properties.Settingdata.Default.cb_allowNoti;
            cb_usePopNoti.IsChecked = Properties.Settingdata.Default.cb_usePopNoti;
            cb_runOnStartup.IsChecked = Properties.Settingdata.Default.cb_runOnStartup;

        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            Console.WriteLine("창이 비활성화됨");

            this.Close();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            this.Left = SystemParameters.WorkArea.Width - this.Width - 100;
            this.Top = SystemParameters.WorkArea.Height - this.Height - 100;

            
        }


        private void cb_allowNoti_Click(object sender, RoutedEventArgs e) //세팅 - 알림허용
        {
            Console.WriteLine("알림 설정");

            Properties.Settingdata.Default.cb_allowNoti = cb_allowNoti.IsChecked.Value;
        }

        private void cb_usePopNoti_Click(object sender, RoutedEventArgs e) //세팅 - 팝업 알림
        {
            Console.WriteLine("팝업 알림");

            Properties.Settingdata.Default.cb_usePopNoti = cb_usePopNoti.IsChecked.Value;
        }

        private void cb_runOnStartup_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("팝업 알림");

            Properties.Settingdata.Default.cb_runOnStartup = cb_runOnStartup.IsChecked.Value;
        }

        private void btn_back_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine("뒤로가기 누름");


            window_info.Show();
            window_info.Left = SystemParameters.WorkArea.Width - this.Width - 100;
            window_info.Top = SystemParameters.WorkArea.Height - this.Height - 100;

            window_info.WindowState = WindowState.Normal;
            window_info.Topmost = true;

        }
    }
}
