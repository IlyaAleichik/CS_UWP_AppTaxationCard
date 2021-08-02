using System;
using System.Collections.Generic;
using System.IO;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace AppTaxationCard.Views
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    /// 
    public sealed partial class SettingsPage : Page
    {

        public string Version
        {
            get
            {
                var version = Windows.ApplicationModel.Package.Current.Id.Version;
                return String.Format("{0}.{1}.{2}.{3}", version.Major, version.Minor, version.Build, version.Revision);
            }
        }
        public SettingsPage()
        {
            this.InitializeComponent();

            if (ElementSoundPlayer.State == ElementSoundPlayerState.On)
                soundToggle.IsOn = true;
            if (ElementSoundPlayer.SpatialAudioMode == ElementSpatialAudioMode.On)
                spatialSoundBox.IsChecked = true;

        }

        private void spatialSoundBox_Checked(object sender, RoutedEventArgs e)
        {
            if (soundToggle.IsOn == true)
            {
                ElementSoundPlayer.SpatialAudioMode = ElementSpatialAudioMode.On;
            }
        }

        private void soundToggle_Toggled(object sender, RoutedEventArgs e)
        {
            if (soundToggle.IsOn == true)
            {
                spatialSoundBox.IsEnabled = true;
                ElementSoundPlayer.State = ElementSoundPlayerState.On;
            }
            else
            {
                spatialSoundBox.IsEnabled = false;
                spatialSoundBox.IsChecked = false;

                ElementSoundPlayer.State = ElementSoundPlayerState.Off;
                ElementSoundPlayer.SpatialAudioMode = ElementSpatialAudioMode.Off;
            }
        }

        private void spatialSoundBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (soundToggle.IsOn == true)
            {
                ElementSoundPlayer.SpatialAudioMode = ElementSpatialAudioMode.Off;
            }

        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            //RadioButton pressed = (RadioButton)sender;

            //switch (pressed.Content.ToString())
            //{
            //    case "Светлая":
            //        RequestedTheme = ElementTheme.Light;
            //        break;
            //    case "Темная":
            //        RequestedTheme = ElementTheme.Dark;
            //        break;
            //    case "Стандартная Windows":
            //        RequestedTheme = ElementTheme.Default;
            //        break;
            //}

        }

        private async void HelpWebView_Loaded(object sender, RoutedEventArgs e)
        {
            //StorageFolder installedLocation = Windows.ApplicationModel.Package.Current.InstalledLocation;

            //// получаем папку Data
            //StorageFolder folder = await installedLocation.GetFolderAsync("Helps");
            //IReadOnlyList<StorageFile> files = await folder.GetFilesAsync();

            //string curDir = Directory.GetCurrentDirectory();
            //var uri = new Uri(System.IO.Path.Combine(curDir, "Assets/helps/index.html"));
            //System.Diagnostics.Process.Start(uri.ToString());
        }
    }
}
