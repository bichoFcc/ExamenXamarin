using Movies.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Movies
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new MoviesPage());
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
