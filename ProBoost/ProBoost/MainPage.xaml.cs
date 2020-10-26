using ProBoost.Models;
using ProBoost.Services;
using ProBoost.ViewModels;
using ProBoost.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ProBoost
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            LoadData();
        }

        private async void LoadData()
        {
            await Task.Delay(500);
        }

        private void logout_Clicked(object sender, EventArgs e)
        {
            DependencyService.Get<IAuthServices>().Logout();
            Application.Current.MainPage = new Splashscreen();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new NavigationPage (new NewProject()));
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Profil());
        }


        private async void ToolbarItem_Clicked_1(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MyProjects(DependencyService.Get<IAuthServices>().CurrentUser.UId));
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var projet = (Projet)(((Grid)sender).BindingContext) ;
            await Navigation.PushAsync(new ProjetView(projet));
        }
    }
}
