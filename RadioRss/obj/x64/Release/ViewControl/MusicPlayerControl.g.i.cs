﻿

#pragma checksum "C:\Users\Administrator\Documents\Visual Studio 2012\Projects\RadioRss\RadioRss\ViewControl\MusicPlayerControl.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "15F9DA720113B59EB605FB6BC3A55609"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RadioRss.ViewControl
{
    partial class MusicPlayerControl : global::Windows.UI.Xaml.Controls.UserControl
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.Button BTN_Play; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.Button BTN_Stop; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.Slider TimeSlider; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.TextBlock DurationText; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.TextBlock DurationMaxText; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.Slider SoundSlider; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.MediaElement MusicMedia; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.Image previewImage; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private bool _contentLoaded;

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent()
        {
            if (_contentLoaded)
                return;

            _contentLoaded = true;
            global::Windows.UI.Xaml.Application.LoadComponent(this, new global::System.Uri("ms-appx:///ViewControl/MusicPlayerControl.xaml"), global::Windows.UI.Xaml.Controls.Primitives.ComponentResourceLocation.Application);
 
            BTN_Play = (global::Windows.UI.Xaml.Controls.Button)this.FindName("BTN_Play");
            BTN_Stop = (global::Windows.UI.Xaml.Controls.Button)this.FindName("BTN_Stop");
            TimeSlider = (global::Windows.UI.Xaml.Controls.Slider)this.FindName("TimeSlider");
            DurationText = (global::Windows.UI.Xaml.Controls.TextBlock)this.FindName("DurationText");
            DurationMaxText = (global::Windows.UI.Xaml.Controls.TextBlock)this.FindName("DurationMaxText");
            SoundSlider = (global::Windows.UI.Xaml.Controls.Slider)this.FindName("SoundSlider");
            MusicMedia = (global::Windows.UI.Xaml.Controls.MediaElement)this.FindName("MusicMedia");
            previewImage = (global::Windows.UI.Xaml.Controls.Image)this.FindName("previewImage");
        }
    }
}



