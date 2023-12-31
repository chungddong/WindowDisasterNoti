﻿using System;
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
        Info window_info; //info 창 선언
        ResourceDictionary Theme; //앱 테마를 위한 선언



        //프로그램 자동실행 관련 선언----------------------------------------------------------- 
        string regPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
        string programName = "재난 알리미";
        //--------------------------------------------------------------------------------------

        

        public setting(Info info)
        {
            InitializeComponent();

            window_info = info;

            //설정데이터에서 저장된 데이터 불러오기
            cb_allowNoti.IsChecked = Properties.Settingdata.Default.cb_allowNoti;
            cb_darkmode.IsChecked = Properties.Settingdata.Default.cb_darkmode;
            cb_runOnStartup.IsChecked = Properties.Settingdata.Default.cb_runOnStartup;

            Console.WriteLine("" + Properties.Settingdata.Default.city.ToString());
            tb_region_user.Text = Properties.Settingdata.Default.city.ToString();


        }

        #region 창 활성화 및 비활성화 관련

        //창이 활성화 됬을 경우
        private void Window_Activated(object sender, EventArgs e)
        {
            //창 시작 위치 조정
            this.Left = SystemParameters.WorkArea.Width - this.Width - 100;
            this.Top = SystemParameters.WorkArea.Height - this.Height - 100;

            //설정버튼 초기화
            cb_allowNoti.IsChecked = Properties.Settingdata.Default.cb_allowNoti;
            cb_darkmode.IsChecked = Properties.Settingdata.Default.cb_darkmode;
            cb_runOnStartup.IsChecked = Properties.Settingdata.Default.cb_runOnStartup;

            //앱 테마 적용
            if (Properties.Settingdata.Default.cb_darkmode == true) //다크모드 on
            {
                Console.WriteLine("다크 모드 켬");
                ChangeTheme(new Uri("Style/Darkmode.xaml", UriKind.Relative));
            }
            else
            {
                Console.WriteLine("다크 모드 끔");
                ChangeTheme(new Uri("Style/Lightmode.xaml", UriKind.Relative));
            }

            //지역 설정 데이터 불러오기
            if (Properties.Settingdata.Default.every_region == true) //모든 지역 설정이 켜져있을 경우
            {
                rb_every.IsChecked = true;
                tb_region_user.IsEnabled = false;

            }
            else //꺼져있을 경우 - 사용자 지정위치로 저장된 경우
            {
                rb_user.IsChecked = true;
                tb_region_user.IsEnabled = true;
            }


        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            Console.WriteLine("창이 비활성화됨");
            this.Close();
        }

        #endregion


        #region 각종 설정 버튼 및 설정 내용 수정 시

        private void cb_allowNoti_Click(object sender, RoutedEventArgs e) //세팅 - 알림허용
        {
            Console.WriteLine("알림 설정");

            Properties.Settingdata.Default.cb_allowNoti = cb_allowNoti.IsChecked.Value;
            Properties.Settingdata.Default.Save();
        }

        private void cb_darkmode_Click(object sender, RoutedEventArgs e) //세팅 - 다크모드 설정
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

        }

        private void tb_region_user_TextChanged(object sender, TextChangedEventArgs e) //사용자 지정지역 변경시
        {
            Console.WriteLine("글씨 바뀌고 저장함" + tb_region_user.Text.ToString());
            Properties.Settingdata.Default.city = tb_region_user.Text;
            Properties.Settingdata.Default.Save();
        }

        private void rb_every_Checked(object sender, RoutedEventArgs e) //모든 지역 알림 수신 설정 체크시
        {
            Console.WriteLine("모든지역 체크됨");
            Properties.Settingdata.Default.every_region = true;
            Properties.Settingdata.Default.Save();
            tb_region_user.IsEnabled = false;
            Console.WriteLine("설정확인 : " + Properties.Settingdata.Default.every_region);
        }

        private void rb_user_Checked(object sender, RoutedEventArgs e) //사용자 지정지역 알림 수신 설정 체크시
        {
            Console.WriteLine("사용자 지정지역 체크됨");
            Properties.Settingdata.Default.every_region = false;
            Properties.Settingdata.Default.Save();
            tb_region_user.IsEnabled = true;
            Console.WriteLine("설정확인 : " + Properties.Settingdata.Default.every_region);
        }

        private void cb_runOnStartup_Click(object sender, RoutedEventArgs e) //자동실행 설정 체크 시
        {
            Console.WriteLine("자동 실행");

            Properties.Settingdata.Default.cb_runOnStartup = cb_runOnStartup.IsChecked.Value;
            Properties.Settingdata.Default.Save();

            if(Properties.Settingdata.Default.cb_runOnStartup == true)
            {
                try
                {
                    using (var regKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(regPath, true))
                    {
                        if(regKey.GetValue(programName) == null)
                        {
                            regKey.SetValue(programName, System.Environment.CurrentDirectory.ToString() + @"\재난 알리미.exe");
                        }
                        else
                        {
                            if(regKey.GetValue(programName) != null)
                            {
                                regKey.DeleteValue(programName, false);
                            }
                        }

                        regKey.Close();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                using (var regKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(regPath, true))
                {
                    if (regKey.GetValue(programName) != null)
                    {
                        regKey.DeleteValue(programName, false);
                    }

                    regKey.Close();
                }
            }
        }

        #endregion




        //테마변경을 위한 함수
        private void ChangeTheme(Uri uri)
        {
            Theme = new ResourceDictionary() { Source = uri };

            App.Current.Resources.Clear();
            App.Current.Resources.MergedDictionaries.Add(Theme);
        }

        private void btn_back_MouseDown(object sender, MouseButtonEventArgs e) //뒤로가기 버튼 클릭 시
        {
            Console.WriteLine("뒤로가기 누름");

            //info창 실행 
            window_info.Show();
            window_info.Left = SystemParameters.WorkArea.Width - this.Width - 100;
            window_info.Top = SystemParameters.WorkArea.Height - this.Height - 100;

            window_info.WindowState = WindowState.Normal;
            window_info.Topmost = true;
            window_info.Activate();

        }

        
    }
}
