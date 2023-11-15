using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Toolkit.Uwp.Notifications;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
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
        //트레이 아이콘 관련 선언-------------------------------------------------
        Winforms.NotifyIcon noti; //트레이 아이콘을 위한 선언
        Winforms.ContextMenuStrip menu; //트레이 아이콘 메뉴
        //------------------------------------------------------------------------


        //타이머 관련 선언---------------------------------------------------------------------------
        DispatcherTimer timer = new DispatcherTimer(); //주기적 데이터 새로고침을 위한 타이머
        public int refreshTime = 10000; //10초 - 데이터 새로고침 주기
        //-------------------------------------------------------------------------------------------


        
        //데이터 분류 및 처리를 위한 선언------------------------------------------------------------
        private string lastnum; //가장 최근의 재난문자 ID
        private List<DisNoti> list; //이전 메시지들 모음을 위한 리스트 
        private string[] regionList; //특정 지역 수신 설정 시 지역 목록 저장 배열
        ResourceDictionary Theme; //앱 테마를 위한 선언

        //수신데이터 관련
        public string url = "https://www.safekorea.go.kr/idsiSFK/sfk/cs/sua/web/DisasterSmsList.do"; //재난문자 데이터 소스링크
        string payloadData;
        //-------------------------------------------------------------------------------------------



        //날짜와 관련된 선언-------------------------------------------------------------------------
        DateTime today = DateTime.Today; //오늘 날짜 
        DateTime startday; //시작 날짜
        public string date_start; //검색 시작 날짜 오늘 날짜의 이틀전
        public string date_end;  //검색 끝 날짜 오늘 날짜
        //-------------------------------------------------------------------------------------------


        //서버와 관련된 선언-------------------------------------------------------------------------
        private HubConnection hubConnection;

        //-------------------------------------------------------------------------------------------


        public Info()
        {
            InitializeComponent();

            setNotiTray(); //노티실행


            ServerConnect();



            //Console.WriteLine("오늘 날짜 : " + date_end + ", " + "그저께 날짜 : " + date_start);
            //"searchBgnDe\":\"2023-07-01\",\"searchEndDe\":\"2023-07-03\" //날짜 payloaddata 형식

        }


        public void LocalConnect() //서버 작동 안할시 로컬 클라이언트에서 직접 데이터 받아오기
        {
            Console.WriteLine("서버대신 로컬로 가져오기 시작");
            timer.Interval = TimeSpan.FromMilliseconds(refreshTime);  //타이머 간격 설정
            timer.Tick += new EventHandler(timer_Tick);  //타이머에 이벤트(timer_tick) 추가


            startday = today.AddDays(-2); //이틀전으로 시작 날짜 설정
            date_start = startday.ToString("yyyy-MM-dd"); //날짜 형식설정 ex) 2023-07-12
            date_end = today.ToString("yyyy-MM-dd");

            //특정기간의 데이터를 받아오긴 위해 보낼 payloadData 형식
            payloadData = "{\"searchInfo\":{\"pageIndex\":\"1\",\"pageUnit\":\"10\",\"pageSize\":\"10\",\"firstIndex\":\"1\",\"lastIndex\":\"1\",\"recordCountPerPage\":\"10\",\"searchBgnDe\":" + "\"" + date_start + "\"," + "\"searchEndDe\":" + "\"" + date_end + "\",\"searchGb\":\"1\",\"searchWrd\":\"\",\"rcv_Area_Id\":\"\",\"dstr_se_Id\":\"\",\"c_ocrc_type\":\"\",\"sbLawArea1\":\"\",\"sbLawArea2\":\"\",\"sbLawArea3\":\"\"}}";

            timer.Start(); //타이머 시작 - 메인 이벤트 막으려면 주석처리
        }

        public async void ServerConnect() //서버 작동 시 서버에서 데이터 수신
        {
            hubConnection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7262/chathub")
                .Build();

            hubConnection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                //서버로부터 데이터 수신했을 때
                //Console.WriteLine(message);

                OnReceiveFromServer(message);
            });

            try
            {
                await hubConnection.StartAsync();
                Console.WriteLine("연결완료");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"연결 오류: {ex.Message}");
                LocalConnect();
            }
        }




        #region 재난문자 데이터 수신 및 처리 관련 코드

        public void OnReceiveFromServer(string message)
        {
            try
            {
                JObject jobject = JObject.Parse(message);

                //수신받은 내역 class에 담기
                var newitem = new DisNoti
                {
                    Title = "" + jobject["disasterSmsList"][0]["DSSTR_SE_NM"] + " - " + jobject["disasterSmsList"][0]["RCV_AREA_NM"],
                    maintext = "" + jobject["disasterSmsList"][0]["MSG_CN"],
                    timeline = "" + jobject["disasterSmsList"][0]["REGIST_DT"],
                    MessageId = "" + jobject["disasterSmsList"][0]["MD101_SN"],
                    region = "" + jobject["disasterSmsList"][0]["RCV_AREA_NM"]
                };
                NotiProcess(newitem);
            }
            catch (Exception err)
            {
                Console.WriteLine("OnReceiveFromServer : " + err.Message);
            }
        }


        private async void timer_Tick(object sender, EventArgs e)
        {
            try //예외처리
            {
                string boardContent = await GetBoardContent(url, payloadData); //url로 payloadData 보내 데이터 수신받기
                JObject jobject = JObject.Parse(boardContent); //jobject 형태로 boardContent 변환하기(json데이터)

                //수신받은 내역 class에 담기
                var newitem = new DisNoti
                {
                    Title = "" + jobject["disasterSmsList"][0]["DSSTR_SE_NM"] + " - " + jobject["disasterSmsList"][0]["RCV_AREA_NM"],
                    maintext = "" + jobject["disasterSmsList"][0]["MSG_CN"],
                    timeline = "" + jobject["disasterSmsList"][0]["REGIST_DT"],
                    MessageId = "" + jobject["disasterSmsList"][0]["MD101_SN"],
                    region = "" + jobject["disasterSmsList"][0]["RCV_AREA_NM"]
                };


                NotiProcess(newitem);
            }
            catch(Exception err)
            {
                Console.WriteLine("에러(timer_tick) : " + err.Message);
            }
            

            timer.Start(); //재귀 실행


        }


        public void NotiProcess(DisNoti newitem)
        {
            //가장 최근에 수신한 재난문자 ID 와 다르면 새로운 메시지를 추가
            if (newitem.MessageId != lastnum)
            {
                //Console.WriteLine("새로운 알림 수신");

                string region = newitem.region; //수신받은 재난문자의 대상지역 
                lastnum = newitem.MessageId; //마지막으로 수신받은 문자 ID 갱신

                //지역 수신 설정에 따라 수신 결정
                if (Properties.Settingdata.Default.every_region == true) //모든지역 알림 수신 상태
                {
                    Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                    {
                        lstbox.Items.Add(newitem); //info 중앙 문자 수신 창에 메시지 추가
                    }));


                    if (Properties.Settingdata.Default.cb_allowNoti == true) //설정의 알림허용이 켜져있을 경우만 알림 수신
                    {
                        //토스트메시지 생성
                        new ToastContentBuilder().AddText(newitem.Title).AddText(newitem.maintext).Show(); //토스트알림
                    }
                }
                else //사용자 지정지역 알림 수신 상태
                {
                    //Console.WriteLine("지역 목록 : " + region);

                    //해당지역을 포함하고 있는지 확인한 다음 알림 진행
                    for (int i = 0; i < regionList.Length; i++)
                    {
                        if (region.Contains(regionList[i])) //지역정보가 설정 지역 리스트 와 겹치는게 있다면
                        {
                            //Console.WriteLine("겹치는 지역 발견!");
                            //UI 담당 스레드 아닐 경우 에러 방지 위해
                            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                            {
                                lstbox.Items.Add(newitem); //info 중앙 문자 수신 창에 메시지 추가
                            }));

                            if (Properties.Settingdata.Default.cb_allowNoti == true) //설정의 알림허용이 켜져있을 경우만 알림 수신
                            {
                                //토스트메시지 생성
                                new ToastContentBuilder().AddText(newitem.Title).AddText(newitem.maintext).Show(); //토스트알림
                            }
                            break; //알림 두번 되면 안되니까 
                        }
                    }
                }

            }
        }

       

        async Task<string> GetBoardContent(string requestUrl, string payloadData)  //url로부터 json데이터 가져오는 코드
        {
            using (HttpClient client = new HttpClient())
            {
                string result = "";
                try
                {
                    StringContent content = new StringContent(payloadData, System.Text.Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync(requestUrl, content);
                    response.EnsureSuccessStatusCode(); // 요청이 성공적으로 완료되었는지 확인

                    result = await response.Content.ReadAsStringAsync();
                    
                }
                catch(Exception err)
                {
                    Console.WriteLine("에러(GetBoardContent) : " + err.Message);
                }

                return result;

            }
        }

        #endregion

        #region UI 이벤트 관련

        private void btn_set_clicked(object sender, MouseButtonEventArgs e) //info 창 내의 세팅버튼 클릭시
        {
            //세팅화면 이동
            window_disaster_noti.setting window_set = new window_disaster_noti.setting(this); 
            window_set.Show();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            noti.Visible = false; //종료후에도 남아있는 트레이 아이콘 지우기
            noti.Icon = null;
        }

        private void ChangeTheme(Uri uri)
        {
            Theme = new ResourceDictionary() { Source = uri };

            App.Current.Resources.Clear();
            App.Current.Resources.MergedDictionaries.Add(Theme);
        }

        #endregion

        #region 창 활성화 및 비활성화 관련

        //창이 활성화된 경우 - 값이 새로고침되어야 하는 것들은 여기서 처리
        private void Window_Activated(object sender, EventArgs e)
        {


            Console.WriteLine("info창 활성화");

            //다크모드 설정되어 있을 경우
            if (Properties.Settingdata.Default.cb_darkmode == true)
            {
                Console.WriteLine("다크 모드 켬");
                ChangeTheme(new Uri("Style/Darkmode.xaml", UriKind.Relative));
            }
            else
            {
                Console.WriteLine("다크 모드 끔");
                ChangeTheme(new Uri("Style/Lightmode.xaml", UriKind.Relative));
            }

            //info창 하단 수신 지역 텍스트 변경
            if (Properties.Settingdata.Default.every_region == true) //모든 지역 설정이 켜져 있을 경우
            {
                label_bottom_gps.Content = "전국 전체";
            }
            else //특정 지역 수신일 경우
            {
                label_bottom_gps.Content = "| ";
                regionList = Properties.Settingdata.Default.city.Split(','); // 설정 지역 리스트 가져오기
                for (int i = 0; i < regionList.Length; i++)
                {
                    label_bottom_gps.Content += regionList[i] + " |";
                }
            }


        }

        //창이 비활성화된 경우
        private void Window_Deactivated(object sender, EventArgs e) // 창 이외의 공간 클릭시
        {
            Console.WriteLine("창이 비활성화됨");

            this.Hide();
        }

        #endregion
        

        #region 트레이 아이콘 관련 코드

        private void setNotiTray() //트레이 관련 코드
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

            noti.Icon = new System.Drawing.Icon(AppDomain.CurrentDomain.BaseDirectory + @"\ICON.ico");
            noti.Visible = true;
            noti.Text = "재난 알리미";
            noti.ContextMenuStrip = menu;


            noti.DoubleClick += delegate (object sender, EventArgs eventArgs) //아이콘 더블클릭시 실행
            {

                this.Activate(); //창이 활성화된상태로 만들기
                // 화면을 최소화 상태에서 다시 보여줍니다.
                this.Show();

                this.Focus();

                this.Left = SystemParameters.WorkArea.Width - this.Width - 100;
                this.Top = SystemParameters.WorkArea.Height - this.Height - 100;

                // 화면 상태를 Normal로 설정합니다.
                this.WindowState = WindowState.Normal;
                this.Topmost = true; //가장 위에 화면이 뜨게



            };
        }

        #endregion
    }




    public class DisNoti  //메시지 출력용 구조 선언
    {
        public string Title { get; set; } //메시지 제목

        public string maintext { get; set; } //메시지 내용

        public string timeline { get; set; } //메시지 수신 시간

        public string MessageId { get; set; }  //메시지 아이디

        public string region { get; set; } //지역
    }
}
