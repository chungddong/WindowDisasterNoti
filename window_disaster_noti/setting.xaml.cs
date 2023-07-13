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
using System.Windows.Media.Animation;
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
            cb_darkmode.IsChecked = Properties.Settingdata.Default.cb_darkmode;
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
            Properties.Settingdata.Default.Save();
        }

        private void cb_darkmode_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settingdata.Default.cb_darkmode = cb_darkmode.IsChecked.Value;
            Properties.Settingdata.Default.Save();

            if (Properties.Settingdata.Default.cb_darkmode == true) //다크모드 실행
            {
                Console.WriteLine("다크 모드 켬");
                ChangeTheme(new Uri("Style/Darkmode.xaml", UriKind.Relative));
            }
            else
            {
                Console.WriteLine("다크 모드 끔");
                ChangeTheme(new Uri("Style/Lightmode.xaml", UriKind.Relative));
            }

            

            /*cb_allowNoti.IsChecked = Properties.Settingdata.Default.cb_allowNoti;
            cb_darkmode.IsChecked = Properties.Settingdata.Default.cb_darkmode;
            cb_runOnStartup.IsChecked = Properties.Settingdata.Default.cb_runOnStartup;

            if (Properties.Settingdata.Default.cb_darkmode == true) //다크모드 실행
            {
                Console.WriteLine("다크 모드 켬");
                ChangeTheme(new Uri("Style/Darkmode.xaml", UriKind.Relative));
            }
            else
            {
                Console.WriteLine("다크 모드 끔");
                ChangeTheme(new Uri("Style/Lightmode.xaml", UriKind.Relative));

            }*/

        }

        ResourceDictionary Theme;


        private void ChangeTheme(Uri uri)
        {
            Theme = new ResourceDictionary() { Source = uri };

            App.Current.Resources.Clear();
            App.Current.Resources.MergedDictionaries.Add(Theme);
        }


        private void cb_darkmode_Unchecked(object sender, RoutedEventArgs e)
        {

        }


        private void cb_runOnStartup_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("자동 실행");

            Properties.Settingdata.Default.cb_runOnStartup = cb_runOnStartup.IsChecked.Value;
            Properties.Settingdata.Default.Save();
        }

        private void btn_back_MouseDown(object sender, MouseButtonEventArgs e) //뒤로가기 버튼 클릭 시
        {
            Console.WriteLine("뒤로가기 누름");


            window_info.Show();
            window_info.Left = SystemParameters.WorkArea.Width - this.Width - 100;
            window_info.Top = SystemParameters.WorkArea.Height - this.Height - 100;

            window_info.WindowState = WindowState.Normal;
            window_info.Topmost = true;
            window_info.Activate();

        }

        
    }
}
