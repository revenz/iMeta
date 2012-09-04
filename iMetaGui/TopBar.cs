using System;
using Gdk;
using Gtk;
using iMetaLibrary;

namespace iMetaGui
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class TopBar : Gtk.Bin
	{
		public delegate void ScanClicked_EventHandler();
		public event ScanClicked_EventHandler ScanClicked;
		public delegate void PreferencesClicked_EventHandler();
		public event PreferencesClicked_EventHandler PreferencesClicked;
		public delegate void RefreshClicked_EventHandler();
		public event RefreshClicked_EventHandler RefreshClicked;
		public delegate void CloseClicked_EventHandler();
		public event CloseClicked_EventHandler CloseClicked;
		public delegate void MinimizeClicked_EventHandler();
		public event MinimizeClicked_EventHandler MinimizeClicked;
		public delegate void MaximizeClicked_EventHandler();
		public event MaximizeClicked_EventHandler MaximizeClicked;
		public delegate void TvShowsClicked_EventHandler();
		public event TvShowsClicked_EventHandler TvShowsClicked;
		public delegate void MoviesClicked_EventHandler();
		public event MoviesClicked_EventHandler MoviesClicked;
		public delegate void ExportClicked_EventHandler();
		public event ExportClicked_EventHandler ExportClicked;
		
		public delegate void Dragged_EventHandler(int MoveX, int MoveY);
		public event Dragged_EventHandler Dragged;		
		
		public delegate void DragBegin_EventHandler2(int MouseButton, int RootX, int RootY, uint TimeStamp);
		public event DragBegin_EventHandler2 DragBegin2;
		
		public TopBar ()
		{
			this.Build ();			
			
			if(PlatformDetection.IsWindows)
				fixedLeft.Remove(macbuttons);
			else
				fixedRight.Remove(windowsbuttons);
			
			bool isdragging = false;
			int orgx = 0, orgy = 0;
			this.eventboxHeader.ButtonPressEvent += delegate(object o, ButtonPressEventArgs args) 
			{		
    			//Double-click
        		if (args.Event.Type == EventType.TwoButtonPress)
        		{
					if(MaximizeClicked != null)
						MaximizeClicked();
				}
				else if(args.Event.Type == EventType.ButtonPress)
				{
					isdragging = true;
					orgx = (int)args.Event.X;
					orgy = (int)args.Event.Y;		
					
					if(DragBegin2 != null)
						DragBegin2((int)args.Event.Button, (int)args.Event.XRoot, (int)args.Event.YRoot, args.Event.Time);
				}
			};
			this.eventboxHeader.ButtonReleaseEvent += delegate(object o, ButtonReleaseEventArgs args) {
				isdragging = false;
			};
			this.eventboxHeader.LeaveNotifyEvent += delegate(object o, LeaveNotifyEventArgs args) {
				//isdragging = false;
			};			
			this.eventboxHeader.MotionNotifyEvent += delegate(object o, MotionNotifyEventArgs args) {
				if(isdragging)
				{
					//int winx, winy;
					//this.GdkWindow.GetPosition(out winx, out winy);
					int xmove = (int)args.Event.X - orgx;
					int ymove = (int)args.Event.Y - orgy;
					
					/*
					this.GdkWindow.Move(winx + xmove, winy + ymove);
					//Logger.Log("win: {0}x{1}, event: {2}x{3}, e2: {4}x{5}, move: {6}x{7}", winx, winy, (int)args.Event.X, (int)args.Event.Y, (int)args.Event.XRoot, (int)args.Event.YRoot, xmove, ymove);
					Logger.Log("move: {0}x{1}", xmove, ymove);
					orgx = (int)args.Event.X;
					orgy = (int)args.Event.Y;	
					*/
					if(Dragged != null)
						Dragged(xmove, ymove);
				}
			};
			
			windowsbuttons.CloseClicked += delegate { if(this.CloseClicked != null) this.CloseClicked(); };
			windowsbuttons.MaximinizedClicked += delegate { if(this.MaximizeClicked != null) this.MaximizeClicked(); };
			windowsbuttons.MinimizedClicked += delegate { if(this.MinimizeClicked != null) this.MinimizeClicked(); };
			macbuttons.CloseClicked += delegate { if(this.CloseClicked != null) this.CloseClicked(); };
			macbuttons.MaximinizedClicked += delegate { if(this.MaximizeClicked != null) this.MaximizeClicked(); };
			macbuttons.MinimizedClicked += delegate { if(this.MinimizeClicked != null) this.MinimizeClicked(); };
						
			ibtnMovies.Clicked += delegate { if(this.MoviesClicked != null) this.MoviesClicked(); ibtnMovies.IsActive = true; ibtnTvShows.IsActive = false; };
			ibtnTvShows.Clicked += delegate { if(this.TvShowsClicked != null) this.TvShowsClicked();ibtnMovies.IsActive = false; ibtnTvShows.IsActive = true; };
			
			btnScan.Clicked += delegate { if(this.ScanClicked != null) this.ScanClicked(); };
			btnRefresh.Clicked += delegate { if(this.RefreshClicked != null) this.RefreshClicked(); };
			btnPreferences.Clicked += delegate { if(this.PreferencesClicked != null) this.PreferencesClicked(); };
			btnExport.Clicked += delegate { if(this.ExportClicked != null) this.ExportClicked(); };
			
			eventboxHeader.ExposeEvent += delegate(object o, ExposeEventArgs args) 
			{	
				if (!eventboxHeader.IsDrawable)
					return;	
				
				Gdk.Pixmap pixmap;
	            //Background picture	
	            Gdk.Pixbuf pixbuf = Images.TopBarBackground;
	            Gdk.Pixmap pixmap_mask; 
	            pixbuf.RenderPixmapAndMask( out pixmap, out pixmap_mask, 255); 
				
				Gdk.GC gc = new Gdk.GC(eventboxHeader.GdkWindow);
            	int w, h; 
            	hboxHeader.GdkWindow.GetSize(out w, out h); 
	
				// Looks like GdkWindow.SetBackPixmap doesn't work very well,
				// so draw the pixmap manually.
				gc.Fill = Gdk.Fill.Tiled;
				gc.Tile = pixmap;
				eventboxHeader.GdkWindow.DrawRectangle (gc, true, 0, 0, w, h);
				
				
				Pango.Layout layout = new Pango.Layout(this.PangoContext);			
				layout.Alignment = Pango.Alignment.Center;
				layout.Width = Pango.Units.FromPixels(w - 300);
				layout.Alignment = Pango.Alignment.Center;
	        	layout.SetMarkup("<span color=\"white\" font=\"12\">iMeta</span>");
				eventboxHeader.GdkWindow.DrawLayout(Style.WhiteGC, 150, PlatformDetection.IsMac ? 5 : 3, layout);
			};
			
			lblTitle.Visible = false;
		}
		
		public void SetStatusText(string Text)
		{
			statuswidget.SetStatusText(Text);
		}
		
		public bool IsStatusPulsing { get{ return statuswidget.IsPulsing; } set { statuswidget.IsPulsing=value;} }
		
		public bool TvShowsButtonIsActive { get{ return ibtnTvShows.IsActive; } set { ibtnTvShows.IsActive= value; } }
		public bool MoviesButtonIsActive { get{ return ibtnMovies.IsActive; } set { ibtnMovies.IsActive= value; } }
	}
}

