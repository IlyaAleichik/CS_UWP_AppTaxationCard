using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using AppTaxationCard.Models;
using Windows.UI.Xaml.Media.Animation;
// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace AppTaxationCard.Views
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            this.InitializeComponent();
          

        }

        private async void Login_Click(object sender, RoutedEventArgs e)
        {


            //string user = usernameBox.Text;
            //string pass = passwordBox.Password;
            using (ModelContext db = new ModelContext())
            {

             
                var account = db.Accounts.FirstOrDefault(u => u.Username == usernameBox.Text);
                if (account != null)
                {
                    if (account.Password == passwordBox.Password)
                    {
                        db.SessionUsers.Add(new SessionUser { SId = account.Id, Username = usernameBox.Text, SessionTime = DateTime.Now, });
                        db.SaveChanges();

                        Frame.Navigate(typeof(MainPage), new DrillInNavigationTransitionInfo());
                    }
                    else
                    {
                        ContentDialog deleteFileDialog = new ContentDialog()
                        {
                            Title = "Сообщение об ошибке",
                            Content = "Не верный пароль, желаете продолжить? ",
                            PrimaryButtonText = "ОК",
                            SecondaryButtonText = "Отмена"
                        };
                        ContentDialogResult result = await deleteFileDialog.ShowAsync();
                    }
                }
                else
                {
                    ContentDialog deleteFileDialog = new ContentDialog()
                    {
                        Title = "Сообщение об ошибке",
                        Content = "Пользователь не найден ",
                        PrimaryButtonText = "ОК"
                    };
                    ContentDialogResult result = await deleteFileDialog.ShowAsync();
                }
            }
        }
        
        private void PhonesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Account selectedPhone = (Account)phonesList.SelectedItem;
            //await new Windows.UI.Popups.MessageDialog($"Выбран {selectedPhone.username}").ShowAsync();
        }

        //private void Page_Loaded(object sender, RoutedEventArgs e)
        //{
        //    // добавление нового объекта
        //    using (ModelContext db = new ModelContext())
        //    {

        //        //db.Accounts.Add(new Account { Username = "Ilya", Password = "121790", CreateTime = DateTime.Now, });
        //        //db.Accounts.Add(new Account { Username = "Vova", Password = "121790", CreateTime = DateTime.Now, });
        //        //db.Accounts.Add(new Account { Username = "Den", Password = "121790", CreateTime = DateTime.Now, });
        //        //db.Accounts.Add(new Account { Username = "Evgen", Password = "121790", CreateTime = DateTime.Now, });
        //        //db.SaveChanges();


        //        //db.Videls.Add(new Videl { NumKvartal = 1, Area = 200, AccountId = 1, Bonitet = "C", });
        //        //db.Videls.Add(new Videl { NumKvartal = 2, Area = 300, AccountId = 1, Bonitet = "C", });
        //        //db.Videls.Add(new Videl { NumKvartal = 1, Area = 100, AccountId = 2, Bonitet = "C", });
        //        //db.Videls.Add(new Videl { NumKvartal = 1, Area = 150, AccountId = 3, Bonitet = "C", });
        //        //db.SaveChanges();

                
        //    }
        //}

        private void SignUp_Click(object sender, RoutedEventArgs e)
        {
           Frame.Navigate(typeof(RegistrationPage), new DrillInNavigationTransitionInfo());
        }
    }
}
