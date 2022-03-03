using System;
using System.Collections.Generic;
using System.Text;

namespace Movies.Resources
{
    public class Routes
    {
        #region ApiKey
        public static string URL = "https://api.themoviedb.org/3/movie/";
        public static string apiKey = "cb6bfa43b9538e5cb792cdb14ba59b34";
        #endregion

        #region Routes
        public static string endpointtopMovies = "top_rated?api_key=" + apiKey + "&language=en-US&page=1";
        public static string endpointupcommingMovies = "upcoming?api_key=" + apiKey + "&language=en-US&page=1";
        public static string endpointpopularMovies = "popular?api_key=" + apiKey + "&language=en-US&page=1";
        public static string endpointdetailMovies = "?api_key=" + apiKey + "&language=en-US";
        public static string endpointcreditsMovies = "credits?api_key=" + apiKey + "&language=en-US";
        #endregion
    }
}
