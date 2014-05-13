using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Callisto.Controls;
using UIElementLeakTester;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace RadioRss.ViewControl
{
    public class FlyMenu
    {
        public static void SimpleFlyout(object sender, string simpleMSG)
        {
            Flyout flyout = new Flyout();

            Border border = new Border();
            border.Width = 300;
            border.Height = 125;

            TextBlock tb = new TextBlock();
            tb.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
            tb.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center;
            tb.TextWrapping = TextWrapping.Wrap;
            tb.FontSize = 24.667;
            tb.Text = simpleMSG;
            tb.Foreground = new SolidColorBrush(Windows.UI.Colors.Black);
            border.Child = tb;

            flyout.Content = border;

            flyout.Placement = (PlacementMode)Enum.Parse(typeof(PlacementMode), "Top");
            flyout.PlacementTarget = sender as UIElement;

            flyout.IsOpen = true;

            ObjectTracker.Track(flyout);
        }

        /// <summary>
        /// 사용 X
        /// </summary>
        public static void FlyAlert()
        {
            Flyout f = new Flyout();

            Border b = new Border();
            b.Width = 300;
            b.Height = 125;

            TextBlock tb = new TextBlock();
            tb.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
            tb.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center;
            tb.TextWrapping = TextWrapping.Wrap;
            tb.FontSize = 24.667;
            tb.Text = "This is a basic ContentControl so put anything you want in here.";

            b.Child = tb;

            f.Content = b;

            f.Placement = (PlacementMode)Enum.Parse(typeof(PlacementMode), "Top");
            //f.PlacementTarget = sender as UIElement;

            f.IsOpen = true;

            ObjectTracker.Track(f);
        }
    }
}
