using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace iMetaLibrary
{
	public class ImageResizer
	{
		public static string ResizeImages (string Folder)
		{
			List<string> images = new List<string> ();
			images.AddRange (Directory.GetFiles (Folder, "*.jpg", SearchOption.AllDirectories));
			images.AddRange (Directory.GetFiles (Folder, "*.tbn", SearchOption.AllDirectories));
			System.Text.StringBuilder invalidFiles = new System.Text.StringBuilder ();
			
			foreach (string file in images) {

				byte[] imagedata = System.IO.File.ReadAllBytes (file);
				using (System.IO.MemoryStream stream = new MemoryStream(imagedata)) {
					stream.Position = 0;
					try {
						using (System.Drawing.Image img = System.Drawing.Image.FromStream(stream)) {
							if (file.ToLower ().Contains ("banner.jpg")) {
								Helpers.ImageHelper.SaveBanner (img, file);
							} else if (file.ToLower ().Contains ("backdrop.jpg") || System.Text.RegularExpressions.Regex.IsMatch (
								file,
								@"fanart(-[\d]+)?.jpg$",
								System.Text.RegularExpressions.RegexOptions.IgnoreCase
							)) {
								Helpers.ImageHelper.SaveFanArt (img, file);
							} else {
								Helpers.ImageHelper.SavePoster (img, file);
							}
						}
					} catch (Exception ex) {
						invalidFiles.AppendLine (file);
					}
				}
			}
			return invalidFiles.ToString ();
		}

        public static void SetBannerAsFolderImage(string Folder)
        {
            string[] files = Directory.GetFiles(Folder, "banner.jpg", SearchOption.AllDirectories);
            foreach (string file in files)
            {
                FileInfo fileinfo = new FileInfo(file);
                string folderJpg = Path.Combine(fileinfo.Directory.FullName, "folder.jpg");
                try
                {
                    fileinfo.CopyTo(folderJpg, true);
                }
                catch (Exception) { }
            }
        }
	}
}

