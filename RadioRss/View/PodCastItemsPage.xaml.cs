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

// 기본 페이지 항목 템플릿에 대한 설명은 http://go.microsoft.com/fwlink/?LinkId=234237에 나와 있습니다.

namespace RadioRss.View
{
    /// <summary>
    /// 대부분의 응용 프로그램에 공통되는 특성을 제공하는 기본 페이지입니다.
    /// </summary>
    public sealed partial class PodCastItemsPage : RadioRss.Common.LayoutAwarePage, Model.StaticVar.IPageBackground, Model.StaticVar.IFunScreen, Model.StaticVar.IMusicPlayerHandler
    {
        public PodCastItemsPage()
        {
            this.InitializeComponent();
        }

        string CategoryName;
        public void ChangeBackGround()
        {
            RootGrid.Background = Model.StaticVar.BackgroundImageSettgins.myBrush;
        }

        /// <summary>
        /// 탐색 중 전달된 콘텐츠로 페이지를 채웁니다. 이전 세션의 페이지를
        /// 다시 만들 때 저장된 상태도 제공됩니다.
        /// </summary>
        /// <param name="navigationParameter">이 페이지가 처음 요청될 때
        /// <see cref="Frame.Navigate(Type, Object)"/>에 전달된 매개 변수 값입니다.
        /// </param>
        /// <param name="pageState">이전 세션 동안 이 페이지에 유지된
        /// 사전 상태입니다. 페이지를 처음 방문할 때는 이 값이 null입니다.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            //Grid_ListView.Children.Add()
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // 배경화면
            ChangeBackGround();
            Model.StaticVar.PodCastItemsPageBG = this;
            Model.StaticVar.FunScreen = this;
            Model.StaticVar.MusicPlayerHandler = this;

            CategoryName = (string)e.Parameter;
            ViewModel.MainViewModel.feedBoard.InitRefreshSet_Blogs(CategoryName);
            pageTitle.Text = CategoryName;

            var lists = ViewModel.MainViewModel.feedBoard.Blogs;
            LV_Channels.ItemsSource = lists;

            //LV_Channels.ItemsSource = ViewModel.MainViewModel.feedBoard.Blogs;
            // MainViewModel에서 Category 목록을 불러온다.
            //ViewModel.MainViewModel.feedBoard.InitRefreshSet_Blogs(CategoryName);

            // 펀스크린
//             funsScreenMain = new FunScreen.FunScreenMain();
//             funsScreenMain.Width = 1366;
//             funsScreenMain.Height = 768;
//             RootGrid.Children.Add(funsScreenMain);
//             ShowFunScreen();
            // 펀스크린을 시간에 맞추어 띄우기 위함
            onFunScreen = false;
            timer = 0;
            if (task != null)
            {
                task = new System.Threading.Tasks.Task(waitingTimeForFunScreen);
            }
        }


        /// <summary>
        /// 응용 프로그램이 일시 중지되거나 탐색 캐시에서 페이지가 삭제된 경우
        /// 이 페이지와 관련된 상태를 유지합니다. 값은
        /// <see cref="SuspensionManager.SessionState"/>의 serialization 요구 사항을 만족해야 합니다.
        /// </summary>
        /// <param name="pageState">serializable 상태로 채워질 빈 사전입니다.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
        }

        private void LV_Channels_ItemClick(object sender, ItemClickEventArgs e)
        {
            var listView = sender as ListView;
            listView.Visibility = Visibility.Collapsed;

            var items = e.ClickedItem as Model.FB_Blog;
            var lists = items.FB_Posts;
            LV_Items.ItemsSource = lists;
            LV_Items.Visibility = Visibility.Visible;
            //BTN_CloseItems.Visibility = Visibility.Visible;
            pageTitle.Text = items.Title;

            SelectedImageUri = items.ImageUri;

            // 백버튼 활용
            backButton.Click -= GoBack;
            backButton.Click += BTN_CloseItems_Click;
        }

        private Uri SelectedImageUri;
        private ListView CurrentListView;
        private int CurrentSelectIDX;
        private void LV_Items_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listView = sender as ListView;
            int selectIndex = listView.SelectedIndex;
            CurrentListView = LV_Items;
            if (selectIndex >= 0)
            {
                CurrentSelectIDX = selectIndex;
                MusicSelect(CurrentSelectIDX);
            }
        }

        public bool SkipBack()
        {
            try
            {
                int select = CurrentSelectIDX;
                MusicSelect(--select);
                CurrentSelectIDX = select;
                CurrentListView.SelectedIndex = select;
                return true;
            }
            catch (System.Exception ex)
            {
                return false;
            }
        }
        public bool SkipAhead()
        {
            try
            {
                int select = CurrentSelectIDX;
                MusicSelect(++select);
                CurrentSelectIDX = select;
                CurrentListView.SelectedIndex = select;
                return true;
            }
            catch (System.Exception ex)
            {
                return false;
            }
        }
        private void MusicSelect(int selectIndex)
        {
            var selectedItem = CurrentListView.Items[selectIndex] as Model.FB_Post;
            musicPlayer.MusicUri = selectedItem.Link;
            musicPlayer.PreviewImageSource = new Windows.UI.Xaml.Media.Imaging.BitmapImage(SelectedImageUri);
        }

        private void BTN_CloseItems_Click(object sender, RoutedEventArgs e)
        {
            LV_Items.ItemsSource = null;
            LV_Channels.Visibility = Visibility.Visible;
            LV_Items.Visibility = Visibility.Collapsed;
            //BTN_CloseItems.Visibility = Visibility.Collapsed;
            pageTitle.Text = CategoryName;

            // 백버튼 활용
            backButton.Click -= BTN_CloseItems_Click;
            backButton.Click += GoBack;
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            var frame = new Frame();
            frame.Navigate(typeof(View.MainCategoryPage));
            Window.Current.Content = frame;
        }

        #region [ FunScreen ]
        System.Threading.Tasks.Task task;
        bool onFunScreen = false;
        int timer = 0;
        FunScreen.FunScreenMain funsScreenMain;
        private void waitingTimeForFunScreen()
        {
            while (true)
            {
                new System.Threading.ManualResetEvent(false).WaitOne(1000);
                if (Model.StaticVar.MusicOn)
                {
                    timer++;
                }
                if (timer > 3)
                {

                }
            }
        }
        public void ShowFunScreen()
        {
            GridRow0.Visibility = Visibility.Collapsed;
            GridRow1.Visibility = Visibility.Collapsed;
            Grid_FunScreen.Visibility = Visibility.Visible;
            UC_FunScreen.ShowRadomScreen();
        }
        public void HideFunScreen()
        {
            GridRow0.Visibility = Visibility.Visible;
            GridRow1.Visibility = Visibility.Visible;
            Grid_FunScreen.Visibility = Visibility.Collapsed;
        } 
        #endregion
    }
}
