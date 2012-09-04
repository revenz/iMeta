﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Diagnostics;

namespace iMetaLibrary
{
    class WebHelper
    {
        public static bool DownloadTo(string Url, string SaveToFilename)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(Url, SaveToFilename);
                    return File.Exists(SaveToFilename);
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                return false;
            }
        }

        public static string GetWebResponse(string Url, int CacheSeconds = 60)
        {
            string cachePath = Path.Combine(Settings.CachePath, "Web");
            if (!Directory.Exists(cachePath))
                Directory.CreateDirectory(cachePath);
            string cacheFile = Path.Combine(cachePath, Helpers.UtilityHelper.CalculateMD5Hash(Url));

            if (File.Exists(cacheFile) && new FileInfo(cacheFile).LastWriteTime > DateTime.Now.AddSeconds(-CacheSeconds))
                return File.ReadAllText(cacheFile);
            
            // Open a connection
            System.Net.HttpWebRequest _HttpWebRequest = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(Url);

            _HttpWebRequest.AllowWriteStreamBuffering = true;
            
            // set timeout for 20 seconds (Optional)
            _HttpWebRequest.Timeout = 20000;

            // Request response:
            using (System.Net.WebResponse _WebResponse = _HttpWebRequest.GetResponse())
            {

                // Open data stream:
                using (System.IO.Stream _WebStream = _WebResponse.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(_WebStream))
                    {
                        string response = reader.ReadToEnd();
                        File.WriteAllText(cacheFile, response);
                        return response;
                    }
                }
            }
        }
    }
}
