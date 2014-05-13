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
    public sealed partial class PrivacyPolicy : UserControl
    {
        public PrivacyPolicy()
        {
            this.InitializeComponent();
//             TB_Content.Text =
//                 "개인정보의 수집 및 이용목적\n" +
//                 "개인정보를 수집하지 않습니다.\n\n" +
//                 "개인정보의 보유 및 이용기간\n" +
//                 "개인정보를 이용하지 않습니다.\n\n" +
//                 "개인정보 수집방법\n" +
//                 "개인정보를 보유하지 않습니다.\n\n" +
//                 "개인정보의 파기 절차 및 방법\n" +
//                 "개인정보를 수집, 이용, 보유하지 않습니다.\n\n" +
//                 "개인정보 공유 및 제3자 제공\n" +
//                 "개인정보를 공유 및 제3자에게 제공하지 않습니다.";
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri("https://medium.com/p/39ed80cec791"));
        }
    }
}
