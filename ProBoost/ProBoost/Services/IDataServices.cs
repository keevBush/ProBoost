using ProBoost.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProBoost.Services
{
    public interface IDataServices
    {
        event EventHandler GetAllProjects;
        Task NewProject(Projet projet);
        Task<Projet> GetProject(string key);
        Task UpdateProject(string key, Projet projet);
        void GetProjets();
        void GetProjetsOf(string owner);
    }

    public enum ProjectEventArgsType
    {
        AllProjects,ProjectMine,OneProject
    }

    public class ProjectEventArgs : EventArgs
    {
        public IEnumerable<Projet> Projets { get; set; }
        public IEnumerable<Projet> ProjetsMine { get; set; }
        public Projet Projet { get; set; }
        public ProjectEventArgsType ProjectEventArgsType { get; set; }
    }
}
