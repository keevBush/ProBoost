using ProBoost.Models;
using ProBoost.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ProBoost.ViewModels
{
    internal class ProjetViewModel:BaseViewModel
    {
        private IDataServices dataServices;
        private IAuthServices authServices;
        
        private string _title = "Mes projets";
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged("Title");
            }
        }

        private string _phone = "---";
        public string Phone
        {
            get => _phone;
            set
            {
                _phone = value;
                OnPropertyChanged("Phone");
            }
        }

        private string _email = "---";
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged("Email");
            }
        }

        private string _name = "---";
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        public ObservableCollection<Projet> Projets { get; set; }
        
        public Command LoadAllDataCommand { get; set; }

        public ProjetViewModel(Projet projet)
        {
            dataServices = DependencyService.Get<IDataServices>();
            authServices = DependencyService.Get<IAuthServices>();
            LoadAllDataCommand = new Command(ExecuteLoadAllDataCommand);
            Projets = new ObservableCollection<Projet>();
            authServices.GetUser += AuthServices_GetUser;
            IsRunning = true;
            Title = "Projet";
            Projets.Add(projet);
            LoadAllDataCommand.Execute(null);
        }

        private async void AuthServices_GetUser(object sender, EventArgs e)
        {
            try
            {
                var data = (UserEventArgs) e;
                if(data.EventType == EventType.UserGet)
                {
                    Email = data.User.Email;
                    Phone = data.User.Phone;
                    Name = data.User.Fullname;
                }
                await Task.Delay(1200);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erreur",ex.Message, "OK");
            }
            finally{
                IsRunning = false;
            }
        }

        private void ExecuteLoadAllDataCommand(object obj)
        {
            IsRunning = true;
            authServices.GetOneUser(Projets[0].OwnerUId);
        }
    }
}