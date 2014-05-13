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

using RadioRss.Model;
// 사용자 정의 컨트롤 항목 템플릿에 대한 설명은 http://go.microsoft.com/fwlink/?LinkId=234236에 나와 있습니다.

namespace RadioRss.ViewControl
{
    public sealed partial class LiveTileControl : UserControl
    {
        List<ControlModel.LiveTileData> tileData = new List<ControlModel.LiveTileData>();
        public ControlModel.LiveTileData AddLiveTile
        {
            set
            {
                if (value == null)
                {
                    tileData.Clear();
                    DataContext = tileData;
                }
                else
                {
                    tileData.Add(value);
                    DataContext = tileData;
                }
            }
            get
            {
                return null;
            }
        }
        private List<ControlModel.LiveTileData> _AddLiveTileList;
        public List<ControlModel.LiveTileData> AddLiveTileList
        {
            set
            {
                DataContext = value;
                _AddLiveTileList = value;
            }
            get
            {
                return _AddLiveTileList;
            }
        }
        
        public string CategoryName { get; set; }

        public LiveTileControl()
        {
            this.InitializeComponent();
        }

        private void OnReadMoreLink_Click(object sender, RoutedEventArgs e)
        {
            Model.StaticVar.CategoryName = CategoryName;

            //var frame = new Frame();
            //frame.Navigate(typeof(View.PodCastItemsPage));
            var frame = Window.Current.Content as Frame;
            frame.Navigate(typeof(View.PodCastItemsPage), CategoryName);
            //Window.Current.Content = frame;
        }
    }
}
