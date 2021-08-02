using AppTaxationCard.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using System.Collections.Generic;
using AppTaxationCard.Services.Printing;
using System.Threading.Tasks;
using Windows.UI.Popups;
using System.ComponentModel.DataAnnotations;
using Windows.UI.Xaml.Media.Animation;

namespace AppTaxationCard.Views
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class VidelListPage : Page
    {
        //
       
        //
        MainPage listpage = new MainPage();
        ObservableCollection<Videl> videls;
        ModelContext context = new ModelContext();
        List<Poroda> porodas;
        Poroda poroda;
        private int idSelectedKvartal { get; set; }
        public VidelListPage()
        {
            this.InitializeComponent();
            PrintService.PrintingContainer = PrintingContainer;
            //
            using (ModelContext db = new ModelContext())
            {
                //InsertIntoData(db);                      
                porodas = db.Porodas.ToList();
         
                //maketThenPorodas = db.MaketThenPorodas.ToList();
            }
            porodaList.ItemsSource = porodas;
            //
        }

   
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            GetDataList();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            if (e.Parameter != null)
                idSelectedKvartal = Convert.ToInt32(e.Parameter.ToString());
        }


        #region CRUD
        ObservableCollection<MaketThen> items = new ObservableCollection<MaketThen>();
        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (phonesList.SelectedItem != null)
            {
                Videl videl = phonesList.SelectedItem as Videl;
                if (videl != null)
                    Frame.Navigate(typeof(CreateVidelPage), videl.Id);
            }
        }
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (phonesList.SelectedItem != null)
                {
                    Videl videl = phonesList.SelectedItem as Videl;
                    if (videl != null)
                    {
                        using (ModelContext db = new ModelContext())
                        {
                            SessionUser current = db.SessionUsers.LastOrDefault();
                            db.Videls.Remove(videl);
                            db.SaveChanges();
                            GetDataList();
                        }
                    }
                }
            }
            catch /*(Exception ex)*/
            {
                //ContentDialog deleteFileDialog = new ContentDialog()
                //{
                //    Title = "Сообщение об ошибке",
                //    Content = ex.Message,
                //    PrimaryButtonText = "ОК",
                //};
                //ContentDialogResult result = await deleteFileDialog.ShowAsync();
            }
         
        }
        private async void Add_ClickAsync(object sender, RoutedEventArgs e)
        {
            poroda = porodaList.SelectedItem as Poroda;
            try
            {
                using (ModelContext db = new ModelContext())
                {
                    SessionUser c_user = db.SessionUsers.LastOrDefault();
                    Videl c_videl = (Videl)phonesList.SelectedItem;

                    MaketThen maket = new MaketThen
                    {
                        Yarus = Convert.ToInt32(txtYarus.Text),
                        Koeficent = Convert.ToInt32(txtKoef.Text),

                        Vozrast = Convert.ToInt32(txtVozrast.Text),
                        Visota = Convert.ToInt32(txtVisota.Text),
                        Diametr = Convert.ToInt32(txtDiametr.Text),
                        Polnota = Convert.ToDouble(txtPoln.Text),
                        Poroda = poroda,
                        CurrentKvartalID = c_videl.KvartalId,
                        CurrentVidelID = c_videl.Id,
                        CurrentAccountID = c_user.SId

                    };
                    //items.Add(maket);
                    db.Porodas.Attach(poroda);
                    db.MaketThens.Add(maket);
                    await Validate(maket, db, 1);

                    //   gridMaket10.ItemsSource = items;
                    gridMaket10.ItemsSource = db.MaketThens.Include(p=>p.Poroda).Where(d => d.CurrentAccountID == c_user.SId & d.CurrentVidelID == c_videl.Id & d.CurrentKvartalID == idSelectedKvartal);
                }
            }
            catch (Exception ex)
            {

                ContentDialog deleteFileDialog = new ContentDialog()
                {
                    Title = "Сообщение",
                    Content = ex.Message,
                    PrimaryButtonText = "ОК",
                };
                ContentDialogResult result = await deleteFileDialog.ShowAsync();
            }



           
        }

        private async Task Validate(object k, ModelContext db, int index)
        {

            var results = new List<ValidationResult>();
            var context = new ValidationContext(k);
            if (!Validator.TryValidateObject(k, context, results, true))
            {
                foreach (var error in results)
                {
                    await (new MessageDialog(error.ErrorMessage, "Сообщение")).ShowAsync();
                }
                index = 0;
            }
            else
            {
               
                db.SaveChanges();
            }

        }
        #endregion

        #region Selectors
        private void PhonesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using (ModelContext db = new ModelContext())
            {
                SessionUser current = db.SessionUsers.LastOrDefault();
                Videl selectedVidel = (Videl)phonesList.SelectedItem;
                gridMaket10.ItemsSource = db.MaketThens.Include(p => p.Poroda).Where(d => d.CurrentAccountID == current.SId & d.CurrentVidelID == selectedVidel.Id & d.CurrentKvartalID == idSelectedKvartal);
            }
        }
        private void BoxExport_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (BoxExport.SelectedIndex)
            {
                case 0:
                    SavePicker();
                    break;
           
            }
        }
        #endregion

        #region Printing
        private void OnPrintButtonClick(object sender, RoutedEventArgs e)
        {
            var service = new PrintService();
            //service.Header = new TextBlock { Text = "Dashboard to Report" };
            service.PageNumbering = PageNumbering.None;

            var cont = new ContentControl();
            cont.ContentTemplate = Resources["ReportTemplate"] as DataTemplate;
            cont.DataContext = this;
            service.AddPrintContent(cont);

            service.Print();
        }
        #endregion

        #region Functions
        private void GetDataList()
        {
            using (ModelContext db = new ModelContext())
            {
                SessionUser current = db.SessionUsers.LastOrDefault();
                videls = new ObservableCollection<Videl>(db.Videls.Include(x => x.Forestry).Include(x => x.Kvartal).Include(x => x.TypeEarth).Include(x => x.TypeErrosion).Include(x => x.TypeOrl).Include(x => x.DegreeErrosion).Include(x => x.ExpositionSlope).Where(v => v.KvartalId == idSelectedKvartal & v.AccountId == current.SId).ToList());
            }
            phonesList.ItemsSource = videls;
        }
        public string SerializeSet(string fileName)
        {
            Videl selectedVidel = (Videl)phonesList.SelectedItem;
       

            Videl vd = new Videl
            {
                Id = selectedVidel.Id,
                NumVidel = selectedVidel.NumVidel,
                Area = selectedVidel.NumVidel,
                Krut = selectedVidel.Krut,
                ForestryId = selectedVidel.ForestryId,
                TypeErrosionId = selectedVidel.TypeErrosionId,
                DegreeErrosionId = selectedVidel.DegreeErrosionId,
                ExpositionSlopeId = selectedVidel.ExpositionSlopeId,
                TypeOrlId = selectedVidel.TypeOrlId,
                TypeEarthId = selectedVidel.TypeEarthId,

                CreateDateVidel = selectedVidel.CreateDateVidel,
                AccountId = selectedVidel.AccountId,
                KvartalId = selectedVidel.KvartalId
            };

            JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            string serialized = JsonConvert.SerializeObject(vd, settings);
            return serialized;
        }
        private async void SavePicker()
        {
            try
            {
                var savePicker = new Windows.Storage.Pickers.FileSavePicker();
                savePicker.SuggestedStartLocation =
                    Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
                savePicker.FileTypeChoices.Add("json", new List<string>() { ".json" });
                savePicker.SuggestedFileName = "Новый документ";


                Windows.Storage.StorageFile file = await savePicker.PickSaveFileAsync();
                Windows.Storage.CachedFileManager.DeferUpdates(file);
                string contents = SerializeSet(file.Name);
                await Windows.Storage.FileIO.WriteTextAsync(file, contents);
                Windows.Storage.Provider.FileUpdateStatus status = await Windows.Storage.CachedFileManager.CompleteUpdatesAsync(file);

                //if (status == Windows.Storage.Provider.FileUpdateStatus.Complete)
                //{
                //    Status.Text = "File " + file.Name + " was saved.";
                //}
                //else
                //{
                //    Status.Text = "File " + file.Name + " couldn't be saved.";
                //}
            }
            catch
            {
                //Status.Text = "Operation canceled.";
            }
        }
        #endregion

        private void EditMaket10_Click(object sender, RoutedEventArgs e)
        {
       
            btnEditMaket10.Visibility = Visibility.Collapsed;
            btnSaveMaket10.Visibility = Visibility.Visible;
            btnDeleteMaket10.Visibility = Visibility.Visible;
            btnAddMaket10.Visibility = Visibility.Visible;
            textBoxMaket10.Visibility = Visibility.Visible;
        }

        private void BtnSaveMaket10_Click(object sender, RoutedEventArgs e)
        {
            btnEditMaket10.Visibility = Visibility.Visible;
            btnSaveMaket10.Visibility = Visibility.Collapsed;
            btnAddMaket10.Visibility = Visibility.Collapsed;
            textBoxMaket10.Visibility = Visibility.Collapsed;
            btnDeleteMaket10.Visibility = Visibility.Collapsed;
        }

        private void BtnDeleteMaket10_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (ModelContext db = new ModelContext())
                {
                    SessionUser c_user = db.SessionUsers.LastOrDefault();
                    Videl c_videl = (Videl)phonesList.SelectedItem;
                    MaketThen mk = (MaketThen)gridMaket10.SelectedItem;
                    db.MaketThens.Remove(mk);
                    db.SaveChanges();
                    gridMaket10.ItemsSource = db.MaketThens.Include(p => p.Poroda).Where(d => d.CurrentAccountID == c_user.SId & d.CurrentVidelID == c_videl.Id & d.CurrentKvartalID == idSelectedKvartal);
                }
            }
            catch
            {

             
            }
          
          
        }
    }
  
}
