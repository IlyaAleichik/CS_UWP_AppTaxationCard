using AppTaxationCard.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Services.Maps;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using System.ComponentModel.DataAnnotations;
using Windows.UI.Popups;
using System.Threading.Tasks;

namespace AppTaxationCard.Views
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class CreateVidelPage : Page
    {
        double la { get; set; } = 0;
        double lo { get; set; } = 0;
        List<MapElement> MyLandmarks = new List<MapElement>();
        List<Forestry> forestries;
        List<TypeEarth> typeEarths;
        List<TypeErrosion> typeErrosions;
        List<DegreeErrosion> degreeErrosions;
        List<ExpositionSlope> expositionSlopes;
        List<TypeOrl> typeOrls;
        //List<MaketThenPoroda> maketThenPorodas;
        Forestry forestry;
        TypeErrosion typeErrosion;
        DegreeErrosion degree;
        ExpositionSlope exposition;
        TypeOrl typeOrl;
        TypeEarth typeEarth;
        int KvNum { get; set; }
        int IdKb { get; set; }
        public CreateVidelPage()
        {
            this.InitializeComponent();
        }
        Videl videl = null;
        //Kvartal kvartal = null;
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
            {
                int id = (int)e.Parameter;
                using (ModelContext db = new ModelContext())
                {
                
   
                    videl = db.Videls.Include(x=>x.Kvartal).FirstOrDefault(c => c.Id == id);
                    numKvartal.Text = videl.Kvartal.NumKvartal.ToString();

                }
            }

            if (videl != null)
            {
                PaneHeader.Text = "Редактирование выдела";
                numViddels.Text = videl.NumVidel.ToString();

                txtArea.Text = videl.Area.ToString();
                txtCrut.Text = videl.Krut.ToString();
                forestry = videl.Forestry;
                typeErrosion = videl.TypeErrosion; ;
                degree = videl.DegreeErrosion; 
                exposition= videl.ExpositionSlope; ;
                typeOrl = videl.TypeOrl; 
                typeEarth = videl.TypeEarth; 

            }
        }


        //private void GetDataForGridMaket( ModelContext db)
        //{

        //    SessionUser session = db.SessionUsers.LastOrDefault();
        //    var dg = db.MaketThens.ToList();          
        //    dataGridMaket.ItemsSource = dg;
        //}
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            using (ModelContext db = new ModelContext())
            {
                //InsertIntoData(db);                      
                forestries = db.Forestries.ToList();
                typeEarths = db.TypeEarths.ToList();
                typeErrosions = db.TypeErrosions.ToList();
                degreeErrosions = db.DegreeErrosions.ToList();
                expositionSlopes = db.ExpositionSlopes.ToList();
                typeOrls = db.TypeOrls.ToList();
           
                //maketThenPorodas = db.MaketThenPorodas.ToList();
            }

            forestryList.ItemsSource = forestries;
            typeEarthList.ItemsSource = typeEarths;
            viewErrosionList.ItemsSource = typeErrosions;
            degreeList.ItemsSource = degreeErrosions;
            expositionList.ItemsSource = expositionSlopes;
            orlList.ItemsSource = typeOrls;
            //txtPorodaMaketThen.ItemsSource = maketThenPorodas;
        }

        #region Map Functions
        private async void InitialLocation(object sender, RoutedEventArgs e)
        {
            try
            {
                var accessStatus = await Geolocator.RequestAccessAsync();

                var geolocator = new Geolocator();

                var position = await geolocator.GetGeopositionAsync();

                var mapLocation = await MapLocationFinder.FindLocationsAtAsync(position.Coordinate.Point);

                await Map.TrySetViewAsync(new Geopoint(new BasicGeoposition { Latitude = position.Coordinate.Point.Position.Latitude, Longitude = position.Coordinate.Point.Position.Longitude }), 16, RotationSlider.Value, PitchSlider.Value, MapAnimationKind.Bow);

                if (mapLocation.Status == MapLocationFinderStatus.Success)
                {
                    MyLocation.Text = mapLocation?.Locations?[0].Address.FormattedAddress;
                }
                else
                {
                    MyLocation.Text = "Not Found";
                }

                BasicGeoposition snPosition = new BasicGeoposition { Latitude = position.Coordinate.Point.Position.Latitude, Longitude = position.Coordinate.Point.Position.Longitude };
                la = snPosition.Latitude;
                lo = snPosition.Longitude;
                Geopoint snPoint = new Geopoint(snPosition);

                var MyLandmarks = new List<MapElement>();

                var spaceNeedleIcon = new MapIcon
                {
                    Location = snPoint,
                    NormalizedAnchorPoint = new Point(0.5, 1.0),
                    ZIndex = 0,
                    Title = "Я"
                };

                MyLandmarks.Add(spaceNeedleIcon);

                var LandmarksLayer = new MapElementsLayer
                {
                    ZIndex = 1,
                    MapElements = MyLandmarks
                };

                Map.Layers.Add(LandmarksLayer);

            }
            catch (Exception ex)
            {

                ContentDialog deleteFileDialog = new ContentDialog()
                {
                    Title = "Сообщение об ошибке",
                    Content = ex.Message,
                    PrimaryButtonText = "ОК",
                };
                ContentDialogResult result = await deleteFileDialog.ShowAsync();
            }
            


            //MyMap.Center = new Geopoint(new BasicGeoposition {Latitude = 52.181427, Longitude = 5.399780});
            //MyMap.ZoomLevel = 16;
        }
        private async void RotationSliderOnValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            await Map.TryRotateToAsync(e.NewValue);

            //Same result, sans the cool animation.
            Map.Heading = e.NewValue;

            //Rotates BY a certain value.
            await Map.TryRotateAsync(e.NewValue);
        }

        //private async void OnMapElementClick(MapControl sender, MapElementClickEventArgs args)
        //{
        //    var resultText = new StringBuilder();

        //    // Yay! Goodbye string.Format!
        //    resultText.AppendLine($"Position={args.Position.X},{args.Position.Y}");
        //    resultText.AppendLine($"Location={args.Location.Position.Latitude:F9},{args.Location.Position.Longitude:F9}");

        //    foreach (var mapObject in args.MapElements)
        //    {
        //        resultText.AppendLine("Found: " + mapObject.ReadData<PointLight>().Name);
        //    }
        //    var dialog = new MessageDialog(resultText.ToString(),
        //      args.MapElements.Any() ? "Found the following objects" : "No data found");
        //    await Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
        //      async () => await dialog.ShowAsync());
        //}

        #endregion

        //private ObservableCollection<Videl> videls ;


        //private async void AddingData(ModelContext db)
        //{

        //}
        private async void Save_Click(object sender, RoutedEventArgs e)
        {

     

            try
            {
                KvNum = Convert.ToInt32(numKvartal.Text);
                forestry = forestryList.SelectedItem as Forestry;
                typeErrosion = viewErrosionList.SelectedItem as TypeErrosion;
                degree = degreeList.SelectedItem as DegreeErrosion;
                exposition = expositionList.SelectedItem as ExpositionSlope;
                typeOrl = orlList.SelectedItem as TypeOrl;
                typeEarth = typeEarthList.SelectedItem as TypeEarth;

                if (forestry == null & typeErrosion == null & degree == null & exposition == null & typeOrl == null & typeEarth == null) return;

                using (ModelContext db = new ModelContext())
                {
                    SessionUser currentUser = db.SessionUsers.LastOrDefault();
            



                    if (videl != null)
                    {

                        videl.NumVidel = Convert.ToInt32(numViddels.Text);
                        videl.Area = Convert.ToInt32(txtArea.Text);
                        videl.Krut = Convert.ToInt32(txtCrut.Text);
                        videl.Forestry = forestry;
                        videl.TypeErrosion = typeErrosion;
                        videl.DegreeErrosion = degree;
                        videl.ExpositionSlope = exposition;
                        videl.TypeOrl = typeOrl;
                        videl.TypeEarth = typeEarth;
                        videl.CreateDateVidel = DateTime.Now;

                        db.Forestries.Attach(forestry);
                        db.TypeErrosions.Attach(typeErrosion);
                        db.DegreeErrosions.Attach(degree);
                        db.ExpositionSlopes.Attach(exposition);
                        db.TypeEarths.Attach(typeEarth);
                        db.TypeOrls.Attach(typeOrl);
                        db.Videls.Update(videl);
                        await Validate(videl, db, 1);
                       
                    }
                    else
                    {

                        var account = db.Kvartals.FirstOrDefault(u => u.NumKvartal == KvNum & u.CreateAcc == currentUser.SId);

                        try
                        {
                            var videll = db.Videls.FirstOrDefault(u => u.AccountId == currentUser.SId & u.KvartalId == account.Id & u.NumVidel == Convert.ToInt32(numViddels.Text));
                            if (videll != null)
                            {

                                ContentDialog deleteFileDialog = new ContentDialog()
                                {
                                    Title = "Сообщение об ошибке",
                                    Content = "Выдел №" + numViddels.Text + " уже существует",
                                    PrimaryButtonText = "ОК",
                                };
                                ContentDialogResult result = await deleteFileDialog.ShowAsync();
                            }
                            else
                            {
                                if (account != null)
                                {


                                    if (account.NumKvartal == KvNum)
                                    {

                                        Kvartal kv = db.Kvartals.Where(x => x.CreateAcc == currentUser.SId).FirstOrDefault(x => x.NumKvartal == KvNum);

                                        Videl videl = new Videl
                                        {
                                            NumVidel = Convert.ToInt32(numViddels.Text),
                                            Area = Convert.ToInt32(txtArea.Text),
                                            Krut = Convert.ToInt32(txtCrut.Text),
                                            Forestry = forestry,
                                            TypeErrosion = typeErrosion,
                                            DegreeErrosion = degree,
                                            ExpositionSlope = exposition,
                                            TypeOrl = typeOrl,
                                            TypeEarth = typeEarth,

                                            CreateDateVidel = DateTime.Now,
                                            AccountId = currentUser.SId,
                                            KvartalId = kv.Id
                                        };

                                        db.Forestries.Attach(forestry);
                                        db.TypeErrosions.Attach(typeErrosion);
                                        db.DegreeErrosions.Attach(degree);
                                        db.ExpositionSlopes.Attach(exposition);
                                        db.TypeEarths.Attach(typeEarth);
                                        db.TypeOrls.Attach(typeOrl);
                                        db.Videls.Add(videl);

                                        await Validate(videl, db, 1);

                                        //   Frame.Navigate(typeof(DashboardPage), new DrillInNavigationTransitionInfo());
                                    }

                                }
                                else
                                {
                                    //if (la != 0 & lo != 0)
                                    //{
                                    Kvartal k = new Kvartal { NumKvartal = KvNum };
                                    //if (la != 0 & lo != 0)

                                    db.Kvartals.Add(new Kvartal { NumKvartal = k.NumKvartal, CreateAcc = currentUser.SId, CreateDateKvartal = DateTime.Now, Latitude = la, Longitude = lo, });

                                    //}


                                    await Validate(k, db, 0);


                                    Kvartal kv = db.Kvartals.Where(x => x.CreateAcc == currentUser.SId).FirstOrDefault(x => x.NumKvartal == KvNum);

                                    Videl videl = new Videl
                                    {
                                        NumVidel = Convert.ToInt32(numViddels.Text),
                                        Area = Convert.ToInt32(txtArea.Text),
                                        Krut = Convert.ToInt32(txtCrut.Text),
                                        Forestry = forestry,
                                        TypeErrosion = typeErrosion,
                                        DegreeErrosion = degree,
                                        ExpositionSlope = exposition,
                                        TypeOrl = typeOrl,
                                        TypeEarth = typeEarth,

                                        CreateDateVidel = DateTime.Now,
                                        AccountId = currentUser.SId,
                                        KvartalId = kv.Id
                                    };

                                    db.Forestries.Attach(forestry);
                                    db.TypeErrosions.Attach(typeErrosion);
                                    db.DegreeErrosions.Attach(degree);
                                    db.ExpositionSlopes.Attach(exposition);
                                    db.TypeEarths.Attach(typeEarth);
                                    db.TypeOrls.Attach(typeOrl);
                                    db.Videls.Add(videl);
                                    await Validate(videl, db, 1);
                                    //}
                                }
                            }
                        }
                        catch {

                            if (account != null)
                            {


                                if (account.NumKvartal == KvNum)
                                {

                                    Kvartal kv = db.Kvartals.Where(x => x.CreateAcc == currentUser.SId).FirstOrDefault(x => x.NumKvartal == KvNum);

                                    Videl videl = new Videl
                                    {
                                        NumVidel = Convert.ToInt32(numViddels.Text),
                                        Area = Convert.ToInt32(txtArea.Text),
                                        Krut = Convert.ToInt32(txtCrut.Text),
                                        Forestry = forestry,
                                        TypeErrosion = typeErrosion,
                                        DegreeErrosion = degree,
                                        ExpositionSlope = exposition,
                                        TypeOrl = typeOrl,
                                        TypeEarth = typeEarth,

                                        CreateDateVidel = DateTime.Now,
                                        AccountId = currentUser.SId,
                                        KvartalId = kv.Id
                                    };

                                    db.Forestries.Attach(forestry);
                                    db.TypeErrosions.Attach(typeErrosion);
                                    db.DegreeErrosions.Attach(degree);
                                    db.ExpositionSlopes.Attach(exposition);
                                    db.TypeEarths.Attach(typeEarth);
                                    db.TypeOrls.Attach(typeOrl);
                                    db.Videls.Add(videl);

                                    await Validate(videl, db, 1);

                                  
                                }

                            }
                            else
                            {
                                Kvartal k = new Kvartal { NumKvartal = KvNum };
                                //if (la != 0 & lo != 0)
                                                   
                                db.Kvartals.Add(new Kvartal { NumKvartal = k.NumKvartal, CreateAcc = currentUser.SId, CreateDateKvartal = DateTime.Now, Latitude = la, Longitude = lo, });

                                await Validate(k, db,0);

                               
                      
                                Kvartal kv = db.Kvartals.Where(x => x.CreateAcc == currentUser.SId).FirstOrDefault(x => x.NumKvartal == KvNum);


                                Videl videl = new Videl
                                {
                                    NumVidel = Convert.ToInt32(numViddels.Text),
                                    Area = Convert.ToInt32(txtArea.Text),
                                    Krut = Convert.ToInt32(txtCrut.Text),
                                    Forestry = forestry,
                                    TypeErrosion = typeErrosion,
                                    DegreeErrosion = degree,
                                    ExpositionSlope = exposition,
                                    TypeOrl = typeOrl,
                                    TypeEarth = typeEarth,

                                    CreateDateVidel = DateTime.Now,
                                    AccountId = currentUser.SId,
                                    KvartalId = kv.Id
                                };

                                db.Forestries.Attach(forestry);
                                db.TypeErrosions.Attach(typeErrosion);
                                db.DegreeErrosions.Attach(degree);
                                db.ExpositionSlopes.Attach(exposition);
                                db.TypeEarths.Attach(typeEarth);
                                db.TypeOrls.Attach(typeOrl);

                                db.Videls.Add(videl);
                                await Validate(videl, db,1);

                          







                            }
                        }
                      
               
                        
                     
                          
                        
                    }

                }
            }
            catch (Exception ex)
            {

                ContentDialog deleteFileDialog = new ContentDialog()
                {
                    Title = "Сообщение об ошибке",
                    Content = ex.Message,
                    PrimaryButtonText = "ОК",
                };
                ContentDialogResult result = await deleteFileDialog.ShowAsync();
            }

          

            //}
            ////catch { }

        }

        private async Task Validate(object k,ModelContext db,int index)
        {
         
            var results = new List<ValidationResult>();
            var context = new ValidationContext(k);
            if (!Validator.TryValidateObject(k, context, results, true))
            {
                foreach (var error in results)
                {
                    await (new MessageDialog(error.ErrorMessage,"Сообщение")).ShowAsync();
                }
                index = 0;
            }
            else
            {
              
                db.SaveChanges();

                if (index !=0)
                {
                    GoToMainPage();
                    //Frame.Navigate(typeof(DashboardPage), new DrillInNavigationTransitionInfo());
                }
                //db.SaveChanges();


            }
          
        }


        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            GoToMainPage();
        }



        private void GoToMainPage()
        {
            if (Frame.CanGoBack)
                Frame.GoBack();
            else
                Frame.Navigate(typeof(DashboardPage));
        }



        private  void MyMap_MapTapped(MapControl sender, MapInputEventArgs args)
        {
            Map.IsEnabled = true;
            try
            {
                MyLandmarks.Clear();
                Map.Layers.Clear();
                BasicGeoposition basgeo_edit_position = args.Location.Position;
                la = basgeo_edit_position.Latitude;
                lo = basgeo_edit_position.Longitude;

                //to get a basicgeoposition of wherever the user clicks on the map
                //just checking to make sure it works
        



                BasicGeoposition snPosition = new BasicGeoposition { Latitude = basgeo_edit_position.Latitude, Longitude = basgeo_edit_position.Longitude };
                Geopoint snPoint = new Geopoint(snPosition);

                var spaceNeedleIcon = new MapIcon
                {
                    Location = snPoint,
                    NormalizedAnchorPoint = new Point(0.5, 1.0),
                    Title = "Новый квартал",
                    ZIndex = 0,

                };

                MyLandmarks.Add(spaceNeedleIcon);

                var LandmarksLayer = new MapElementsLayer
                {
                    ZIndex = 1,
                    MapElements = MyLandmarks
                };

                Map.Layers.Add(LandmarksLayer);
            }
            catch
            {

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

            BasicGeoposition cityPosition = new BasicGeoposition() { Latitude = 53.542198, Longitude = 28.050123 };
            Geopoint cityCenter = new Geopoint(cityPosition);

            // Set the map location.      
            await Map.TrySetSceneAsync(MapScene.CreateFromLocationAndRadius(cityCenter, 300000));
            var mapLocation = await MapLocationFinder.FindLocationsAtAsync(cityCenter);

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
        }
        private void EditMapIconPosition(BasicGeoposition basgeo_edit_position)
        {
            throw new NotImplementedException();
        }

        private void Map_GettingFocus(UIElement sender, Windows.UI.Xaml.Input.GettingFocusEventArgs args)
        {
            Map.IsEnabled = true;
        }
    }
}
