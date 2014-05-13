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

using Windows.UI.ViewManagement;
using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml.Media.Imaging;
//using XamlControlsUITestApp;
// 빈 페이지 항목 템플릿에 대한 설명은 http://go.microsoft.com/fwlink/?LinkId=234238에 나와 있습니다.

namespace RadioRss.View
{
    /// <summary>
    /// 자체에서 사용하거나 프레임 내에서 탐색할 수 있는 빈 페이지입니다.
    /// </summary>
    public sealed partial class MainCategoryPage : Page, Model.StaticVar.IPageBackground
    {
        public MainCategoryPage()
        {
            this.InitializeComponent();
            // 사이즈 변경시
            this.SizeChanged += (a, b) =>
            {
                ApplicationViewState views = ApplicationView.Value;
                switch (views)
                {
                    case ApplicationViewState.Snapped:
                        //WG_Category.Orientation = Orientation.Horizontal;
                        break;
                    case ApplicationViewState.FullScreenLandscape:

                        break;
                    case ApplicationViewState.FullScreenPortrait:

                        break;
                    case ApplicationViewState.Filled:

                        break;
                }
                VisualStateManager.GoToState(this, views.ToString(), true);
            };
        }

        public void ChangeBackGround()
        {
            RootGrid.Background = Model.StaticVar.BackgroundImageSettgins.myBrush;
        }

        /// <summary>
        /// 이 페이지가 프레임에 표시되려고 할 때 호출됩니다.
        /// </summary>
        /// <param name="e">페이지에 도달한 방법을 설명하는 이벤트 데이터입니다. Parameter
        /// 속성은 일반적으로 페이지를 구성하는 데 사용됩니다.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // FunScreen을 띄워보는 테스트 소스
//             var funScreen = new test.MyUserControl3();
//             funScreen.Width = 1300;
//             funScreen.Height = 600;
//             funScreen.Begin();
//             funScreen.Visibility = Visibility.Visible;
//             ChildGrid.Visibility = Visibility.Collapsed;
//             GridAll.Children.Add(funScreen);
//             Canvas.SetZIndex(GridAll, 0);
//             Canvas.SetZIndex(ChildGrid, 1);
            


//             // 이미지 입력
//             ImageBrush myBrush = new ImageBrush();
//             Image image = new Image();
//             image.Source = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri("ms-appx:///Image/BackGround/testImg2.jpg"));
//             myBrush.ImageSource = Model.StaticVar.BackgroundImageSettgins.BGList[0].BGImage.Source;
            ChangeBackGround();
            Model.StaticVar.CategoryPageBG = this;
            
            // 카테고리 삽입
            var categoryList = Model.FeedBoard.Ori_Category;
            foreach (Model.FB_Category cate in categoryList)
            {
                var tiles = new List<RadioRss.Model.ControlModel.LiveTileData>();
                foreach (Model.FB_Blog blog in cate.FB_Blogs)
                {
                    var tile = new RadioRss.Model.ControlModel.LiveTileData
                    {
                        Name = cate.Name,
                        Description = blog.Title,
                        ImageUri = blog.ImageUri,
                        ReadMoreUri = blog.BlogUri
                    };
                    tiles.Add(tile);
                }

                var liveTile = new ViewControl.LiveTileControl();
                liveTile.AddLiveTileList = tiles;
                liveTile.CategoryName = cate.Name;
                liveTile.Height = 500;
                liveTile.Width = 270;
                liveTile.Margin = new Windows.UI.Xaml.Thickness(10, 0, 20, 0);
                liveTile.Name = "LT_1";

                LV_Category.Items.Add(liveTile);
                // 테스트용도
                //                 LiveTile1.AddLiveTileList = tiles;
                //                 LiveTile1.CategoryName = cate.Name;
            }
            //LiveTile1.AddLiveTileList
            //SettingsPane.GetForCurrentView().CommandsRequested -= BlankPage_CommandsRequested;
            //SettingsPane.GetForCurrentView().CommandsRequested += BlankPage_CommandsRequested;
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            //SettingsPane.GetForCurrentView().CommandsRequested -= BlankPage_CommandsRequested;  
            base.OnNavigatingFrom(e);
        }

        private void WG_Category_SizeChanged(object sender, Windows.UI.Xaml.SizeChangedEventArgs e)
        {
        	// TODO: 여기에 구현된 이벤트 처리기를 추가하십시오.
            var wg = sender as WrapGrid;
            ApplicationViewState views = ApplicationView.Value;
            switch (views)
            {
                case ApplicationViewState.Snapped:
                    wg.Orientation = Orientation.Horizontal;
                    break;
                case ApplicationViewState.FullScreenLandscape:
                    wg.Orientation = Orientation.Vertical;
                    break;
                case ApplicationViewState.FullScreenPortrait:
                    wg.Orientation = Orientation.Vertical;
                    break;
                case ApplicationViewState.Filled:
                    wg.Orientation = Orientation.Vertical;
                    break;
            }
        }

        private void textBlock_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Windows.UI.ApplicationSettings.SettingsPane.Show();
        }

    }
}
