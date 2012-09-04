using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;

namespace iMetaLibrary.Helpers
{
    public class ImageHelper
    {
        public enum ImageResizeMode
        {
            Trim = 0,
            Pad = 1,
            Stretch = 2,
            Crop = 3,
            CenterTop = 4,
			BestFit = 5
        }

        public static Image CreateThumbnail (Image Source, int MaxWidth, int MaxHeight, ImageResizeMode Resize = ImageResizeMode.Trim)
		{
			int newImageWidth = MaxWidth;
			int newImageHeight = MaxHeight;

			int xpos = 0;
			int ypos = 0;
			int width = MaxWidth;
			int height = (int)(((double)MaxWidth / Source.Width) * Source.Height);
			if (height > MaxHeight) {
				ypos = 0;
				height = MaxHeight;
				width = (int)(((double)MaxHeight / Source.Height) * Source.Width);
			}

			if (Resize == ImageResizeMode.Crop) {
				width = MaxWidth;
				height = (int)(((double)MaxWidth / Source.Width) * Source.Height);
				if (height < MaxHeight) {
					ypos = 0;
					height = MaxHeight;
					width = (int)(((double)MaxHeight / Source.Height) * Source.Width);
				}
			}

			if (Resize != ImageResizeMode.Stretch) {
				ypos = (MaxHeight - height) / 2;
				xpos = (MaxWidth - width) / 2;
			}

			if (Resize == ImageResizeMode.Trim) {
				newImageHeight = height;
				newImageWidth = width;
			}

			if (Resize == ImageResizeMode.BestFit) {				
				xpos = 0;
				ypos = 0;
				newImageHeight = height;
				newImageWidth = width;
			}

            if (Resize == ImageResizeMode.CenterTop)
            {
                height -= ypos;
                ypos = 0;
                newImageHeight = height;
            }
			
			if(Resize == ImageResizeMode.Stretch)
			{
				xpos = 0;
				ypos = 0;
				width = newImageWidth;
				height= newImageHeight;
			}
			
			//return Source.GetThumbnailImage(width, height, null, IntPtr.Zero);
			Image thumb = new Bitmap(newImageWidth, newImageHeight);
            using (Graphics g = Graphics.FromImage(thumb))
            {
				g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
				g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
				g.DrawImage(Source, xpos, ypos, width, height);
            }
            return thumb;

        }

        public static void CreateThumbnail(string Source, string Destination, int MaxWidth, int MaxHeight, ImageResizeMode Resize = ImageResizeMode.Trim)
        {
            using (Image img = Image.FromFile(Source))
            {
                using (Image thumb = CreateThumbnail(img, MaxWidth, MaxHeight, Resize))
                {
                    thumb.Save(Destination);
                }
            }
        }

        public static string ImageToBase64(Image Image, System.Drawing.Imaging.ImageFormat Format = null)
        {
            if (Format == null)
                Format = System.Drawing.Imaging.ImageFormat.Jpeg;
            using (MemoryStream ms = new MemoryStream())
            {
                // Convert Image to byte[]
                Image.Save(ms, Format);
                byte[] imageBytes = ms.ToArray();

                // Convert byte[] to Base64 String
                string base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
        }
		
		
		public static void SaveImage (System.Drawing.Image Image, string Destination)
		{
			System.Drawing.Imaging.ImageCodecInfo jgpEncoder = GetEncoder(System.Drawing.Imaging.ImageFormat.Jpeg);
			System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
			System.Drawing.Imaging.EncoderParameters myEncoderParameters = new System.Drawing.Imaging.EncoderParameters(1);
			System.Drawing.Imaging.EncoderParameter myEncoderParameter = new System.Drawing.Imaging.EncoderParameter(myEncoder, (long)Settings.ImageQuality);
			myEncoderParameters.Param[0] = myEncoderParameter;
			
            Image.Save(Destination, jgpEncoder, myEncoderParameters);
		}

		private static System.Drawing.Imaging.ImageCodecInfo GetEncoder(System.Drawing.Imaging.ImageFormat format)
		{
		    System.Drawing.Imaging.ImageCodecInfo[] codecs = System.Drawing.Imaging.ImageCodecInfo.GetImageDecoders();

		    foreach (System.Drawing.Imaging.ImageCodecInfo codec in codecs)
		    {
		        if (codec.FormatID == format.Guid)
		        {
		            return codec;
		        }
		    }
		    return null;
		}
		
		public static void SavePoster (Image Original, string Destination)
        {
            SaveImage(Original, Destination, Settings.PosterWidth, Settings.PosterHeight);
		}
		
		public static void SaveFanArt (Image Original, string Destination)
        {
            SaveImage(Original, Destination, Settings.FanArtWidth, Settings.FanArtHeight);
		}

        public static void SaveBanner(Image Original, string Destination)
        {
            SaveImage(Original, Destination, Settings.BannerWidth, Settings.BannerHeight);
        }

        private static void SaveImage(Image Original, string Destination, int DesiredWidth, int DesiredHeight)
        {
			if (Original == null)
				return;
			if (Original.Width <= DesiredWidth && Original.Height <= DesiredHeight) {
				SaveImage (Original, Destination);
			} else {
				Image result = ImageHelper.CreateThumbnail (
					Original,
					DesiredWidth,
					DesiredHeight,
					ImageHelper.ImageResizeMode.BestFit
				);
				SaveImage (result, Destination);
			}

        }
    }
}
