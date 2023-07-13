using Microsoft.Toolkit.Uwp.Notifications;
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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using Winforms = System.Windows.Forms;


namespace window_disaster_noti
{
    /// <summary>
    /// main.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Info : Window
    {
        Winforms.NotifyIcon noti; //notifyicon을 위한 선언
        Winforms.ContextMenuStrip menu;

        DispatcherTimer timer = new DispatcherTimer();
        private List<noti> list; //이전 메시지들 모음을 위한 리스트 

        private string lastnum; //가장 최근의 재난문자 ID

        public string url = "https://www.safekorea.go.kr/idsiSFK/sfk/cs/sua/web/DisasterSmsList.do"; //재난문자 데이터 소스링크

        public int refreshTime = 3000;

        DateTime today = DateTime.Today;
        DateTime startday; //시작 날짜
        public string date_start; //검색 시작 날짜 오늘 날짜의 이틀전
        public string date_end;  //검색 끝 날짜 오늘 날짜

        string payloadDataBakup = "{\"searchInfo\":{\"pageIndex\":\"1\",\"pageUnit\":\"10\",\"pageSize\":\"10\",\"firstIndex\":\"1\",\"lastIndex\":\"1\",\"recordCountPerPage\":\"10\",\"searchBgnDe\":\"2023-07-01\",\"searchEndDe\":\"2023-07-03\",\"searchGb\":\"1\",\"searchWrd\":\"\",\"rcv_Area_Id\":\"\",\"dstr_se_Id\":\"\",\"c_ocrc_type\":\"\",\"sbLawArea1\":\"\",\"sbLawArea2\":\"\",\"sbLawArea3\":\"\"}}";
        string payloadData;

        

        public Info()
        {
            InitializeComponent();


            setNotiTray(); //노티실행

            Console.WriteLine("info창 시작");

            timer.Interval = TimeSpan.FromMilliseconds(refreshTime);    //시간간격 설정

            timer.Tick += new EventHandler(timer_Tick);          //이벤트 추가


            startday = today.AddDays(-2); //이틀전으로 시작 날짜 설정
            date_start = startday.ToString("yyyy-MM-dd");
            date_end = today.ToString("yyyy-MM-dd");

            payloadData = "{\"searchInfo\":{\"pageIndex\":\"1\",\"pageUnit\":\"10\",\"pageSize\":\"10\",\"firstIndex\":\"1\",\"lastIndex\":\"1\",\"recordCountPerPage\":\"10\",\"searchBgnDe\":" + "\"" + date_start + "\"," + "\"searchEndDe\":" + "\"" + date_end + "\",\"searchGb\":\"1\",\"searchWrd\":\"\",\"rcv_Area_Id\":\"\",\"dstr_se_Id\":\"\",\"c_ocrc_type\":\"\",\"sbLawArea1\":\"\",\"sbLawArea2\":\"\",\"sbLawArea3\":\"\"}}";

            Console.WriteLine("오늘 날짜 : " + date_end + ", " + "그저께 날짜 : " + date_start);

            //timer.Start(); //타이머 시작 - 메인 이벤트 막으려면 주석처리

            //"searchBgnDe\":\"2023-07-01\",\"searchEndDe\":\"2023-07-03\" //날짜 payloaddata 형식

            

        }

        private void setNotiTray()
        {
            Console.WriteLine("노티 실행");
            this.Hide();
            menu = new Winforms.ContextMenuStrip();
            noti = new Winforms.NotifyIcon();

            ToolStripMenuItem set = new ToolStripMenuItem();
            ToolStripMenuItem exit = new ToolStripMenuItem();

            set.Name = "set";
            set.Text = "세팅";

            set.Click += delegate (object click, EventArgs eClick) //세팅버튼
            {
                Console.WriteLine("세팅 클릭");
                window_disaster_noti.setting window_set = new window_disaster_noti.setting(this);

                window_set.Show();
            };

            exit.Name = "eixt";
            exit.Text = "종료";

            exit.Click += delegate (object click, EventArgs eClick) //종료버튼
            {
                Console.WriteLine("종료 클릭");
                this.Close();
                Environment.Exit(0); //위 아래 둘중 하나만
            };

            menu.Items.Add(set);
            menu.Items.Add(exit);

            noti.Icon = new System.Drawing.Icon("ICON.ico");
            noti.Visible = true;
            noti.Text = "재난 알리미";
            noti.ContextMenuStrip = menu;
            

            noti.DoubleClick += delegate (object sender, EventArgs eventArgs) //아이콘 더블클릭시 실행
            {
                
                this.Activate(); //창이 활성화된상태로 만들기
                // 화면을 최소화 상태에서 다시 보여줍니다.
                this.Show();

                this.Left = SystemParameters.WorkArea.Width - this.Width - 100;
                this.Top = SystemParameters.WorkArea.Height - this.Height - 100;

                // 화면 상태를 Normal로 설정합니다.
                this.WindowState = WindowState.Normal;
                this.Topmost = true; //가장 위에 화면이 뜨게


            };
        }

        private async void timer_Tick(object sender, EventArgs e)
        {
            string boardContent = await GetBoardContent(url, payloadData);


            JObject jobject = JObject.Parse(boardContent); //jobject 형태로 payloadData 가져오기(json데이터)

            //List<DisData> disData = JsonConvert.DeserializeObject<List<DisData>>(boardContent);

            Console.WriteLine("n번째 실행중"); //콘솔창 테스트용(백그라운드 작동) - 나중에 삭제해도 됨

            //아래 에외발생 주의 - try catch 달기
            var newitem = new noti { Title = "" + jobject["disasterSmsList"][0]["DSSTR_SE_NM"] + " - " + jobject["disasterSmsList"][0]["RCV_AREA_NM"], maintext = "" + jobject["disasterSmsList"][0]["MSG_CN"], timeline = "" + jobject["disasterSmsList"][0]["REGIST_DT"]};

            if("" + jobject["disasterSmsList"][0]["MD101_SN"] != lastnum) //가장 최근의 재난문자 id 와 다르면 새로운 메시지를 추가
            {
                Console.WriteLine("새로운 알림 수신");
                lastnum = "" + jobject["disasterSmsList"][0]["MD101_SN"];
                lstbox.Items.Add(newitem);

                new ToastContentBuilder().AddText("" + jobject["disasterSmsList"][0]["DSSTR_SE_NM"]).AddText("" + jobject["disasterSmsList"][0]["MSG_CN"]).Show(); //토스트알림

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

        private void btn_set_clicked(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine("세팅버튼 눌림");
            window_disaster_noti.dialog_set ds = new window_disaster_noti.dialog_set();

            ds.Show();
        }

        private void Window_Deactivated(object sender, EventArgs e) // 창 이외의 공간 클릭시
        {
            Console.WriteLine("창이 비활성화됨");

            this.Hide();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            noti.Visible = false; //종료후에도 남아있는 트레이 아이콘 지우는 거 - 나중에 다시 확인 
            noti.Icon = null;
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            Console.WriteLine("info창 활성화");

            
        }
    }

    public class noti  //메시지 출력용 구조 선언
    {
        public string Title { get; set; } //제목

        public string maintext { get; set; }

        public string timeline { get; set; }
    }
}
