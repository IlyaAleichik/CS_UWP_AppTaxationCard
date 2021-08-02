using AppTaxationCard.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Microsoft.EntityFrameworkCore;
// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace AppTaxationCard.Views
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class VidelListPage : Page
    {
        
        MainPage listpage = new MainPage();
        ObservableCollection<Videl> videls;

        ModelContext context = new ModelContext();
        private int idSelectedKvartal { get; set; }

        public VidelListPage()
        {
            this.InitializeComponent();
      
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
                idSelectedKvartal = Convert.ToInt32(e.Parameter.ToString());
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {





            GetDataList();
            //using (ModelContext db = new ModelContext())
            //{
            //    gridMaket10.ItemsSource = db.MaketThens.ToList();
            //}
              
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
            Videl videl = phonesList.SelectedItem as Videl;
            using (ModelContext db = new ModelContext())
            {
                Videl selectedVidel = (Videl)phonesList.SelectedItem;
                MaketThen maket = db.MaketThens.Include(c => c.MaketThenPoroda).FirstOrDefault(c => c.CurrentVidel == selectedVidel.Id);

                if (phonesList.SelectedItem != null)
                {
                   
                    if (videl != null )
                    {

                        SessionUser current = db.SessionUsers.LastOrDefault();
                        //db.MaketThens.Remove();
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

                //foreach (MaketThen m in maket)
                //{
                //    ;
                Videl selectedVidel = (Videl)phonesList.SelectedItem;
                //}var

                var sel = db.MaketThens.Include(d => d.MaketThenPoroda).Where(d => d.AccountEditId == current.SId & d.CurrentVidel == selectedVidel.Id & d.CurrentKvartal == idSelectedKvartal).ToList();
                if(sel == null)
                {
                    gridMaket10.ItemsSource = sel;
                }
             
            }
          




        }
    }
}
