using Movies.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Movies.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetailMoviePage : ContentPage
    {
        public DetailMoviePage(string id)
        {
            InitializeComponent();
            BindingContext = new DetailMovieViewModel(Navigation, id);
        }
    }
}