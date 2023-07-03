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
        DispatcherTimer timer = new DispatcherTimer(); //타이머 선언
        private List<noti> list; //이전 메시지들 모음을 위한 리스트 

        private string lastnum; //가장 최근의 재난문자 ID

        public string url = "https://www.safekorea.go.kr/idsiSFK/sfk/cs/sua/web/DisasterSmsList.do"; //재난문자 데이터 소스링크

        public main()
        {
            InitializeComponent();

            


            timer.Interval = TimeSpan.FromMilliseconds(3000);    //시간간격 설정

            timer.Tick += new EventHandler(timer_Tick);          //이벤트 추가

            //timer.Start(); //타이머 시작 - 메인 이벤트 막으려면 주석처리

        }

        private async void timer_Tick(object sender, EventArgs e)
        {
            string payloadData = "{\"searchInfo\":{\"pageIndex\":\"1\",\"pageUnit\":\"10\",\"pageSize\":\"10\",\"firstIndex\":\"1\",\"lastIndex\":\"1\",\"recordCountPerPage\":\"10\",\"searchBgnDe\":\"2023-07-01\",\"searchEndDe\":\"2023-07-03\",\"searchGb\":\"1\",\"searchWrd\":\"\",\"rcv_Area_Id\":\"\",\"dstr_se_Id\":\"\",\"c_ocrc_type\":\"\",\"sbLawArea1\":\"\",\"sbLawArea2\":\"\",\"sbLawArea3\":\"\"}}";

            string boardContent = await GetBoardContent(url, payloadData);


            JObject jobject = JObject.Parse(boardContent); //jobject 형태로 payloadData 가져오기(json데이터)

            //List<DisData> disData = JsonConvert.DeserializeObject<List<DisData>>(boardContent);

            var newitem = new noti { Title = "" + jobject["disasterSmsList"][0]["DSSTR_SE_NM"] + " - " + jobject["disasterSmsList"][0]["RCV_AREA_NM"], maintext = "" + jobject["disasterSmsList"][0]["MSG_CN"], timeline = "" + jobject["disasterSmsList"][0]["REGIST_DT"]};

            if("" + jobject["disasterSmsList"][0]["MD101_SN"] != lastnum) //가장 최근의 재난문자 id 와 다르면 새로운 메시지를 추가
            {
                lastnum = "" + jobject["disasterSmsList"][0]["MD101_SN"];
                lstbox.Items.Add(newitem);
            }
            

            timer.Start(); //재귀 실행


        }

        async Task<string> GetBoardContent(string requestUrl, string payloadData)  //url로부터 json데이터 가져오는 코드
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

        private void btn_set_clicked(object sender, MouseButtonEventArgs e)  //세팅버튼 눌렸을 경우
        {
            MessageBox.Show("눌림");
        }
    }

    public class noti  //메시지 출력용 구조 선언
    {
        public string Title { get; set; } //제목

        public string maintext { get; set; } //내용

        public string timeline { get; set; } //시간
    }
}
