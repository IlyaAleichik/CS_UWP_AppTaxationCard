using AppTaxationCard.Models;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Services.Maps;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace AppTaxationCard.Views
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    /// 

  
    public class DateTimeOffsetToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            DateTime sourceTime = (DateTime)value;
            DateTime targetTime = sourceTime.Date;
            return targetTime.ToString("dd.MM.yyyy");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
          
            return null;
        }
    }
    public sealed partial class DashboardPage : Page
    {

        Forestry forestry;
        List<MapElement> MyLandmarks = new List<MapElement>();
        public DashboardPage()
        {
            this.InitializeComponent();
            this.Loaded += Page_Loaded;
        }

        public int CountVidls { get; set; }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
           
            using (ModelContext db = new ModelContext())
            {

              
                 SessionUser current = db.SessionUsers.LastOrDefault();

                // db.Videls.Select(x => x.NumKvartal).Distinct().ToList();
                //               SELECT P_id, (SELECT DISTINCT City FROM Persons) City
                //FROM Persons;
                //newItemGridView.ItemsSource = db.Videls.Where(c => c.AccountId == current.SId).ToList();
                GetMarkerMap();
                newItemGridView.ItemsSource = db.Kvartals.Where(x => x.CreateAcc == current.SId ).ToList();
                ItemListView.ItemsSource = db.Kvartals.Where(x => x.CreateAcc == current.SId).ToList();

            }
        }
        List<Kvartal> kvartals;
        private void GetMarkerMap()
        {
            Geopoint snPoint;
            BasicGeoposition snPosition;
            using (ModelContext db = new ModelContext())
            {
                SessionUser session = db.SessionUsers.LastOrDefault();
                var kv = db.Kvartals.Where(d => d.CreateAcc == session.SId).ToList();
              
                foreach (Kvartal u in kv)
                {
                    snPosition = new BasicGeoposition { Latitude = u.Latitude, Longitude = u.Longitude };
                  
                    snPoint = new Geopoint(snPosition);
                    var spaceNeedleIcon = new MapIcon
                    {
                        Location = snPoint,
                        NormalizedAnchorPoint = new Point(0.5, 1.0),
                        ZIndex = 0,
                        Title = "Space Needle"
                    };

                    MyLandmarks.Add(spaceNeedleIcon);

                    var LandmarksLayer = new MapElementsLayer
                    {
                        ZIndex = 1,
                        MapElements = MyLandmarks
                    };

                    Map.Layers.Add(LandmarksLayer);
                }
            
            }
                
           

       
        }
        private void GetDefaultCountry()
        {
      
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(CreateVidelPage));
          
        }


        private void NewItemGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
        }
   
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (newItemGridView.SelectedItem != null)
            {
                Kvartal company = newItemGridView.SelectedItem as Kvartal;
                if (company != null)
                {
                    using (ModelContext db = new ModelContext())
                    {
                        SessionUser current = db.SessionUsers.LastOrDefault();
                        db.Kvartals.Remove(company);
                        db.SaveChanges();
                        newItemGridView.ItemsSource = db.Kvartals.Where(x => x.CreateAcc == current.SId).ToList();
                    }
                }
            }
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            if (newItemGridView.SelectedItem != null)
            {
                Kvartal company = newItemGridView.SelectedItem as Kvartal;

                if (company != null)
                    Frame.Navigate(typeof(VidelListPage), company.Id);
            }
        }

     
        private void ListViewVisible_Click(object sender, RoutedEventArgs e)
        {
            NameCard.Foreground = App.Current.Resources["BlackColor"] as SolidColorBrush;
            rectngle.Fill = App.Current.Resources["BlackColor"] as SolidColorBrush;
            MyHeaderGrid.Background.Opacity = 0;
            newItemGridView.Visibility = Visibility.Collapsed;         
            Map.Visibility = Visibility.Collapsed;
            ItemListView.Visibility = Visibility.Visible;
        }

        private void GridViewVisible_Click(object sender, RoutedEventArgs e)
        {
            NameCard.Foreground = App.Current.Resources["BlackColor"] as SolidColorBrush;
            rectngle.Fill = App.Current.Resources["BlackColor"] as SolidColorBrush;
            MyHeaderGrid.Background.Opacity = 0;
            newItemGridView.Visibility = Visibility.Visible;
            Map.Visibility = Visibility.Collapsed;
            ItemListView.Visibility = Visibility.Collapsed;
        }

        private void MapView_Click(object sender, RoutedEventArgs e)
        {
           
           
            NameCard.Foreground = App.Current.Resources["WhiteColor"] as SolidColorBrush;
            rectngle.Fill = App.Current.Resources["WhiteColor"] as SolidColorBrush;
            MyHeaderGrid.Background.Opacity = 1;
            newItemGridView.Visibility = Visibility.Collapsed;
            Map.Visibility = Visibility.Visible;
            ItemListView.Visibility = Visibility.Collapsed;
        }

        private void Map_ActualCameraChanged(Windows.UI.Xaml.Controls.Maps.MapControl sender, Windows.UI.Xaml.Controls.Maps.MapActualCameraChangedEventArgs args)
        {
            var buttons = new Dictionary<Point, Button>();

            foreach (var child in Map.Children)
            {

                var btn = child as Button;
                Point point;
                try
                {
                    point = btn.TransformToVisual(Map).TransformPoint(new Point());
                }
                catch (Exception)
                {
                    return;
                }

                if (point.X > 0 && point.X <= Map.ActualWidth &&
                    point.Y > 0 && point.Y <= Map.ActualHeight)
                {
                    btn.IsTabStop = true;
                    buttons.Add(point, btn);
                }
                else
                {
                    btn.IsTabStop = false;
                }
            }

            Button previosBtn = null;
            var orderedButtons = buttons.OrderBy(b => b.Key.X);

            foreach (var point in orderedButtons)
            {
                var button = point.Value;

                button.XYFocusUp = button;
                button.XYFocusRight = button;
                button.XYFocusLeft = previosBtn != null ? previosBtn : button;
             //   button.XYFocusDown = MainControlsViewOldAdventuresButton;

                if (previosBtn != null)
                {
                    previosBtn.XYFocusRight = button;
                }

                previosBtn = button;
            }

            if (orderedButtons.Count() > 1)
            {
                orderedButtons.Last().Value.XYFocusRight = orderedButtons.First().Value;
                orderedButtons.First().Value.XYFocusLeft = orderedButtons.Last().Value;
            }
        }

        private async void MapControl_Loaded(object sender, RoutedEventArgs e)
        {
            var toolbar = Map.FindDescendant<StackPanel>();
            if (toolbar != null)
            {
                toolbar.Visibility = Visibility.Collapsed;
            }

            var swapchainpanel = Map.FindDescendant<SwapChainPanel>();
            if (swapchainpanel != null && swapchainpanel.Parent != null)
            {
                var grid = swapchainpanel.Parent as Grid;


                var border = new Border()
                {
                    IsHitTestVisible = false,

                    Opacity = 0.4
                };

                grid.Children.Add(border);
            }

            //  53.542198, 28.050123 Belarus Coorinate

            BasicGeoposition cityPosition = new BasicGeoposition() { Latitude = 53.542198, Longitude = 28.050123 };
            Geopoint cityCenter = new Geopoint(cityPosition);

            // Set the map location.

            //await Map.TrySetViewAsync(cityCenter, 16, RotationSlider.Value, PitchSlider.Value, MapAnimationKind.Bow);
            await Map.TrySetSceneAsync(MapScene.CreateFromLocationAndRadius(cityCenter, 300000));  

            var mapLocation = await MapLocationFinder.FindLocationsAtAsync(cityCenter);

            //await Map.TrySetViewAsync(new Geopoint(new BasicGeoposition { Latitude = seattlePoint.Position.Latitude, Longitude = seattlePoint.Position.Longitude }), 16, MapAnimationKind.Bow);


            if (mapLocation.Status == MapLocationFinderStatus.Success)
            {
                DefaultLocation.Text = mapLocation?.Locations?[0].Address.Country;
            }
            else
            {
                DefaultLocation.Text = "Not Found";
            }




        }

        //private async void Expand()
        //{
        //    var currentAV = ApplicationView.GetForCurrentView();
        //    var newAV = CoreApplication.CreateNewView();
        //    await newAV.Dispatcher.RunAsync(
        //                    CoreDispatcherPriority.Normal,
        //                    async () =>
        //                    {
        //                        var newWindow = Window.Current;
        //                        var newAppView = ApplicationView.GetForCurrentView();
        //                        newAppView.Title = "New window";

        //                        var frame = new Frame();
        //                        frame.Navigate(typeof(EditUserPage), null);
        //                        newWindow.Content = frame;
        //                        newWindow.Activate();

        //                        await ApplicationViewSwitcher.TryShowAsStandaloneAsync(
        //                            newAppView.Id,
        //                            ViewSizePreference.UseMinimum,
        //                            currentAV.Id,
        //                            ViewSizePreference.UseMinimum);
        //                    });
        //}

        
        private void EditUser_Click(object sender, RoutedEventArgs e)
        {
            using (ModelContext db = new ModelContext())
            {
                SessionUser session = db.SessionUsers.LastOrDefault();
                Frame.Navigate(typeof(EditUserPage), session.SId);
            }

        
        }

        private void ItemListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ItemListView.SelectedItem != null)
            {
                Kvartal company = ItemListView.SelectedItem as Kvartal;

                if (company != null)
                    Frame.Navigate(typeof(VidelListPage), company.Id);
            }
        }
    }
}