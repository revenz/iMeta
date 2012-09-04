using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace iMetaLibrary
{
    /// <summary>
    /// Class to help with dealing with YouTube
    /// </summary>
    public static class Youtube
    {
        /// <summary>
        /// Gets a movie and saves it
        /// </summary>
        /// <param name="FileLocation">Location to save the file</param>
        /// <param name="Url">The youtube url for the movie</param>
        public static bool GetMovie(string FileLocation, string Url)
        {
            if (File.Exists(FileLocation))
                return true;
            try
            {
                string content = WebHelper.GetWebResponse(Url);

                int start = content.IndexOf("vorbi");
                int end = content.IndexOf("mp4", start) + 3;
                content = content.Substring(start, end - start);

                int start1 = content.LastIndexOf("http");
                int end1 = content.LastIndexOf("mp4") + 3;
                content = content.Substring(start1, end1 - start1);

                content = System.Web.HttpUtility.UrlDecode(content);
                content = System.Web.HttpUtility.UrlDecode(content);

                string videoUrl = new Uri(content).AbsoluteUri;
                WebClient wc = new WebClient();
                wc.DownloadFile(videoUrl, FileLocation);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}