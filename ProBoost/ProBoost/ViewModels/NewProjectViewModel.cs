using ProBoost.Helpers;
using ProBoost.Models;
using ProBoost.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace ProBoost.ViewModels
{
    public class NewProjectViewModel:BaseViewModel
    {
        private string _name;
        public string Name 
        { 
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        private string _currentCategory;
        public string CurrentCategory
        {
            get => _currentCategory;
            set
            {
                _currentCategory = value;
                OnPropertyChanged("CurrentCategory");
            }
        }
        public ObservableCollection<string>Categories { get; set; }
        private string _description;
        public string Description 
        { 
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged("Description");
            }
        }
        IDataServices _dataServices;
        IAuthServices _authServices;
        public Command SaveCommand { get; set; }
        public Command AddCategoryCommand { get; set; }
        public Command DeteteCategoryCommand { get; set; }

        public NewProjectViewModel()
        {
            Categories = new ObservableCollection<string>();

            SaveCommand = new Command(ExecuteSaveCommand);
            AddCategoryCommand = new Command(ExecuteAddCategoryCommand);
            DeteteCategoryCommand = new Command(ExecuteDeteteCategoryCommand);
            _authServices = DependencyService.Get<IAuthServices>();
            _dataServices = DependencyService.Get<IDataServices>();
        }

        private void ExecuteDeteteCategoryCommand(object obj)
        {
            var category = (string)obj;
            Categories.Remove(category);
        }

        private async void ExecuteAddCategoryCommand(object obj)
        {
            try
            {
                Validation.IsRquired("Categorie", CurrentCategory);
                if (Categories.Contains(CurrentCategory))
                    throw new ApplicationException("Categorie déjà existante");
                Categories.Add(CurrentCategory);
                CurrentCategory = "";
            }
            catch (Exception e)
            {
                await Application.Current.MainPage.DisplayAlert("Erreur", $"{e.Message}", "OK");
            }

        }

        private async void ExecuteSaveCommand(object obj)
        {
            try
            {
                IsRunning = true;
                Validation.IsRquired("Name", Name);
                var data = new Projet
                {
                    Categories = Categories,
                    Date = DateTime.Now,
                    Description = Description,
                    Name = Name,
                    ProjectId = Guid.NewGuid().ToString(),
                    OwnerUId = _authServices.CurrentUser.UId
                };
                await _dataServices.NewProject(data);
                await Application.Current.MainPage.DisplayAlert("Confirmation", "Projet enregistré avec succes", "OK");
        }
            catch (Exception e)
            {
                await Application.Current.MainPage.DisplayAlert("Erreur", $"{e.Message}", "OK");
    }
            finally {
                IsRunning = false;

            }
        }
    }
}
