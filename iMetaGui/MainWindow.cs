using System;
using Gtk;
using iMetaLibrary;
using iMetaLibrary.Metadata;
using System.Collections.Generic;
using iMetaGui.GuiComponents;

namespace iMetaGui
{

	public partial class MainWindow: Gtk.Window
	{	
		iMetaLibrary.Scanners.TvScanner TvScanner;
		iMetaLibrary.Scanners.MovieScanner MovieScanner;
		System.Timers.Timer Timer;
		
		public MainWindow (): base (Gtk.WindowType.Toplevel)
		{
			Build ();
			
			#region decorate the window all pretty like
			this.Decorated = false;		
			this.BorderWidth = 1;
			this.ModifyBg(StateType.Normal, new Gdk.Color(0, 0, 0));
			#endregion
						
			#region setup the log		
			txtLog.Editable = false;
			iMetaLibrary.Logger.LogMessage += delegate(string RawMessage, string FormattedMessage) {
				Gtk.Application.Invoke(delegate
	          	{
					txtLog.Buffer.PlaceCursor(txtLog.Buffer.EndIter);
					txtLog.Buffer.InsertAtCursor(FormattedMessage + Environment.NewLine);
				});
			};
			iMetaLibrary.Logger.Log("iMeta Started");
			#endregion
			
			notebook1.ShowTabs = false;
			
			if(PlatformDetection.IsMac)
			{
				topbar.Dragged += delegate(int MoveX, int MoveY) {		
					if((this.GdkWindow.State & Gdk.WindowState.Maximized) == Gdk.WindowState.Maximized)
						return;
					int winx, winy;
					this.GdkWindow.GetPosition(out winx, out winy);
					this.GdkWindow.Move(winx + MoveX, winy + MoveY);
			};
			}
			else
			{
				topbar.DragBegin2 += delegate(int MouseButton, int RootX, int RootY, uint TimeStamp) {
					this.GdkWindow.BeginMoveDrag(MouseButton, RootX, RootY, TimeStamp);
				};
			}
			topbar.CloseClicked += delegate { Application.Quit(); };
			topbar.MaximizeClicked += delegate 
			{ 
				if((this.GdkWindow.State & Gdk.WindowState.Maximized) == Gdk.WindowState.Maximized)
					this.Unmaximize();
				else
					this.Maximize();
			};
			topbar.MinimizeClicked += delegate { this.GdkWindow.Iconify (); };
			
			topbar.MoviesClicked += delegate
			{ 
				notebook1.Page = 0;
				topbar.SetStatusText(MovieStatusText);
				topbar.IsStatusPulsing = MovieScanner.IsRunning;
			};
			topbar.TvShowsClicked += delegate 
			{
				nbkTvShows.Page = 0;	
				notebook1.Page = 1; 
				topbar.SetStatusText(TvStatusText);
				topbar.IsStatusPulsing = TvScanner.IsRunning;
			};
			
			vboxTvShows.Spacing = 0;
			nbkTvShows.ShowTabs = false;
			nbkTvShows.Page = 0;
			
			topbar.MoviesButtonIsActive = true;
			
			this.notebook1.Page = 0; // start on movies tab
				
			SetupTvScanner();
			
			SetupMovieScanner();
			
			#region setup the timer for auto scans
			this.Timer = new System.Timers.Timer(1 * 1000); // scan after 1 seconds
			this.Timer.Elapsed += delegate {
				//this.MovieScanner.Scan();
				//this.TvScanner.Scan();	
				
				this.Timer.AutoReset = true;
				this.Timer.Interval = 3 * 60 * 60 * 1000;  // scan every three hours
			};
			this.Timer.Start();
			#endregion
			topbar.ScanClicked += delegate
			{
				if(notebook1.Page == 0)
					MovieScanner.Scan();
				else if(notebook1.Page == 1)
					TvScanner.Scan();
				topbar.IsStatusPulsing = true;
			};
			topbar.RefreshClicked += delegate
			{
				if(notebook1.Page == 0)
				{
					MovieMeta[] movies = guiMovieList.GetSelected();
					MovieScanner.RefreshFiles(movies);
				}
				else if(notebook1.Page == 1)
				{
					// todo: refresh tv shows
				}
			};
			topbar.PreferencesClicked += delegate
			{
				Gtk.Application.Invoke(delegate {
					using(PreferencesDialog dialog = new PreferencesDialog())
					{
						dialog.Run();
						dialog.Destroy();
					}		
				});
			};
			topbar.ExportClicked += delegate {
				if(MovieScanner.IsRunning){
					MessageBox.Show("Cannot export while Movie scanner is running.");
					return;	
				}
				if(TvScanner.IsRunning){
					MessageBox.Show("Cannot export while TV scanner is running.");
					return;	
				}
				using(ExportingDialog dialog = new ExportingDialog(MovieScanner, TvScanner))
				{
					dialog.Run();
					dialog.Destroy();
				}
			};
			
		}
		
		protected void OnDeleteEvent (object sender, DeleteEventArgs a)
		{
			this.Timer.Stop();
			Application.Quit();
			a.RetVal = true;
		}
		
		protected void btnTvScan_Activated (object sender, System.EventArgs e)
		{
			this.TvScanner.Scan();
		}
		
		private string TvStatusText = "", MovieStatusText = "";
		
		private void UpdateStatusText(string Text, bool MovieText)
		{
			Gtk.Application.Invoke(delegate
            {
				if(MovieText)
				{
					MovieStatusText = Text;
					if(notebook1.Page == 0)
						topbar.SetStatusText(Text);
				}
				else
				{
					TvStatusText = Text;
					if(notebook1.Page == 1)
						topbar.SetStatusText(Text);
				}
			});
		}
		
		private void SetupTvScanner()
		{		
			#region setup the tv scanner
			this.TvScanner = new iMetaLibrary.Scanners.TvScanner();	
			this.TvScanner.Started += delegate{  };
			this.TvScanner.Completed += delegate
			{ 
				if(notebook1.Page == 1)
					topbar.IsStatusPulsing = false;
				UpdateStatusText("TV Shows: {0}, Episodes: {1}".FormatStr(TvScanner.NumberOfShows, TvScanner.Items.Count), false);
			};
			this.TvScanner.ItemAdded += delegate(Meta Item) 
			{ 		
				UpdateStatusText("Found Episode: {0}".FormatStr(Item.Filename), false);
			};
			this.TvScanner.ScanningItem += delegate(Meta Item) 
			{ 
				UpdateStatusText("Scanning File: {0}".FormatStr(Item.Filename), false);
			};
			this.TvScanner.AllItemsFound += delegate(iMetaLibrary.Metadata.Meta[] NewItems) {
				List<TvShowNode> storeItems = new List<TvShowNode>();
				foreach(string key in TvScanner.Shows.Keys) // need to check if already in the store...
				{
					List<TvFileMeta> episodes = TvScanner.Shows[key];
					// construct these off the main thread
					
					var enumerator = storeItems.GetEnumerator();
					bool showfound = false;
					while(enumerator.MoveNext())
					{
						TvShowNode node = (TvShowNode)enumerator.Current;
						if(node.Title.ToLower() == episodes[0].ShowTitle.ToLower())
						{
							showfound = true;
							node.NumberOfEpisodes = episodes.Count;
							break;
						}
					}
					if(!showfound)
					{
						System.Drawing.Image poster = episodes[0].GetPoster();
						storeItems.Add(new TvShowNode() 
						              { 
											Title = episodes[0].ShowTitle, 
											NumberOfEpisodes = episodes.Count, 
											Poster = poster == null ? Images.NoPosterSmallPixbuf : GuiHelper.ImageToPixbufIcon(poster, 120, 180) 
									  });
						
						if(poster != null)
							poster.Dispose();
					}
				}
				guiTvShowList.AddToStore(storeItems);
			};
			guiTvShowList.ItemActivated += delegate(string Label)
			{
				if(String.IsNullOrEmpty(Label) || TvScanner.Shows == null || !TvScanner.Shows.ContainsKey(Label))
					return;
				TvShowMeta showmeta = TvScanner.Shows[Label][0].LoadShowMeta();
				
				Gtk.Application.Invoke(delegate
	            {
					// show the tv show gui
					guiTvShow.InitializeShow(showmeta, TvScanner.Shows[Label].ToArray());				
					topbar.TvShowsButtonIsActive = false;
					nbkTvShows.Page = 1;	
				});			
			};
			/*
			btnTvBack.Activated += delegate {
				nbkTvShows.Page = 0;
			};
			btnTvFolders.Activated += delegate {
				using(iMetaGui.FolderListGui flg = new iMetaGui.FolderListGui("TV Folders", "Here you can configure the folders to scan for TV episodes.", iMetaLibrary.Settings.TvFolders)){
					flg.Run();
					if(flg.DialogResult == ResponseType.Ok)
					{
						iMetaLibrary.Settings.TvFolders = flg.Folders;
						iMetaLibrary.Settings.SaveSettings();
					}
				}
			};*/
			#endregion
			
		}
		
		
		private void SetupMovieScanner()
		{		
			#region setup the movie scanner			
			this.MovieScanner = new iMetaLibrary.Scanners.MovieScanner();	
			this.MovieScanner.Started += delegate{ };
			this.MovieScanner.Completed += delegate
			{ 
				if(notebook1.Page == 0)
					topbar.IsStatusPulsing = false;
				UpdateStatusText("Movies Found: {0}".FormatStr(this.MovieScanner.Items.Count), true);
			};
			this.MovieScanner.ItemAdded += delegate(Meta Item) 
			{ 			
				UpdateStatusText("Movie Found: {0}".FormatStr(Item.Filename), true);
			};
			this.MovieScanner.ScanningItem += delegate(Meta Item) 
			{ 
				UpdateStatusText("Scanning File: {0}".FormatStr(Item.Filename), true);
			};
			this.MovieScanner.AllItemsFound += delegate(iMetaLibrary.Metadata.Meta[] NewItems) {
				
				SortedDictionary<string, List<iMetaLibrary.Metadata.MovieMeta>> storeItems = new SortedDictionary<string, List<iMetaLibrary.Metadata.MovieMeta>>();
				foreach(iMetaLibrary.Metadata.MovieMeta meta in NewItems)
				{
					string indexer = meta.StartChar.ToString();
					if(!storeItems.ContainsKey(indexer))
						storeItems.Add(indexer, new List<MovieMeta>());
					storeItems[indexer].Add(meta);
				}
				guiMovieList.AddToStore(storeItems);
			};
			guiMovieList.ItemActivated += delegate(MovieMeta Meta)
			{
				using(iMetaGui.MovieEditor editor = new iMetaGui.MovieEditor(Meta))
				{
					editor.Run();
					editor.Destroy();
				}
			};
			/*
			btnMoviesFolders1.Activated += delegate {
				using(iMetaGui.FolderListGui flg = new iMetaGui.FolderListGui("Movie Folders", "Here you can configure the folders to scan for Movies.", iMetaLibrary.Settings.MovieFolders)){
					flg.Run();
					if(flg.DialogResult == ResponseType.Ok)
					{
						iMetaLibrary.Settings.MovieFolders = flg.Folders;
						iMetaLibrary.Settings.SaveSettings();
					}
				}
			};
			btnMoviesRefresh.Activated += delegate {
				MovieMeta[] movies = guiMovieList.GetSelected();
				MovieScanner.RefreshFiles(movies);
			};
			*/
			#endregion
		}
	}
}
