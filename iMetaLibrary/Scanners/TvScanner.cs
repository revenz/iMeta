using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Drawing;
using iMetaLibrary.Metadata;

namespace iMetaLibrary.Scanners
{
    public class TvScanner : Scanner
    {
        public SortedDictionary<string, List<Metadata.TvFileMeta>> Shows = new SortedDictionary<string, List<Metadata.TvFileMeta>>();
        private Dictionary<string, bool> ShowsWithPosters = new Dictionary<string, bool>();

        public int NumberOfShows
        {
            get { return Shows.Count; }
        }

        public TvScanner()
        {       
        }

        protected override void Scan_Executing(object oFolders = null)
        {
            TriggerStatusUpdated("Searching for TV Episodes...");
            string[] Folders = oFolders as string[];
            if (Folders == null)
                Folders = Settings.TvFolders;
            string[] extensions = Settings.VideoExtensions;

            #region get files
            List<Meta> newItems = new List<Meta>();
            // tv shows must be in the format: [Show]\[Season \d]\[Episodes]
            Regex rgxTvShow = new Regex(Settings.TvRegularExpression, RegexOptions.IgnoreCase);
            foreach (string folder in Folders)
            {
                try
                {
                    List<string> knownFiles = new List<string>();
                    foreach (List<Metadata.TvFileMeta> eplist in Shows.Values)
                        knownFiles.AddRange(from ep in eplist select ep.Filename.ToLower());
                    string[] files = System.IO.Directory.GetFiles(folder, "*", System.IO.SearchOption.AllDirectories);
                    foreach (string file in files)
                    {
                        if (this.Items.ContainsKey(file.ToLower()) || knownFiles.Contains(file.ToLower()))
                            continue;
                        string ext = file.Substring(file.LastIndexOf(".") + 1).ToLower();
                        if (!extensions.Contains(ext))
                            continue;
                        FileInfo fileInfo = new FileInfo(file);
						// incase the year is in the file remove it, as the year can be picked up as the season and ep number
						string strRgxfiletest = Regex.Replace(file, @"([\d]{4}\-[\d]{2}\-[\d]{2})|(720p)|(1080p)|(480p)", "");
                        if (!rgxTvShow.IsMatch(strRgxfiletest))
                            continue;
                        Metadata.TvFileMeta meta = new Metadata.TvFileMeta(file);
                        newItems.Add(meta);
                        AddItem(meta);
						
						if(!Shows.ContainsKey(meta.ShowTitle))
							Shows.Add(meta.ShowTitle, new List<TvFileMeta>());
						Shows[meta.ShowTitle].Add(meta);
                    }
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                }
            }
            #endregion
			
			foreach(string key in Shows.Keys)
			{
				Shows[key].Sort(delegate(Metadata.TvFileMeta a, Metadata.TvFileMeta b)
				               {
									if(a == null && b == null)
										return 0;
									else if (a == null) return 1;
									else if (b == null) return -1;
									if(a.Season != b.Season) return a.Season.CompareTo(b.Season);
									return a.EpisodeNumbers[0].CompareTo(b.EpisodeNumbers[0]);
							   });
			}

            TriggerAllItemsFound(newItems.ToArray());

            #region constructor TheTvdb Handler
            string cacheDirectory = Path.Combine(Settings.CachePath, "TheTvDb");
            if (!Directory.Exists(cacheDirectory))
                Directory.CreateDirectory(cacheDirectory);

            TvdbLib.Cache.ICacheProvider cacheProvider = new TvdbLib.Cache.XmlCacheProvider(cacheDirectory);
            TvdbLib.TvdbHandler tvdbHandler = new TvdbLib.TvdbHandler(cacheProvider, Settings.TvdbApiKey);
            #endregion

            foreach (Metadata.TvFileMeta meta in newItems.OrderBy(x => ((TvFileMeta)x).ExistingNfoFile ? 1 : 0))
            {
                TriggerScanningItem(meta);
                int result = meta.Load();
                TriggerUpdated(meta, result);
            }

            TriggerCompleted();
        }
		
		protected override void RefreshFiles_Executing (object oFilenames)
		{
			
		}

        public System.Drawing.Image GetPoster(string ShowTitle)
        {
            if (String.IsNullOrEmpty(ShowTitle) || !Shows.ContainsKey(ShowTitle) || Shows[ShowTitle].Count == 0)
                return null;
            return Shows[ShowTitle][0].GetPoster();
        }

        private string ShowTitleToMetaId(string ShowTitle)
        {
            ShowTitle = ShowTitle.ToLower();
            if (ShowTitle.StartsWith("the "))
                ShowTitle = ShowTitle.Substring(4) + "the";
            return Regex.Replace(ShowTitle, "[^a-z0-9]", "");
        }
		
		public override string CreateHtmlElement (string BaseDirectory)
		{
			
			StringBuilder html = new StringBuilder();
			html.AppendLine("<ul class=\"tvshows\">");
			foreach(string showname in this.Shows.Keys)
			{
				var show = this.Shows[showname][0].LoadShowMeta();
				if(show == null)
					continue;
				// create poster thumb in specified directory
				string imgelement = "";		
				var image = show.LoadPoster();
				if(image != null)
				{
					string shortname = CreateImageThumbnail(image, Path.Combine(BaseDirectory, "images"), 200, 300);
					imgelement = "<img src=\"images/{0}\" alt=\"{0}\" />".FormatStr(shortname.HtmlEncode());
					image.Dispose();
				}				SortedDictionary<int, List<TvFileMeta>> seasons = new SortedDictionary<int, List<TvFileMeta>>();
				foreach(TvFileMeta fm in this.Shows[showname])
				{
					if(!seasons.ContainsKey(fm.Season))
						seasons.Add(fm.Season, new List<TvFileMeta>());
					seasons[fm.Season].Add(fm);
				}
				System.Text.StringBuilder htmlSeason = new System.Text.StringBuilder();
				foreach(int season in seasons.Keys)
				{
					htmlSeason.AppendLine("<li class=\"season\" data-num=\"{0}\"><span class=\"seasonname\">{1}</span><span class=\"numepisodes\">[{2} Episode{3}]</span><ul class=\"episodes\">".FormatStr(season, season == 0 ? "Specials" : "Season " + season, seasons[season].Count, seasons[season].Count == 1 ? "" : "s"));
					
					foreach(TvFileMeta ep in seasons[season])
						htmlSeason.AppendLine(("<li class=\"episode\">" + Environment.NewLine +
					                      	  "		<span class=\"episodes\">{0}</span>" + Environment.NewLine +
					                      	  "		<span class=\"name\">{1}</span>" + Environment.NewLine + 
					                      	  "		<span class=\"rating {2}\">{3}</span>" + Environment.NewLine +
						                      "</li>").FormatStr(ep.EpisodeNumbersString.HtmlEncode(), 
						            							ep.EpisodeNames.HtmlEncode(),
						            							(int)ep.Rating, ep.Rating));
					htmlSeason.AppendLine("</ul></li>");					
				}
				
				html.AppendLine("     <li class=\"tvshow\">");
				html.AppendLine("          <span class=\"poster\">{0}</span>".FormatStr(imgelement));
				html.AppendLine("          <span class=\"title\">{0}</span>".FormatStr(show.Title.HtmlEncode()));
				html.AppendLine("          <span class=\"year\">{0}</span>".FormatStr(show.Year > 0 ? show.Year.ToString() : System.Text.RegularExpressions.Regex.Match(@"[\d]{4}", show.Premiered ?? "").Value ?? ""));
				html.AppendLine("          <span class=\"runtime\">{0}</span>".FormatStr(show.Runtime > 0 ? "{0} minutes".FormatStr(show.Runtime / 60) : ""));
				html.AppendLine("          <span class=\"rating {0}\">{1}</span>".FormatStr(((int)show.Rating).ToString(), show.Rating.ToString()));
				html.AppendLine("          <span class=\"numepisodes\">{0} Episode{1}</span>".FormatStr(Shows[showname].Count, Shows[showname].Count == 1 ? "" : "s"));
				html.AppendLine("		   <ul class=\"seasons\">" + htmlSeason.ToString() +  "</ul>");
		  		html.AppendLine("     </li>");
				                                       
			}
			html.AppendLine("</ul>");
			return html.ToString();
		}
    }
}

