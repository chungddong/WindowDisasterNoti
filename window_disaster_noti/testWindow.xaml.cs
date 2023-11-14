using Microsoft.Toolkit.Uwp.Notifications;
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
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.SignalR.Client;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace window_disaster_noti
{
    /// <summary>
    /// testWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class testWindow : Window
    {
        private HubConnection hubConnection;

        public testWindow()
        {
            InitializeComponent();
            

        }

        private async void InitializeSignalRConnection()
        {
            hubConnection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7262/chathub")
                .Build();

            hubConnection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                /*Dispatcher.Invoke(() => //Console.WriteLine("Name : " + user + ", Message : " + message));
                new ToastContentBuilder().AddText(user).AddText(message).Show());*/
                Console.WriteLine(message);


                

                /*JObject jobject = JObject.Parse(message); //jobject 형태로 boardContent 변환하기(json데이터)

                new ToastContentBuilder().AddText("" + jobject["disasterSmsList"][0]["DSSTR_SE_NM"]).AddText("" + jobject["disasterSmsList"][0]["MSG_CN"]).Show(); //토스트알림*/
            });

            try {
                await hubConnection.StartAsync();
                Console.WriteLine("연결완료");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"연결 오류: {ex.Message}");
            }



            
        }






        private void toastBtn_Click(object sender, RoutedEventArgs e)
        {
            InitializeSignalRConnection();
            //new Uri(@"D:\바탕화면\개발\오버레이덱 - 이미지소스\ic_nextPage.png")
            //new ToastContentBuilder().AddText("[대구광역시]").AddText("침수 우려에 따라 7시30분로 신천동로 양방향 전면 교통통제 실시합니다. 우회도로를 이용하시기 바랍니다.").AddAppLogoOverride(new Uri(@"D:\바탕화면\개발\오버레이덱 - 이미지소스\traffic.png")).Show();
        }

    }
}
