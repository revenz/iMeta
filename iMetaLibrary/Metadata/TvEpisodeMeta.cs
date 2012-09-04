using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using System.Text.RegularExpressions;
using iMetaLibrary.Helpers;

namespace iMetaLibrary.Metadata
{
    [Serializable]
    public class TvEpisodeMeta:VideoMeta
    {
        public int Season { get; set; }
        public int Episode { get; set; }
        public DateTime Aired { get; set; }
        public DateTime Premiered { get; set; }
        public string Studio { get; set; }
        public int DisplaySeason { get; set; }
        public int DisplayEpisode { get; set; }
        public KeyValuePair<string, string>[] Actors { get; set; }
        public string ShowTitle { get; set; }
        public int TvdbId { get; set; }
        public int TvdbSeriesId { get; set; }
        /// <summary>
        /// Gets or sets the number of seconds into the file the episode begins at, use for multiple part episodes.
        /// </summary>
        public int EpBookmark { get; set; }

        public TvEpisodeMeta(string Filename):base(Filename)
        {
            this.FileInfo = VideoFileMeta.Load(Filename);
        }

        public XElement CreateXElement()
        {
            string filenameNoExtension = Filename.Substring(0, Filename.LastIndexOf("."));
            XElement element = new XElement("episodedetails");
            if(this.TvdbId > 0)
                element.Add(new XElement("tvdbid", this.TvdbId));
            if (this.TvdbSeriesId > 0)
                element.Add(new XElement("tvdbseriesid", this.TvdbSeriesId));
            element.Add(new XElement("title", this.Title ?? ""));
            element.Add(new XElement("showtitle", this.ShowTitle ?? ""));
            if(this.Rating > 0)
                element.Add(new XElement("rating", this.Rating));
            if(this.Aired > new DateTime(1910, 1,1)) // i doubt think anyone will have a tv show from before 1910 :)
                element.Add(new XElement("aired", this.Aired.ToString("yyyy-MM-dd")));
            if (this.Premiered > new DateTime(1910, 1, 1))
                element.Add(new XElement("premiered", this.Premiered.ToString("yyyy-MM-dd")));
            element.Add(new XElement("season", this.Season));
            element.Add(new XElement("episode", this.Episode));
            element.Add(new XElement("displayseason", this.DisplaySeason));
            element.Add(new XElement("displayepisode", this.DisplayEpisode));
            element.Add(new XElement("plot", this.Plot));
			if(this.EpBookmark > 0)
            	element.Add(new XElement("epbookmark", this.EpBookmark));
            if(!String.IsNullOrEmpty(this.Thumb))
            {
                string strThumb = this.Thumb;
                if(this.Thumb.StartsWith("http"))
                {
                    FileInfo thumbFile = new FileInfo(filenameNoExtension + ".tbn");
                    if (thumbFile.Exists || ImageDownloader.DownloadImage(this.Thumb, thumbFile.FullName)) // only set to local file if exists or successfully downloads, if fails keep the URL in there
                        strThumb = thumbFile.Name;
                }
                element.Add(new XElement("thumb", strThumb));
            }

            if(this.Directors != null)
            {
                foreach(string director in this.Directors)
                    element.Add(new XElement("director", director));
            }
            if(!String.IsNullOrEmpty(this.Studio))
                element.Add(new XElement("studio", this.Studio));
            if(!String.IsNullOrEmpty(Mpaa))
                element.Add(new XElement("mpaa", this.Mpaa));
            if(this.Actors != null)
            {
                foreach (KeyValuePair<string, string> actor in this.Actors)
                {
                    XElement xActor = new XElement("actor", new XElement("name", actor.Key));
                    if (!String.IsNullOrEmpty(actor.Value))
                        xActor.Add(new XElement("role", actor.Value));
                    element.Add(xActor);
                }
            }
            if(this.FileInfo != null)
            {
                XElement fileinfo = this.FileInfo.CreateXElement();
                if(fileinfo != null)
                    element.Add(fileinfo);
            }
            return element;
        }

        public override string ToString()
        {
            return this.Title;
        }

        public TvEpisodeMeta Clone()
        {
            try
            {
                return Helpers.ObjectCopier.Clone(this);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
