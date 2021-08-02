using AppTaxationCard.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace AppTaxationCard.Views
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    /// 
    public sealed partial class EditUserPage : Page
    {
        StorageFile file;
        StorageFolder localFolder = ApplicationData.Current.LocalFolder;
        Account account;
        public EditUserPage()
        {
            this.InitializeComponent();
            this.Loaded += Page_Loaded;
        }

        #region System
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            this.textBlock.Text = localFolder.Path;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
            {
                int id = (int)e.Parameter;
                using (ModelContext db = new ModelContext())
                {
                    account = db.Accounts.FirstOrDefault(d => d.Id == id);
                }
            }
            if (account != null)
            {

                nameBox.Text = account.Name;
                surnameBox.Text = account.Surname;
                emailBox.Text = account.Email;
                phoneBox.Text = account.Phone;
                try
                {
                    PrifileImage.ProfilePicture = new BitmapImage(new Uri(account.ProfileImage));
                }
                catch { }


                passwordBox.IsEnabled = true;
            }
        }
        #endregion

        #region Actions
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ImageProfilePicker();
        }
        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            using (ModelContext db = new ModelContext())
            {
                SessionUser session = db.SessionUsers.LastOrDefault();
                Account currentUser = db.Accounts.FirstOrDefault(d => d.Id == session.SId);
                if (currentUser != null)
                {
                    currentUser.Name = nameBox.Text;
                    currentUser.Surname = surnameBox.Text;
                    currentUser.Phone = phoneBox.Text;
                    currentUser.Email = emailBox.Text;
                    currentUser.Password = passwordBox.Password;
                    if (this.file != null)
                    {
                        currentUser.ProfileImage = "ms-appdata:///local/" + file.Name;
                    }
                    else currentUser.ProfileImage = null;
                }
                db.Accounts.Update(currentUser);
                await db.SaveChangesAsync();
                Frame.Navigate(typeof(DashboardPage));
            }
        }
        #endregion

        #region Functions
        private async void ImageProfilePicker()
        {
            var picker = new FileOpenPicker();
            picker.ViewMode = PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");
            file = await picker.PickSingleFileAsync();

            using (ModelContext db = new ModelContext())
            {
                SessionUser session = db.SessionUsers.LastOrDefault();
                Account currentUser = db.Accounts.FirstOrDefault(d => d.Id == session.SId);
                string strNameFile = "ms-appdata:///local/" + file.Name;
                if (currentUser.ProfileImage != strNameFile)
                {
                    if (file != null)
                    {
                        //Insert and save image in database

                        try
                        {
                            await file.CopyAsync(localFolder);
                        }
                        catch { }

                        // Application now has read/write access to the picked file
                        this.textBlock.Text = "Выбрано изображение: " + file.DisplayName;
                    }
                    else
                    {
                        this.textBlock.Text = "Операция завершена.";
                    }
                }

            }
        }
        private async Task<byte[]> ConvertImageToByte(StorageFile file)
        {
            using (var inputStream = await file.OpenSequentialReadAsync())
            {
                var readStream = inputStream.AsStreamForRead();

                var byteArray = new byte[readStream.Length];
                await readStream.ReadAsync(byteArray, 0, byteArray.Length);
                return byteArray;
            }

        }
        #endregion
    }
}
