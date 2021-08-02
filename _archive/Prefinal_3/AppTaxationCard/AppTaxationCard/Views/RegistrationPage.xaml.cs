using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using AppTaxationCard.Models;
using Windows.UI.Xaml.Media.Animation;
using System.Threading.Tasks;
using Windows.UI.Popups;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

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
            using (ModelContext db = new ModelContext())
            {
                try
                {
                    Account acc = new Account
                    {
                        Username = usernameBox.Text,
                        Name = nameBox.Text,
                        Surname = surnameBox.Text,
                        Phone = phoneBox.Text,
                        Email = emailBox.Text,
                        Password = passwordBox.Password,
                        CreateTime = DateTime.Now,
                    };

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
                    else
                    {
                        db.Accounts.Add(acc);
                        await Validate(acc,db,1);
                      
                    }

                }
                catch(Exception ex)
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

        }

        private async Task Validate(object k, ModelContext db, int index)
        {

            var results = new List<ValidationResult>();
            var context = new ValidationContext(k);
            if (!Validator.TryValidateObject(k, context, results, true))
            {
                foreach (var error in results)
                {
                    ContentDialog deleteFileDialog = new ContentDialog()
                    {
                        Title = "Сообщение",
                        Content = error.ErrorMessage ,
                        PrimaryButtonText = "ОК",

                    };
                    ContentDialogResult result = await deleteFileDialog.ShowAsync();
                }
                index = 0;
            }
            else
            {

                db.SaveChanges();

                if (index != 0)
                {
                    GoToMainPage();
                    //Frame.Navigate(typeof(DashboardPage), new DrillInNavigationTransitionInfo());
                }
                //db.SaveChanges();


            }

        }



        private void GoToMainPage()
        {
            if (Frame.CanGoBack)
                Frame.GoBack();
            else
                Frame.Navigate(typeof(DashboardPage));
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
