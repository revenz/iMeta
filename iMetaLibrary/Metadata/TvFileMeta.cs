using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Xml.Linq;

namespace iMetaLibrary.Metadata
{
    /***
       A single episode file may contain mulitple episodes eg Batman - 1x01-03 - A Three Parter.avi
       <episodedetails>
         <title>The Episode - Part 1</title>
         <season>1</season>
         <episode>1</episode>
         <plot>Plot of the first part.</plot>
         <runtime>45 min</runtime>
         <aired>2011-10-23</aired>
         <epbookmark>0</epbookmark>
       </episodedetails>
       <episodedetails>
         <title>The Episode - Part 2</title>
         <season>1</season>
         <episode>2</episode>
         <plot>Plot of the second part.</plot>
         <runtime>45 min</runtime>
         <aired>2011-10-23</aired>
         <epbookmark>2700</epbookmark>
       </episodedetails>
     */
    public class TvFileMeta:Meta
    {
        public VideoFileMeta FileInfo { get; set; }
        public string NfoFile { get; set; }
        public TvEpisodeMeta[] Episodes { get; set; }

        public int[] EpisodeNumbers { get; set; }

        public TvFileMeta(string Filename) : base(Filename) 
        {
            if (String.IsNullOrEmpty(Filename))
                return;
            // regex showname and all.
            Regex rgxTvShow = new Regex(Settings.TvRegularExpression, RegexOptions.IgnoreCase);
			
            FileInfo fileInfo = new FileInfo(Filename);
			// incase the year is in the file remove it, as the year can be picked up as the season and ep number
			#region old way using single expression
			/*
			string strRgxfiletest = Regex.Replace(Filename, @"([\d]{4}\-[\d]{2}\-[\d]{2})|(720p)|(1080p)|(480p)", "");
            var match = rgxTvShow.Match(strRgxfiletest);
			Logger.Log("matching tv show: {0}", Filename);
			int grpindex = 0;
			foreach(Group grp in match.Groups)
				Logger.Log("TV group[{0}] = {1}", grpindex++, grp.Value);
            string showname = match.Groups[1].Value.Trim();
            if (showname.EndsWith(" -"))
                showname = showname.Substring(0, showname.Length - 2);
			showname = Regex.Replace(showname, @"\.([sS])?(0)?$", "");
            this.ShowTitle = showname.Replace(".", " ").Trim();
			Logger.Log("Show name: " + this.ShowTitle);

            this.Season = int.Parse(match.Groups[2].Value);
			
            string strEpisode = match.Groups[3].Value;           
            */
			#endregion			
			
			string showname = Regex.Match(Filename, Settings.TvShowTitleExpression, RegexOptions.IgnoreCase).Value;
            this.ShowTitle = showname.Replace(".", " ").Trim();
			
			this.Season = int.Parse(Regex.Match(Filename, Settings.TvSeasonExpression, RegexOptions.IgnoreCase).Value);
			string strEpisode = Regex.Match(Filename, Settings.TvEpisodeExpression, RegexOptions.IgnoreCase).Value;
            #region work out the episode numbers for this file
			
			
            List<int> epNumbers = new List<int>();
            MatchCollection epNumMatches = Regex.Matches(strEpisode, @"[\d]+");
            if (epNumMatches.Count != 2)
            {
                foreach (Match t in epNumMatches)
                    epNumbers.Add(int.Parse(t.Value));
            }
            else
            {
                int low = int.Parse(epNumMatches[0].Value);
                int high = int.Parse(epNumMatches[1].Value);
                for (int i = low; i <= high; i++)
                    epNumbers.Add(i);
            }
            this.EpisodeNumbers = epNumbers.ToArray();
			this.Episodes = new TvEpisodeMeta[this.EpisodeNumbers.Length];
			for(int i=0;i<Episodes.Length;i++)
				this.Episodes[i] = new TvEpisodeMeta(this.Filename) { Season = this.Season, Episode = EpisodeNumbers[i]};
            #endregion
        }
		
		

        public string EpisodeNumbersString
        {
            get
            {
                if (this.EpisodeNumbers == null || this.EpisodeNumbers.Length == 0)
                    return String.Empty;
                return String.Join(", ", (from ep in this.EpisodeNumbers select ep.ToString()).ToArray());
            }
        }

        public int Season { get; set; }
        private string _ShowTitle = null;
        public string ShowTitle
        {
            get { return _ShowTitle; }
            set
            {
                if (value != null && value.StartsWith("The "))
                    _ShowTitle = value.Substring(4) + ", The";
                else
                    _ShowTitle = value;
            }
        }

        public TvShowMeta TvShow { get; set; }
        
        public int SortIndex
        {
            get
            {
                int index = this.Season * 10000;
                if (this.EpisodeNumbers != null && this.EpisodeNumbers.Length > 0)
                {
                    index += EpisodeNumbers[0] * 100;
                }
                return index;
            }
        }

        public string EpisodeNames
        {
            get
            {
                if (this.Episodes == null || this.Episodes.Length == 0)
                    return String.Empty;
                else if (this.Episodes.Length == 1)
                    return this.Episodes[0].TitleWithTags;
                else // todo make this smarter
                    return "{0} ({1} - {2})".FormatStr(this.Episodes[0].Title, 1, this.Episodes.Length);
            }
        }

        public int Load(bool ForceFromInternet = false)
        {
            try
            {
                this.CompletionLevel = MetaCompletionLevel.Loading;
                string filenameNoExtension = Filename.Substring(0, Filename.LastIndexOf("."));
                // if there is already a nfo file, skip it
                string path = new FileInfo(Filename).Directory.FullName;
                this.NfoFile = filenameNoExtension + ".nfo";
                if (!ForceFromInternet && File.Exists(this.NfoFile))
                {
                    // load nfo
                    LoadFromNfo();
                    return 1;
                }
                else
                {
                    this.FileInfo = VideoFileMeta.Load(Filename);
                    // lookup
                    if(LoadFromTheTvDb())
                    	Save();
                    return 2;
                }
            }
            finally
            {
                if (this.Episodes == null || this.Episodes.Length == 0)
                    this.CompletionLevel = MetaCompletionLevel.None;
                else
                {
                    this.CompletionLevel = MetaCompletionLevel.Full;
                    if (Settings.AutoRenameEpisodes)
                    {
                        RenameEpisode();
                    }
                }

                if (this.Episodes != null && this.Episodes.Length > 0)
                    this.Rating = (from e in this.Episodes select e.Rating).Average();
            }
        }

        private void RenameEpisode()
        {
			if(String.IsNullOrWhiteSpace(this.Episodes[0].ShowTitle))
				return;
            string epNumStr = String.Join("-", (from num in this.EpisodeNumbers orderby num select num.ToString("D2")));
            string epName = this.Episodes[0].Title;
            if(this.EpisodeNumbers.Length > 1)
                epName = Regex.Replace(epName, @"\((Part )?[\dIVX]+\)", "");
            FileInfo fileInfo = new FileInfo(this.Filename);
			string suffix = "";
			if(!String.IsNullOrWhiteSpace(epName))
				suffix = " - {0}".FormatStr(epName);
            string expectedName = "{0} - {1}x{2}{3}".FormatStr(this.Episodes[0].ShowTitle, this.Season, epNumStr, suffix).Replace(": ", " - ").ToSafeFilename();
            string nameNoExtension = fileInfo.Name.Substring(0, fileInfo.Name.Length - fileInfo.Extension.Length);
            if(nameNoExtension != expectedName)
            {
                try
                {
                    // rename required
                    foreach (FileInfo fileToRename in fileInfo.Directory.GetFiles(nameNoExtension + ".*"))
                    {
                        fileToRename.MoveTo(Path.Combine(fileInfo.Directory.FullName, expectedName + fileToRename.Extension));
                    }
                    this.Filename = Path.Combine(fileInfo.Directory.FullName, expectedName + fileInfo.Extension);
                }
                catch (Exception ex) 
                {
                    Trace.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                }
            }
        }

        private void LoadFromNfo()
        {
            string rawXml = System.IO.File.ReadAllText(this.NfoFile);
            List<TvEpisodeMeta> episodes = new List<TvEpisodeMeta>();
            foreach (Match match in Regex.Matches(rawXml, "<episodedetails>(.*?)</episodedetails>", RegexOptions.Singleline))
            {
                XDocument doc = XDocument.Parse(match.Value);
                TvEpisodeMeta episode = new TvEpisodeMeta(this.Filename);
                NfoLoader.Load<TvEpisodeMeta>(episode, doc.Element("episodedetails"));
                episodes.Add(episode);
            }
            this.Episodes = episodes.ToArray();
			this.EpisodeNumbers = (from e in episodes select e.Episode).ToArray();
			if(this.Episodes.Length > 0)
				this.Season = this.Episodes[0].Season;
        }

        private bool LoadFromTheTvDb()
        {
            FileInfo fileInfo = new FileInfo(Filename);

            #region constructor TheTvdb Handler
            string cacheDirectory = Path.Combine(Settings.CachePath, "TheTvDb");
            if (!Directory.Exists(cacheDirectory))
                Directory.CreateDirectory(cacheDirectory);

            TvdbLib.Cache.ICacheProvider cacheProvider = new TvdbLib.Cache.XmlCacheProvider(cacheDirectory);
            TvdbLib.TvdbHandler tvdbHandler = new TvdbLib.TvdbHandler(cacheProvider, Settings.TvdbApiKey);
            #endregion

            LoadShowMeta(tvdbHandler);
            if (this.TvShow == null)
            {
                Trace.WriteLine(String.Format("Failed to find tv show '{0}' for file: {1}", this.ShowTitle, this.Filename));
                return false;
            }

            TvdbLib.Data.TvdbEpisode.EpisodeOrdering ordering = TvdbLib.Data.TvdbEpisode.EpisodeOrdering.DefaultOrder;
            if (this.Filename.ToUpper().Contains("[DVD]") && this.Season > 0) // specials dont use dvd order... i think...
                ordering = TvdbLib.Data.TvdbEpisode.EpisodeOrdering.DvdOrder;

            int epbookmark = 0;
            int episodeLength = 0;
			if(this.FileInfo != null && this.FileInfo.Video != null)
				episodeLength = this.FileInfo.Video.DurationInSeconds / this.EpisodeNumbers.Length;
            
			if(this.Episodes== null || this.Episodes.Length != this.EpisodeNumbers.Length)
				this.Episodes = new TvEpisodeMeta[this.EpisodeNumbers.Length];
			
			int index = 0;
            foreach (int epNumber in this.EpisodeNumbers)
            {
                TvdbLib.Data.TvdbEpisode tvdbEpisode = null;
                try
                {
                    tvdbEpisode = tvdbHandler.GetEpisode(this.TvShow.Id, this.Season, epNumber, ordering, TvdbLib.Data.TvdbLanguage.DefaultLanguage);
                }
                catch (Exception)
                {
                    if(ordering == TvdbLib.Data.TvdbEpisode.EpisodeOrdering.DvdOrder) // fail back to default ordering, incase there is no dvd ordering available (eg Sliders Season 3)
                        tvdbEpisode = tvdbHandler.GetEpisode(this.TvShow.Id, this.Season, epNumber, TvdbLib.Data.TvdbEpisode.EpisodeOrdering.DefaultOrder, TvdbLib.Data.TvdbLanguage.DefaultLanguage);
                }
                if (tvdbEpisode == null)
                    continue;
				if(this.Episodes[index] == null)
					this.Episodes[index] = new TvEpisodeMeta(this.Filename);
				
                TvEpisodeMeta meta = this.Episodes[index];
                meta.TvdbId = tvdbEpisode.Id;
                meta.TvdbSeriesId = tvdbEpisode.SeriesId;
                meta.Actors = (from gs in tvdbEpisode.GuestStars select new KeyValuePair<string, string>(gs, null)).ToArray();
                meta.Aired = tvdbEpisode.FirstAired;
                meta.Directors = tvdbEpisode.Directors.ToArray();
                meta.DisplayEpisode = tvdbEpisode.AirsBeforeEpisode >= 0 ? tvdbEpisode.AirsBeforeEpisode : -1;
                meta.DisplaySeason = tvdbEpisode.AirsBeforeSeason >= 0 ? tvdbEpisode.AirsBeforeSeason : -1;
                meta.Episode = tvdbEpisode.EpisodeNumber;
                meta.Season = tvdbEpisode.SeasonNumber;
                meta.Plot = tvdbEpisode.Overview;
                meta.Premiered = tvdbEpisode.FirstAired;
                meta.Rating = (float)tvdbEpisode.Rating;
                meta.ShowTitle = this.TvShow.Title;
                meta.Mpaa = this.TvShow.Mpaa;
                meta.Studio = this.TvShow.Studio;
                meta.EpBookmark = epbookmark;
                if (!String.IsNullOrEmpty(tvdbEpisode.BannerPath))
                    meta.Thumb = "http://thetvdb.com/banners/" + tvdbEpisode.BannerPath;
                meta.Title = tvdbEpisode.EpisodeName;
                meta.Writers = tvdbEpisode.Writer.ToArray();
                epbookmark += episodeLength; // not perfect, but good starting point.

                if (ordering == TvdbLib.Data.TvdbEpisode.EpisodeOrdering.DvdOrder)
                {
                    meta.Episode = (int)tvdbEpisode.DvdEpisodeNumber;
                    meta.Season = tvdbEpisode.DvdSeason;
                }
				
                index++;
            }
            return true;
        }

        public void Save()
        {
            if (this.Episodes == null || this.Episodes.Length < 1)
                return;

            StringBuilder builder = new StringBuilder();
            foreach(TvEpisodeMeta episode in this.Episodes)
                builder.AppendLine(episode.CreateXElement().ToString());

            string filenameNoExtension = Filename.Substring(0, Filename.LastIndexOf("."));
            File.WriteAllText(filenameNoExtension + ".nfo", builder.ToString());
        }

        public override string ToString()
        {
            if (this.Episodes == null || this.Episodes.Length == 0)
                return this.Filename;
            string epnumbers = String.Join("-", (from e in Episodes select e.Episode.ToString()).ToArray());
            return "{0} - {1} - {2}".FormatStr(this.Episodes[0].ShowTitle, epnumbers, this.Episodes[0].Title);
        }

        public System.Drawing.Image GetPoster()
        {
            string tbnFile = Path.Combine(new FileInfo(this.Filename).Directory.Parent.FullName, "poster.jpg");
            if (!File.Exists(tbnFile))
            {
                tbnFile = Path.Combine(new FileInfo(this.Filename).Directory.FullName, "poster.jpg");
                if (!File.Exists(tbnFile))
                    return null;
            }
            return System.Drawing.Image.FromFile(tbnFile);
        }
		
        public TvShowMeta LoadShowMeta(){ return LoadShowMeta(null);}
        public TvShowMeta LoadShowMeta(TvdbLib.TvdbHandler tvdbHandler)
        {
            if (this.TvShow == null)
            {
                FileInfo fileInfo = new FileInfo(this.Filename);
                string showPath = fileInfo.Directory.FullName;
                // get directory of show.
                if (Regex.IsMatch(fileInfo.Directory.Name, "(Season|Series|Specials)", RegexOptions.IgnoreCase))
                    showPath = fileInfo.Directory.Parent.FullName;

                this.TvShow = TvShowMeta.Load(tvdbHandler, this.ShowTitle, Path.Combine(showPath, "tvshow.nfo"));
            }
            return this.TvShow;
        }
    }
}
