using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
using System.Windows.Threading;

namespace window_disaster_noti
{
    /// <summary>
    /// main.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class main : Window
    {
        DispatcherTimer timer = new DispatcherTimer();
        private List<noti> list;

        private string lastnum;

        public main()
        {
            InitializeComponent();

            


            timer.Interval = TimeSpan.FromMilliseconds(3000);    //시간간격 설정

            timer.Tick += new EventHandler(timer_Tick);          //이벤트 추가

            timer.Start();

        }

        private async void timer_Tick(object sender, EventArgs e)
        {

            string url = "https://www.safekorea.go.kr/idsiSFK/sfk/cs/sua/web/DisasterSmsList.do";
            string payloadData = "{\"searchInfo\":{\"pageIndex\":\"1\",\"pageUnit\":\"10\",\"pageSize\":\"10\",\"firstIndex\":\"1\",\"lastIndex\":\"1\",\"recordCountPerPage\":\"10\",\"searchBgnDe\":\"2023-06-25\",\"searchEndDe\":\"2023-06-27\",\"searchGb\":\"1\",\"searchWrd\":\"\",\"rcv_Area_Id\":\"\",\"dstr_se_Id\":\"\",\"c_ocrc_type\":\"\",\"sbLawArea1\":\"\",\"sbLawArea2\":\"\",\"sbLawArea3\":\"\"}}";

            string boardContent = await GetBoardContent(url, payloadData);


            JObject jobject = JObject.Parse(boardContent);

            //List<DisData> disData = JsonConvert.DeserializeObject<List<DisData>>(boardContent);

            var newitem = new noti { Title = "" + jobject["disasterSmsList"][0]["MD101_SN"], maintext = "" + jobject["disasterSmsList"][0]["MSG_CN"] };

            if("" + jobject["disasterSmsList"][0]["MD101_SN"] != lastnum)
            {
                lastnum = "" + jobject["disasterSmsList"][0]["MD101_SN"];
                lstbox.Items.Add(newitem);
            }
            

            timer.Start();


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

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }

    public class noti
    {
        public string Title { get; set; }

        public string maintext { get; set; }
    }
}
