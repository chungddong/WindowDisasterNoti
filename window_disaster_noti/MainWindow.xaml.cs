using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PuppeteerSharp;

namespace window_disaster_noti
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer();

        public int num = 0;

        public MainWindow()
        {
            InitializeComponent();

            textBox.Text = "dsfsdf";

            timer.Interval = TimeSpan.FromMilliseconds(3000);    //시간간격 설정

            timer.Tick += new EventHandler(timer_Tick);          //이벤트 추가

            timer.Start();                                       //타이머 시작. 종료는 timer.Stop(); 으로 한다



        }


        private async void timer_Tick(object sender, EventArgs e)
        {

            string url = "https://www.safekorea.go.kr/idsiSFK/sfk/cs/sua/web/DisasterSmsList.do";
            string payloadData = "{\"searchInfo\":{\"pageIndex\":\"1\",\"pageUnit\":\"10\",\"pageSize\":\"10\",\"firstIndex\":\"1\",\"lastIndex\":\"1\",\"recordCountPerPage\":\"10\",\"searchBgnDe\":\"2023-06-25\",\"searchEndDe\":\"2023-06-27\",\"searchGb\":\"1\",\"searchWrd\":\"\",\"rcv_Area_Id\":\"\",\"dstr_se_Id\":\"\",\"c_ocrc_type\":\"\",\"sbLawArea1\":\"\",\"sbLawArea2\":\"\",\"sbLawArea3\":\"\"}}";

            string boardContent = await GetBoardContent(url, payloadData);

            
            textBox.Text = boardContent;

            JObject jobject = JObject.Parse(boardContent);

            //List<DisData> disData = JsonConvert.DeserializeObject<List<DisData>>(boardContent);

            Console.WriteLine("" + jobject.ToString());

            //text_newest.Content = "게시물 번호 : " + jobject["disasterSmsList"][0]["MD101_SN"];
            //textBox_content.Text = "게시물 내용 : " +jobject["disasterSmsList"][0]["MSG_CN"];

            text_update_num.Content = num + "번째 업데이트";
            num++;
            //timer.Start();
        }

        async Task<string> GetBoardContent(string requestUrl, string payloadData)
        {
            using (HttpClient client = new HttpClient())
            {
                StringContent content = new StringContent(payloadData, System.Text.Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(requestUrl, content);
                response.EnsureSuccessStatusCode(); // 요청이 성공적으로 완료되었는지 확인

                string result = await response.Content.ReadAsStringAsync();
                return result;
            }
        }


        public class DisData
        {
            public string DSSTR_SE_NM { get; set; }
            public string CREAT_DT { get; set; }
            public string RCV_AREA_NM { get; set; }
            public string MD101_SN { get; set; }
            public string DSSTR_SE_ID { get; set; }
            public string MSG_CN { get; set; }
            public string REGIST_DT { get; set; }
            public string EMRGNCY_STEP_NM { get; set; }

        }

        private void textBox_Copy_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
