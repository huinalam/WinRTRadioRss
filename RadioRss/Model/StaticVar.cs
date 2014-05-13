using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioRss.Model
{
    public static class StaticVar
    {
        public static string CategoryName = "";
        public static bool MusicOn = false;

        public interface IMusicPlayerHandler
        {
            bool SkipAhead();
            bool SkipBack();
        }
        public static IMusicPlayerHandler MusicPlayerHandler;

        public interface IFunScreen
        {
            void ShowFunScreen();
            void HideFunScreen();
        }
        public static IFunScreen FunScreen;

        public static IPageBackground CategoryPageBG;
        public static IPageBackground PodCastItemsPageBG;
        public interface IPageBackground
        {
            void ChangeBackGround();
        }

        public static class BackgroundImageSettgins
        {
            /// <summary>
            /// 해당 클래스를 사용하기위해 초기화를 시켜준다.
            /// </summary>
            public static void Init()
            {
                // 필요한 이미지를 세팅시켜둔다.
                BGList.Add( new BackgroundElement(
                "노스텔지아",
                new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri("ms-appx:///Image/BackGround/BG1.jpg"))));
                BGList.Add(new BackgroundElement(
                "커피",
                new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri("ms-appx:///Image/BackGround/BG2.jpg"))));
                BGList.Add(new BackgroundElement(
                "파스텔",
                new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri("ms-appx:///Image/BackGround/BG3.jpg"))));
                BGList.Add(new BackgroundElement(
                "노을",
                new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri("ms-appx:///Image/BackGround/BG4.png"))));

                // 초기값
                myBrush.ImageSource = Model.StaticVar.BackgroundImageSettgins.BGList[0].BGImage.Source;
            }
            public static Windows.UI.Xaml.Media.ImageBrush SelectedBackgroundImage(int idx)
            {
                myBrush.ImageSource = Model.StaticVar.BackgroundImageSettgins.BGList[idx].BGImage.Source;
                return myBrush;
            }

            public static Windows.UI.Xaml.Media.ImageBrush myBrush = new Windows.UI.Xaml.Media.ImageBrush();
            public static List<BackgroundElement> BGList = new List<BackgroundElement>();
            public class BackgroundElement
            {
                public BackgroundElement(string name, Windows.UI.Xaml.Media.Imaging.BitmapImage bitmap)
                {
                    _name = name;
                    _BGImageSource = bitmap;
                    _BGImage.Source = _BGImageSource;                    
                }
                private string _name;
                public string Name
                {
                    get { return _name; }
                    set { _name = value; }
                }
                private Windows.UI.Xaml.Controls.Image _BGImage = new Windows.UI.Xaml.Controls.Image();
                public Windows.UI.Xaml.Controls.Image BGImage
                {
                    get { return _BGImage; }
                    set { _BGImage = value; }
                }
                Windows.UI.Xaml.Media.Imaging.BitmapImage _BGImageSource;
                public Windows.UI.Xaml.Media.Imaging.BitmapImage BGImageSource
                {
                    get { return _BGImageSource; }
                    set { _BGImageSource = value; }
                }
            }
        }
    }
}
