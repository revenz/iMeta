using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

namespace iMetaLibrary.Metadata
{
    [Serializable]
    public class VideoMeta:Meta
    {
        public string Title { get; set; }
        public string TitleWithTags
        {
            get { return (this.Title ?? "") + String.Join("", (from t in this.Tags ?? new string[] { } select String.Format(" [{0}]", t)).ToArray()); }
        }
        public string[] Tags { get; set; }
        public int Playcount { get; set; }
        public bool Watched { get; set; }
        public string[] Genres { get; set; }
        public string[] Directors { get; set; }
        public string[] Writers { get; set; }
        public int Runtime { get; set; }
        public string Thumb { get; set; }
        public string Mpaa { get; set; }
        public string Plot { get; set; }
        public DateTime ReleaseDate { get; set; }
        public VideoFileMeta FileInfo { get; set; }

        public string NfoFile { get; protected set; }

        public VideoMeta(string Filename) : base(Filename) { }

        public Image LoadThumbnail(int MaxWidth = 300, int MaxHeight = 300)
        {
            string thumbFile = this.Filename.Substring(0, this.Filename.LastIndexOf(".") + 1) + "tbn";
            if (!File.Exists(thumbFile))
                return null;
            try
            {
				Image img = Image.FromFile(thumbFile);
				if(MaxWidth == 0 && MaxHeight == 0)
					return img; // return full image
				
                int width = MaxWidth;
                int height = (int)(((double)MaxWidth / img.Width) * img.Height);
                if (height > MaxHeight)
                {
                    height = MaxHeight;
                    width = (int)(((double)MaxWidth / img.Height) * img.Width);
                }
                Image small = new Bitmap(img, width, height);
                return small;
				img.Dispose();
				img = null;
            }
            catch (Exception) { return null; }
        }
    }
}
