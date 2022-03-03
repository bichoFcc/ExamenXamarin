using Movies.Models;
using Movies.Resources;
using Movies.Services;
using Movies.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Movies.ViewModels
{
    public class MoviesViewModel : ViewModelBase
    {
        #region Attributes
        private string _question;
        private string _search;
        private string _recommended;
        private string _nextReleases;
        private string _popular;
        private string _seeall;
        private string _searchText;

        private List<Result> _recommendedMovies;
        private List<Result> _recommendedtmpMovies;
        private List<Result> _premieresMovies;
        private List<Result> _premierestmpMovies;
        private List<Result> _popularMovies;
        private List<Result> _populartmpMovies;
        #endregion

        #region Services
        ApiServices apiServices = new ApiServices();
        #endregion

        #region Constructor
        public MoviesViewModel(INavigation navigation)
        {
            Navigation = navigation;

            Question = Resources_Esp.questionMoviesPage;
            Search = Resources_Esp.searchMoviesPage;
            Recommended = Resources_Esp.recommendedMoviesPage;
            NextReleases = Resources_Esp.premieresMoviesPage;
            Popular = Resources_Esp.popularMoviesPage;
            SeeAll = Resources_Esp.seeallMoviesPage;

            ListTopMovies();
            ListUpcCommingMovies();
            PopularsMovies();
        }
        #endregion

        #region Properties
        public string Question
        {
            get => _question;
            set
            {
                _question = value;
                OnpropertyChanged();
            }
        }

        public string Search
        {
            get => _search;
            set
            {
                _search = value;
                OnpropertyChanged();
            }
        }

        public string Recommended
        {
            get => _recommended;
            set
            {
                _recommended = value;
                OnpropertyChanged();
            }
        }

        public string NextReleases
        {
            get => _nextReleases;
            set
            {
                _nextReleases = value;
                OnpropertyChanged();
            }
        }

        public string Popular
        {
            get => _popular;
            set
            {
                _popular = value;
                OnpropertyChanged();
            }
        }

        public string SeeAll
        {
            get => _seeall;
            set
            {
                _seeall = value;
                OnpropertyChanged();
            }
        }

        public string SearchText
        {
            get => _searchText; 
            set
            {
                _searchText = value;
                SearchUserText(_searchText);
                OnpropertyChanged();
            }
        }

        public List<Result> RecommendedMovies
        {
            get => _recommendedMovies;
            set
            {
                _recommendedMovies = value;
                OnPropertyChanged();
            }
        }

        public List<Result> PremieresMovies
        {
            get => _premieresMovies;
            set
            {
                _premieresMovies = value;
                OnPropertyChanged();
            }
        }

        public List<Result> PopularMovies
        {
            get => _popularMovies;
            set
            {
                _popularMovies = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Methods
        //Load list of top rate movies
        private async void ListTopMovies()
        {
            try
            {
                List<Result> list = new List<Result>();
                Response response = await apiServices.Get<MovieModel>(Routes.URL, Routes.endpointtopMovies);
                if (!response.IsSuccess)
                {
                    await App.Current.MainPage.DisplayAlert(Resources_Esp.headerAlert, response.Message, Resources_Esp.footerAlert);
                    return;
                }

                MovieModel responseMovies = (MovieModel)response.Result;

                foreach (var item in responseMovies.results)
                {
                    double calification = (item.vote_average * 5) / 10;
                    item.vote_average = Convert.ToInt32(Math.Round(Convert.ToDouble(calification), 0, MidpointRounding.AwayFromZero));
                }

                if (responseMovies.results.Count > 10)
                    list = responseMovies.results.GetRange(0, 10);
                else
                    list = responseMovies.results;

                RecommendedMovies = list;
                _recommendedtmpMovies = list;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                await App.Current.MainPage.DisplayAlert(Resources_Esp.headerAlert, Resources_Esp.listmoviestopAlert, Resources_Esp.footerAlert);
            }
        }

        //Load list of upcoming releases
        private async void ListUpcCommingMovies()
        {
            try
            {
                List<Result> list = new List<Result>();
                Response response = await apiServices.Get<MovieModel>(Routes.URL, Routes.endpointupcommingMovies);
                if (!response.IsSuccess)
                {
                    await App.Current.MainPage.DisplayAlert(Resources_Esp.headerAlert, response.Message, Resources_Esp.footerAlert);
                    return;
                }

                MovieModel responseMovies = (MovieModel)response.Result;

                foreach (var item in responseMovies.results)
                {
                    double calification = (item.vote_average * 5) / 10;
                    item.vote_average = Convert.ToInt32(Math.Round(Convert.ToDouble(calification), 0, MidpointRounding.AwayFromZero));
                }

                if (responseMovies.results.Count > 10)
                    list = responseMovies.results.GetRange(0, 10);
                else
                    list = responseMovies.results;

                PremieresMovies = list;
                _premierestmpMovies = list;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                await App.Current.MainPage.DisplayAlert(Resources_Esp.headerAlert, Resources_Esp.listmoviesupcomingAlert, Resources_Esp.footerAlert);
            }
        }

        //Load list of popular movies
        private async void PopularsMovies()
        {
            try
            {
                List<Result> list = new List<Result>();
                Response response = await apiServices.Get<MovieModel>(Routes.URL, Routes.endpointpopularMovies);
                if (!response.IsSuccess)
                {
                    await App.Current.MainPage.DisplayAlert(Resources_Esp.headerAlert, response.Message, Resources_Esp.footerAlert);
                    return;
                }

                MovieModel responseMovies = (MovieModel)response.Result;

                foreach (var item in responseMovies.results)
                {
                    double calification = (item.vote_average * 5) / 10;
                    item.vote_average = Convert.ToInt32(Math.Round(Convert.ToDouble(calification), 0, MidpointRounding.AwayFromZero));
                }

                if (responseMovies.results.Count > 10)
                    list = responseMovies.results.GetRange(0, 10);
                else
                    list = responseMovies.results;

                PopularMovies = list;
                _populartmpMovies = list;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                await App.Current.MainPage.DisplayAlert(Resources_Esp.headerAlert, Resources_Esp.listmoviespopularAlert, Resources_Esp.footerAlert);
            }
        }

        //Look for matches in the lists
        public async void SearchUserText(string text)
        {
            try
            {
                if (string.IsNullOrEmpty(text))
                {
                    RecommendedMovies = _recommendedtmpMovies;
                    PremieresMovies = _premierestmpMovies;
                    PopularMovies = _populartmpMovies;
                }
                else
                {
                    if (text.Count() > 3)
                    {
                        List<Result> newList = _recommendedMovies.Where(x => x.title.ToLower().Contains(text.ToLower())).ToList();
                        RecommendedMovies = newList;

                        List<Result> newListP = _premieresMovies.Where(x => x.title.ToLower().Contains(text.ToLower())).ToList();
                        PremieresMovies = newListP;

                        List<Result> newListPo = _popularMovies.Where(x => x.title.ToLower().Contains(text.ToLower())).ToList();
                        PopularMovies = newListPo;
                    }
                    else
                    {
                        if (_recommendedtmpMovies.Count() != RecommendedMovies.Count())
                            RecommendedMovies = _recommendedtmpMovies;

                        if (_premierestmpMovies.Count() != PremieresMovies.Count())
                            PremieresMovies = _premierestmpMovies;

                        if (_populartmpMovies.Count() != PopularMovies.Count())
                            PopularMovies = _populartmpMovies;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                await App.Current.MainPage.DisplayAlert(Resources_Esp.headerAlert, Resources_Esp.errorfilterAlert, Resources_Esp.footerAlert);
            }
        }

        //Go to detail screen
        public async Task SendDetailMovie(Result movie)
        {
            try
            {
                await Navigation.PushAsync(new DetailMoviePage(movie.id.ToString()));
            }
            catch (Exception ex)
            {
                await DisplayAlert(Resources_Esp.headerAlert, ex.Message, Resources_Esp.footerAlert);
            }
        }
        #endregion

        #region Commands
        //Command to select a movie
        public ICommand SelectedMovie => new Command<Result>(async (p) => await SendDetailMovie(p));
        #endregion
    }
}
