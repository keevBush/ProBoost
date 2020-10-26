using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Gms.Extensions;
using Android.Gms.Tasks;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Firestore;
using Java.Lang;
using Java.Util;
using ProBoost.Droid.DependencyServices;
using ProBoost.Models;
using ProBoost.Services;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

[assembly: Dependency(typeof(DataServices))]
namespace ProBoost.Droid.DependencyServices
{
    public class DataServices : Java.Lang.Object, IDataServices, IOnSuccessListener
    {
        public event EventHandler GetAllProjects;

        public async System.Threading.Tasks.Task<Projet> GetProject(string key)
        {
            await System.Threading.Tasks.Task.Delay(500);
            var document = Firebase.FirebaseFirestore.Document($"projects/{key}").Get().Result;
            var data = (IProjet)(document.JavaCast<ProjetJava>());
            return (Projet)data;
        }

        public void GetProjets()
        {
            Firebase.FirebaseFirestore.Collection($"projects")
                    .OrderBy("date")
                    .Get(Source.Default).AddOnCompleteListener(new OnCompleteEventHandleListener(obj =>
                    {
                        if (obj.IsSuccessful)
                        {
                            var collectionSnapshot = (QuerySnapshot)obj.Result;
                            var list = new List<Projet>();
                            collectionSnapshot.Documents.ForEach(d =>
                            {
                                var categories = new List<string>();
                                var data = (Android.Runtime.JavaList)(d.Get("categories"));
                                foreach(var d1 in data)
                                {
                                    categories.Add(d1.ToString());
                                }
                                list.Add(new Projet
                                {
                                    ProjectId = d.GetString("projectId"),
                                    Date = DateTime.Parse(d.GetString("date")),
                                    Description = d.GetString("description"),
                                    Name = d.GetString("name"),
                                    OwnerUId = d.GetString("ownerUId"),
                                    Categories = categories
                                });
                            });
                            GetAllProjects.Invoke(this, new ProjectEventArgs
                            {
                                ProjectEventArgsType = ProjectEventArgsType.AllProjects,
                                Projets = list
                            });
                        }
                    }
            ));
        }

        public void GetProjetsOf(string key)
        {
            Firebase.FirebaseFirestore.Collection($"projects")
                    .OrderBy("date")
                    .Get(Source.Default).AddOnCompleteListener(new OnCompleteEventHandleListener(obj =>
                    {
                        if (obj.IsSuccessful)
                        {
                            var collectionSnapshot = (QuerySnapshot)obj.Result;
                            var list = new List<Projet>();
                            collectionSnapshot.Documents.ForEach(d =>
                            {
                                var categories = new List<string>();
                                var data = (Android.Runtime.JavaList)(d.Get("categories"));
                                foreach (var d1 in data)
                                {
                                    categories.Add(d1.ToString());
                                }
                                list.Add(new Projet
                                {
                                    ProjectId = d.GetString("projectId"),
                                    Date = DateTime.Parse(d.GetString("date")),
                                    Description = d.GetString("description"),
                                    Name = d.GetString("name"),
                                    OwnerUId = d.GetString("ownerUId"),
                                    Categories = categories
                                });
                            });
                            GetAllProjects.Invoke(this, new ProjectEventArgs
                            {
                                ProjectEventArgsType = ProjectEventArgsType.ProjectMine,
                                Projets = list.Where(p => p.OwnerUId == key).ToList()
                            });
                        }
                    }
            ));
        }

        public async System.Threading.Tasks.Task NewProject(Projet projet)
        {
            var dictionary = new JavaDictionary<string,object>();
            dictionary.Add("projectId", projet.ProjectId);
            dictionary.Add("ownerUId", projet.OwnerUId);
            dictionary.Add("name", projet.Name);
            dictionary.Add("date", projet.Date.ToString());
            dictionary.Add("description", projet.Description);
            var lst = new JavaList(projet.Categories);
            dictionary.Add("categories", lst);
            await Firebase.FirebaseFirestore.Document($"projects/{projet.ProjectId}/").Set(dictionary);
        }

        public void OnSuccess(Java.Lang.Object result)
        {
            throw new NotImplementedException();
        }

        public async System.Threading.Tasks.Task UpdateProject(string key, Projet projet)
        {
            projet.ProjectId = key;
            var data = new JavaDictionary();
            data.Add("ProjectId", projet.ProjectId);
            data.Add("OwnerUId", projet.OwnerUId);
            data.Add("Date", projet.Date.ToString());
            data.Add("Description", projet.Description);
            data.Add("Categories", new JavaList (projet.Categories));
            await Firebase.FirebaseFirestore.Document($"projects/{key}").Set((ProjetJava)((IProjet)projet));
        } 
    }
    
    public class ProjetJava : Java.Lang.Object,IJavaObject ,IProjet
    {
        public IntPtr Handle => throw new NotImplementedException();

        public string ProjectId { get ; set ; }
        public string Name { get ; set ; }
        public string Description { get ; set ; }
        public IEnumerable<string> Categories { get ; set ; }
        public DateTime Date { get ; set ; }
        public string OwnerUId { get ; set ; }

        public void Dispose()
        {
        }
    }
}