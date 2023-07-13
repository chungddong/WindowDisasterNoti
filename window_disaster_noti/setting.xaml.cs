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
        public setting()
        {
            InitializeComponent();


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
    }
}
