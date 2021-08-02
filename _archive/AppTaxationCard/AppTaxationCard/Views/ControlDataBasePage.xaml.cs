using AppTaxationCard.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using Windows.Storage;
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
       
        public ControlDataBasePage()
        {
            this.InitializeComponent();
            this.Loaded += BtnGetVidels_Click;

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
                new ListTable {Id=5, Title="Лесничества"},
            };
            NavLinksList.ItemsSource = listTables;    
        }
   
        private void NavLinksList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListTable selectedTable = (ListTable)NavLinksList.SelectedItem;
            using (ModelContext db = new ModelContext())
            {
                switch (NavLinksList.SelectedIndex)
                {
                    //case 0:
                    //    TableName.Text = selectedTable.Title;
                    //    dataGrid.ItemsSource = db.TypeEarth.ToList();
                    //    break;
                    //case 1:
                    //    TableName.Text = selectedTable.Title;
                    //    dataGrid.ItemsSource = db.TypeOrl.ToList();
                    //    break;
                    //case 2:
                    //    TableName.Text = selectedTable.Title;
                    //    dataGrid.ItemsSource = db.Expositions.ToList();
                    //    break;
                    //case 3:
                    //    TableName.Text = selectedTable.Title;
                    //    dataGrid.ItemsSource = db.TypeErrosion.ToList();
                    //    break;
                    //case 4:
                    //    TableName.Text = selectedTable.Title;
                    //    dataGrid.ItemsSource = db.DegreeErrosion.ToList();
                    //    break;
                    case 5:
                        TableName.Text = selectedTable.Title;
                        dataGrid.ItemsSource = db.Forestries.ToList();
                        break;

                }
            } 
          
        }
        ObservableCollection<Videl> videls;
        private void BtnGetVidels_Click(object sender, RoutedEventArgs e)
        {

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

                db.MaketThenPorodas.Add(new MaketThenPoroda { NamePoroda = "С", });
                db.MaketThenPorodas.Add(new MaketThenPoroda { NamePoroda = "Б", });
                db.MaketThenPorodas.Add(new MaketThenPoroda { NamePoroda = "Д", });
                db.SaveChanges();
            }
        }
    }
}
        
        
   
