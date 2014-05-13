//#define Release
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using Callisto.Controls;
using UIElementLeakTester;
//using LinqToVisualTree;
using Callisto.Controls.Common;
using Windows.UI;

using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Networking.Connectivity;
// 빈 페이지 항목 템플릿에 대한 설명은 http://go.microsoft.com/fwlink/?LinkId=234238에 나와 있습니다.

namespace RadioRss
{
    /// <summary>
    /// 자체에서 사용하거나 프레임 내에서 탐색할 수 있는 빈 페이지입니다.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 이 페이지가 프레임에 표시되려고 할 때 호출됩니다.
        /// </summary>
        /// <param name="e">페이지에 도달한 방법을 설명하는 이벤트 데이터입니다. Parameter
        /// 속성은 일반적으로 페이지를 구성하는 데 사용됩니다.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // 초기화 세팅
            Model.StaticVar.BackgroundImageSettgins.Init();
            SettingsPane.GetForCurrentView().CommandsRequested += SelectBG_CommandsRequested;
            SettingsPane.GetForCurrentView().CommandsRequested += FunScreenSettings_CommandsRequested;            
            SettingsPane.GetForCurrentView().CommandsRequested += PrivacyPolicy_CommandsRequested;
            SettingsPane.GetForCurrentView().CommandsRequested += ByTeam_CommandsRequested;
            InternetLoading();
        }

        private void InternetLoading()
        {
            var profile = NetworkInformation.GetInternetConnectionProfile();
            if (profile == null)
            {
                TB_Loading.Text = "인터넷 연결이 정상적이지 않습니다.";
                BTN_Reloading.Visibility = Visibility.Visible;
                return;
            }
            if (profile.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess)
            {
                TB_Loading.Text = "";
                BTN_Reloading.Visibility = Visibility.Collapsed;
                AnalyzePodCast();
            }
            else
            {// 인터넷 연결 실패시 에러코드
                TB_Loading.Text = "인터넷 연결이 정상적이지 않습니다.";
                BTN_Reloading.Visibility = Visibility.Visible;
            }
        }

        private void AnalyzePodCast()
        {
            // 1초뒤에 반복 가능
            TimeSpan delay = TimeSpan.FromMilliseconds(1);
            Windows.System.Threading.ThreadPoolTimer DelayTimer = Windows.System.Threading.ThreadPoolTimer.CreateTimer(
                async (source) =>
                {
                    AddElement();
                }, delay);
        }

        int List_idx = 0;
        private async void AddElement()
        {
            Model.FeedBoard feedBoard = ViewModel.MainViewModel.feedBoard;
            List<PodcastItem> list = new List<PodcastItem>();

            list.Add(new PodcastItem("http://wizard2.sbs.co.kr/w3/podcast/V0000365040.xml", "TV 및 영화"));
            list.Add(new PodcastItem("http://old.ddanzi.com/appstream/Movie.xml", "TV 및 영화"));
            list.Add(new PodcastItem("http://www.iblug.com/xml/itunes/byundurijk.xml", "TV 및 영화"));

            list.Add(new PodcastItem("http://pod.ssenhosting.com/rss/guitarkirk/13another.xml", "음악"));
            list.Add(new PodcastItem("http://cfs.tistory.com/custom/blog/18/187772/skin/images/PHS_Cast_M.xml", "음악"));

            //list.Add(new PodcastItem("http://wizard2.sbs.co.kr/w3/podcast/V0000364436.xml", "코미디"));
            list.Add(new PodcastItem("http://pod.ssenhosting.com/rss/kih21042.xml", "코미디"));
            list.Add(new PodcastItem("http://pod.ssenhosting.com/rss/kontiki21.xml", "코미디"));

            list.Add(new PodcastItem("http://www.iblug.com/podcastxml/easysandroidapp", "컴퓨터"));
            list.Add(new PodcastItem("http://www.iblug.com/xml/itunes/logostein.xml", "컴퓨터"));
            list.Add(new PodcastItem("http://www.iblug.com/podcastxml/comkang4", "컴퓨터"));

            list.Add(new PodcastItem("http://www.iblug.com/podcastxml/agebreak", "게임 및 취미"));
            list.Add(new PodcastItem("http://pod.ssenhosting.com/rss/comofather.xml", "게임 및 취미"));
            list.Add(new PodcastItem("http://www.iblug.com/xml/itunes/crow272.xml", "게임 및 취미"));
#if Release
            list.Add(new PodcastItem("http://www.iblug.com/podcastxml/ilbangbang", "교육"));
            list.Add(new PodcastItem("http://www.jrchina.com/jrchina/podcast/nihaojrc.xml", "교육"));
            list.Add(new PodcastItem("http://pod.ssenhosting.com/rss/guitarkirk/06newenglish.xml", "교육"));

            list.Add(new PodcastItem("http://www.aoosung.com/podcast/aoosung.xml", "건강"));
            list.Add(new PodcastItem("http://pod.ssenhosting.com/rss/yeshira.xml", "건강"));
            list.Add(new PodcastItem("http://podcast2.synapsetech.co.kr/users/saip2013/saip2013.xml", "건강"));

            list.Add(new PodcastItem("http://www.iblug.com/xml/itunes/mlbn.xml", "스포츠 및 레크레이션"));
            list.Add(new PodcastItem("http://www.iblug.com/xml/itunes/tripleplay.xml", "스포츠 및 레크레이션"));
            list.Add(new PodcastItem("http://media.daum.net/export/c4007dc20b9c4564bf81f6369aea5e85.xml?rootdisable=true", "스포츠 및 레크레이션"));

            list.Add(new PodcastItem("http://www.wisdomhouse.kr/sns/redbooks/redbooks.xml", "예술"));
            list.Add(new PodcastItem("http://pod.ssenhosting.com/rss/guitarkirk/guitaefather.xml", "예술"));
            list.Add(new PodcastItem("http://pod.ssenhosting.com/rss/guitarkirk/06humanities.xml", "예술"));

            list.Add(new PodcastItem("http://www.docdocdoc.co.kr/podcast/iam_doctors.xml", "과학 및 의학"));
            list.Add(new PodcastItem("http://www.iblug.com/xml/itunes/sciencewithpeople.xml", "과학 및 의학"));
            list.Add(new PodcastItem("http://feeds.feedburner.com/twinklescience", "과학 및 의학"));

            list.Add(new PodcastItem("http://pod.ssenhosting.com/rss/guitarkirk/kimseotalk.xml", "뉴스 및 정치"));
            list.Add(new PodcastItem("http://www.iblug.com/xml/itunes/facttv00008.xml", "뉴스 및 정치"));
            list.Add(new PodcastItem("http://download.newstapa.org/podcast/podcast.xml", "뉴스 및 정치"));

            list.Add(new PodcastItem("http://pod.ssenhosting.com/rss/guitarkirk/anbo.xml", "정부 및 조직"));
            list.Add(new PodcastItem("http://pod.ssenhosting.com/rss/guitarkirk/chacha.xml", "정부 및 조직"));
            list.Add(new PodcastItem("http://pod.ssenhosting.com/rss/homelearn1.xml", "정부 및 조직"));

            list.Add(new PodcastItem("http://www.iblug.com/xml/itunes/system.xml", "비즈니스"));
            list.Add(new PodcastItem("http://pod.ssenhosting.com/rss/stdfirm.xml", "비즈니스"));
            list.Add(new PodcastItem("http://pod.ssenhosting.com/rss/guitarkirk/19young.xml", "비즈니스"));

            list.Add(new PodcastItem("http://jungto.libsyn.com/rss", "종교 및 역학"));
            list.Add(new PodcastItem("http://morningzion.com/podcast/feed.xml", "종교 및 역학"));
            list.Add(new PodcastItem("http://www.woorichurch.org/bwoori/podcast/rss1-n.xml", "종교 및 역학"));
#endif
            // 샘플
//             feedBoard.AddElement("http://pod.ssenhosting.com/rss/ingyeoin.xml", Model.FeedBoard.strCommon);
//             feedBoard.AddElement"http://www.wisdomhouse.kr/sns/redbooks/redbooks.xml", "카테고리2");

            int Length = list.Count;
            //PB_Loading.Maximum = (double)Length;
            await Dispatcher.RunAsync(
                        Windows.UI.Core.CoreDispatcherPriority.High,
                        () =>{PB_Loading.Maximum = Length;});
            for (List_idx = 0; List_idx < list.Count; List_idx++)
            {
                feedBoard.AddElement(list[List_idx].address, list[List_idx].category);
                await Dispatcher.RunAsync(
                Windows.UI.Core.CoreDispatcherPriority.High,
                () => { PB_Loading.Value = (double)List_idx; });
            }

            await Dispatcher.RunAsync(
                        Windows.UI.Core.CoreDispatcherPriority.Low,
                        () =>
                        {
                            MovePage();
                        });
        }

        struct PodcastItem
        {
            public string address;
            public string category;
            public PodcastItem(string address, string category)
            {
                this.address = address;
                this.category = category;
            }
        }

        private static void MovePage()
        {
            var frame = new Frame();
            //var frame = (Frame)Window.Current.Content;
            //frame.Navigate(typeof(View.PodCastItemsPage));
            frame.Navigate(typeof(View.MainCategoryPage));
            Window.Current.Content = frame;
        }

        #region [ 옵션 적용 ]
        private void SelectBG_CommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            SettingsCommand cmd = new SettingsCommand("옵션", "배경화면 선택", (x) =>
            {
                // create a new instance of the flyout
                SettingsFlyout settings = new SettingsFlyout();
                // set the desired width.  If you leave this out, you will get Narrow (346px)
                settings.FlyoutWidth = (Callisto.Controls.SettingsFlyout.SettingsFlyoutWidth)Enum.Parse(typeof(Callisto.Controls.SettingsFlyout.SettingsFlyoutWidth), "Narrow");

                // optionally change header and content background colors away from defaults (recommended)
                // if using Callisto's AppManifestHelper you can grab the element from some member var you held it in
                // settings.HeaderBrush = new SolidColorBrush(App.VisualElements.BackgroundColor);
                settings.HeaderBrush = new SolidColorBrush(Colors.Coral);
                settings.HeaderText = string.Format("배경화면 선택");
                // 
                //                 // provide some logo (preferrably the smallogo the app uses)
                BitmapImage bmp = new BitmapImage(new Uri("ms-appx:///Assets/logo_small.png"));
                settings.SmallLogoImageSource = bmp;

                // set the content for the flyout
                // 유저 컨트롤을 입력하여, 컨트롤을 사용할 수 있도록 해준다!
                settings.Content = new ViewControl.SelectBackground();

                // open it
                settings.IsOpen = true;

                // this is only for the test app and not needed
                // you would not use this code in your real app
                ObjectTracker.Track(settings);
            });

            args.Request.ApplicationCommands.Add(cmd);
        }
        private void PrivacyPolicy_CommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            SettingsCommand cmd = new SettingsCommand("옵션", "개인 정보 취급 방침", (x) =>
            {
                // create a new instance of the flyout
                SettingsFlyout settings = new SettingsFlyout();
                // set the desired width.  If you leave this out, you will get Narrow (346px)
                settings.FlyoutWidth = (Callisto.Controls.SettingsFlyout.SettingsFlyoutWidth)Enum.Parse(typeof(Callisto.Controls.SettingsFlyout.SettingsFlyoutWidth), "Narrow");

                // optionally change header and content background colors away from defaults (recommended)
                // if using Callisto's AppManifestHelper you can grab the element from some member var you held it in
                // settings.HeaderBrush = new SolidColorBrush(App.VisualElements.BackgroundColor);
                settings.HeaderBrush = new SolidColorBrush(Colors.Brown);
                settings.HeaderText = string.Format("개인 정보 취급 방침");
                // 
                // provide some logo (preferrably the smallogo the app uses)
                BitmapImage bmp = new BitmapImage(new Uri("ms-appx:///Assets/logo_small.png"));
                settings.SmallLogoImageSource = bmp;

                // set the content for the flyout
                // 유저 컨트롤을 입력하여, 컨트롤을 사용할 수 있도록 해준다!
                settings.Content = new ViewControl.PrivacyPolicy();

                // open it
                settings.IsOpen = true;

                // this is only for the test app and not needed
                // you would not use this code in your real app
                ObjectTracker.Track(settings);
            });

            args.Request.ApplicationCommands.Add(cmd);
        }
        private void ByTeam_CommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            SettingsCommand cmd = new SettingsCommand("옵션", "만든 사람들", (x) =>
            {
                // create a new instance of the flyout
                SettingsFlyout settings = new SettingsFlyout();
                // set the desired width.  If you leave this out, you will get Narrow (346px)
                settings.FlyoutWidth = (Callisto.Controls.SettingsFlyout.SettingsFlyoutWidth)Enum.Parse(typeof(Callisto.Controls.SettingsFlyout.SettingsFlyoutWidth), "Narrow");

                // optionally change header and content background colors away from defaults (recommended)
                // if using Callisto's AppManifestHelper you can grab the element from some member var you held it in
                // settings.HeaderBrush = new SolidColorBrush(App.VisualElements.BackgroundColor);
                settings.HeaderBrush = new SolidColorBrush(Colors.Brown);
                settings.HeaderText = string.Format("만든 사람들");
                // 
                // provide some logo (preferrably the smallogo the app uses)
                BitmapImage bmp = new BitmapImage(new Uri("ms-appx:///Assets/logo_small.png"));
                settings.SmallLogoImageSource = bmp;

                // set the content for the flyout
                // 유저 컨트롤을 입력하여, 컨트롤을 사용할 수 있도록 해준다!
                settings.Content = new ViewControl.ByTeam();

                // open it
                settings.IsOpen = true;

                // this is only for the test app and not needed
                // you would not use this code in your real app
                ObjectTracker.Track(settings);
            });

            args.Request.ApplicationCommands.Add(cmd);
        }
        private void FunScreenSettings_CommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            SettingsCommand cmd = new SettingsCommand("옵션", "FunScreen 설정", (x) =>
            {
                // create a new instance of the flyout
                SettingsFlyout settings = new SettingsFlyout();
                // set the desired width.  If you leave this out, you will get Narrow (346px)
                settings.FlyoutWidth = (Callisto.Controls.SettingsFlyout.SettingsFlyoutWidth)Enum.Parse(typeof(Callisto.Controls.SettingsFlyout.SettingsFlyoutWidth), "Narrow");

                // optionally change header and content background colors away from defaults (recommended)
                // if using Callisto's AppManifestHelper you can grab the element from some member var you held it in
                // settings.HeaderBrush = new SolidColorBrush(App.VisualElements.BackgroundColor);
                settings.HeaderBrush = new SolidColorBrush(Colors.DarkGreen);
                settings.HeaderText = string.Format("FunScreen 설정");
                // 
                // provide some logo (preferrably the smallogo the app uses)
                BitmapImage bmp = new BitmapImage(new Uri("ms-appx:///Assets/logo_small.png"));
                settings.SmallLogoImageSource = bmp;

                // set the content for the flyout
                // 유저 컨트롤을 입력하여, 컨트롤을 사용할 수 있도록 해준다!
                settings.Content = new ViewControl.FunScreenSettings();

                // open it
                settings.IsOpen = true;

                // this is only for the test app and not needed
                // you would not use this code in your real app
                ObjectTracker.Track(settings);
            });

            args.Request.ApplicationCommands.Add(cmd);
        } 
        #endregion

        private void BTN_Reloading_Click(object sender, RoutedEventArgs e)
        {
            InternetLoading();
        }
    }
}
