using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.IO;
using System.Drawing;
//using MediaInfoLib;
using System.Globalization;
using iMetaLibrary.Helpers;

namespace iMetaLibrary.Metadata
{
    public class MovieMeta:VideoMeta
    {
        public string OriginalTitle { get; set; }
        public string SortTitle { get; set; }
        public string Set { get; set; }
        public int Year { get; set; }
        public int Votes { get; set; }
        public string Outline { get; set; }
        public string TagLine { get; set; }
        public string Id { get; set; }
        public string Trailer { get; set; }
        public string[] Credits { get; set; }
        public KeyValuePair<string, string>[] Actors { get; set; }

        private string PosterUrl { get; set; }
        private string[] FanArtUrls { get; set; }


        public MovieMeta(string Filename):base(Filename)
        {
            this.FileInfo = new VideoFileMeta();
            this.ReleaseDate = DateTime.MinValue;
			
			if(Settings.UseFolderNameForMovieLookup)
			{
				var dirinfo = new System.IO.FileInfo(Filename).Directory;
				this.Title = System.Text.RegularExpressions.Regex.Replace(dirinfo.Name, @"[\s]+\[[^\]]+\]", "");
				if(Regex.IsMatch(dirinfo.Name, @"(?<=(\[|\())[\d]{4}(?=(\]|\)))"))
				{
					this.Year = int.Parse(Regex.Match(dirinfo.Name, @"(?<=(\[|\())[\d]{4}(?=(\]|\)))").Value);
					this.ReleaseDate = new DateTime(this.Year, 1, 1);
					this.Title = Regex.Replace(this.Title, @"[\s]*\([\d]{4}\)[\s]*", "");
				}
			}
			else
			{
				this.Title = new System.IO.FileInfo(Filename).Name;
			}
        }
		
		public char StartChar
		{
			get
			{ 
				if(String.IsNullOrEmpty(this.Title))
					return '#';
				if(this.Title.ToLower().StartsWith("the ") && this.Title.Length > 4)
					return this.Title.ToCharArray()[4];
				else if(!Regex.IsMatch(this.Title, "^[a-zA-Z]"))
					return '#';
				return this.Title.ToCharArray()[0];
			}
		}
        public void Save(bool SaveImages = true)
        {
            string filenameNoExtension = this.Filename.Substring(0, this.Filename.LastIndexOf("."));
            if (SaveImages)
            {
                // save poster
                if (!String.IsNullOrEmpty(this.PosterUrl))
                {
					ImageDownloader.SavePoster (this.PosterUrl, filenameNoExtension + ".tbn");
                }

                // save backgrounds
                if (this.FanArtUrls != null && this.FanArtUrls.Length > 0)
                {
                    string path = new FileInfo(this.Filename).Directory.FullName;
					ImageDownloader.SaveFanArt(this.FanArtUrls[0], filenameNoExtension + "-fanart.jpg");
                    for (int i = 1; i < Settings.MaxFanArt && i < this.FanArtUrls.Length; i++)
                    {
                        if (i == 1 && !Directory.Exists(Path.Combine(path, "ExtraFanArt")))
                            Directory.CreateDirectory(Path.Combine(path, "ExtraFanArt"));
						ImageDownloader.SaveFanArt(this.FanArtUrls[i], Path.Combine(path, "ExtraFanArt", String.Format("fanart-{0}.jpg", i)));
                    }
                }

                if (Settings.AttemptTrailerDownload && !String.IsNullOrEmpty(this.Trailer) && this.Trailer.ToLower().Contains("youtube.com"))
                {
                    // only support youtube downloading ATM
                    FileInfo trailerFile = new FileInfo(filenameNoExtension + "-trailer.mp4");
                    if (Youtube.GetMovie(trailerFile.FullName, this.Trailer))
                        this.Trailer = trailerFile.Name; // want relative path
                }
            }
            CreateNfoFile();
        }

		/// <summary>
		/// Gets the poster filename.
		/// </summary>
		/// <value>
		/// The poster filename.
		/// </value>
		public string PosterFilename 
		{
			get 
			{
				string filenameNoExtension = this.Filename.Substring(0, this.Filename.LastIndexOf("."));
				return filenameNoExtension + ".tbn";
			}
		}

        private void CreateNfoFile()
        {
            XElement movie = new XElement("movie");
            movie.Add(new XElement("title", this.TitleWithTags));
            movie.Add(new XElement("originaltitle", this.OriginalTitle ?? ""));
            movie.Add(new XElement("sorttitle", this.SortTitle ?? ""));
            movie.Add(new XElement("set", this.Set ?? ""));
            movie.Add(new XElement("rating", this.Rating.ToString()));
            if (this.Year > 0)
                movie.Add(new XElement("year", this.Year.ToString()));
            movie.Add(new XElement("votes", this.Votes.ToString()));
            if (!String.IsNullOrEmpty(this.Outline))
                movie.Add(new XElement("outline", this.Outline ?? ""));
            movie.Add(new XElement("plot", this.Plot ?? ""));
            movie.Add(new XElement("tagline", this.TagLine ?? ""));
            if(this.Runtime > 0)
                movie.Add(new XElement("runtime", this.Runtime / 60)); // runtime for movies is in minutes...
            movie.Add(new XElement("thumb", this.Thumb ?? ""));
            movie.Add(new XElement("mpaa", this.Mpaa ?? ""));
            movie.Add(new XElement("playcount", this.Playcount.ToString()));
            movie.Add(new XElement("watched", this.Watched.ToString().ToLower()));
            movie.Add(new XElement("id", this.Id ?? ""));
            if(this.ReleaseDate > new DateTime(1850, 1, 1))
                movie.Add(new XElement("releasedate", this.ReleaseDate.ToString("yyyy-MM-dd")));
            // dont write filenamandpath, incase the files are moved (im always doing this)
            //movie.Add(new XElement("filenameandpath", this.FilenameAndPath ?? ""));
            if (!String.IsNullOrEmpty(Trailer))
                movie.Add(new XElement("trailer", this.Trailer));
            foreach (string genre in Genres ?? new string[] { })
                movie.Add(new XElement("genre", genre));
            foreach (KeyValuePair<string, string> actor in Actors ?? new KeyValuePair<string, string>[] { })
                movie.Add(new XElement("actor", new XElement("name", actor.Key), new XElement("role", actor.Value)));
            foreach (string director in Directors ?? new string[] { })
                movie.Add(new XElement("director", director));
            foreach (string writer in Writers ?? new string[] { })
                movie.Add(new XElement("writer", writer));
            // credits, not sure what this is
            movie.Add(new XElement("credits", ""));

            if (this.FileInfo != null)
                movie.Add(this.FileInfo.CreateXElement());

            XDocument doc = new XDocument(movie);
            doc.Save(this.NfoFile);
        }

        public static MovieMeta LookupMovie(string Filename)
        {
            try
            {
                MovieMeta meta = new MovieMeta(Filename);                
                meta.Load();
                return meta;
            }
            catch (Exception) { return null; }
        }

        public int Load(bool ForceFromInternet = false)
        {
            this.CompletionLevel = MetaCompletionLevel.Loading;
            try
            {
                string filenameNoExtension = Filename.Substring(0, Filename.LastIndexOf("."));
                // if there is already a nfo file, skip it
                string path = new FileInfo(Filename).Directory.FullName;
                this.NfoFile = filenameNoExtension + ".nfo";
                if (File.Exists(Path.Combine(path, "movie.nfo")) && !ForceFromInternet)
                {
                    this.NfoFile = Path.Combine(path, "movie.nfo");
                    LoadFromNfo();
                    return 1;
                }
                else if (File.Exists(this.NfoFile) && !ForceFromInternet)
                {
                    LoadFromNfo();
                    return 1;
                }
                else
                {
                    if(LoadFromTheMovieDb(ForceFromInternet))
                    	Save();
                    return 2;
                }
            }
            finally
            {
                if (String.IsNullOrEmpty(this.Title) || String.IsNullOrEmpty(this.Plot) || this.Genres == null || this.Genres.Length == 0 || this.ReleaseDate == DateTime.MinValue || this.Year == 0)
                    this.CompletionLevel = MetaCompletionLevel.Partial;
                else
                    this.CompletionLevel = MetaCompletionLevel.Full;
            }
        }

        private void LoadFromNfo()
        {
            NfoLoader.Load<MovieMeta>(this, this.NfoFile);
            this.Runtime *= 60; // convert minutes to seconds
			if(Settings.AttemptTrailerDownloadMissingTrailers)
			{
				if(this.Trailer.StartsWith("http"))
				{
            		string filenameNoExtension = this.Filename.Substring(0, this.Filename.LastIndexOf("."));
                    // only support youtube downloading ATM
                    FileInfo trailerFile = new FileInfo(filenameNoExtension + "-trailer.mp4");
                    if (Youtube.GetMovie(trailerFile.FullName, this.Trailer)){
                        this.Trailer = trailerFile.Name; // want relative path
						CreateNfoFile();
					}
				}
			}
        }

        private bool LoadFromTheMovieDb(bool ForceRefresh = false)
        {
			try
			{
				Logger.Log("Loading from TheMovieDb: {0}", this.Filename);
	            TheMovieDb.TmdbApi api = new TheMovieDb.TmdbApi("1f5e5026bd038bfcfe62261e6da0634f");
	            string foldername = "";
	            foreach (Match match in Regex.Matches(Filename, @"(?<=((\\|/)))[^\\/]+(?=(\\|/))"))
	                foldername = match.Value;
	
	            // see if there is year in the foldername
	            string year = null;
	            if (Regex.IsMatch(foldername, @"(?<=(\())[\d]{4}(?=(\)))"))
	                year = Regex.Match(foldername, @"(?<=(\())[\d]{4}(?=(\)))").Value;
	
	            string movieTitle = Regex.Match(foldername, @"^[^(\[]+").Value.Trim();
	            string matchTitle = Regex.Replace(movieTitle.ToLower(), "[^a-z0-9]", "");
				string searchStr = movieTitle.Replace(" - ", " ").Replace(".", " ").Replace("_", " ");
				Logger.Log("Searching Str: {0}", searchStr);
	            IEnumerable<TheMovieDb.TmdbMovie> results = api.MovieSearch(searchStr, ForceRefresh);
	            string[] names = (from r in results select r.Name).ToArray();
	            TheMovieDb.TmdbMovie movie = null;
	            foreach (TheMovieDb.TmdbMovie result in results)
	            {
					Logger.Log("Search result: {0} ({1})", result.Name, result.Released);
	                if (movie == null)
	                    movie = result;
	                List<string> resultMatchTitles = new string[] { Regex.Replace(result.Name.ToLower(), "[^a-z0-9]", "") }.ToList();
	                if (!String.IsNullOrEmpty(result.AlternativeName))
	                    resultMatchTitles.Add(Regex.Replace(result.AlternativeName.ToLower(), "[^a-z0-9]", ""));
	                if (year != null && result.Released != null && Regex.Match(result.Released, @"[\d]{4}").Value == year)
	                {
	                    // likely to be this
	                    if (resultMatchTitles.Contains(matchTitle))
	                    {
	                        // exact match!!!
	                        movie = result;
	                        break;
	                    }
	                }
	                else if (String.IsNullOrEmpty(year) && resultMatchTitles.Contains(matchTitle)) // if year is specified, then the year MUST match
	                {
	                    movie = result;
	                }
	            }
	
	            if (movie != null)
	            {
					Logger.Log("Found movie id: {0}", movie.Id);
	                // get complete data
	                movie = api.GetMovieInfo(movie.Id, ForceRefresh);
	
	                MatchCollection tags = Regex.Matches(foldername, @"(?<=(\[))[^\]]+(?=(\]))");
	                this.Tags = new string[tags.Count];
	                for (int i = 0; i < tags.Count; i++)
	                    this.Tags[i] = tags[i].Value;
	
	                this.Actors = (from c in movie.Cast ?? new List<TheMovieDb.TmdbCastPerson>() where new string[] { "acting", "actor", "actors" }.Contains(c.Department.ToLower()) select new KeyValuePair<string, string>(c.Name, c.Character)).ToArray();
	                this.Directors = (from c in movie.Cast ?? new List<TheMovieDb.TmdbCastPerson>() where new string[] { "directing", "director" }.Contains(c.Department.ToLower()) select c.Name).ToArray();
	                this.Writers = (from c in movie.Cast ?? new List<TheMovieDb.TmdbCastPerson>() where new string[] { "writing", "writer" }.Contains(c.Department.ToLower()) select c.Name).ToArray();
	                this.Genres = (from g in movie.Genres ?? new List<TheMovieDb.TmdbGenre>() select g.Name).ToArray();
	                this.Id = movie.ImdbId;
	                this.Mpaa = movie.Certification;
	                this.OriginalTitle = movie.OriginalName ?? movie.Name;
	                //this.Outline = movie.Overview;
	                this.Plot = movie.Overview;
	                this.Rating = float.Parse(movie.Rating ?? "0");
	                int runtime = 0;
	                if(int.TryParse(movie.Runtime, out runtime))
	                    this.Runtime = runtime * 60; // convert to seconds
	                this.SortTitle = movie.Name;
	                this.TagLine = movie.Tagline;
	                this.Title = movie.Name;
	                this.Trailer = movie.Trailer;
	                this.Votes = int.Parse(movie.Votes ?? "0");
	                if (Regex.IsMatch(movie.Released ?? "", @"[\d]{4}"))
	                    this.Year = int.Parse(Regex.Match(movie.Released, @"[\d]{4}").Value ?? "0");
	                if (Regex.IsMatch(movie.Released ?? "", @"[\d]{4}-[\d]{2}-[\d]{2}"))
	                    this.ReleaseDate = DateTime.ParseExact(Regex.Match(movie.Released, @"[\d]{4}-[\d]{2}-[\d]{2}").Value, "yyyy-MM-dd", new CultureInfo("en-us"));
	
	                List<TheMovieDb.TmdbImage> posters = (from p in movie.Posters ?? new List<TheMovieDb.TmdbImage>() where p.ImageInfo.Size == "mid" select p).ToList();
	                if (posters != null && posters.Count > 0)
	                {
	                    this.PosterUrl = posters[0].ImageInfo.Url;
	                    if (posters.Count > 1 && this.Tags != null && this.Tags.Length > 0)
	                        this.PosterUrl = posters[1].ImageInfo.Url;
	                }
	
	                // backgrounds
	                TheMovieDb.TmdbImage[] backdrops = (from bd in movie.Backdrops ?? new List<TheMovieDb.TmdbImage>() where bd.ImageInfo.Size == "w1280" select bd).ToArray();
	                if (backdrops.Length > 0)
	                {
	                    this.FanArtUrls = new string[backdrops.Length];
	                    for (int i = 0; i < backdrops.Length; i++)
	                        this.FanArtUrls[i] = backdrops[i].ImageInfo.Url;
	                }
	
	                // movie file meta
	                this.FileInfo = VideoFileMeta.Load(Filename);
	
	                // rename it.
	                MovieRenamer.Rename(this);
					
					return true;
	            }
			}
			catch(Exception ex)
			{
				Logger.Log(ex.Message + Environment.NewLine + ex.StackTrace);
			}
			return false;
        }

        public override string ToString()
        {
            return String.IsNullOrEmpty(this.Title) ? this.Filename : String.Format("{0} ({1})", this.Title, this.Filename);
        }
    }
}
