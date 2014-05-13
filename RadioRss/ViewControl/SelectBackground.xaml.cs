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
    public sealed partial class SelectBackground : UserControl
    {
        public SelectBackground()
        {
            this.InitializeComponent();
            LV_Background.ItemsSource = Model.StaticVar.BackgroundImageSettgins.BGList;
        }

        private void LV_Background_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listView = sender as ListView;
            int selectIndex = listView.SelectedIndex;
            if (selectIndex >= 0)
            {
                Model.StaticVar.BackgroundImageSettgins.SelectedBackgroundImage(selectIndex);

                if (Model.StaticVar.PodCastItemsPageBG != null)
                    Model.StaticVar.PodCastItemsPageBG.ChangeBackGround();
                if (Model.StaticVar.CategoryPageBG!= null)
                    Model.StaticVar.CategoryPageBG.ChangeBackGround();
            }
        }


    }
}
