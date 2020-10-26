using ProBoost.Models;
using ProBoost.Services;
using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using System.Threading.Tasks;
using System.Linq;

namespace ProBoost.ViewModels
{
    public class MyProjectsViewModel:BaseViewModel
    {
        private string _key;
        public string Key
        {
            get => _key;
            set
            {
                _key = value;
                OnPropertyChanged("Key");
            }
        }

        public ObservableCollection<Projet> Projets { get; set; }
        private IDataServices _dataServices;
        private IAuthServices _authServices;
        public Command LoadAllProjectsCommand { get; set; }

        public MyProjectsViewModel(string key)
        {
            Key = key;

            _dataServices = DependencyService.Get<IDataServices>();
            _authServices = DependencyService.Get<IAuthServices>();
            Projets = new ObservableCollection<Projet>();
            LoadAllProjectsCommand = new Command(ExecuteLoadAllProjectsCommand);
            LoadAllProjectsCommand.Execute(null);
            _dataServices.GetAllProjects += _dataServices_GetAllProjects; ;
        }

        private async void _dataServices_GetAllProjects(object sender, EventArgs e)
        {
            var data = (ProjectEventArgs)e;
            if (data.ProjectEventArgsType == ProjectEventArgsType.ProjectMine)
            {
                Projets.Clear();
                data.Projets.ForEach(d =>
                {
                    _authServices.GetOneUser(d.OwnerUId);
                    _authServices.GetUser += (o, ev) =>
                    {
                        if (((UserEventArgs)ev).EventType == EventType.UserGet)
                        {
                            if (Projets.Where(p => p.ProjectId == d.ProjectId).Count() == 0)
                                Projets.Add(new Projet
                                {
                                    Categories = d.Categories,
                                    ProjectId = d.ProjectId,
                                    Date = d.Date,
                                    Description = d.Description,
                                    Name = d.Name,
                                    OwnerName = ((UserEventArgs)ev).User.Fullname,
                                    OwnerUId = d.OwnerUId
                                });
                        }
                    };
                });
                await Task.Delay(1200);
                IsRunning = false;
            }
        }

        private void ExecuteLoadAllProjectsCommand(object obj)
        {
            IsRunning = true;
            _dataServices.GetProjetsOf(Key);
        }
    }
}