using Movies.Models;
using Movies.Resources;
using Movies.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Movies.ViewModels
{
    public class DetailMovieViewModel : ViewModelBase
    {
        #region Attributes
        private string _id;
        private string _photoMovie;
        private string _titleMovie;
        private string _watchNow;
        private int _vote;
        private string _descriptionMovie;
        private string _studio;
        private string _typeStudio;
        private string _category;
        private string _categories;
        private string _release;
        private string _dateRelease;

        private List<Crew> _listCredits;
        #endregion

        #region Services
        ApiServices apiServices = new ApiServices();
        #endregion

        #region Constructor
        public DetailMovieViewModel(INavigation navigation, string id)
        {
            Navigation = navigation;

            _id = id;
            FillDetailMovie(_id);
        }
        #endregion

        #region Properties
        public string PhotoMovie
        {
            get => _photoMovie;
            set
            {
                _photoMovie = value;
                OnPropertyChanged();
            }
        }

        public string TitleMovie
        {
            get => _titleMovie;
            set
            {
                _titleMovie = value;
                OnpropertyChanged();
            }
        }

        public string WatchNow
        {
            get => _watchNow;
            set
            {
                _watchNow = value;
                OnpropertyChanged();
            }
        }

        public int Vote
        {
            get => _vote;
            set
            {
                _vote = value;
                OnpropertyChanged();
            }
        }

        public string DescriptionMovie
        {
            get => _descriptionMovie;
            set
            {
                _descriptionMovie = value;
                OnpropertyChanged();
            }
        }

        public List<Crew> ListCredits
        {
            get => _listCredits;
            set
            {
                _listCredits = value;
                OnPropertyChanged();
            }
        }

        public string Studio
        {
            get => _studio;
            set
            {
                _studio = value;
                OnpropertyChanged();
            }
        }

        public string TypeStudio
        {
            get => _typeStudio;
            set
            {
                _typeStudio = value;
                OnpropertyChanged();
            }
        }

        public string Category
        {
            get => _category;
            set
            {
                _category = value;
                OnpropertyChanged();
            }
        }

        public string Categories
        {
            get => _categories;
            set
            {
                _categories = value;
                OnpropertyChanged();
            }
        }

        public string Release
        {
            get => _release;
            set
            {
                _release = value;
                OnpropertyChanged();
            }
        }

        public string DateRelease
        {
            get => _dateRelease;
            set
            {
                _dateRelease = value;
                OnpropertyChanged();
            }
        }
        #endregion

        #region Methods
        //Load the details of the selected movie
        private async void FillDetailMovie(string id)
        {
            try
            {
                Response response = await apiServices.Get<DetailMovieModel>(Routes.URL + id, Routes.endpointdetailMovies);
                if (!response.IsSuccess)
                {
                    await App.Current.MainPage.DisplayAlert(Resources_Esp.headerAlert, response.Message, Resources_Esp.footerAlert);
                    return;
                }

                DetailMovieModel responseDetail = (DetailMovieModel)response.Result;
                PhotoMovie = responseDetail.poster_path;
                TitleMovie = responseDetail.title;
                WatchNow = Resources_Esp.watchnowDetatailMoviePage;

                double calification = (responseDetail.vote_average * 5) / 10;
                Vote = Convert.ToInt32(Math.Round(Convert.ToDouble(calification), 0, MidpointRounding.AwayFromZero));

                DescriptionMovie = responseDetail.overview;

                await PaintListCredits(id);

                Studio = Resources_Esp.studioDetatailMoviePage;

                if (responseDetail.production_companies.Any())
                    TypeStudio = responseDetail.production_companies.First().name;
                else
                    TypeStudio = Resources_Esp.nodataDetatailMoviePage;

                Category = Resources_Esp.categoryDetatailMoviePage;

                if (responseDetail.genres.Any())
                {
                    int i;
                    for (i = 0; i < responseDetail.genres.Count; i++)
                    {
                        if (i == responseDetail.genres.Count - 1)
                            Categories += responseDetail.genres[i].name + ".";
                        else
                            Categories += responseDetail.genres[i].name + ", ";
                    }
                }
                else
                {
                    Categories = Resources_Esp.nodataDetatailMoviePage;
                }

                Release = Resources_Esp.releaseDetatailMoviePage;
                var dateRelease = Convert.ToDateTime(responseDetail.release_date);
                DateRelease = dateRelease.ToString("yyyy");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                await App.Current.MainPage.DisplayAlert(Resources_Esp.headerAlert, Resources_Esp.detailmovieAlert, Resources_Esp.footerAlert);
            }
        }

        //Load the list of credits
        private async Task PaintListCredits(string id)
        {
            try
            {
                List<Crew> list = new List<Crew>();
                Response response = await apiServices.Get<CreditsModel>(Routes.URL + id + "/", Routes.endpointcreditsMovies);
                if (!response.IsSuccess)
                {
                    await App.Current.MainPage.DisplayAlert(Resources_Esp.headerAlert, response.Message, Resources_Esp.footerAlert);
                    return;
                }

                CreditsModel responseDetail = (CreditsModel)response.Result;
                if (responseDetail.crew.Count > 10)
                    list = responseDetail.crew.GetRange(0, 10);
                else
                    list = responseDetail.crew;

                foreach (var item in list)
                {
                    string[] words = item.name.Split(' ');
                    item.name = "";
                    for (var i = 0; i < words.Length; i++)
                        if (i < 2)
                            item.name += words[i] + " \n";
                }
                ListCredits = list;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                await App.Current.MainPage.DisplayAlert(Resources_Esp.headerAlert, Resources_Esp.loadcreditsAlert, Resources_Esp.footerAlert);
            }
        }

        //Return to the page of all the movies
        public async Task ReturnPage()
        {
            await Navigation.PopAsync();
        }
        #endregion

        #region Commands
        //Command to select back button
        public ICommand Return => new Command(async () => await ReturnPage());
        #endregion
    }
}
