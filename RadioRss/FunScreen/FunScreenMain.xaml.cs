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

namespace RadioRss.FunScreen
{
    public sealed partial class FunScreenMain : UserControl
    {
        public FunScreenMain()
        {
            this.InitializeComponent();
            InitScreen();
            ShowRadomScreen();
        }
        List<UserControl> list = new List<UserControl>();

        private void InitScreen()
        {
            var obj = new test.MyUserControl3();
            obj.Begin();
            list.Add(obj);
            var obj2 = new test.user3();
            obj2.Begin();
            list.Add(obj2);
            var obj3 = new test.user4();
            obj3.Begin();
            list.Add(obj3);
            GD_Row1.Children.Add(SelectFunScreen());
        }
        // 다른 램던 스크린을 띄운다.
        public void ShowRadomScreen()
        {
            GD_Row1.Children.RemoveAt(0);
            GD_Row1.Children.Add(SelectFunScreen());
        }
        private UserControl SelectFunScreen()
        {
            int ramdom = new Random().Next(0, list.Count);
            return list[ramdom];
        }
    }
}
