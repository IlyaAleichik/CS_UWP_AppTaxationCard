using AppTaxationCard.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.AppService;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace AppTaxationCard.Views
{


    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    /// 


  
    public class ListTable
    {
        public int Id;
        public string Title;
    }

   
    public sealed partial class ControlDataBasePage : Page
    {
       
        ObservableCollection<Videl> videls;
        private ValueSet table = null;
        public ControlDataBasePage()
        {
            this.InitializeComponent();
            App.AppServiceConnected += MainPage_AppServiceConnected;

            this.Loaded += BtnGetVidels_Click;     
    
        }


        private async void ExportToExcel_Click(object sender, RoutedEventArgs e)
        {
            // create a ValueSet from the datacontext
            table = new ValueSet();
            table.Add("REQUEST", "CreateSpreadsheet");
            for (int i = 0; i < videls.Count; i++)
            {
                table.Add("Id" + i.ToString(), videls[i].Id);
                table.Add("Крутизна" + i.ToString(), videls[i].Krut);
                table.Add("Квартал" + i.ToString(), videls[i].Kvartal);
                table.Add("Номер выдела" + i.ToString(), videls[i].NumVidel);
            }

            // launch the fulltrust process and for it to connect to the app service            
            if (ApiInformation.IsApiContractPresent("Windows.ApplicationModel.FullTrustAppContract", 1, 0))
            {
                await FullTrustProcessLauncher.LaunchFullTrustProcessForCurrentAppAsync();
            }
            else
            {
                MessageDialog dialog = new MessageDialog("This feature is only available on Windows 10 Desktop SKU");
                await dialog.ShowAsync();
            }
        }

        private async void MainPage_AppServiceConnected(object sender, EventArgs e)
        {
            // send the ValueSet to the fulltrust process
            AppServiceResponse response = await App.Connection.SendMessageAsync(table);

            // check the result
            object result;
            response.Message.TryGetValue("RESPONSE", out result);
            if (result.ToString() != "SUCCESS")
            {
                MessageDialog dialog = new MessageDialog(result.ToString());
                await dialog.ShowAsync();
            }
            // no longer need the AppService connection
            App.AppServiceDeferral.Complete();
        }





        private void StartClock()
        {
            // Init the clock and start the thread for the clock.
            Task.Run(async () => {
                while (true)
                {
                    await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        this.txt_clock.Text = DateTime.Now.ToString("HH:mm:ss");
                    });
                    await Task.Delay(500);
                }
            });
        }
        private static ObservableCollection<Videl> _items;
        private static CollectionViewSource groupedItems;

        public CollectionViewSource GroupData()
        {
            using (ModelContext db = new ModelContext())
            {
                _items = new ObservableCollection<Videl>(db.Videls.Include(x => x.Forestry).Include(x => x.Kvartal).Include(x => x.TypeEarth).Include(x => x.TypeErrosion).Include(x => x.TypeOrl).Include(x => x.ExpositionSlope).ToList());
         
              
            ObservableCollection<GroupInfoCollection<Videl>> groups = new ObservableCollection<GroupInfoCollection<Videl>>();
            var query = from item in _items
                        orderby item
                        group item by item.TypeEarth.Id into g
                        select new { GroupName = g.Key, Items = g };

            foreach (var g in query)
            {
                GroupInfoCollection<Videl> info = new GroupInfoCollection<Videl>();
                info.Key = g.GroupName;
                foreach (var item in g.Items)
                {
                    info.Add(item);
                }
                groups.Add(info);
            }

            groupedItems = new CollectionViewSource();
            groupedItems.IsSourceGrouped = true;
            groupedItems.Source = groups;
            }
            return groupedItems;
        }

        public class GroupInfoCollection<T> : ObservableCollection<T>
        {
            public object Key { get; set; }

            public new IEnumerator<T> GetEnumerator()
            {
                return (IEnumerator<T>)base.GetEnumerator();
            }
        }

        public ObservableCollection<ListTable> listTables { get; set; }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {        
            listTables = new ObservableCollection<ListTable>
            {
                new ListTable {Id=1, Title="Виды земель"},
                new ListTable {Id=2, Title="ОРЛ"},
                new ListTable {Id=3, Title="Экспозиции"},
                new ListTable {Id=4, Title="Виды эрозии"},
                new ListTable {Id=5, Title="Степени эрозии"},
                new ListTable {Id=6, Title="Лесничества"},
    
            };
            StartClock();
            NavLinksList.ItemsSource = listTables;    
        }


        private void NavLinksList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            vc_kvartal.Visibility = Visibility.Collapsed;
            vc_forestry.Visibility = Visibility.Collapsed;
            vc_numvidel.Visibility = Visibility.Collapsed;
            vc_area.Visibility = Visibility.Collapsed;
            vc_typeearth.Visibility = Visibility.Collapsed;
            vc_typeorl.Visibility = Visibility.Collapsed;
            vc_krut.Visibility = Visibility.Collapsed;
            vc_expositions.Visibility = Visibility.Collapsed;
            vc_typeerosion.Visibility = Visibility.Collapsed;
            vc_degree.Visibility = Visibility.Collapsed;
            ListTable selectedTable = (ListTable)NavLinksList.SelectedItem;
            using (ModelContext db = new ModelContext())
            {

                switch (NavLinksList.SelectedIndex)
                {
                    default:
                     
                        txtTypeEarth.IsEnabled = false;
                        txtTypeOrl.IsEnabled = false;
                        txtExpositions.IsEnabled = false;
                        txtTypeErosion.IsEnabled = false;
                        txtDegreeErosion.IsEnabled = false;
                        txtForestry.IsEnabled = false;
                        break;

                    case 0:

                        c_forestry.Visibility = Visibility.Collapsed;
                        c_typeearth.Visibility = Visibility.Visible;
                        c_typeorl.Visibility = Visibility.Collapsed;
                        c_expositions.Visibility = Visibility.Collapsed;
                        c_erosion.Visibility = Visibility.Collapsed;
                        c_degree.Visibility = Visibility.Collapsed;

                      
                        txtTypeOrl.IsEnabled = false;
                        txtExpositions.IsEnabled = false;
                        txtTypeErosion.IsEnabled = false;
                        txtDegreeErosion.IsEnabled = false;
                        txtForestry.IsEnabled = false;
                        txtTypeEarth.IsEnabled = true;

                        TableName.Text = selectedTable.Title;
                        dataGrid.ItemsSource = db.TypeEarths.ToList();
                        break;
                    case 1:



                        c_forestry.Visibility = Visibility.Collapsed;
                        c_typeearth.Visibility = Visibility.Collapsed;
                        c_typeorl.Visibility = Visibility.Visible;
                        c_expositions.Visibility = Visibility.Collapsed;
                        c_erosion.Visibility = Visibility.Collapsed;
                        c_degree.Visibility = Visibility.Collapsed;

                        txtTypeOrl.IsEnabled = true;
                        txtExpositions.IsEnabled = false;
                        txtTypeErosion.IsEnabled = false;
                        txtDegreeErosion.IsEnabled = false;
                        txtForestry.IsEnabled = false;
                        txtTypeEarth.IsEnabled = false;

                        TableName.Text = selectedTable.Title;
                        dataGrid.ItemsSource = db.TypeOrls.ToList();
                        break;
                    case 2:


                        c_forestry.Visibility = Visibility.Collapsed;
                        c_typeearth.Visibility = Visibility.Collapsed;
                        c_typeorl.Visibility = Visibility.Collapsed;
                        c_expositions.Visibility = Visibility.Visible;
                        c_erosion.Visibility = Visibility.Collapsed;
                        c_degree.Visibility = Visibility.Collapsed;

                        txtTypeOrl.IsEnabled = false;
                        txtExpositions.IsEnabled = true;
                        txtTypeErosion.IsEnabled = false;
                        txtDegreeErosion.IsEnabled = false;
                        txtForestry.IsEnabled = false;
                        txtTypeEarth.IsEnabled = false;

                        TableName.Text = selectedTable.Title;
                        dataGrid.ItemsSource = db.ExpositionSlopes.ToList();
                        break;
                    case 3:


                        c_forestry.Visibility = Visibility.Collapsed;
                        c_typeearth.Visibility = Visibility.Collapsed;
                        c_typeorl.Visibility = Visibility.Collapsed;
                        c_expositions.Visibility = Visibility.Collapsed;
                        c_erosion.Visibility = Visibility.Visible;
                        c_degree.Visibility = Visibility.Collapsed;

                        txtTypeOrl.IsEnabled = false;
                        txtExpositions.IsEnabled = false;
                        txtTypeErosion.IsEnabled = true;
                        txtDegreeErosion.IsEnabled = false;
                        txtForestry.IsEnabled = false;
                        txtTypeEarth.IsEnabled = false;

                        TableName.Text = selectedTable.Title;
                        dataGrid.ItemsSource = db.TypeErrosions.ToList();
                        break;
                    case 4:


                        c_forestry.Visibility = Visibility.Collapsed;
                        c_typeearth.Visibility = Visibility.Collapsed;
                        c_typeorl.Visibility = Visibility.Collapsed;
                        c_expositions.Visibility = Visibility.Collapsed;
                        c_erosion.Visibility = Visibility.Collapsed;
                        c_degree.Visibility = Visibility.Visible;


                        txtTypeOrl.IsEnabled = false;
                        txtExpositions.IsEnabled = false;
                        txtTypeErosion.IsEnabled = false;
                        txtDegreeErosion.IsEnabled = true;
                        txtForestry.IsEnabled = false;
                        txtTypeEarth.IsEnabled = false;

                        TableName.Text = selectedTable.Title;
                        dataGrid.ItemsSource = db.DegreeErrosions.ToList();
                        break;
                    case 5:

                 
                        c_forestry.Visibility = Visibility.Visible;                       
                        c_typeearth.Visibility = Visibility.Collapsed;
                        c_typeorl.Visibility = Visibility.Collapsed;                 
                        c_expositions.Visibility = Visibility.Collapsed;
                        c_erosion.Visibility = Visibility.Collapsed;
                        c_degree.Visibility = Visibility.Collapsed;


                        txtTypeOrl.IsEnabled = false;
                        txtExpositions.IsEnabled = false;
                        txtTypeErosion.IsEnabled = false;
                        txtDegreeErosion.IsEnabled = false;
                        txtForestry.IsEnabled = true;
                        txtTypeEarth.IsEnabled = false;

                        TableName.Text = selectedTable.Title;
                        dataGrid.ItemsSource = db.Forestries.ToList();
                        break;
        
                }
            } 
          
        }

   
        private void BtnGetVidels_Click(object sender, RoutedEventArgs e)
        {
            TableName.Text = btnGetVidels.Content.ToString();
            c_forestry.Visibility = Visibility.Collapsed;
            c_typeearth.Visibility = Visibility.Collapsed;
            c_typeorl.Visibility = Visibility.Collapsed;
            c_expositions.Visibility = Visibility.Collapsed;
            c_erosion.Visibility = Visibility.Collapsed;
            c_degree.Visibility = Visibility.Collapsed;

            vc_kvartal.Visibility = Visibility.Visible;
            vc_forestry.Visibility = Visibility.Visible;
            vc_numvidel.Visibility = Visibility.Visible;
            vc_area.Visibility = Visibility.Visible;
            vc_typeearth.Visibility = Visibility.Visible;
            vc_typeorl.Visibility = Visibility.Visible;
            vc_krut.Visibility = Visibility.Visible;
            vc_expositions.Visibility = Visibility.Visible;
            vc_typeerosion.Visibility = Visibility.Visible;
            vc_degree.Visibility = Visibility.Visible;

            using (ModelContext db = new ModelContext())
            {
                SessionUser current = db.SessionUsers.LastOrDefault();
                videls = new ObservableCollection<Videl>(db.Videls.Include(x => x.Forestry).Include(x => x.Kvartal).Include(x => x.TypeEarth).Include(x => x.TypeErrosion).Include(x => x.TypeOrl).Include(x => x.ExpositionSlope).ToList());
            }
            dataGrid.ItemsSource = videls;

        }



        private void SearchBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            using (ModelContext db = new ModelContext())
            {
          
                //if (args.CheckCurrent())
                //{

                //    var term = searchBox.Text.ToLower();
                //    var results = GetData("VidelsView").Where(i => i.ToLower().Contains(term)).ToList();
                //    searchBox.ItemsSource = results;
                //}
            }
        
        }

  
     

        private void DataGrid_LoadingRowGroup(object sender, Microsoft.Toolkit.Uwp.UI.Controls.DataGridRowGroupHeaderEventArgs e)
        {
            ICollectionViewGroup group = e.RowGroupHeader.CollectionViewGroup;
            Videl item = group.GroupItems[0] as Videl;
            e.RowGroupHeader.PropertyValue = item.TypeEarth.ToString();
        }

        private void BtnGroupBy_Click(object sender, RoutedEventArgs e)
        {
      
            if (dataGrid != null)
            {
                dataGrid.ItemsSource = GroupData().View;
            }
        }
       
        private void BtnGetDataBaseData_Click(object sender, RoutedEventArgs e)
        {
            using (ModelContext db = new ModelContext())
            {
                
                db.Forestries.Add(new Forestry { NameForestry = "Заславльское", });
                db.Forestries.Add(new Forestry { NameForestry = "Новосельское", });
                db.Forestries.Add(new Forestry { NameForestry = "Путчинское", });
                db.Forestries.Add(new Forestry { NameForestry = "Кайковское", });
                db.Forestries.Add(new Forestry { NameForestry = "Держинское", });
                db.Forestries.Add(new Forestry { NameForestry = "Станьковское", });
                db.Forestries.Add(new Forestry { NameForestry = "Волмянское", });

                db.SaveChanges();

                db.TypeErrosions.Add(new TypeErrosion { NameTypeErrosion = "Водная", });
                db.TypeErrosions.Add(new TypeErrosion { NameTypeErrosion = "Ветровая", });
                db.SaveChanges();

                db.DegreeErrosions.Add(new DegreeErrosion { NameDegreeErrosion = "Совсем смытые", });
                db.DegreeErrosions.Add(new DegreeErrosion { NameDegreeErrosion = "Слабая", });
                db.DegreeErrosions.Add(new DegreeErrosion { NameDegreeErrosion = "Средняя", });
                db.DegreeErrosions.Add(new DegreeErrosion { NameDegreeErrosion = "Сильная", });
                db.SaveChanges();

                db.ExpositionSlopes.Add(new ExpositionSlope { NameExpositionSlope = "С", });
                db.ExpositionSlopes.Add(new ExpositionSlope { NameExpositionSlope = "Ю", });
                db.ExpositionSlopes.Add(new ExpositionSlope { NameExpositionSlope = "З", });
                db.ExpositionSlopes.Add(new ExpositionSlope { NameExpositionSlope = "В", });
                db.ExpositionSlopes.Add(new ExpositionSlope { NameExpositionSlope = "СЗ", });
                db.ExpositionSlopes.Add(new ExpositionSlope { NameExpositionSlope = "ЮЗ", });
                db.ExpositionSlopes.Add(new ExpositionSlope { NameExpositionSlope = "СВ", });
                db.ExpositionSlopes.Add(new ExpositionSlope { NameExpositionSlope = "ЮВ", });
                db.SaveChanges();


                db.TypeOrls.Add(new TypeOrl { NameTypeOrl = "Прибережные полосы леса", });
                db.TypeOrls.Add(new TypeOrl { NameTypeOrl = "Участки леса в поймах рек", });
                db.TypeOrls.Add(new TypeOrl { NameTypeOrl = "4-я зона радиактивного загрязнения", });
                db.TypeOrls.Add(new TypeOrl { NameTypeOrl = "Леса генетических резерватов", });
                db.SaveChanges();

                db.TypeEarths.Add(new TypeEarth { NameTypeEarth = "Насаждения естественного происхождения", });
                db.TypeEarths.Add(new TypeEarth { NameTypeEarth = "Лесные культуры", });
                db.TypeEarths.Add(new TypeEarth { NameTypeEarth = "Квартальные просеки", });
                db.SaveChanges();

                //db.MaketThenPorodas.Add(new MaketThenPoroda { NamePoroda = "С", });
                //db.MaketThenPorodas.Add(new MaketThenPoroda { NamePoroda = "Б", });
                //db.MaketThenPorodas.Add(new MaketThenPoroda { NamePoroda = "Д", });
                //db.SaveChanges();
            }
        }

        private void BtnAddDataSave_Click(object sender, RoutedEventArgs e)
        {
            using (ModelContext db = new ModelContext())
            {
                switch (NavLinksList.SelectedIndex)
                {

                    case 0:
                        TypeEarth typeEarth = new TypeEarth
                        {
                            NameTypeEarth = txtForestry.Text
                        };
                        db.TypeEarths.Add(typeEarth);
                        db.SaveChanges();
                        dataGrid.ItemsSource = db.TypeEarths.ToList();
                        break;
                    case 1:
                        TypeOrl typeOrl = new TypeOrl
                        {
                            NameTypeOrl = txtTypeOrl.Text
                        };
                        db.TypeOrls.Add(typeOrl);
                        db.SaveChanges();
                        dataGrid.ItemsSource = db.TypeOrls.ToList();
                        break;
                    case 2:
                        ExpositionSlope expositionSlope = new ExpositionSlope
                        {
                            NameExpositionSlope = txtExpositions.Text
                        };
                        db.ExpositionSlopes.Add(expositionSlope);
                        db.SaveChanges();
                        dataGrid.ItemsSource = db.ExpositionSlopes.ToList();
                        break;
                    case 3:
                        TypeErrosion typeErrosion = new TypeErrosion
                        {
                            NameTypeErrosion = txtTypeErosion.Text
                        };
                        db.TypeErrosions.Add(typeErrosion);
                        db.SaveChanges();
                        dataGrid.ItemsSource = db.TypeErrosions.ToList();
                        break;
                    case 4:
                        DegreeErrosion degreeErrosion = new DegreeErrosion
                        {
                            NameDegreeErrosion = txtDegreeErosion.Text
                        };
                        db.DegreeErrosions.Add(degreeErrosion);
                        db.SaveChanges();
                        dataGrid.ItemsSource = db.DegreeErrosions.ToList();
                        break;
                    case 5:
                        Forestry forestry = new Forestry
                        {
                            NameForestry = txtForestry.Text
                        };
                        db.Forestries.Add(forestry);
                        db.SaveChanges();
                        dataGrid.ItemsSource = db.Forestries.ToList();
                        break;

                }
            }
        }

        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            //string text = sender.Text;
            //if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            //{
            //    if (args.CheckCurrent())
            //    {
            //        sender.ItemsSource = await Task<string[]>.Run(() => { return this.GetSuggestions(sender.Text);} );
            //    }
            //}
           
        }
        private string[] suggestions = new string[] {"Amal","Edoo","Trim"};
        private string [] GetSuggestions(string text)
        {
            string[] result = null;
            return result = suggestions.Where(x=>x.Contains(text)).ToArray();
        }

        private void Expander1_Collapsed(object sender, EventArgs e)
        {
            txtTypeEarth.IsEnabled = false;
            txtTypeOrl.IsEnabled = false;
            txtExpositions.IsEnabled = false;
            txtTypeErosion.IsEnabled = false;
            txtDegreeErosion.IsEnabled = false;
            txtForestry.IsEnabled = false;
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using (ModelContext db = new ModelContext())
            {
                try {
                    switch (NavLinksList.SelectedIndex)
                    {
                       

                        case 0:
                            TypeEarth typeEarth = (TypeEarth)dataGrid.SelectedItem;
                            txtTypeEarth.Text = typeEarth.NameTypeEarth;
                            txtTypeOrl.Text = "";
                            txtExpositions.Text = "";
                            txtTypeErosion.Text = "";
                            txtDegreeErosion.Text = "";
                            txtForestry.Text = "";
                            db.SaveChanges();

                            break;
                        case 1:
                            TypeOrl typeOrl = (TypeOrl)dataGrid.SelectedItem;
                            txtTypeOrl.Text = typeOrl.NameTypeOrl;
                            txtTypeEarth.Text = "";
                            txtExpositions.Text = "";
                            txtTypeErosion.Text = "";
                            txtDegreeErosion.Text = "";
                            txtForestry.Text = "";
                            db.SaveChanges();

                            break;
                        case 2:
                            ExpositionSlope exposition = (ExpositionSlope)dataGrid.SelectedItem;
                            txtExpositions.Text = exposition.NameExpositionSlope;
                            txtTypeOrl.Text = "";
                            txtTypeEarth.Text = "";
                            txtTypeErosion.Text = "";
                            txtDegreeErosion.Text = "";
                            txtForestry.Text = "";
                            db.SaveChanges();

                            break;
                        case 3:
                            TypeErrosion typeErrosion = (TypeErrosion)dataGrid.SelectedItem;
                        
                            txtTypeErosion.Text = typeErrosion.NameTypeErrosion;
                            txtExpositions.Text = "";
                            txtTypeOrl.Text = "";
                            txtTypeEarth.Text = "";
                            txtDegreeErosion.Text = "";
                            txtForestry.Text = "";
                            db.SaveChanges();

                            break;
                        case 4:
                            DegreeErrosion degree = (DegreeErrosion)dataGrid.SelectedItem;
                            txtDegreeErosion.Text = degree.NameDegreeErrosion;
                            txtTypeErosion.Text = "";
                            txtExpositions.Text = "";
                            txtTypeOrl.Text = "";
                            txtTypeEarth.Text = "";
                            txtForestry.Text = "";
                            db.SaveChanges();

                            break;
                        case 5:
                            Forestry forestry = (Forestry)dataGrid.SelectedItem;
                            txtForestry.Text = forestry.NameForestry;
                            txtDegreeErosion.Text = "";
                            txtTypeErosion.Text = "";
                            txtExpositions.Text = "";
                            txtTypeOrl.Text = "";
                            txtTypeEarth.Text = "";
                     
                            db.SaveChanges();
                            break;

                    }
                } catch { }
               
            }
        }

        private void Refresh()
        {

        }
        private void Delete_Click(object sender, RoutedEventArgs e)
        {                                  
            using (ModelContext db = new ModelContext())
            {
                switch (NavLinksList.SelectedIndex)
                {


                    case 0:
                        TypeEarth typeEarth = (TypeEarth)dataGrid.SelectedItem;
                        db.TypeEarths.Remove(typeEarth);
                        db.SaveChanges();
                        dataGrid.ItemsSource = db.TypeEarths.ToList();
                        break;
                    case 1:
                        TypeOrl typeOrl = (TypeOrl)dataGrid.SelectedItem;
                        db.TypeOrls.Remove(typeOrl);
                        db.SaveChanges();
                        dataGrid.ItemsSource = db.TypeOrls.ToList();
                        break;
                    case 2:
                        ExpositionSlope exposition = (ExpositionSlope)dataGrid.SelectedItem;
                        db.ExpositionSlopes.Remove(exposition);
                        db.SaveChanges();
                        dataGrid.ItemsSource = db.ExpositionSlopes.ToList();
                        break;
                    case 3:
                        TypeErrosion typeErrosion = (TypeErrosion)dataGrid.SelectedItem;
                        db.TypeErrosions.Remove(typeErrosion);
                        db.SaveChanges();
                        dataGrid.ItemsSource = db.TypeErrosions.ToList();
                        break;
                    case 4:
                        DegreeErrosion degree = (DegreeErrosion)dataGrid.SelectedItem;
                        db.DegreeErrosions.Remove(degree);
                        db.SaveChanges();
                        dataGrid.ItemsSource = db.DegreeErrosions.ToList();
                        break;
                    case 5:
                        Forestry forestry = (Forestry)dataGrid.SelectedItem;
                        db.Forestries.Remove(forestry);
                        db.SaveChanges();
                        dataGrid.ItemsSource = db.Forestries.ToList();
                        break;

                }
            }   
        }
    }
}
        
        
   
