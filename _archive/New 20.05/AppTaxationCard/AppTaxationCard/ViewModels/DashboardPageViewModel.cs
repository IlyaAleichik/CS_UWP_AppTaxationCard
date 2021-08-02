using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using AppTaxationCard.Models;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Controls;
using Windows.Storage;

namespace AppTaxationCard.ViewModels
{
   public class DashboardPageViewModel : ViewModelBase
    {
        private bool _isLoading = false;
        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }
            set
            {
                _isLoading = value;
                RaisePropertyChanged("IsLoading");

            }
        }

        #region VariableData
        private int _userId;
        public int UserId
        {

            get
            {
                return _userId;
            }
            set
            {
                if (value != _userId)
                {
                    _userId = value;
                    RaisePropertyChanged("UserId");
                }
            }
        }


        private string _title;
        public string Title
        {

            get
            {
                return _title;
            }
            set
            {
                if (value != _title)
                {
                    _title = value;
                    RaisePropertyChanged("Title");
                }
            }
        }

        private string _username;
        public string Username
        {

            get
            {
                return _username;
            }
            set
            {
                if (value != _username)
                {
                    _username = value;
                    RaisePropertyChanged("Username");
                }
            }
        }

        private string _email;
        public string Email
        {

            get
            {
                return _email;
            }
            set
            {
                if (value != _email)
                {
                    _email = value;
                    RaisePropertyChanged("Email");
                }
            }
        }
        private string _name;
        public string Name
        {

            get
            {
                return _name;
            }
            set
            {
                if (value != _name)
                {
                    _name = value;
                    RaisePropertyChanged("Name");
                }
            }
        }
        private string _surname;
        public string Surname
        {

            get
            {
                return _surname;
            }
            set
            {
                if (value != _surname)
                {
                    _surname = value;
                    RaisePropertyChanged("Surname");
                }
            }
        }

        private string _phone;
        public string Phone
        {

            get
            {
                return _phone;
            }
            set
            {
                if (value != _phone)
                {
                    _phone = value;
                    RaisePropertyChanged("Phone");
                }
            }
        }

        private DateTime _time;
        public DateTime Time
        {

            get
            {
                return _time;
            }
            set
            {
                if (value != _time)
                {
                    _time = value;
                    RaisePropertyChanged("Time");
                }
            }
        }

        private int _countKv;
        public int CountKv
        {

            get
            {
                return _countKv;
            }
            set
            {
                if (value != _countKv)
                {
                    _countKv = value;
                    RaisePropertyChanged("CountKv");
                }
            }
        }


        private int _countVd;
        public int CountVd
        {

            get
            {
                return _countVd;
            }
            set
            {
                if (value != _countVd)
                {
                    _countVd = value;
                    RaisePropertyChanged("CountVd");
                }
            }
        }

        private int _countArea;
        public int CountArea
        {

            get
            {
                return _countArea;
            }
            set
            {
                if (value != _countArea)
                {
                    _countArea = value;
                    RaisePropertyChanged("CountArea");
                }
            }
        }

        private ImageSource _userImage;
        public ImageSource UserImage
        {

            get
            {
                return _userImage;
            }
            set
            {
                if (value != _userImage)
                {
                    _userImage = value;
                    RaisePropertyChanged("CountKv");
                }
            }
        }


        #endregion

   
        public  DashboardPageViewModel()
        {
            GetUserDataDefaultAsync();
            GetStatusDataCount();

        }


       
        public void GetStatusDataCount()
        {
            using (ModelContext db = new ModelContext())
            {
                SessionUser session = db.SessionUsers.LastOrDefault();
                UserId = session.SId;               
                CountKv = db.Kvartals.Where(d => d.CreateAcc == UserId).Count();
                CountVd = db.Videls.Where(d => d.AccountId == UserId).Count();
                CountArea = db.Videls.Where(d => d.AccountId == UserId).Sum(d => d.Area);


            }
        }
        
        public void GetUserDataDefaultAsync()
        {
            using (ModelContext db = new ModelContext())
            { 
                 
                SessionUser session = db.SessionUsers.LastOrDefault();
                UserId = session.SId;
                Account currentUser = db.Accounts.FirstOrDefault(d => d.Id == UserId);
                if (currentUser.ProfileImage != null)
                {
          
                    UserImage = new BitmapImage(new Uri(currentUser.ProfileImage));
                }
                else UserImage = null;
                Username = currentUser.Username;
                Name = currentUser.Name;
                Surname = currentUser.Surname;
                Email = currentUser.Email;
                Phone = currentUser.Phone;
                Time = session.SessionTime;
            }
        }
    }
}
