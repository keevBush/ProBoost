using ProBoost.Models;
using ProBoost.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProBoost.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Splashscreen : ContentPage
    {
        public Splashscreen()
        {
            InitializeComponent();
            CheckUser();
        }

        private async void CheckUser()
        {
            try
            {
                loading.IsRunning = true;
                await Task.Delay(2000);
                User currentUser = DependencyService.Get<IAuthServices>().CurrentUser;
                if (currentUser.UId != null)
                {
                    Application.Current.MainPage = new NavigationPage(new MainPage());
                }
                else
                {
                    Application.Current.MainPage = new NavigationPage(new LoginView());
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                loading.IsRunning = false;
            }
        }
    }
}