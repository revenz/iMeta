using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text.RegularExpressions;
using System.Web;
using System.Text;

namespace TheMovieDb
{
    public class TmdbApi
    {
        public delegate void MovieSearchAsyncCompletedEventHandler(object sender, TmdbMovieSearchCompletedEventArgs e);
        public delegate void MovieInfoAsyncCompletedEventHandler(object sender, TmdbMovieInfoCompletedEventArgs e);
        public delegate void PersonSearchAsyncCompletedEventHandler(object sender, TmdbPersonSearchCompletedEventArgs e);
        public delegate void PersonInfoAsyncCompletedEventHandler(object sender, TmdbPersonInfoCompletedEventArgs e);
        public delegate void GenresAsyncCompletedEventHandler(object sender, TmdbGenresCompletedEventArgs e);

        public event MovieSearchAsyncCompletedEventHandler MovieSearchCompleted;
        public event MovieSearchAsyncCompletedEventHandler MovieBrowseCompleted;
        public event MovieSearchAsyncCompletedEventHandler MovieSearchByImdbCompleted;

        public event MovieInfoAsyncCompletedEventHandler GetMovieInfoCompleted;
        public event MovieInfoAsyncCompletedEventHandler GetMovieImagesCompleted;

        public event PersonSearchAsyncCompletedEventHandler PersonSearchCompleted;
        public event PersonInfoAsyncCompletedEventHandler GetPersonInfoCompleted;

        public event GenresAsyncCompletedEventHandler GetGenresCompleted;

        private const string MovieGetInfoUrl = "Movie.getInfo/{2}/json/{0}/{1}";
        private const string MovieImdbLookupUrl = "Movie.imdbLookup/{2}/json/{0}/{1}";
        private const string MovieSearchUrl = "Movie.search/{2}/json/{0}/{1}";
        private const string MovieGetImagesUrl = "Movie.getImages/{2}/json/{0}/{1}";
        private const string MovieBrowseUrl = "Movie.browse/{1}/json/{0}?{2}";

        private const string PersonSearchUrl = "Person.search/{2}/json/{0}/{1}";
        private const string PersonGetInfoUrl = "Person.getInfo/{2}/json/{0}/{1}";

        private const string GenresGetListUrl = "Genres.getList/{0}/json/{1}";

        private readonly WebClient _client;

        public string ApiKey { get; set; }
        public string Language { get; set; }

        /// <summary>
        /// TmdbApi constructor
        /// </summary>
        /// <param name="apiKey">Api Key</param>
        public TmdbApi(string apiKey) : this(apiKey, "en-US")
        {
        }

        public TmdbApi(string apiKey, string language)
        {
            ApiKey = apiKey;
            Language = language;
            _client = new WebClient {BaseAddress = "http://api.themoviedb.org/2.1/"};
            //_client.OpenReadCompleted += ClientOpenReadCompleted;
        }

        /// <summary>
        /// The Movie.browse method is probably the most powerful single method on the entire TMDb API. 
        /// While it might not be used by all apps, it is a great place to start if you're interested in 
        /// building any kind of a top 10 list.
        /// Some examples include getting a list of the top 'drama' movies, or maybe the top science 
        /// fiction movies released since 2000. These are fairly simple examples as you can also add 
        /// any number of extra attributes to your search. These are all passed as URL query parameters 
        /// and are outlined below.
        /// </summary>
        /// <param name="orderBy"></param>
        /// <param name="order"></param>
        /// <param name="perPage">This value sets the number of results to display per page (or request). Without it, the default is 30 results. 'page' is required if you're using per_page.</param>
        /// <param name="page">Results are paginated if you use the page & per_page parameters. You can quickly scan through multiple pages with these options.</param>
        /// <param name="query">The search query parameter is used to search for some specific text from a title</param>
        /// <param name="minVotes">Only return movies with a certain minimum number of votes.</param>
        /// <param name="ratingMin">If you'd only like to see movies with a certain minimum rating, use this. If used, ratingMax is required.</param>
        /// <param name="ratingMax">Used in conjunction with ratingMin. Sets the upper limit of movies to return based on their rating.</param>
        /// <param name="genres">The genres parameter is to be passed the genres id(s) you want to search for. You can get these ids from the Genres.getList method.</param>
        /// <param name="genresSelector">Used when you search for more than 1 genre and useful to combine your genre searches.</param>
        /// <param name="releaseMin">Useful if you'd like to only search for movies from a particular date and on. If used, releaseMax is required.</param>
        /// <param name="releaseMax">Sets the upper date limit to search for. If used, releaseMin is required.</param>
        /// <param name="year">If you'd only like to search for movies from a particular year, this if your option.</param>
        /// <param name="certifications">The values to be used here are the MPAA values like 'R' or 'PG-13'. When more than one value is passed, it is assumed to be an OR search.</param>
        /// <param name="companies">Useful if you'd like to find the movies from a particular studio. When more than one id is passed, it is assumed to be an OR search.</param>
        /// <param name="countries">If you'd like to limit your result set to movies from a particular country you can pass their 2 letter country code. When more than one id is passed, it is assumed to be an OR search.</param>
        /// <returns></returns>
        public IEnumerable<TmdbMovie> MovieBrowse(TmdbOrderBy orderBy, TmdbOrder order, int perPage = 0, int page = 0, string query = ""
            , int minVotes = 0, float ratingMin = 0, float ratingMax = 0, IEnumerable<int> genres = null, GenresSelector? genresSelector = null,
            DateTime? releaseMin = null, DateTime? releaseMax = null, int year = 0, IEnumerable<string> certifications = null
            , IEnumerable<string> companies = null, IEnumerable<string> countries = null)
        {
            var nvc = new NameValueCollection();
            nvc["order_by"] = orderBy.ToString().ToLower();
            nvc["order"] = order.ToString().ToLower();

            if(perPage > 0 && page > 0)
                nvc["per_page"] = perPage.ToString();
            if(page > 0)
                nvc["page"] = page.ToString();

            if(!string.IsNullOrEmpty(query))
                nvc["query"] = query;

            if (minVotes > 0)
                nvc["min_votes"] = minVotes.ToString();

            if (ratingMin > 0 && ratingMax > 0)
            {
                nvc["rating_min"] = ratingMin.ToString();
                nvc["rating_max"] = ratingMax.ToString();
            }

            if(genres != null)
                nvc["genres"] = string.Join(",",genres);

            if (genresSelector.HasValue)
                nvc["genres_selector"] = genresSelector.ToString().ToLower();

            if(releaseMax.HasValue && releaseMin.HasValue)
            {
                var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                nvc["release_min"] = releaseMin.Value.AddTicks(-epoch.Ticks).Ticks.ToString();
                nvc["release_max"] = releaseMax.Value.AddTicks(-epoch.Ticks).Ticks.ToString();
            }

            if (year > 0)
                nvc["year"] = year.ToString();

            if (certifications != null)
                nvc["certifications"] = string.Join(",", certifications);

            if (companies != null)
                nvc["companies"] = string.Join(",", companies);

            if (countries != null)
                nvc["countries"] = string.Join(",", countries);

            var searchQuery = string.Join("&", nvc.AllKeys.Select(k => string.Format("{0}={1}", k, HttpUtility.UrlEncode(nvc[k]))));
            var movies = DownloadData(string.Format(MovieBrowseUrl, ApiKey, Language, searchQuery), typeof(TmdbMovies)) as TmdbMovies;
            return movies;
        }

        #region MovieBrowseAsyncMethods
        private delegate void MovieBrowseDelegate(TmdbOrderBy orderBy, TmdbOrder order, int perPage = 0, int page = 0, string query = ""
            , int minVotes = 0, float ratingMin = 0, float ratingMax = 0, IEnumerable<int> genres = null, GenresSelector? genresSelector = null,
            DateTime? releaseMin = null, DateTime? releaseMax = null, int year = 0, IEnumerable<string> certifications = null
            , IEnumerable<string> companies = null, IEnumerable<string> countries = null, object userState = null, AsyncOperation asyncOp = null);

        public void MovieBrowseAsync(TmdbOrderBy orderBy, TmdbOrder order, int perPage = 0, int page = 0, string query = ""
            , int minVotes = 0, float ratingMin = 0, float ratingMax = 0, IEnumerable<int> genres = null, GenresSelector? genresSelector = null,
            DateTime? releaseMin = null, DateTime? releaseMax = null, int year = 0, IEnumerable<string> certifications = null
            , IEnumerable<string> companies = null, IEnumerable<string> countries = null)
        {
            MovieBrowseAsync(orderBy, order,perPage,page,query,minVotes,ratingMin,ratingMax,genres,genresSelector,releaseMin,releaseMax,year,certifications,companies,countries, null);
        }
        public void MovieBrowseAsync(TmdbOrderBy orderBy, TmdbOrder order, int perPage = 0, int page = 0, string query = ""
            , int minVotes = 0, float ratingMin = 0, float ratingMax = 0, IEnumerable<int> genres = null, GenresSelector? genresSelector = null,
            DateTime? releaseMin = null, DateTime? releaseMax = null, int year = 0, IEnumerable<string> certifications = null
            , IEnumerable<string> companies = null, IEnumerable<string> countries = null, object userState = null)
        {
            var asyncOp = AsyncOperationManager.CreateOperation(null);
            var worker = new MovieBrowseDelegate(MovieBrowseWorker);
            worker.BeginInvoke(orderBy, order, perPage, page, query, minVotes, ratingMin, ratingMax, genres, genresSelector, releaseMin, releaseMax, year, certifications, companies, countries, userState, asyncOp, null, null);
        }
        private void MovieBrowseWorker(TmdbOrderBy orderBy, TmdbOrder order, int perPage = 0, int page = 0, string query = ""
            , int minVotes = 0, float ratingMin = 0, float ratingMax = 0, IEnumerable<int> genres = null, GenresSelector? genresSelector = null,
            DateTime? releaseMin = null, DateTime? releaseMax = null, int year = 0, IEnumerable<string> certifications = null
            , IEnumerable<string> companies = null, IEnumerable<string> countries = null, object userState = null, AsyncOperation asyncOp = null)
        {
            Exception exception = null;
            IEnumerable<TmdbMovie> movies = null;
            try
            {
                movies = MovieBrowse(orderBy, order, perPage, page, query, minVotes, ratingMin, ratingMax, genres, genresSelector, releaseMin, releaseMax, year, certifications, companies, countries);
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            var args = new TmdbMovieSearchCompletedEventArgs(movies, exception, false, userState);
            asyncOp.PostOperationCompleted(
                e => OnMovieBrowseCompleted((TmdbMovieSearchCompletedEventArgs)e),
                args);
        }
        protected virtual void OnMovieBrowseCompleted(TmdbMovieSearchCompletedEventArgs e)
        {
            if (MovieBrowseCompleted != null)
                MovieBrowseCompleted(this, e);
        }
        #endregion

        /// <summary>
        /// the easiest and quickest way to search for a movie. It is a mandatory method in order to get the movie id to pass to the GetMovieInfo method.
        /// </summary>
        /// <param name="title">The title of the movie to search for.</param>
        /// <returns></returns>
        public IEnumerable<TmdbMovie> MovieSearch(string title, bool ForceRefresh = false)
        {
            var movies = DownloadData(string.Format(MovieSearchUrl, ApiKey, Escape(title), Language), typeof(TmdbMovies), ForceRefresh) as TmdbMovies;
            return movies;
        }

        ///// <summary>
        ///// the easiest and quickest way to search for a movie. It is a mandatory method in order to get the movie id to pass to the GetMovieInfo method.
        ///// </summary>
        ///// <param name="title">The title of the movie to search for.</param>
        ///// <returns></returns>
        //public void MovieSearch(string title, Action<IEnumerable<TmdbMovie>> callback)
        //{
        //    DownloadDataAsync(string.Format(MovieSearchUrl, ApiKey, Escape(title), Language), typeof (TmdbMovies),
        //                      (movies) =>
        //                          {
                                      
        //                          });
        //}

        #region MovieSearchAsyncMethods
        private delegate void MovieSearchDelegate(string title, object userState, AsyncOperation asyncOp);
        public void MovieSearchAsync(string title)
        {
            MovieSearchAsync(title, null);
        }
        public void MovieSearchAsync(string title, object userState)
        {
            var asyncOp = AsyncOperationManager.CreateOperation(null);
            var worker = new MovieSearchDelegate(MovieSearchWorker);
            worker.BeginInvoke(title, userState, asyncOp, null, null);
        }
        private void MovieSearchWorker(string title, object userState, AsyncOperation asyncOp)
        {
            Exception exception = null;
            IEnumerable<TmdbMovie> movies = null;
            try
            {
                movies = MovieSearch(title);
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            var args = new TmdbMovieSearchCompletedEventArgs(movies, exception, false, userState);
            asyncOp.PostOperationCompleted(
                e => OnMovieSearchCompleted((TmdbMovieSearchCompletedEventArgs) e),
                args);
        }
        protected virtual void OnMovieSearchCompleted(TmdbMovieSearchCompletedEventArgs e)
        {
            if (MovieSearchCompleted != null)
                MovieSearchCompleted(this, e);
        }
        #endregion

        /// <summary>
        /// The easiest and quickest way to search for a movie based on it's IMDb ID. You can use Movie.imdbLookup method to get the TMDb id of a movie if you already have the IMDB id.
        /// </summary>
        /// <param name="imdbId">The IMDb ID of the movie you are searching for.</param>
        /// <returns></returns>
        public IEnumerable<TmdbMovie> MovieSearchByImdb(string imdbId)
        {
            var movies = DownloadData(string.Format(MovieImdbLookupUrl, ApiKey, imdbId, Language), typeof(TmdbMovies)) as TmdbMovies;
            return movies;
        }

        #region MovieSearchByImdbAsyncMethods

        public void MovieSearchByImdbAsync(string imdbId)
        {
            MovieSearchByImdbAsync(imdbId, null);
        }
        public void MovieSearchByImdbAsync(string imdbId, object userState)
        {
            AsyncOperation asyncOp = AsyncOperationManager.CreateOperation(null);
            var worker = new MovieSearchDelegate(MovieSearchByImdbWorker);
            worker.BeginInvoke(imdbId, userState, asyncOp, null, null);
        }
        private void MovieSearchByImdbWorker(string imdbId, object userState, AsyncOperation asyncOp)
        {
            Exception exception = null;
            IEnumerable<TmdbMovie> movies = null;
            try
            {
                movies = MovieSearchByImdb(imdbId);
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            var args = new TmdbMovieSearchCompletedEventArgs(movies, exception, false, userState);
            asyncOp.PostOperationCompleted(
                e => OnMovieSearchByImdbCompleted((TmdbMovieSearchCompletedEventArgs) e),
                args);
        }
        protected virtual void OnMovieSearchByImdbCompleted(TmdbMovieSearchCompletedEventArgs e)
        {
            if (MovieSearchByImdbCompleted != null)
                MovieSearchByImdbCompleted(this, e);
        }
        #endregion

        /// <summary>
        /// retrieve specific information about a movie. Things like overview, release date, cast data, genre's, YouTube trailer link, etc...
        /// </summary>
        /// <param name="id">The ID of the TMDb movie you are searching for.</param>
        /// <returns></returns>
        public TmdbMovie GetMovieInfo(int id, bool ForceRefresh = false)
        {
            var movies = DownloadData(string.Format(MovieGetInfoUrl, ApiKey, id, Language), typeof(TmdbMovies), ForceRefresh) as TmdbMovies;
            if (movies != null)
                return movies.FirstOrDefault();
            return null;
        }

        #region GetMovieInfoAsyncMethods
        private delegate void GetMovieInfoDelegate(int id, object userState, AsyncOperation asyncOp);
        public void GetMovieInfoAsync(int id)
        {
            GetMovieInfoAsync(id, null);
        }
        public void GetMovieInfoAsync(int id, object userState)
        {
            AsyncOperation asyncOp = AsyncOperationManager.CreateOperation(null);
            var worker = new GetMovieInfoDelegate(GetMovieInfoWorker);
            worker.BeginInvoke(id, userState, asyncOp, null, null);
        }
        private void GetMovieInfoWorker(int id, object userState, AsyncOperation asyncOp)
        {
            Exception exception = null;
            TmdbMovie movie = null;
            try
            {
                movie = GetMovieInfo(id);
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            var args = new TmdbMovieInfoCompletedEventArgs(movie, exception, false, userState);
            asyncOp.PostOperationCompleted(
                e => OnGetMovieInfoCompleted((TmdbMovieInfoCompletedEventArgs) e),
                args);
        }
        protected virtual void OnGetMovieInfoCompleted(TmdbMovieInfoCompletedEventArgs e)
        {
            if (GetMovieInfoCompleted != null)
                GetMovieInfoCompleted(this, e);
        }
        #endregion

        /// <summary>
        /// Retrieve all of the backdrops and posters for a particular movie. This is useful to scan for updates, or new images if that's all you're after.
        /// </summary>
        /// <param name="id">TMDb ID (starting with tt) of the movie you are searching for.</param>
        /// <returns></returns>
        public TmdbMovie GetMovieImages(int id)
        {
            return GetMovieImages(id.ToString());
        }

        #region GetMovieImagesAsyncMethods
        private delegate void GetMovieImagesDelegate(string imdbId, object userState, AsyncOperation asyncOp);
        public void GetMovieImagesAsync(int id)
        {
            GetMovieImagesAsync(id, null);
        }
        public void GetMovieImagesAsync(int id, object userState)
        {
            AsyncOperation asyncOp = AsyncOperationManager.CreateOperation(null);
            var worker = new GetMovieImagesDelegate(GetMovieImagesWorker);
            worker.BeginInvoke(id.ToString(), userState, asyncOp, null, null);
        }
        private void GetMovieImagesWorker(string id, object userState, AsyncOperation asyncOp)
        {
            Exception exception = null;
            TmdbMovie movie = null;
            try
            {
                movie = GetMovieImages(id);
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            var args = new TmdbMovieInfoCompletedEventArgs(movie, exception, false, userState);
            asyncOp.PostOperationCompleted(
                e => OnGetMovieImagesCompleted((TmdbMovieInfoCompletedEventArgs) e),
                args);
        }
        protected virtual void OnGetMovieImagesCompleted(TmdbMovieInfoCompletedEventArgs e)
        {
            if (GetMovieImagesCompleted != null)
                GetMovieImagesCompleted(this, e);
        }
        #endregion

        /// <summary>
        /// Retrieve all of the backdrops and posters for a particular movie. This is useful to scan for updates, or new images if that's all you're after.
        /// </summary>
        /// <param name="imdbId">IMDB ID (starting with tt) of the movie you are searching for.</param>
        /// <returns></returns>
        public TmdbMovie GetMovieImages(string imdbId)
        {
            var movies = DownloadData(string.Format(MovieGetImagesUrl, ApiKey, imdbId, Language), typeof(TmdbMovies)) as TmdbMovies;
            if (movies != null)
                return movies.FirstOrDefault();
            return null;
        }

        #region GetMovieImagesAsyncMethods
        public void GetMovieImagesAsync(string imdbId)
        {
            GetMovieImagesAsync(imdbId, null);
        }
        public void GetMovieImagesAsync(string imdbId, object userState)
        {
            var asyncOp = AsyncOperationManager.CreateOperation(null);
            var worker = new GetMovieImagesDelegate(GetMovieImagesWorker);
            worker.BeginInvoke(imdbId, userState, asyncOp, null, null);
        }

        #endregion

        /// <summary>
        /// Search for an actor, actress or production member.
        /// </summary>
        /// <param name="name">The name of the person you are searching for.</param>
        /// <returns>TmdbCastPerson[]</returns>
        public IEnumerable<TmdbPerson> PersonSearch(string name)
        {
            var persons = DownloadData(string.Format(PersonSearchUrl, ApiKey, Escape(name), Language), typeof(TmdbPersons)) as TmdbPersons;
            return persons;
        }

        #region PersonSearchAsyncMethods
        private delegate void PersonSearchDelegate(string name, object userState, AsyncOperation asyncOp);
        public void PersonSearchAsync(string name)
        {
            PersonSearchAsync(name, null);
        }
        public void PersonSearchAsync(string name, object userState)
        {
            AsyncOperation asyncOp = AsyncOperationManager.CreateOperation(null);
            var worker = new PersonSearchDelegate(PersonSearchWorker);
            worker.BeginInvoke(name, userState, asyncOp, null, null);
        }
        private void PersonSearchWorker(string name, object userState, AsyncOperation asyncOp)
        {
            Exception exception = null;
            IEnumerable<TmdbPerson> people = null;
            try
            {
                people = PersonSearch(name);
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            var args = new TmdbPersonSearchCompletedEventArgs(people, exception, false, userState);
            asyncOp.PostOperationCompleted(
                e => OnPersonSearchCompleted((TmdbPersonSearchCompletedEventArgs) e),
                args);
        }
        protected virtual void OnPersonSearchCompleted(TmdbPersonSearchCompletedEventArgs e)
        {
            if (PersonSearchCompleted != null)
                PersonSearchCompleted(this, e);
        }
        #endregion

        /// <summary>
        /// Retrieve the full filmography, known movies, images and things like birthplace for a specific person in the TMDb database.
        /// </summary>
        /// <param name="id">The ID of the TMDb person you are searching for.</param>
        /// <returns>TmdbPerson</returns>
        public TmdbPerson GetPersonInfo(int id)
        {
            var persons = DownloadData(string.Format(PersonGetInfoUrl, ApiKey, id, Language), typeof(TmdbPersons)) as TmdbPersons;
            if (persons != null)
                return persons.FirstOrDefault();
            return null;
        }

        #region GetPersonInfoAsyncMethods
        private delegate void GetPersonInfoDelegate(int id, object userState, AsyncOperation asyncOp);
        public void GetPersonInfoAsync(int id)
        {
            GetPersonInfoAsync(id, null);
        }
        public void GetPersonInfoAsync(int id, object userState)
        {
            AsyncOperation asyncOp = AsyncOperationManager.CreateOperation(null);
            var worker = new GetPersonInfoDelegate(GetPersonInfoWorker);
            worker.BeginInvoke(id, userState, asyncOp, null, null);
        }
        private void GetPersonInfoWorker(int id, object userState, AsyncOperation asyncOp)
        {
            Exception exception = null;
            TmdbPerson person = null;
            try
            {
                person = GetPersonInfo(id);
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            var args = new TmdbPersonInfoCompletedEventArgs(person, exception, false, userState);
            asyncOp.PostOperationCompleted(
                e => OnGetPersonInfoCompleted((TmdbPersonInfoCompletedEventArgs) e),
                args);
        }
        protected virtual void OnGetPersonInfoCompleted(TmdbPersonInfoCompletedEventArgs e)
        {
            if (GetPersonInfoCompleted != null)
                GetPersonInfoCompleted(this, e);
        }
        #endregion

        /// <summary>
        /// The Genres.getList method is used to retrieve a list of valid genres within TMDb.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TmdbGenre> GetGenres()
        {
            var genres = DownloadData(string.Format(GenresGetListUrl, Language, ApiKey), typeof(TmdbGenres)) as TmdbGenres;
            if(genres != null && genres.Count > 0 && string.IsNullOrEmpty(genres[0].Name))
                genres.RemoveAt(0);
            return genres;
        }

        #region GetGenresAsyncMethods
        private delegate void GetGenresDelegate(object userState, AsyncOperation asyncOp);
        public void GGetGenresAsync()
        {
            GetGenresAsync(null);
        }
        public void GetGenresAsync(object userState)
        {
            AsyncOperation asyncOp = AsyncOperationManager.CreateOperation(null);
            var worker = new GetGenresDelegate(GetGenresWorker);
            worker.BeginInvoke(userState, asyncOp, null, null);
        }
        private void GetGenresWorker(object userState, AsyncOperation asyncOp)
        {
            Exception exception = null;
            IEnumerable<TmdbGenre> genres = null;
            try
            {
                genres = GetGenres();
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            var args = new TmdbGenresCompletedEventArgs(genres, exception, false, userState);
            asyncOp.PostOperationCompleted(
                e => OnGetGenresCompleted((TmdbGenresCompletedEventArgs)e),
                args);
        }
        protected virtual void OnGetGenresCompleted(TmdbGenresCompletedEventArgs e)
        {
            if (GetGenresCompleted != null)
                GetGenresCompleted(this, e);
        }
        #endregion

        private object DownloadData(string url, Type type, bool ForceRefresh = false)
        {
            //using (var stream = _client.OpenRead(url))
            string cacheFile = null;
            using (var stream = OpenReadStream(url, out cacheFile, ForceRefresh))
            {
                if (stream == null)
                    return null;
                try
                {
                    var dcs = new DataContractJsonSerializer(type);
                    var o = dcs.ReadObject(stream);
                    return o;
                }
                catch (Exception ex)
                {
					Logger.Log("Failed to Download Data: {0}{1}{2}{3}{4}", url, Environment.NewLine, ex.Message, Environment.NewLine, ex.StackTrace);
                    if (File.Exists(cacheFile))
                    {
                        try
                        {
							Logger.Log(File.ReadAllText(cacheFile));
                            File.Delete(cacheFile); // incase the response is bad.
                        }
                        catch (Exception) { }
                    }
                    throw ex;
                }
            }
        }

        public static string CacheDirectory { get; set; }

        private static string CalculateMD5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }

        private Stream OpenReadStream(string Url, out string CacheFile, bool ForceRefresh = false)
        {
            CacheFile = null;
            if(String.IsNullOrWhiteSpace(CacheDirectory))
			{
				string response = WebHelper.GetWebResponse(_client.BaseAddress + Url);
				return new MemoryStream(Encoding.ASCII.GetBytes( response ));
				//return new WebClient().("http://api.themoviedb.org/2.1/" + Url);
                return _client.OpenRead(Url);
			}
            CacheFile = Path.Combine(CacheDirectory, CalculateMD5Hash(Url));
            if (!File.Exists(CacheFile) || new FileInfo(CacheFile).CreationTime < DateTime.Now.AddMinutes(-60) || ForceRefresh)
            {
                using (Stream stream = _client.OpenRead(Url))
                {
                    if (stream == null)
                        return null;
                    using (StreamReader sr = new StreamReader(stream))
                    {
                        if (!Directory.Exists(CacheDirectory))
                            Directory.CreateDirectory(CacheDirectory);
                        File.WriteAllText(CacheFile, sr.ReadToEnd());
                    }
                }
            }
            return new FileStream(CacheFile, FileMode.Open);
        }

        //private void DownloadDataAsync(string url, Type type, Action<object> callback)
        //{
        //    _client.OpenReadAsync(new Uri(_client.BaseAddress + url), new AsyncState { Callback = callback, Type = type });
        //}

        //private static void ClientOpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        //{
        //    var state = e.UserState as AsyncState;
        //    if (state == null)
        //        return;
        //    if(e.Error == null)
        //    {
        //        using (e.Result)
        //        {
        //            var dcs = new DataContractJsonSerializer(state.Type);
        //            var o = dcs.ReadObject(e.Result);
        //            state.Callback(o);
        //        }
        //    }
        //    else
        //    {
        //        state.Callback(null);
        //    }
        //}

        private static string Escape(string s)
        {
            return Regex.Replace(s, "[" + Regex.Escape(new String(Path.GetInvalidFileNameChars())) + "]", "-");
        }

        //private class AsyncState
        //{
        //    public Type Type { get; set; }
        //    public Action<object> Callback { get; set; }
        //}
    }
}
