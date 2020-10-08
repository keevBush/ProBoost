using ProBoost.Helpers;
using ProBoost.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ProBoost.ViewModels
{
    public class LoginViewModel: BaseViewModel
    {
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


        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged("Password");
            }
        }

        public Command LoginCommand { get; set; }

        public LoginViewModel()
        {
            LoginCommand = new Command(ExecuteLoginCommand);
        }

        private async void ExecuteLoginCommand(object obj)
        {
            try
            {

                Validation.IsRquired("Email", Email);
                Validation.IsRquired("Mot de passe", Password);
                IsRunning = true;
                await DependencyService.Get<IAuthServices>().Login(Email, Password);
                IsRunning = false;
                Application.Current.MainPage = new NavigationPage(new MainPage());
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
