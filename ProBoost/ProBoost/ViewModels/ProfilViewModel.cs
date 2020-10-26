using ProBoost.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ProBoost.ViewModels
{
    public class ProfilViewModel:BaseViewModel
    {
        private string _fullname;
        public string Fullname
        {
            get => _fullname;
            set
            {
                _fullname = value;
                OnPropertyChanged("Fullname");
            }
        }

        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged("Email");
            }
        }

        private string _phone;
        public string Phone
        {
            get => _phone;
            set
            {
                _phone = value;
                OnPropertyChanged("Phone");
            }
        }

        private IAuthServices _authServices;

        public Command SaveCommand { get; set; }

        public ProfilViewModel()
        {
            _authServices = DependencyService.Get<IAuthServices>();
            Email = _authServices.CurrentUser.Email;
            Fullname = _authServices.CurrentUser.Fullname;
            _authServices.UserPhone();
            IsRunning = true;
            _authServices.GetUser += _authServices_GetUser;
            SaveCommand = new Command(ExecuteSaveCommand);
        }

        //event
        private void _authServices_GetUser(object sender, EventArgs e)
        {
            IsRunning = false;
            var user = (UserEventArgs)e;
            if(user.EventType == EventType.User)
            {
                Fullname = user.User.Fullname;
                Phone = user.User.Phone;
            }
        }

        private async void ExecuteSaveCommand(object obj)
        {
            try
            {
                IsRunning = true;
                await _authServices.UpdateUser(new Models.User
                {
                    Email = Email,
                    Fullname = Fullname,
                    Phone = Phone,
                    UId = _authServices.CurrentUser.UId
                });
            }
            catch (Exception e)
            {
                await Application.Current.MainPage.DisplayAlert("Erreur", $"{e.Message}", "OK");
            }
            finally
            {
                IsRunning = false;
            }
            

        }
    }
}
