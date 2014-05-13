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

// 사용자 정의 컨트롤 항목 템플릿에 대한 설명은 http://go.microsoft.com/fwlink/?LinkId=234236에 나와 있습니다.

namespace RadioRss.ViewControl
{
    public sealed partial class ByTeam : UserControl
    {
        public ByTeam()
        {
            this.InitializeComponent();
        }

        private void ClickCho(object sender, RoutedEventArgs e)
        {
            RadioRss.ViewControl.FlyMenu.SimpleFlyout(sender, ".........오빠들...........................");
        }

        private void ClickLee(object sender, RoutedEventArgs e)
        {
            RadioRss.ViewControl.FlyMenu.SimpleFlyout(sender, "허허허허허허허허헣ㅎ허허허헣ㅎ");
        }

        private void ClickPark(object sender, RoutedEventArgs e)
        {
            RadioRss.ViewControl.FlyMenu.SimpleFlyout(sender, "푸하하핳ㅎ하하하핳ㅎ하핳ㅎ");
        }
    }
}
