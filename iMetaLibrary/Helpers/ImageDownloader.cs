using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.Drawing.Imaging;

namespace iMetaLibrary.Helpers
{
    class ImageDownloader
    {
		public static void SavePoster (string Url, string Destination)
		{
			using (Image img = DownloadPoster(Url)) {
				if (img != null)
					ImageHelper.SaveImage(img, Destination);
			}
		}
		public static void SaveFanArt (string Url, string Destination)
		{
			using (Image img = DownloadFanArt(Url)) {
				if(img != null)
					ImageHelper.SaveImage(img, Destination);
			}
		}
        public static void SaveBanner(string Url, string Destination)
        {
            using (Image img = DownloadFanArt(Url))
            {
                if (img != null)
                    ImageHelper.SaveImage(img, Destination);
            }
        }

		public static Image DownloadPoster(string Url)
		{
			Image original = DownloadImage(Url);
			if(original == null)
				return null;
			if(original.Width <= Settings.PosterWidth && original.Height <= Settings.PosterHeight)
				return original;
			Image result = ImageHelper.CreateThumbnail(original, Settings.PosterWidth, Settings.PosterHeight, ImageHelper.ImageResizeMode.BestFit);
			original.Dispose();
			original = null;
			return result;
		}

		public static Image DownloadFanArt(string Url)
		{
			Image original = DownloadImage(Url);
			if(original == null)
				return null;
			if(original.Width <= Settings.FanArtWidth && original.Height <= Settings.FanArtHeight)
				return original;
			Image result = ImageHelper.CreateThumbnail(original, Settings.FanArtWidth, Settings.FanArtHeight, ImageHelper.ImageResizeMode.BestFit);
			original.Dispose();
			original = null;
			return result;
		}

        public static Image DownloadBanner(string Url)
        {
            Image original = DownloadImage(Url);
            if (original == null)
                return null;
            if (original.Width <= Settings.BannerWidth && original.Height <= Settings.BannerHeight)
                return original;
            Image result = ImageHelper.CreateThumbnail(original, Settings.BannerWidth, Settings.BannerHeight, ImageHelper.ImageResizeMode.BestFit);
            original.Dispose();
            original = null;
            return result;
        }

        /// <summary>
        /// Function to download Image from website
        /// </summary>
        /// <param name="_URL">URL address to download image</param>
        /// <returns>Image</returns>
        public static Image DownloadImage(string _URL, ImageFormat Format = null)
        {
            if (Format == null)
                Format = ImageFormat.Jpeg;
            string md5 = Helpers.UtilityHelper.CalculateMD5Hash(_URL);
            string cacheFile = Path.Combine(Settings.CachePath, "images", md5 + "." + Format.ToString().ToLower().Replace("jpeg", "jpg"));
            if (!Directory.Exists(Path.Combine(Settings.CachePath, "images")))
                Directory.CreateDirectory(Path.Combine(Settings.CachePath, "images"));

            if (File.Exists(cacheFile))
                try
                {
                    return Image.FromFile(cacheFile);
                }
                catch (Exception)
                {
                    try
                    {
                        File.Delete(cacheFile);
                    }
                    catch (Exception) { }
                }


            try
            {
                // Open a connection
                System.Net.HttpWebRequest _HttpWebRequest = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(_URL);

                _HttpWebRequest.AllowWriteStreamBuffering = true;

                // You can also specify additional header values like the user agent or the referer: (Optional)
                //_HttpWebRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1)";
                //_HttpWebRequest.Referer = "http://www.google.com/";

                // set timeout for 20 seconds (Optional)
                _HttpWebRequest.Timeout = 20000;

                // Request response:
                System.Net.WebResponse _WebResponse = _HttpWebRequest.GetResponse();

                // Open data stream:
                System.IO.Stream _WebStream = _WebResponse.GetResponseStream();

                // convert webstream to image
                using (Image _tmpImage = Image.FromStream(_WebStream))
                {

                    try
                    {
                        _tmpImage.Save(cacheFile, Format);
                    }
                    catch (Exception)
                    {
                        using (Bitmap bitmap = new Bitmap(_tmpImage.Width, _tmpImage.Height, _tmpImage.PixelFormat))
                        {
                            using (Graphics g = Graphics.FromImage(bitmap))
                            {
                                g.DrawImage(_tmpImage, new Rectangle(0, 0, _tmpImage.Width, _tmpImage.Height));
                            }
                            bitmap.Save(cacheFile, Format);
                        }
                    }
                    // Cleanup
                    _WebResponse.Close();
                    _WebResponse.Close();
                }
            }
            catch (Exception _Exception)
            {
                // Error
                Console.WriteLine("Exception caught in process: {0}", _Exception.ToString());
                return null;
            }
            return Image.FromFile(cacheFile);
        }

        public static bool DownloadImage(string Url, string SaveToFileName, ImageFormat Format = null)
        {
            if (String.IsNullOrEmpty(Url))
                return false;
            if (Format == null)
                Format = ImageFormat.Jpeg;
            using (Image image = DownloadImage(Url, Format))
            {
                if (image != null)
                {
                    image.Save(SaveToFileName, Format);
                    return true;
                }
                return false;
            }
        }
    }
}
