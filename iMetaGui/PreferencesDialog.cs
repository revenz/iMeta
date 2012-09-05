using System;
using System.Collections.Generic;
using iMetaGui;
using iMetaLibrary;
using Gtk;
using Cairo;

namespace iMetaGui
{
	public partial class PreferencesDialog : Gtk.Dialog
	{
		ListStore storeVideoExtensions;
		NodeStore storeMovieFolders, storeTvFolders;

		
		public PreferencesDialog ()
		{
			this.Build ();

			notebook1.ShowTabs = false;
			notebook1.ShowBorder = false;
			
			ChangeTab(0);
			
			storeMovieFolders = new NodeStore (typeof(StringNode));
			storeTvFolders = new NodeStore (typeof(StringNode));
			
			foreach (string folder in iMetaLibrary.Settings.MovieFolders)
				storeMovieFolders.AddNode (new StringNode (){ Value = folder});
			nvMovieFolders.NodeStore = storeMovieFolders;
			nvMovieFolders.AppendColumn ("Folder", new Gtk.CellRendererText (), "text", 0);
			
			foreach (string folder in iMetaLibrary.Settings.TvFolders)
				storeTvFolders.AddNode (new StringNode (){ Value = folder});
			nvTvFolders.NodeStore = storeTvFolders;
			nvTvFolders.AppendColumn ("Folder", new Gtk.CellRendererText (), "text", 0);
			
			
			btnMovieFolderAdd.Clicked += delegate {
				AddFolder (storeMovieFolders);
			};
			btnTvFolderAdd.Clicked += delegate {
				AddFolder (storeTvFolders);
			};
			btnMovieFolderRemove.Clicked += delegate {
				RemoveFolder (nvMovieFolders, storeMovieFolders);
			};
			btnTvFolderRemove.Clicked += delegate {
				RemoveFolder (nvTvFolders, storeTvFolders);
			};	 
			eventTabGeneral.ButtonPressEvent += delegate {
				ChangeTab(0);
			};
			eventTabMovies.ButtonPressEvent += delegate {
				ChangeTab (1);
			};
			eventTabTvShows.ButtonPressEvent += delegate {
				ChangeTab (2);
			};
			eventTabAdvanced.ButtonPressEvent += delegate {
				ChangeTab (3);
			};
			eventTabMovies.ExposeEvent += delegate {
				
				using (Cairo.Context ctx = Gdk.CairoHelper.Create(eventTabMovies.GdkWindow)) {
					//ctx.SetSourceRGB(0.2, 0.23, 0.9);
					//ctx.Rectangle(0, 0, 200, 200);
					//ctx.Fill();
					/*
					Cairo.Gradient pat = new Cairo.LinearGradient (0, 0, 100, 100);
					pat.AddColorStop (0, new Cairo.Color (0,0,0));
					pat.AddColorStop (1, new Cairo.Color (1,1,1));
					ctx.Pattern = pat;
					*/
					
					// Shape
					ctx.MoveTo (new PointD (0, 0));
					ctx.MoveTo (new PointD (200, 0));
					ctx.MoveTo (new PointD (200, 200));
					ctx.MoveTo (new PointD (0, 0));
			        
					ctx.ClosePath ();
					// Save the state to restore it later. That will NOT save the path
					ctx.Save ();
					Cairo.Gradient pat = new Cairo.LinearGradient (100, 200, 200, 100);
					pat.AddColorStop (0, new Cairo.Color (0, 0, 0, 1));
					pat.AddColorStop (1, new Cairo.Color (1, 0, 0, 1));
					ctx.Pattern = pat;
			 
					// Fill the path with pattern
					ctx.FillPreserve ();
			 
					// We "undo" the pattern setting here
					ctx.Restore ();
			 
					// Color for the stroke
					ctx.Color = new Color (0, 0, 0);
			 
					ctx.LineWidth = 3;
					ctx.Stroke ();
				}
			};
			
			
			fcbCachePath.SetUri (iMetaLibrary.Settings.CachePath);
			
			storeVideoExtensions = new ListStore (typeof(string));
			List<string> extensions = new List<string> (iMetaLibrary.Settings.VideoExtensions);
			extensions.Sort ();
			foreach (string extension in extensions)
				storeVideoExtensions.AppendValues (extension);
			nvVideoExtensions.Model = storeVideoExtensions;
			
			nvVideoExtensions.AppendColumn ("Extension", new CellRendererText (), "text", 0);
			
			txtTvRegex.Text = iMetaLibrary.Settings.TvRegularExpression;
			txtTvEpisodeNumbersRegex.Text = iMetaLibrary.Settings.TvEpisodeExpression;
			txtTvSeasonRegex.Text = iMetaLibrary.Settings.TvSeasonExpression;
			txtTvShowTitleRegex.Text = iMetaLibrary.Settings.TvShowTitleExpression;
			
			numMaxFanArt.Value = iMetaLibrary.Settings.MaxFanArt;
			chkAddYearToMovieFolders.Active = iMetaLibrary.Settings.AddYearToMovieFolders;
			chkAttempToDownloadTrailers.Active = iMetaLibrary.Settings.AttemptTrailerDownload;
			chkAttemptToDownloadMissingTrailers.Active = iMetaLibrary.Settings.AttemptTrailerDownloadMissingTrailers;
			chkUseFolderNameForMovieLookup.Active = iMetaLibrary.Settings.UseFolderNameForMovieLookup;
			
			chkAutoRenameEpisodes.Active = iMetaLibrary.Settings.AutoRenameEpisodes;

			txtFanArtResolution.Text = iMetaLibrary.Settings.FanArtWidth + "x" + iMetaLibrary.Settings.FanArtHeight;
			txtPosterResolution.Text = iMetaLibrary.Settings.PosterWidth + "x" + iMetaLibrary.Settings.PosterHeight;
			txtBannerResolution.Text = iMetaLibrary.Settings.BannerWidth + "x" + iMetaLibrary.Settings.BannerHeight;
			txtImageQuality.Text = iMetaLibrary.Settings.ImageQuality.ToString ();
			chkUseBannersForFolderImage.Active = iMetaLibrary.Settings.UseBannersForTvFolders;
												
			buttonOk.Clicked += HandleButtonOkClicked;
			/*
			buttonCancel.Clicked += delegate
			{
				this.DialogResult = Gtk.ResponseType.Cancel;
				this.Destroy();				
			};*/
			
			btnAddVideoExtension.Clicked += delegate {
				
			};
			btnRemoveVideoExtension.Clicked += delegate {
				
			};
		}

		void ChangeTab (int TabIndex)
		{
			notebook1.Page = TabIndex;
			imgTabAdvanced.Pixbuf = Images.TabAdvancedInactive;
			imgTabGeneral.Pixbuf = Images.TabGeneralInactive;
			imgTabMovies.Pixbuf = Images.TabMoviesInactive;
			imgTabTvShows.Pixbuf = Images.TabTvInactive;
			switch (TabIndex) {
				case 0: 
					imgTabGeneral.Pixbuf = Images.TabGeneral;
					break;
				case 1: 
					imgTabMovies.Pixbuf = Images.TabMovies;
					break;
				case 2: 
					imgTabTvShows.Pixbuf = Images.TabTv;
					break;
				case 3: 
					imgTabAdvanced.Pixbuf = Images.TabAdvanced;
					break;
			}
		}

		void HandleButtonOkClicked (object sender, EventArgs e)
		{
			// check resolutions
			System.Text.RegularExpressions.Regex rgxDimensions = new System.Text.RegularExpressions.Regex (@"^[\d]+x[\d]+$");
			if (!rgxDimensions.IsMatch (txtFanArtResolution.Text)) {
				MessageBox.Show ("Invalid Fan Art Resolution");
				return;
			}
			if (!rgxDimensions.IsMatch (txtPosterResolution.Text)) {
				MessageBox.Show ("Invalid Poster Resolution");
				return;
			}
			if (!rgxDimensions.IsMatch (txtBannerResolution.Text)) {
				MessageBox.Show ("Invalid Banner Resolution");
				return;
			}
			iMetaLibrary.Settings.FanArtWidth = int.Parse (txtFanArtResolution.Text.Split ('x') [0]);
			iMetaLibrary.Settings.FanArtHeight = int.Parse (txtFanArtResolution.Text.Split ('x') [1]);
			iMetaLibrary.Settings.PosterWidth = int.Parse (txtPosterResolution.Text.Split ('x') [0]);
			iMetaLibrary.Settings.PosterHeight = int.Parse (txtPosterResolution.Text.Split ('x') [1]);
			iMetaLibrary.Settings.BannerWidth = int.Parse (txtBannerResolution.Text.Split ('x') [0]);
			iMetaLibrary.Settings.BannerHeight = int.Parse (txtBannerResolution.Text.Split ('x') [1]);
			iMetaLibrary.Settings.ImageQuality = int.Parse (txtImageQuality.Text);
			iMetaLibrary.Settings.UseBannersForTvFolders = chkUseBannersForFolderImage.Active;

			// save cache path
			iMetaLibrary.Settings.CachePath = fcbCachePath.Filename;
			
			// video file extensions
			var enumerator = storeVideoExtensions.GetEnumerator();
			List<string> extensions = new List<string>();
			while(enumerator.MoveNext())
			{
				if(enumerator.Current == null)
					continue;
				string @value = enumerator.Current as string;
				if(@value == null && enumerator.Current is object[] && ((object[])enumerator.Current).Length > 0)
					@value = ((object[])enumerator.Current)[0] as string;
				string extension = (@value ?? "").ToLower().Trim();
				if(extension.StartsWith("."))
					extension = extension.Substring(1);
				if(!extensions.Contains(extension) && !String.IsNullOrWhiteSpace(extension))
					extensions.Add(extension);
			}
			iMetaLibrary.Settings.VideoExtensions = extensions.ToArray();
			
			// movie settings
			iMetaLibrary.Settings.UseFolderNameForMovieLookup = chkUseFolderNameForMovieLookup.Active;
			iMetaLibrary.Settings.AddYearToMovieFolders = chkAddYearToMovieFolders.Active;
			iMetaLibrary.Settings.AttemptTrailerDownloadMissingTrailers = chkAttemptToDownloadMissingTrailers.Active;
			iMetaLibrary.Settings.AttemptTrailerDownload = chkAttempToDownloadTrailers.Active;
			iMetaLibrary.Settings.MaxFanArt = (int)numMaxFanArt.Value;
			iMetaLibrary.Settings.MovieFolders = GetStoreDirectories(storeMovieFolders);
			
			// tv settings
			iMetaLibrary.Settings.AutoRenameEpisodes = chkAutoRenameEpisodes.Active;
			iMetaLibrary.Settings.TvFolders = GetStoreDirectories(storeTvFolders);
			
			// advanced settings
			iMetaLibrary.Settings.TvRegularExpression = txtTvRegex.Text;
			iMetaLibrary.Settings.TvEpisodeExpression = txtTvEpisodeNumbersRegex.Text;
			iMetaLibrary.Settings.TvSeasonExpression = txtTvSeasonRegex.Text;
			iMetaLibrary.Settings.TvShowTitleExpression = txtTvShowTitleRegex.Text;
			
			iMetaLibrary.Settings.SaveSettings();
				
			
			this.Destroy();			
		}
		
		string[] GetStoreDirectories(NodeStore Store)
		{
			List<string> nodes = new List<string>();				
			var enumerator = Store.GetEnumerator();
			while(enumerator.MoveNext())
			{
				string folder = ((StringNode)enumerator.Current).Value;
				if(folder.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString()))
					folder = folder.Substring(0, folder.Length - 1);
				nodes.Add(folder);
			}
			return nodes.ToArray();
		}
		
		void AddFolder(NodeStore Store)
		{
            if (PlatformDetection.IsWindows)
            {
                using (Ionic.Utils.FolderBrowserDialogEx dialog = new Ionic.Utils.FolderBrowserDialogEx())
				{
                    dialog.Description = "Choose the Folder to scan.";
                    dialog.ShowNewFolderButton = false;
                    dialog.ShowFullPathInEditBox = true;
                    if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
					{
                        if (!String.IsNullOrWhiteSpace(dialog.SelectedPath))
                        {
                            try
                            {
                                Store.AddNode(new StringNode() { Value = new System.IO.DirectoryInfo(dialog.SelectedPath).FullName });
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }
					}
				}
            }
            else
            {
                using (Gtk.FileChooserDialog fc = new Gtk.FileChooserDialog("Choose the Folder to scan.",
                                                                            this,
                                                                            FileChooserAction.SelectFolder,
                                                                            "Cancel", ResponseType.Cancel,
                                                                            "Open", ResponseType.Accept))
                {
                    fc.LocalOnly = false;
                    if (fc.Run() == (int)ResponseType.Accept)
                    {
                        try
                        {
                            Store.AddNode(new StringNode() { Value = new System.IO.DirectoryInfo(fc.Filename).FullName });
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                    //Don't forget to call Destroy() or the FileChooserDialog window won't get closed.
                    fc.Destroy();
                }
            }
		}
		
		void RemoveFolder(NodeView View, NodeStore Store)
		{	
			foreach(var node in View.NodeSelection.SelectedNodes)
				Store.RemoveNode(node);
		}
	}
}

