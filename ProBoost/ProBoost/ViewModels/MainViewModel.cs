﻿using ProBoost.Models;
using ProBoost.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace ProBoost.ViewModels
{
    public class MainViewModel:BaseViewModel
    {
        public ObservableCollection<Projet> Projets { get; set; }
        private IDataServices _dataServices;
        private IAuthServices _authServices;
        public Command LoadAllProjectsCommand { get; set; }
        public MainViewModel()
        {
            _dataServices = DependencyService.Get<IDataServices>();
            _authServices = DependencyService.Get<IAuthServices>();
            Projets = new ObservableCollection<Projet>();
            LoadAllProjectsCommand = new Command(ExecuteLoadAllProjectsCommand);
            LoadAllProjectsCommand.Execute(null);
            _dataServices.GetAllProjects += _dataServices_GetAllProjects;
        }

        private async void _dataServices_GetAllProjects(object sender, EventArgs e)
        {
            var data = (ProjectEventArgs)e;
            if(data.ProjectEventArgsType == ProjectEventArgsType.AllProjects)
            {
                Projets.Clear();
                data.Projets.ForEach(d =>
                {
                    _authServices.GetOneUser(d.OwnerUId);
                    _authServices.GetUser += (o, ev) =>
                      {
                          if(((UserEventArgs)ev).EventType == EventType.UserGet )
                          {
                              if(Projets.Where(p => p.ProjectId == d.ProjectId ).Count() == 0)
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

        private async void ExecuteLoadAllProjectsCommand(object obj)
        {
            IsRunning = true;
            _dataServices.GetProjets();
        }
    }
}
