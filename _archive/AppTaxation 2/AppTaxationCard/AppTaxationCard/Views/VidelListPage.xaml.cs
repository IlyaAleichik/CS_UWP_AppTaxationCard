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


// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace AppTaxationCard.Views
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    /// 

    public sealed partial class VidelListPage : Page
    {

        MainPage listpage = new MainPage();
        ObservableCollection<Videl> videls;

        ModelContext context = new ModelContext();
        private int idSelectedKvartal { get; set; }

        public VidelListPage()
        {
            this.InitializeComponent();
            PrintService.PrintingContainer = PrintingContainer;
        }
        #region Printing Funct
  
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
           
            if (e.Parameter != null)
                idSelectedKvartal = Convert.ToInt32(e.Parameter.ToString());
        }

        #endregion

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            GetDataList();                    
        }
        private void GetDataList()
        {
            using (ModelContext db = new ModelContext())
            {
                SessionUser current = db.SessionUsers.LastOrDefault();
                videls = new ObservableCollection<Videl>(db.Videls.Include(x => x.Forestry).Include(x => x.Kvartal).Include(x => x.TypeEarth).Include(x => x.TypeErrosion).Include(x => x.TypeOrl).Include(x => x.ExpositionSlope).Where(v => v.KvartalId == idSelectedKvartal & v.AccountId == current.SId).ToList());

            }
            phonesList.ItemsSource = videls;
        }

  
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
   

        private void PhonesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using (ModelContext db = new ModelContext())
            {
                SessionUser current = db.SessionUsers.LastOrDefault();
                Videl selectedVidel = (Videl)phonesList.SelectedItem;       
                gridMaket10.ItemsSource = db.MaketThens.Where(d => d.CurrentAccountID == current.SId & d.CurrentVidelID == selectedVidel.Id & d.CurrentKvartalID == idSelectedKvartal);
            }
          




        }


  

        ObservableCollection<MaketThen> items = new ObservableCollection<MaketThen>();
        private void Add_Click(object sender, RoutedEventArgs e)
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
                    Polnota = Convert.ToInt32(txtPoln.Text),
                    Poroda = txtPoroda.Text,
                    CurrentKvartalID = c_videl.KvartalId,
                    CurrentVidelID = c_videl.Id,
                    CurrentAccountID = c_user.SId

                };
                items.Add(maket);
                db.MaketThens.Add(maket);
                db.SaveChanges();
            }


            gridMaket10.ItemsSource = items;
        }






        public string SerializeSet(string fileName)
        {
           

            Person person1 = new Person("Tom", 29, new Company("Microsoft"));
            Person person2 = new Person("Bill", 25, new Company("Apple"));
            Person[] people = new Person[] { person1, person2 };

            JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            string serialized = JsonConvert.SerializeObject(people, settings);
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

        private void BoxExport_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            switch (BoxExport.SelectedIndex)
            {
                case 0:
          
                    break;
                case 1:
                    // Videl selectedVidel = (Videl)phonesList.SelectedItem;
                    SavePicker();

                    break;
                case 2:
                    break;
            }
          ;

        }

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
    }
    public class Person
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int Age { get; set; }
        [DataMember]
        public Company Company { get; set; }

        public Person()
        { }

        public Person(string name, int age, Company comp)
        {
            Name = name;
            Age = age;
            Company = comp;
        }
    }

    public class Company
    {
        public string Name { get; set; }

        public Company() { }

        public Company(string name)
        {
            Name = name;
        }
    }

}
