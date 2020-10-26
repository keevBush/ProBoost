using ProBoost.Helpers;
using ProBoost.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ProBoost.ViewModels
{
    public class RegisterViewModel:BaseViewModel
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
        
        private string _passwordConfirm;
        public string PasswordConfirm
        {
            get => _passwordConfirm;
            set
            {
                _passwordConfirm = value;
                OnPropertyChanged("PasswordConfirm");
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

        public Command RegisterCommand { get; set; }

        public RegisterViewModel()
        {
            RegisterCommand = new Command(ExecuteRegisterCommand);
        }

        private async void ExecuteRegisterCommand(object obj)
        {
            try
            {
                Validation.IsRquired("Nom complet", Fullname); Validation.IsRquired("Email", Email);
                Validation.IsRquired("Mot de passe", Password);Validation.IsRquired("Confirmation de mot de passe", PasswordConfirm);
                if (PasswordConfirm != Password)
                    throw new ApplicationException("Mot de passe saisie different");
                IsRunning = true;
                await DependencyService.Get<IAuthServices>().Register(Email.Trim(), Fullname.Trim(), Password);
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
