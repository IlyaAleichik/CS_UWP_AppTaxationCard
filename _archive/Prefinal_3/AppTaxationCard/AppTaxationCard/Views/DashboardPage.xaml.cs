using AppTaxationCard.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.ApplicationModel.Core;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Services.Maps;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;

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
        // Forestry forestry

        ObservableCollection<string> search = new ObservableCollection<string>();
        public Visibility IsVisible { get; set; }
       
  

        List<MapElement> MyLandmarks = new List<MapElement>();
        public DashboardPage()
        {
            this.InitializeComponent();
            this.Loaded += Page_Loaded;
           
   
        }
        
        #region System
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

            using (ModelContext db = new ModelContext())
            {


                SessionUser current = db.SessionUsers.LastOrDefault();

    
                GetMarkerMap();
                LoadDataSearch(db);
                newItemGridView.ItemsSource = db.Kvartals.Where(x => x.CreateAcc == current.SId).ToList();
                ItemListView.ItemsSource = db.Kvartals.Where(x => x.CreateAcc == current.SId).ToList();
           


            }
        }

        private void LoadDataSearch(ModelContext db)
        {
            var kvart = db.Kvartals.ToList();
            foreach (Kvartal k in kvart)
            {


                search.Add(k.NumKvartal.ToString());

            }
        
     
        }

        #endregion

        #region Actions

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

        private async void MapView_Click(object sender, RoutedEventArgs e)
        {

     
            // Set the map location.      


            NameCard.Foreground = App.Current.Resources["WhiteColor"] as SolidColorBrush;
            rectngle.Fill = App.Current.Resources["WhiteColor"] as SolidColorBrush;
            MyHeaderGrid.Background.Opacity = 1;
            newItemGridView.Visibility = Visibility.Collapsed;
            Map.Visibility = Visibility.Visible;
            ItemListView.Visibility = Visibility.Collapsed;
            BasicGeoposition cityPosition = new BasicGeoposition() { Latitude = 53.542198, Longitude = 28.050123 };
            Geopoint cityCenter = new Geopoint(cityPosition);
            await Map.TrySetSceneAsync(MapScene.CreateFromLocationAndRadius(cityCenter, 300000));
            var mapLocation = await MapLocationFinder.FindLocationsAtAsync(cityCenter);
        }

        #endregion

        #region Selectors
        private void ItemListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ItemListView.SelectedItem != null)
            {
              
                Kvartal selectedKvartal = (Kvartal)ItemListView.SelectedItem;
                if (selectedKvartal != null)
                {
                    Frame.Navigate(typeof(VidelListPage), selectedKvartal.Id);
                }
                    
            }
        }
        #endregion

        #region CRUD
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(CreateVidelPage));
        }
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (newItemGridView.SelectedItem != null)
            {
                Kvartal kvv = newItemGridView.SelectedItem as Kvartal;
                if (kvv != null)
                {
                    using (ModelContext db = new ModelContext())
                    {
                        SessionUser current = db.SessionUsers.LastOrDefault();
                        db.Kvartals.Remove(kvv);
                        db.SaveChanges();
                        newItemGridView.ItemsSource = db.Kvartals.Where(x => x.CreateAcc == current.SId).ToList();
                    }
                }
            }
        }
        private void EditUser_Click(object sender, RoutedEventArgs e)
        {
            using (ModelContext db = new ModelContext())
            {
                SessionUser session = db.SessionUsers.LastOrDefault();
                Frame.Navigate(typeof(EditUserPage), session.SId);
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

        #endregion

        #region MapControl      
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
            await Map.TrySetSceneAsync(MapScene.CreateFromLocationAndRadius(cityCenter, 300000));
            var mapLocation = await MapLocationFinder.FindLocationsAtAsync(cityCenter);
            if (mapLocation.Status == MapLocationFinderStatus.Success)
            {
                DefaultLocation.Text = mapLocation?.Locations?[0].Address.Country;
            }
            else
            {
                DefaultLocation.Text = "Not Found";
            }

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

        #endregion

        #region Functions
        Geopoint geopoint;
        BasicGeoposition position;

        private void GetMarkerMap()
        {         
            using (ModelContext db = new ModelContext())
            {
                SessionUser session = db.SessionUsers.LastOrDefault();

                var kvartals = db.Kvartals.Where(d => d.CreateAcc == session.SId).ToList();

                foreach (Kvartal kvartal in kvartals)
                {
                    if (kvartal.Latitude != 0 & kvartal.Longitude != 0 )
                    {
                        IsVisible = Visibility.Visible;                  
                    }
                    else
                    {
                        IsVisible = Visibility.Collapsed;
                    }

                    position = new BasicGeoposition { Latitude = kvartal.Latitude, Longitude = kvartal.Longitude };

                    geopoint = new Geopoint(position);

                    var ellipse = new Ellipse()
                    {
                        Height = 50,
                        Width = 50,
                        Stroke = App.Current.Resources["BrandColor"] as SolidColorBrush,
                        StrokeThickness = 2,
                        Visibility = IsVisible
                    };

                    var button = new Button();
                    button.DataContext = kvartal;
                    button.Template = App.Current.Resources["MapButtonStyle"] as ControlTemplate;
                    button.UseSystemFocusVisuals = false;
                    button.Content = ellipse;
                    button.Click += MapButtonClicked;
                    Map.Children.Add(button);
                    MapControl.SetLocation(button, geopoint);
                    MapControl.SetNormalizedAnchorPoint(button, new Point(0.5, 0.5));

                }

            }
        }
        private void MapButtonClicked(object sender, RoutedEventArgs e)
        {
            var adventure = (sender as Button).DataContext as Kvartal;

            Frame.Navigate(typeof(VidelListPage), adventure.Id);
        }

        private async void Expand()
        {
            var currentAV = ApplicationView.GetForCurrentView();
            var newAV = CoreApplication.CreateNewView();
            await newAV.Dispatcher.RunAsync(
                            CoreDispatcherPriority.Normal,
                            async () =>
                            {
                                var newWindow = Window.Current;
                                var newAppView = ApplicationView.GetForCurrentView();
                                newAppView.Title = "New window";

                                var frame = new Frame();
                                frame.Navigate(typeof(EditUserPage), null);
                                newWindow.Content = frame;
                                newWindow.Activate();

                                await ApplicationViewSwitcher.TryShowAsStandaloneAsync(
                                    newAppView.Id,
                                    ViewSizePreference.UseMinimum,
                                    currentAV.Id,
                                    ViewSizePreference.UseMinimum);
                            });
        }
        #endregion

   

        private void ControlsSearchBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            using (ModelContext db = new ModelContext())
            {
              
                var kva = db.Kvartals.Where(p => EF.Functions.Like(p.NumKvartal.ToString(), args.SelectedItem as string));
                newItemGridView.ItemsSource = kva;
            }         
        }

        private void ControlsSearchBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            using (ModelContext db = new ModelContext())
            {
                SessionUser current = db.SessionUsers.LastOrDefault();
                newItemGridView.ItemsSource = db.Kvartals.Where(x => x.CreateAcc == current.SId).ToList();
            }
               
        }

        private void ControlsSearchBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
        
            if (args.CheckCurrent())
            {
                var term = controlsSearchBox.Text.ToLower();
                var results = search.Where(i => i.ToLower().Contains(term)).ToList();
                controlsSearchBox.ItemsSource = results;

                using (ModelContext db = new ModelContext())
                {
                  
                    if (controlsSearchBox.Text != null)
                    {
                        var kva = db.Kvartals.Where(p => EF.Functions.Like(p.NumKvartal.ToString(), controlsSearchBox.Text));
                        newItemGridView.ItemsSource = kva;
                    }
                 
                      
                    

                }
            }
         
        }

   
    }
}