using System;
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
    /// 


    public sealed partial class RegistrationPage : Page
    {
        public RegistrationPage()
        {
            this.InitializeComponent();
        }
      
        private async void SignIn_Click(object sender, RoutedEventArgs e)
        {
            //Account account = new Account();
            using (ModelContext db = new ModelContext())
            {
                try
                {

                    if (isPasswordBox.Password != passwordBox.Password)
                    {
                        ContentDialog deleteFileDialog = new ContentDialog()
                        {
                            Title = "Сообщение",
                            Content = "Пароли не совпадают ",
                            PrimaryButtonText = "ОК",

                        };
                        ContentDialogResult result = await deleteFileDialog.ShowAsync();
                    }

                    //if ( isPasswordBox.Password != passwordBox.Password)
                    //{
                    //    ContentDialog deleteFileDialog = new ContentDialog()
                    //    {
                    //        Title = "Сообщение",
                    //        Content = "Пароли не совпадают ",
                    //        PrimaryButtonText = "ОК",

                    //    };
                    //    ContentDialogResult result = await deleteFileDialog.ShowAsync();                 
                    //}
                    else
                    {
                        db.Accounts.Add(new Account {
                            Username = usernameBox.Text,
                            Name = nameBox.Text,
                            Surname = surnameBox.Text,
                            Phone = phoneBox.Text,
                            Email = emailBox.Text,
                            Password = passwordBox.Password,
                            CreateTime = DateTime.Now, });
                        db.SaveChanges();
                        Frame.Navigate(typeof(LoginPage), new DrillInNavigationTransitionInfo());
                    }


                }
                catch { }
                
               
               
           
            }
           
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }
    }
}
