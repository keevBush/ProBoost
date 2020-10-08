using ProBoost.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProBoost
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new Splashscreen();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
