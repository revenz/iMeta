using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
//using iMeta.Views;
using iMetaLibrary.Metadata;

namespace iMetaLibrary.Scanners
{
    public class MovieScanner:Scanner
    {
		public MovieScanner()
        {
        }
        protected override void Scan_Executing(object oFolders)
        {
            TriggerStatusUpdated("Searching for Movies...");

            string[] Folders = oFolders as string[];
            if (Folders == null)
                Folders = Settings.MovieFolders;
            string[] extensions = Settings.VideoExtensions;
            List<Meta> newItems = new List<Meta>();
            foreach (string folder in Folders)
            {
                string[] files = null;
                try
                {
                    files = System.IO.Directory.GetFiles(folder, "*", System.IO.SearchOption.AllDirectories);
                }
                catch (Exception) { continue; }
                foreach (string file in files)
                {
                    if (Items.ContainsKey(file.ToLower()))
                        continue;

                    if (file.ToLower().Contains("-trailer"))
                        continue; // skip trailers
                    string ext = file.Substring(file.LastIndexOf(".") + 1).ToLower();
                    if (!extensions.Contains(ext))
                        continue;
                    Meta meta = new MovieMeta(file);
                    AddItem(meta);
                    newItems.Add(meta);

                }
            }
			
			// sort them, so they are scanned in the order they should appear in a GUI
			newItems.Sort(delegate(Meta a, Meta b)
			             {
							if(a == null && b == null) return 0;
							if(a == null) return -1;
							if(b == null) return 1;
							string aSortTitle = ((MovieMeta)a).Title.ToLower();
							if(aSortTitle.StartsWith("the ")) aSortTitle = aSortTitle.Substring(4);				
							string bSortTitle = ((MovieMeta)b).Title.ToLower();
							if(bSortTitle.StartsWith("the ")) bSortTitle = bSortTitle.Substring(4);
							return aSortTitle.CompareTo(bSortTitle);
						 });
			
            TriggerAllItemsFound(newItems.ToArray());

            foreach (MovieMeta meta in newItems)
            {
                try
                {
                    TriggerScanningItem(meta);
                    int result = meta.Load();
                    TriggerUpdated(meta, result);
                }
                catch (Exception ex) { Logger.Log(ex.Message + Environment.NewLine + ex.StackTrace); }
            }

            TriggerCompleted();
        }
		
		protected override void RefreshFiles_Executing(object oMetaList)
		{
			Meta[] metalist = oMetaList as Meta[];
			if(metalist == null || metalist.Length == 0)
				return;
			
			foreach(MovieMeta meta in metalist)
			{
				try
				{
					TriggerScanningItem(meta);					
					int result = meta.Load(true);
					TriggerUpdated(meta, result);
				}
				catch(Exception ex){ Logger.Log(ex.Message + Environment.NewLine + ex.StackTrace) ; }
			}
			
            TriggerCompleted();
		}
		
		public override string CreateHtmlElement(string BaseDirectory)
		{
			List<MovieMeta> movies = new List<MovieMeta>();
			foreach(MovieMeta meta in this.Items.Values)
				movies.Add(meta);
			
			movies.Sort(delegate(MovieMeta a, MovieMeta b)
			           {
							if(a == null && b == null) return 0;
							if(a == null) return -1;
							if(b == null) return 1;
							string acompareto = !String.IsNullOrWhiteSpace(a.SortTitle) ? a.SortTitle.ToLower() : a.Title.ToLower() ?? "";
							string bcompareto = !String.IsNullOrWhiteSpace(b.SortTitle) ? b.SortTitle.ToLower() : b.Title.ToLower() ?? "";
							if(acompareto.StartsWith("the "))						
								acompareto = acompareto.Substring(4);
							if(bcompareto.StartsWith("the "))
								bcompareto = bcompareto.Substring(4);
							return acompareto.CompareTo(bcompareto);
					   });
			
			string htmltemplate = "     <li class=\"movie\">" + Environment.NewLine +
										"          <span class=\"poster\">{0}</span>"  + Environment.NewLine +
										"          <span class=\"title\">{1}</span>"  + Environment.NewLine +
										"          <span class=\"year\">{2}</span>"  + Environment.NewLine + 
										"          <span class=\"runtime\">{3}</span>"  + Environment.NewLine +
										"          <span class=\"rating {4}\">{5}</span>"  + Environment.NewLine +
										"          <span class=\"directors\">{8}</span>"  + Environment.NewLine +
										"          <span class=\"writers\">{9}</span>"  + Environment.NewLine +
										"          <span class=\"tagline\">{7}</span>"  + Environment.NewLine +
										"          <span class=\"description\">{6}</span>"  + Environment.NewLine +
								  "     </li>";
			
			StringBuilder html = new StringBuilder();
			html.AppendLine("<ul class=\"movies\">");
			foreach(MovieMeta movie in movies)
			{
				// create poster thumb in specified directory
				string imgelement = "";		
				var image = movie.LoadThumbnail();
				if(image != null)
				{
					string shortname = CreateImageThumbnail(image, Path.Combine(BaseDirectory, "images"), 200, 300);
					imgelement = "<img src=\"images/{0}\" alt=\"{0}\" />".FormatStr(shortname.HtmlEncode());
					image.Dispose();
				}
				html.AppendLine(htmltemplate.FormatStr(imgelement, 
				                                       movie.Title.HtmlEncode(), 
				                                       movie.Year > 0 ? movie.Year.ToString() : movie.ReleaseDate > new DateTime(1850,1,1) ? movie.ReleaseDate.Year.ToString() : "",
				                                       movie.Runtime > 0 ? "{0} minutes".FormatStr(movie.Runtime / 60) : "",
				                                       (int)movie.Rating,
				                                       movie.Rating.ToString(""),
			                                       	   (movie.Plot ?? "").HtmlEncode(),
				                                       (movie.TagLine ?? "").HtmlEncode(),
				                                       String.Join(", ", movie.Directors ?? new string[]{}).HtmlEncode(),
				                                       String.Join(", ", movie.Writers ?? new string[]{}).HtmlEncode()));
				                                       
			}
			html.AppendLine("</ul>");
			return html.ToString();
		}
    }
}
