using System;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Configuration;

namespace iMetaLibrary
{
	public class Settings
	{
		static Settings()
		{
			Logger.Log("Loading settings");
			
			Settings.MovieFolders = LoadDirectories("MovieFolders");
			Settings.TvFolders = LoadDirectories("TvFolders");
			string strVideoExtensions = AppSettings("VideoExtensions");
			if(String.IsNullOrWhiteSpace(strVideoExtensions))
				strVideoExtensions = "mp4;m4v;wmv;mpeg;mpg;mpe;mpe2;avi;divx;mkv;ts;mov";
			Settings.VideoExtensions = strVideoExtensions.Split(new char[] {';'}, StringSplitOptions.RemoveEmptyEntries);
			Settings.MaxFanArt = AppSettingsIntValue("MaxFanArt", 5);
			Settings.CachePath = AppSettings("CachePath") ?? "cache";
			Settings.AttemptTrailerDownload = AppSettingsBoolValue("AttemptTrailerDownload", false);
			AttemptTrailerDownloadMissingTrailers = AppSettingsBoolValue("AttemptTrailerDownloadMissingTrailers", false);
			Settings.AutoRenameEpisodes = AppSettingsBoolValue("AutoRenameEpisodes", false);
			Settings.AddYearToMovieFolders = AppSettingsBoolValue("AddYearToMovieFolders", false);
			Settings.UseFolderNameForMovieLookup = AppSettingsBoolValue("UseFolderNameForMovieLookup", true);
			Settings.TvRegularExpression = AppSettings("TvRegularExpression");
			
			Settings.TvSeasonExpression = AppSettings("TvSeasonExpression");
			Settings.TvShowTitleExpression = AppSettings("TvShowTitleExpression");
			Settings.TvEpisodeExpression = AppSettings("TvEpisodeExpression");
			
			Settings.FanArtWidth = AppSettingsIntValue("FanArtWidth", 960);
			Settings.FanArtHeight = AppSettingsIntValue("FanArtHeight", 540);
			Settings.PosterWidth = AppSettingsIntValue("PosterWidth", 300);
            Settings.PosterHeight = AppSettingsIntValue("PosterHeight", 450);
            Settings.BannerWidth = AppSettingsIntValue("BannerWidth", 500);
            Settings.BannerHeight = AppSettingsIntValue("BannerHeight", 100);
			Settings.ImageQuality = AppSettingsIntValue("ImageQuality", 70);

            Settings.UseBannersForTvFolders = AppSettingsBoolValue("UseBannersForTvFolders", true);
		}
		
		private static string[] LoadDirectories(string AppSettingName)
		{
			string @value = AppSettings(AppSettingName) ?? "";
			System.Collections.Generic.List<string> results = new System.Collections.Generic.List<string>();
			foreach(string str in @value.Split(new char[] { ';'}, StringSplitOptions.RemoveEmptyEntries))
			{
				string folder = str;
				if(folder.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString()))
					folder = folder.Substring(0, folder.Length - 1);
				results.Add(folder);
			}
			return results.ToArray();
		}
		
		private static string AppSettings(string Key)
		{
			Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\iMeta");
			if(key == null)
				return null;
			object result = key.GetValue(Key);
			return result == null ? null : result.ToString();
		}
		
		private static int AppSettingsIntValue(string Key, int DefaultValue)
		{
			string svalue = AppSettings(Key);
			int result = DefaultValue;
			if(!int.TryParse(svalue, out result))
				result = DefaultValue;
			return result;
		}
		
		private static bool AppSettingsBoolValue(string Key, bool DefaultValue)
		{
			string svalue = AppSettings(Key);
			bool result = DefaultValue;
			if(!bool.TryParse(svalue, out result))
				result = DefaultValue;
			return result;
		}
		
        public static string[] MovieFolders { get; set; }
		public static string[] TvFolders { get; set; }
		public static string[] VideoExtensions { get; set; }
	    public static int MaxFanArt { get; set; }
        public static bool AttemptTrailerDownload { get; set; }
        public static bool AttemptTrailerDownloadMissingTrailers { get; set; }
		public static bool AutoRenameEpisodes { get; set; }
		public static bool AddYearToMovieFolders { get; set; }
		public static bool UseFolderNameForMovieLookup { get; set; }
        public static string CachePath { get; set; }

		public static int FanArtWidth { get; set; }
		public static int FanArtHeight { get; set; }
		public static int PosterWidth { get; set; }
        public static int PosterHeight { get; set; }
        public static int BannerWidth { get; set; }
        public static int BannerHeight { get; set; }
		public static int ImageQuality { get; set; }
        public static bool UseBannersForTvFolders { get; set; }

        private static string _CachePathThumbs;
        public static string CachePathThumbs
        {
            get
            {
                if (_CachePathThumbs == null)
                {
                    _CachePathThumbs = Path.Combine(Settings.CachePath, "Thumbs");
                    if (!Directory.Exists(_CachePathThumbs))
                        Directory.CreateDirectory(_CachePathThumbs);
                }
                return _CachePathThumbs;
            }
        }
		private static string _TvRegularExpression;
		public static string TvRegularExpression
		{
			get { return _TvRegularExpression; }
			set
			{
				bool valid = false;
				try
				{
				 	new System.Text.RegularExpressions.Regex(value); // try parse it
					if(!String.IsNullOrWhiteSpace(value))
						valid = true;
				}
				catch(Exception) { }
				
				_TvRegularExpression = valid ? value : @"(?<=[\\/])([^\\/]+)(?:[\s\-\.]*)[s\.\s]+([\d]{1,2})[ex]?([\d]{2}|(?<=[ex])[\d]{1})";
			}
		}
		
		private static string _TvShowTitleExpression;
		public static string TvShowTitleExpression
		{
			get{ return _TvShowTitleExpression; }
			set 
			{
				bool valid = false;
				try
				{
				 	new System.Text.RegularExpressions.Regex(value); // try parse it
					if(!String.IsNullOrWhiteSpace(value))
						valid = true;
				}
				catch(Exception) { }
				
				_TvShowTitleExpression = valid ? value : @"(?<=(\\|/))[^\\/]+(?=([\\/]{1}(Season|Special|Series)))";
			}
		}
		
		private static string _TvSeasonExpression;
		public static string TvSeasonExpression
		{
			get{ return _TvSeasonExpression; }
			set 
			{
				bool valid = false;
				try
				{
				 	new System.Text.RegularExpressions.Regex(value); // try parse it
					if(!String.IsNullOrWhiteSpace(value))
						valid = true;
				}
				catch(Exception) { }
				
				_TvSeasonExpression = valid ? value : @"(?<!([\d(]))[\d]{1,2}(?=(x|e|[\d]{2}))";
			}
		}
		
		private static string _TvEpisodeExpression;
		public static string TvEpisodeExpression
		{
			get{ return _TvEpisodeExpression; }
			set 
			{
				bool valid = false;
				try
				{
				 	new System.Text.RegularExpressions.Regex(value); // try parse it
					if(!String.IsNullOrWhiteSpace(value))
						valid = true;
				}
				catch(Exception) { }
				
				_TvEpisodeExpression = valid ? value : @"(?<=(e|x|[\d]{1,2}))([\d]{2}\-)*[\d]{2}(?![\d)])";
			}
		}

        public static string TvdbApiKey { get { return "B32D5E15749F2D9E"; } }
		
		public static void SaveSettings()
		{
			Logger.Log("Saving Settings");
			try{
				SaveRegistryValue("MovieFolders", String.Join(";", Settings.MovieFolders ?? new string[] {}));
				SaveRegistryValue("TvFolders", String.Join(";", Settings.TvFolders ?? new string[] {}));
				SaveRegistryValue("VideoExtensions", String.Join(";", Settings.VideoExtensions ?? new string[] {}));
				SaveRegistryValue("MaxFanArt", Settings.MaxFanArt.ToString());
				SaveRegistryValue("AttemptTrailerDownload", Settings.AttemptTrailerDownload.ToString());
				SaveRegistryValue("AttemptTrailerDownloadMissingTrailers", Settings.AttemptTrailerDownloadMissingTrailers.ToString());				
				SaveRegistryValue("AutoRenameEpisodes", Settings.AutoRenameEpisodes.ToString());
				SaveRegistryValue("AddYearToMovieFolders", Settings.AddYearToMovieFolders.ToString());
				SaveRegistryValue("CachePath", Settings.CachePath ?? "");
				SaveRegistryValue("UseFolderNameForMovieLookup", Settings.UseFolderNameForMovieLookup.ToString());
				SaveRegistryValue("TvRegularExpression", Settings.TvRegularExpression);

				SaveRegistryValue("PosterHeight", Settings.PosterHeight.ToString());
				SaveRegistryValue("PosterWidth", Settings.PosterWidth.ToString());
				SaveRegistryValue("FanArtHeight", Settings.FanArtHeight.ToString());
                SaveRegistryValue("FanArtWidth", Settings.FanArtWidth.ToString());
                SaveRegistryValue("BannerHeight", Settings.BannerHeight.ToString());
                SaveRegistryValue("BannerWidth", Settings.BannerWidth.ToString());
				SaveRegistryValue("ImageQuality", Settings.ImageQuality.ToString());
                SaveRegistryValue("UseBannersForTvFolders", Settings.UseBannersForTvFolders.ToString());
			}
			catch(Exception ex)
			{
				Logger.Log(ex.Message + Environment.NewLine + ex.StackTrace);
			}
		}
		
		private static void SaveRegistryValue(string Key, string Value)
		{
			Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\iMeta", true);
			if(key == null)
				key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(@"Software\iMeta");
			key.SetValue(Key, Value);
		}		
	}
}

