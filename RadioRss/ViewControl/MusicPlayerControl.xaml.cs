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
using Windows.Media;
// 사용자 정의 컨트롤 항목 템플릿에 대한 설명은 http://go.microsoft.com/fwlink/?LinkId=234236에 나와 있습니다.

namespace RadioRss.ViewControl
{
    public sealed partial class MusicPlayerControl : UserControl
    {
        public ImageSource PreviewImageSource
        {
            set
            {
                ImageBrush myBrush = new ImageBrush();
//                 Image image = new Image();
//                 image.Source = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri("ms-appx:///Image/BackGround/testImg2.jpg"));
                myBrush.ImageSource = value;
                GD_root.Background = myBrush;
                //previewImage.Source = value;
                Model.StaticVar.MusicOn = false;
            }
        }
        public Image PreviewImage
        {
            set
            {
                previewImage = value;
            }
            get
            {
                return previewImage;
            }
        }
        public Uri MusicUri
        {
            set
            {
                if (ticks != null)
                    ticks.Stop();
                MusicMedia.Stop();

                MusicMedia.Source = value;
                BTN_Play.Content = "▶";
                Model.StaticVar.MusicOn = false;                
                MusicMedia.Position = new TimeSpan(0, 0, 0, 0, 0);

                TimeSlider.Value = MusicMedia.Position.TotalSeconds;
                DurationMaxText.Text = "";
            }
        }

        public MusicPlayerControl()
        {
            this.InitializeComponent();
            //media = mediaTest;
            //media = Model.StaticVar.Media;
            
            //MusicMedia.Source = new System.Uri("http://soundcloud.com/ddanzi/15-6/download.mp3", UriKind.Absolute);
            TimeSlider.Value = 0;
            MusicMedia.AutoPlay = false;
            //  Source="ms-appx:///Data/test1.mp3"
            //media.Source = new System.Uri("ms-appx:///Data/test1.mp3", UriKind.Absolute);
            //media.SetValue(MediaElement.SourceProperty, "http://soundcloud.com/ddanzi/15-6/download.mp3");
            //media.Play();


            MediaControl.PlayPressed += MediaControl_PlayPressed;
            MediaControl.PausePressed += MediaControl_PausePressed;
            MediaControl.PlayPauseTogglePressed += MediaControl_PlayPauseTogglePressed;
            MediaControl.StopPressed += MediaControl_StopPressed;
        }

        private void TimeSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (MusicMedia != null)
            {
//                 var slider = sender as Slider;
//                 MusicMedia.Position = TimeSpan.FromSeconds(slider.Value);

                var slider = sender as Slider;
                int slidervalue = (int)slider.Value;
                TimeSpan ts = new TimeSpan(0, 0, 0, 0, slidervalue);
                MusicMedia.Position = ts;
                DurationText.Text = Milliseconds_to_Minute((long)MusicMedia.Position.TotalMilliseconds);
            }
//             double nv = e.NewValue;
//             double ov = e.OldValue;
//             nv = 1.0;
        }

        private void SoundSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (MusicMedia != null)
            {
                MusicMedia.Volume = e.NewValue; 
            }
        }

        private void BTN_Play_Click(object sender, RoutedEventArgs e)
        {
            if (MusicMedia.DownloadProgress < 0.001)
            {
                RadioRss.ViewControl.FlyMenu.SimpleFlyout(sender, "음원 다운로드 중입니다.\n잠시 후 다시 눌러보세요.");
                return;
            }
            if ((MusicMedia.CurrentState == MediaElementState.Stopped) || (MusicMedia.CurrentState == MediaElementState.Paused))
            {
                var Style = App.Current.Resources["PauseAppBarButtonStyle"] as Windows.UI.Xaml.Style;
                BTN_Play.Style = Style;
                MusicMedia.Play();
                BTN_Play.Content = "∥";
                MusicMedia.Position = new TimeSpan(0, 0, 0, 0, (int)TimeSlider.Value);
                Model.StaticVar.MusicOn = true;
                if (ticks != null)
                    ticks.Start();
                DurationMaxText.Text = Milliseconds_to_Minute((long)MusicMedia.NaturalDuration.TimeSpan.TotalMilliseconds);
            }
            else
            {
                var Style = App.Current.Resources["PlayAppBarButtonStyle"] as Windows.UI.Xaml.Style;
                BTN_Play.Style = Style;
                MusicMedia.Pause();
                BTN_Play.Content = "▶";
                Model.StaticVar.MusicOn = false;
                if (ticks!=null)
                    ticks.Stop();
            }
        }

        private void BTN_Stop_Click(object sender, RoutedEventArgs e)
        {
            if (ticks != null)
                ticks.Stop();
            MusicMedia.Stop();
            BTN_Play.Content = "▶";
            Model.StaticVar.MusicOn = false;

            TimeSlider.Value = 0;
            MusicMedia.Position = new TimeSpan(0, 0, 0, 0, 0);
            DurationText.Text = Milliseconds_to_Minute(0);
        }

        // music settings
        DispatcherTimer ticks;
        private void VideoPlayer_MediaOpened(object sender, RoutedEventArgs e)
        {
            ticks = new DispatcherTimer();
            TimeSlider.Maximum = MusicMedia.NaturalDuration.TimeSpan.TotalMilliseconds;
            TimeSlider.Value = 0;
            ticks.Interval = TimeSpan.FromMilliseconds(1);
            ticks.Tick -= ticks_Tick;
            ticks.Tick += ticks_Tick;
        }
        //Updating the Slider Value of Media(Video Duration)
        void ticks_Tick(object sender, object e)
        {
            TimeSlider.Value = MusicMedia.Position.TotalMilliseconds;
            DurationText.Text = Milliseconds_to_Minute((long)MusicMedia.Position.TotalMilliseconds);
        }

        // 음악 종료시
        private void MusicMedia_MediaEnded(object sender, RoutedEventArgs e)
        {
            MusicMedia.Stop();
            BTN_Play.Content = "▶";
            Model.StaticVar.MusicOn = false;
            if (ticks != null)
                ticks.Stop();
        }

        // music background
        #region [ 백그라운드 용도 ]
        private async void MediaControl_StopPressed(object sender, object e)
        {
            sender = sender ?? new object();
            e = e ?? new object();

            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => MusicMedia.Stop());
        }

        private async void MediaControl_PlayPauseTogglePressed(object sender, object e)
        {
            sender = sender ?? new object();
            e = e ?? new object();

            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                try
                {
                    if (MusicMedia.CurrentState == MediaElementState.Stopped)
                        MusicMedia.Play();
                    else
                        MusicMedia.Stop();
                }
                catch
                {
                }
            });
        }

        private async void MediaControl_PausePressed(object sender, object e)
        {
            sender = sender ?? new object();
            e = e ?? new object();

            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => MusicMedia.Stop());
        }

        private async void MediaControl_PlayPressed(object sender, object e)
        {
            sender = sender ?? new object();
            e = e ?? new object();

            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => MusicMedia.Play());
        } 
        #endregion

        #region Functions
        //Conversion of Milliseconds to Time Display Format
        public string Milliseconds_to_Minute(long milliseconds)
        {
            int minute = (int)(milliseconds / (1000 * 60));
            int seconds = (int)(milliseconds / 1000) % 60;
            return (minute + " : " + seconds); 
        }

        #endregion

        private void BTN_SkipBack(object sender, RoutedEventArgs e)
        {
            Model.StaticVar.MusicPlayerHandler.SkipBack();
        }

        private void BTN_SkipAhead(object sender, RoutedEventArgs e)
        {
            Model.StaticVar.MusicPlayerHandler.SkipAhead();
        }

    }
}
