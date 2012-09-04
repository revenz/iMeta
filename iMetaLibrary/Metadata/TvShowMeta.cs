using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Drawing;
using iMetaLibrary.Helpers;

namespace iMetaLibrary.Metadata
{
    public class TvShowMeta
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public float Rating { get; set; }
        public int Year { get; set; }
        public string Outline { get; set; }
        public string Plot { get; set; }
        public string Tagline { get; set; }
        public string[] Genres { get; set; }
        public string Premiered { get; set; }
        public string Status { get; set; }
        public string Mpaa { get; set; }
        public string EpisodeGuide { get; set; }
        public string Studio { get; set; }
        public int Runtime { get; set; }
        public KeyValuePair<string, string>[] Actors { get; set; }
        public string[] Thumb { get; set; }
        public Dictionary<int, string[]> SeasonBanners { get; set; }
        public FanArtImage[] FanArt { get; set; }
        public string ClearArtUrl { get; set; }
        public string ClearLogoUrl { get; set; }

        public string PosterUrl { get; set; }
        public string BannerUrl { get; set; }
        public string FanArtUrl { get; set; }
        public Dictionary<int, string> SeasonPosters { get; set; }
        public string TvTuneUrl { get; set; }

        public string FilePath { get; set; }

        public TvShowMeta(string FilePath)
        {
            this.FilePath = FilePath;
            SeasonBanners = new Dictionary<int, string[]>();
            this.SeasonPosters = new Dictionary<int, string>();
        }

        public void Save (string Directory)
		{
			if (!String.IsNullOrEmpty (this.ClearArtUrl))
				ImageDownloader.DownloadImage (
					this.ClearArtUrl,
					Path.Combine(Directory, "clearart.png"),
					System.Drawing.Imaging.ImageFormat.Png
				);
			if (!String.IsNullOrEmpty (this.ClearLogoUrl))
				ImageDownloader.DownloadImage (
					this.ClearLogoUrl,
					Path.Combine(Directory, "logo.png"),
					System.Drawing.Imaging.ImageFormat.Png
				);
            if (!String.IsNullOrEmpty(this.BannerUrl))
            {
                ImageDownloader.SaveBanner(this.BannerUrl, Path.Combine(Directory, "banner.jpg"));
                if (Settings.UseBannersForTvFolders)
                {
                    if (File.Exists(Path.Combine(Directory, "folder.jpg")))
                        File.Delete(Path.Combine(Directory, "folder.jpg"));
                    File.Copy(Path.Combine(Directory, "banner.jpg"), Path.Combine(Directory, "folder.jpg"));
                }
            }
			if (!String.IsNullOrEmpty (this.PosterUrl)) {
				ImageDownloader.SaveFanArt (this.PosterUrl, Path.Combine (Directory, "poster.jpg"));
                if (!Settings.UseBannersForTvFolders)
                {
                    if (File.Exists(Path.Combine(Directory, "folder.jpg")))
                        File.Delete(Path.Combine(Directory, "folder.jpg"));
                    File.Copy(Path.Combine(Directory, "poster.jpg"), Path.Combine(Directory, "folder.jpg"));
                }
            }
            if (!String.IsNullOrEmpty(this.FanArtUrl))
            {
				ImageDownloader.SaveFanArt(this.FanArtUrl, Path.Combine(Directory, "fanart.jpg"));
                if (File.Exists(Path.Combine(Directory, "backdrop.jpg")))
                    File.Delete(Path.Combine(Directory, "backdrop.jpg"));
                File.Copy(Path.Combine(Directory, "fanart.jpg"), Path.Combine(Directory, "backdrop.jpg"));
            }
            if (SeasonPosters != null)
            {
                foreach (int season in SeasonPosters.Keys)
                {
                    ImageDownloader.SavePoster(SeasonPosters[season], Path.Combine(Directory, String.Format("season{0}.tbn", season == -1 ? "-all" : season.ToString("D2"))));
                }
            }
            if (!String.IsNullOrEmpty(this.TvTuneUrl))
                WebHelper.DownloadTo(this.TvTuneUrl, Path.Combine(Directory, "theme.mp3"));

            // do this last, since if this file is found, the whole process is skipped, so if the user aborts mid way through, the restarts the tvshow.nfo will only be there if everything was saved
            CreateNfoFile(Path.Combine(Directory, "tvshow.nfo"));
        }

        private void CreateNfoFile(string NfoFile)
        {
            XElement root = new XElement("tvshow");
            root.Add(new XElement("id", this.Id.ToString()));
            root.Add(new XElement("title", this.Title ?? ""));
            root.Add(new XElement("rating", this.Rating.ToString()));
            root.Add(new XElement("plot", this.Plot ?? ""));
            if(this.Year > 0)
                root.Add(new XElement("year", this.Year.ToString()));
            if (this.Runtime > 0)
                root.Add(new XElement("runtime", this.Runtime.ToString()));
            if (!String.IsNullOrEmpty(this.Premiered))
                root.Add(new XElement("premiered", this.Premiered));
            if (!String.IsNullOrEmpty(this.Studio))
                root.Add(new XElement("studio", this.Studio));
            if (!String.IsNullOrEmpty(this.Mpaa))
                root.Add(new XElement("mpaa", this.Mpaa));
            if (!String.IsNullOrEmpty(this.Status))
                root.Add(new XElement("status", this.Status));
            if (!String.IsNullOrEmpty(this.Tagline))
                root.Add(new XElement("tagline", this.Tagline));
            if(!String.IsNullOrEmpty(this.EpisodeGuide))
                root.Add(new XElement("episodeguide", new XElement("url", this.EpisodeGuide, new XAttribute("cache", this.Id + ".xml"))));
            if (this.Genres != null && this.Genres.Length > 0)
            {
                foreach (string genre in this.Genres)
                    root.Add(new XElement("genre", genre));
            }
            if (this.Thumb != null && this.Thumb.Length > 0)
            {
                foreach (string thumb in this.Thumb)
                    root.Add(new XElement("thumb", thumb));
            }
            if (this.SeasonBanners != null)
            {
                foreach (int season in this.SeasonBanners.Keys)
                {
                    foreach (string seasonbanner in this.SeasonBanners[season])
                        root.Add(new XElement("thumb", new XAttribute("type", "season"), new XAttribute("season", season), seasonbanner));
                }
            }
            if (this.FanArt != null && this.FanArt.Length > 0)
            {
                XElement eleFanArt = new XElement("fanart", new XAttribute("url", "http://thetvdb.com/banners"));
                foreach (FanArtImage fai in this.FanArt)
                    eleFanArt.Add(fai.CreateXElement());
                root.Add(eleFanArt);
            }
            if (this.Actors != null && this.Actors.Length > 0)
            {
                foreach (KeyValuePair<string, string> actor in this.Actors)
                    root.Add(new XElement("actor", new XElement("name", actor.Key), new XElement("role", actor.Value)));
            }

            XDocument doc = new XDocument();
            doc.Add(root);
            doc.Save(NfoFile);
        }

        public static TvShowMeta Load(TvdbLib.TvdbHandler tvdbHandler, string ShowName, string NfoFile)
        {
            try
            {
                TvShowMeta meta = new TvShowMeta(new FileInfo(NfoFile).Directory.FullName);
                if (!File.Exists(NfoFile))
                {
                    if (tvdbHandler == null)
                        return null;
                    // try download it
                    List<TvdbLib.Data.TvdbSearchResult> results = tvdbHandler.SearchSeries(ShowName);
                    if (results == null || results.Count == 0)
                    {
                        Trace.WriteLine(String.Format("Unable to find show '{0}'", ShowName));
                        return null;
                    }
                    TvdbLib.Data.TvdbSearchResult showMatch = null;
                    foreach (TvdbLib.Data.TvdbSearchResult result in results)
                    {
                        if (showMatch == null || Regex.Replace(result.SeriesName, "[^a-zA-Z0-9]", "").ToLower() == Regex.Replace(ShowName, "[^a-zA-Z0-9]", "").ToLower())
                            showMatch = result;
                    }
                    if (showMatch == null)
                        return null;
                    TvdbLib.Data.TvdbSeries series = tvdbHandler.GetSeries(showMatch.Id, TvdbLib.Data.TvdbLanguage.DefaultLanguage, false, true, true);
                    // save the nfo
                    meta.Id = series.Id;
                    meta.Title = series.SeriesName;                    
                    meta.Plot = series.Overview;
                    meta.Premiered = series.FirstAired.ToString("yyyy-MM-dd");
                    meta.Rating = (float)series.Rating;
                    meta.Status = series.Status;
                    meta.Genres = series.Genre.ToArray();
                    meta.Year = series.FirstAired.Year;
                    meta.Mpaa = series.ContentRating;
                    meta.Studio = series.Network;
                    meta.Runtime = (int)series.Runtime;
                    meta.EpisodeGuide = String.Format("http://www.thetvdb.com/api/{0}/series/{1}/all/en.zip", Settings.TvdbApiKey, series.Id);
                    meta.Actors = (from a in series.TvdbActors select new KeyValuePair<string, string>(a.Name, a.Role)).ToArray();
                    meta.Thumb = (from t in series.SeriesBanners orderby t.Rating descending, t.RatingCount descending select String.Format("http://thetvdb.com/banners/{0}", t.BannerPath)).ToArray();
                    
                    foreach (int season in (from t in series.SeasonBanners orderby t.Season select t.Season).Distinct())
                    {
                        meta.SeasonBanners.Add(season, (from t in series.SeasonBanners orderby t.Rating descending, t.RatingCount descending where t.Season == season select String.Format("http://thetvdb.com/banners/{0}", t.BannerPath)).ToArray());
                    }
                    meta.FanArt = (from f in series.FanartBanners
                                   orderby f.Rating descending, f.RatingCount descending
                                   select new FanArtImage()
                                   {
                                       Colors = String.Format("|{0},{1},{2}|{3},{4},{5}|{6},{7},{8}|", f.Color1.R, f.Color1.G, f.Color1.B, f.Color2.R, f.Color2.G, f.Color2.B, f.Color3.R, f.Color3.G, f.Color3.B),
                                       Dimensions = String.Format("{0}x{1}", f.Resolution.X, f.Resolution.Y),
                                       Url = f.BannerPath,
                                       PreviewUrl = f.ThumbPath
                                   }).ToArray();

                    meta.PosterUrl = (from p in series.PosterBanners orderby p.Resolution.X descending, p.Resolution.Y descending, p.Rating descending, p.RatingCount descending select String.Format("http://thetvdb.com/banners/{0}", p.BannerPath)).FirstOrDefault();
                    meta.BannerUrl = (from b in series.SeriesBanners orderby (b.BannerType == TvdbLib.Data.Banner.TvdbSeriesBanner.Type.graphical ? 0 : 1), b.Rating descending, b.RatingCount descending select String.Format("http://thetvdb.com/banners/{0}", b.BannerPath)).FirstOrDefault();
                    meta.FanArtUrl = (from b in series.FanartBanners orderby b.Resolution.Y descending, b.Resolution.X descending, b.Rating descending, b.RatingCount descending select String.Format("http://thetvdb.com/banners/{0}", b.BannerPath)).FirstOrDefault();
                    foreach (int season in (from b in series.SeasonBanners where b.BannerType == TvdbLib.Data.Banner.TvdbSeasonBanner.Type.season select b.Season).Distinct())
                    {
                        meta.SeasonPosters[season] = (from b in series.SeasonBanners where b.Season == season && b.BannerType == TvdbLib.Data.Banner.TvdbSeasonBanner.Type.season orderby b.Rating descending, b.RatingCount descending select String.Format("http://thetvdb.com/banners/{0}", b.BannerPath)).FirstOrDefault();
                    }
                    // get the all seasons season poster
                    meta.SeasonPosters[-1] = (from b in series.PosterBanners orderby b.Rating descending, b.RatingCount descending select String.Format("http://thetvdb.com/banners/{0}", b.BannerPath)).FirstOrDefault();
                    
                    // get the clearart 
                    try
                    {
                        XDocument xmlClearArt = XDocument.Load("http://fanart.tv/api/fanart.php?id=" + series.Id);
                        if (xmlClearArt.Element("fanart").Element("clearlogos") != null && xmlClearArt.Element("fanart").Element("clearlogos").Element("clearlogo") != null)
                            meta.ClearLogoUrl = xmlClearArt.Element("fanart").Element("clearlogos").Element("clearlogo").Attribute("url").Value;
                        if (xmlClearArt.Element("fanart").Element("cleararts") != null && xmlClearArt.Element("fanart").Element("cleararts").Element("clearart") != null)
                            meta.ClearArtUrl = xmlClearArt.Element("fanart").Element("cleararts").Element("clearart").Attribute("url").Value;
                    }
                    catch (Exception) { }

                    // get the tv tune
                    try
                    {
                        string tvTunePage = WebHelper.GetWebResponse(String.Format("http://www.televisiontunes.com/search.php?searWords={0}&search=", meta.Title));
                        string content = Regex.Match(tvTunePage, "(?<=(<h1>Search</h1>))(.*?)(?=((<a href=\"search)|(</td>)))", RegexOptions.IgnoreCase | RegexOptions.Singleline).Value;
                        Dictionary<string, string> matches = new Dictionary<string, string>();
                        foreach (Match match in Regex.Matches(content, @"(?<=([\d]+\.&nbsp;))(.*?)</a>", RegexOptions.IgnoreCase | RegexOptions.Singleline))
                        {
                            string name = Regex.Match(match.Value, @"(?<=(>))[^<]+").Value;
                            string url = Regex.Match(match.Value, @"(?<=(href=""))[^""]+").Value;
                            matches.Add(url, name);
                        }
                        string themeUrlPage = null;
                        foreach (string url in matches.Keys)
                        {
                            if (Regex.Replace(meta.Title, "[^0-9a-zA-Z]", "").ToLower() == Regex.Replace(matches[url], "[^0-9a-zA-Z]", "").ToLower())
                            {
                                Trace.WriteLine("Found theme song: " + url);

                                themeUrlPage = url;
                                break;
                            }
                            else if (matches[url].IndexOf(" - ") > 0 && Regex.Replace(meta.Title, "[^0-9a-zA-Z]", "").ToLower() == Regex.Replace(matches[url].Substring(0, matches[url].IndexOf(" - ")), "[^0-9a-zA-Z]", "").ToLower()) // look for a close match, eg "Come fly with Me - Openning"
                            {
                                Trace.WriteLine("Found close theme song: " + url);

                                themeUrlPage = url;
                            }
                        }

                        if (!String.IsNullOrEmpty(themeUrlPage))
                        {
                            string themeSongPage = WebHelper.GetWebResponse(themeUrlPage);
                            Match themeUrlMatch = Regex.Match(themeSongPage, "/themesongs/(.*?).mp3", RegexOptions.IgnoreCase);
                            if (themeUrlMatch.Success)
                                meta.TvTuneUrl = "http://www.televisiontunes.com" + themeUrlMatch.Value;
                        }
                    }
                    catch (Exception) { }

                    meta.Save(new FileInfo(NfoFile).Directory.FullName);
                    return meta;
                }

                XElement root = XDocument.Load(NfoFile).Element("tvshow");
                int id = 0;
                if (int.TryParse(root.Element("id") != null ? root.Element("id").Value : "0", out id))
                    meta.Id = id;
                meta.Title = root.Element("title") != null ? root.Element("title").Value : "";
                meta.Tagline = root.Element("tagline") != null ? root.Element("tagline").Value : "";
                meta.Outline = root.Element("outline") != null ? root.Element("outline").Value : "";
                meta.Plot = root.Element("plot") != null ? root.Element("plot").Value : "";
                meta.Premiered = root.Element("premiered") != null ? root.Element("premiered").Value : "";
                meta.Mpaa = root.Element("mpaa") != null ? root.Element("mpaa").Value : "";
                float rating = 0;
                if (float.TryParse(root.Element("rating") != null ? root.Element("rating").Value : "0", out rating))
                    meta.Rating = rating;
                meta.Status = root.Element("status") != null ? root.Element("status").Value : "";
                meta.Studio = root.Element("studio") != null ? root.Element("studio").Value : "";
                int year = 0;
                if (int.TryParse(root.Element("year") != null ? root.Element("year").Value : "0", out year))
                    meta.Year = year;
                meta.Genres = (from g in root.Elements("genre") select g.Value).ToArray();
                if (root.Element("episodeguide") != null && root.Element("episodeguide").Element("url") != null)
                    meta.EpisodeGuide = root.Element("episodeguide").Element("url").Value;

                return meta;
            }
            catch (Exception ex) 
            {
                Trace.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                return null; 
            }
        }

        public Image LoadBanner()
        {
            if (!File.Exists(Path.Combine(this.FilePath, "banner.jpg")))
                return null;
            return Image.FromFile(Path.Combine(this.FilePath, "banner.jpg"));
        }

        public Image LoadPoster()
        {
            if (!File.Exists(Path.Combine(this.FilePath, "poster.jpg")))
                return null;
            return Image.FromFile(Path.Combine(this.FilePath, "poster.jpg"));
        }
    }

    public class FanArtImage
    {
        public string Dimensions { get; set; }
        public string Colors { get; set; }
        public string Url { get; set; }
        public string PreviewUrl { get; set; }

        public XElement CreateXElement()
        {
            XElement element = new XElement("thumb", this.Url);
            if (!String.IsNullOrEmpty(this.Dimensions))
                element.Add(new XAttribute("dim", this.Dimensions));
            if (!String.IsNullOrEmpty(this.Colors))
                element.Add(new XAttribute("colors", this.Colors));
            if (!String.IsNullOrEmpty(this.PreviewUrl))
                element.Add(new XAttribute("preview", this.PreviewUrl));
            return element;
        }
    }
}
