
// This file has been generated by the GUI designer. Do not modify.
namespace iMetaGui
{
	public partial class TopBar
	{
		private global::Gtk.EventBox eventboxHeader;
		private global::Gtk.HBox hboxHeader;
		private global::Gtk.Fixed fixedLeft;
		private global::iMetaGui.iButton ibtnMovies;
		private global::iMetaGui.iButton ibtnTvShows;
		private global::Gtk.Fixed fixedToolbar;
		private global::iMetaGui.MenuButton btnScan;
		private global::iMetaGui.MenuButton btnRefresh;
		private global::iMetaGui.MenuButton btnPreferences;
		private global::iMetaGui.MenuButton btnExport;
		private global::iMetaGui.MacButtons macbuttons;
		private global::Gtk.VBox vbox6;
		private global::Gtk.Label lblTitle;
		private global::iMetaGui.StatusWidget statuswidget;
		private global::Gtk.Label lblStatusSpacer;
		private global::Gtk.Fixed fixedRight;
		private global::iMetaGui.WindowsButtons windowsbuttons;
		
		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget iMetaGui.TopBar
			global::Stetic.BinContainer.Attach (this);
			this.HeightRequest = 85;
			this.Name = "iMetaGui.TopBar";
			// Container child iMetaGui.TopBar.Gtk.Container+ContainerChild
			this.eventboxHeader = new global::Gtk.EventBox ();
			this.eventboxHeader.Name = "eventboxHeader";
			// Container child eventboxHeader.Gtk.Container+ContainerChild
			this.hboxHeader = new global::Gtk.HBox ();
			this.hboxHeader.Name = "hboxHeader";
			this.hboxHeader.Spacing = 1;
			// Container child hboxHeader.Gtk.Box+BoxChild
			this.fixedLeft = new global::Gtk.Fixed ();
			this.fixedLeft.WidthRequest = 350;
			this.fixedLeft.Name = "fixedLeft";
			this.fixedLeft.HasWindow = false;
			// Container child fixedLeft.Gtk.Fixed+FixedChild
			this.ibtnMovies = new global::iMetaGui.iButton ();
			this.ibtnMovies.WidthRequest = 84;
			this.ibtnMovies.HeightRequest = 22;
			this.ibtnMovies.Events = ((global::Gdk.EventMask)(256));
			this.ibtnMovies.Name = "ibtnMovies";
			this.ibtnMovies.Text = "Movies";
			this.ibtnMovies.RoundRight = false;
			this.ibtnMovies.RoundLeft = true;
			this.ibtnMovies.IsActive = true;
			this.ibtnMovies.Width = 0;
			this.ibtnMovies.Height = 0;
			this.fixedLeft.Add (this.ibtnMovies);
			global::Gtk.Fixed.FixedChild w1 = ((global::Gtk.Fixed.FixedChild)(this.fixedLeft [this.ibtnMovies]));
			w1.X = 30;
			w1.Y = 33;
			// Container child fixedLeft.Gtk.Fixed+FixedChild
			this.ibtnTvShows = new global::iMetaGui.iButton ();
			this.ibtnTvShows.WidthRequest = 84;
			this.ibtnTvShows.HeightRequest = 22;
			this.ibtnTvShows.Events = ((global::Gdk.EventMask)(256));
			this.ibtnTvShows.Name = "ibtnTvShows";
			this.ibtnTvShows.Text = "TV Shows";
			this.ibtnTvShows.RoundRight = true;
			this.ibtnTvShows.RoundLeft = false;
			this.ibtnTvShows.IsActive = false;
			this.ibtnTvShows.Width = 0;
			this.ibtnTvShows.Height = 0;
			this.fixedLeft.Add (this.ibtnTvShows);
			global::Gtk.Fixed.FixedChild w2 = ((global::Gtk.Fixed.FixedChild)(this.fixedLeft [this.ibtnTvShows]));
			w2.X = 113;
			w2.Y = 33;
			// Container child fixedLeft.Gtk.Fixed+FixedChild
			this.fixedToolbar = new global::Gtk.Fixed ();
			this.fixedToolbar.WidthRequest = 120;
			this.fixedToolbar.HeightRequest = 22;
			this.fixedToolbar.Name = "fixedToolbar";
			this.fixedToolbar.HasWindow = false;
			// Container child fixedToolbar.Gtk.Fixed+FixedChild
			this.btnScan = new global::iMetaGui.MenuButton ();
			this.btnScan.WidthRequest = 28;
			this.btnScan.HeightRequest = 22;
			this.btnScan.Events = ((global::Gdk.EventMask)(256));
			this.btnScan.Name = "btnScan";
			this.btnScan.ButtonType = "scan";
			this.btnScan.IsActive = false;
			this.fixedToolbar.Add (this.btnScan);
			// Container child fixedToolbar.Gtk.Fixed+FixedChild
			this.btnRefresh = new global::iMetaGui.MenuButton ();
			this.btnRefresh.WidthRequest = 28;
			this.btnRefresh.HeightRequest = 22;
			this.btnRefresh.Events = ((global::Gdk.EventMask)(256));
			this.btnRefresh.Name = "btnRefresh";
			this.btnRefresh.ButtonType = "refresh";
			this.btnRefresh.IsActive = false;
			this.fixedToolbar.Add (this.btnRefresh);
			global::Gtk.Fixed.FixedChild w4 = ((global::Gtk.Fixed.FixedChild)(this.fixedToolbar [this.btnRefresh]));
			w4.X = 28;
			// Container child fixedToolbar.Gtk.Fixed+FixedChild
			this.btnPreferences = new global::iMetaGui.MenuButton ();
			this.btnPreferences.WidthRequest = 28;
			this.btnPreferences.HeightRequest = 22;
			this.btnPreferences.Events = ((global::Gdk.EventMask)(256));
			this.btnPreferences.Name = "btnPreferences";
			this.btnPreferences.ButtonType = "preferences";
			this.btnPreferences.IsActive = false;
			this.fixedToolbar.Add (this.btnPreferences);
			global::Gtk.Fixed.FixedChild w5 = ((global::Gtk.Fixed.FixedChild)(this.fixedToolbar [this.btnPreferences]));
			w5.X = 84;
			// Container child fixedToolbar.Gtk.Fixed+FixedChild
			this.btnExport = new global::iMetaGui.MenuButton ();
			this.btnExport.WidthRequest = 28;
			this.btnExport.HeightRequest = 22;
			this.btnExport.Events = ((global::Gdk.EventMask)(256));
			this.btnExport.Name = "btnExport";
			this.btnExport.ButtonType = "export";
			this.btnExport.IsActive = false;
			this.fixedToolbar.Add (this.btnExport);
			global::Gtk.Fixed.FixedChild w6 = ((global::Gtk.Fixed.FixedChild)(this.fixedToolbar [this.btnExport]));
			w6.X = 55;
			this.fixedLeft.Add (this.fixedToolbar);
			global::Gtk.Fixed.FixedChild w7 = ((global::Gtk.Fixed.FixedChild)(this.fixedLeft [this.fixedToolbar]));
			w7.X = 210;
			w7.Y = 33;
			// Container child fixedLeft.Gtk.Fixed+FixedChild
			this.macbuttons = new global::iMetaGui.MacButtons ();
			this.macbuttons.Events = ((global::Gdk.EventMask)(256));
			this.macbuttons.Name = "macbuttons";
			this.fixedLeft.Add (this.macbuttons);
			global::Gtk.Fixed.FixedChild w8 = ((global::Gtk.Fixed.FixedChild)(this.fixedLeft [this.macbuttons]));
			w8.X = 5;
			w8.Y = 4;
			this.hboxHeader.Add (this.fixedLeft);
			global::Gtk.Box.BoxChild w9 = ((global::Gtk.Box.BoxChild)(this.hboxHeader [this.fixedLeft]));
			w9.Position = 0;
			w9.Expand = false;
			w9.Fill = false;
			// Container child hboxHeader.Gtk.Box+BoxChild
			this.vbox6 = new global::Gtk.VBox ();
			this.vbox6.Name = "vbox6";
			// Container child vbox6.Gtk.Box+BoxChild
			this.lblTitle = new global::Gtk.Label ();
			this.lblTitle.HeightRequest = 24;
			this.lblTitle.Name = "lblTitle";
			this.lblTitle.LabelProp = global::Mono.Unix.Catalog.GetString ("iMeta");
			this.vbox6.Add (this.lblTitle);
			global::Gtk.Box.BoxChild w10 = ((global::Gtk.Box.BoxChild)(this.vbox6 [this.lblTitle]));
			w10.Position = 0;
			w10.Expand = false;
			w10.Fill = false;
			// Container child vbox6.Gtk.Box+BoxChild
			this.statuswidget = new global::iMetaGui.StatusWidget ();
			this.statuswidget.HeightRequest = 48;
			this.statuswidget.Events = ((global::Gdk.EventMask)(256));
			this.statuswidget.Name = "statuswidget";
			this.statuswidget.IsPulsing = false;
			this.vbox6.Add (this.statuswidget);
			global::Gtk.Box.BoxChild w11 = ((global::Gtk.Box.BoxChild)(this.vbox6 [this.statuswidget]));
			w11.Position = 1;
			// Container child vbox6.Gtk.Box+BoxChild
			this.lblStatusSpacer = new global::Gtk.Label ();
			this.lblStatusSpacer.HeightRequest = 13;
			this.lblStatusSpacer.Name = "lblStatusSpacer";
			this.lblStatusSpacer.LabelProp = global::Mono.Unix.Catalog.GetString ("[Spacer]");
			this.vbox6.Add (this.lblStatusSpacer);
			global::Gtk.Box.BoxChild w12 = ((global::Gtk.Box.BoxChild)(this.vbox6 [this.lblStatusSpacer]));
			w12.Position = 2;
			w12.Expand = false;
			w12.Fill = false;
			this.hboxHeader.Add (this.vbox6);
			global::Gtk.Box.BoxChild w13 = ((global::Gtk.Box.BoxChild)(this.hboxHeader [this.vbox6]));
			w13.Position = 1;
			// Container child hboxHeader.Gtk.Box+BoxChild
			this.fixedRight = new global::Gtk.Fixed ();
			this.fixedRight.WidthRequest = 350;
			this.fixedRight.Name = "fixedRight";
			this.fixedRight.HasWindow = false;
			// Container child fixedRight.Gtk.Fixed+FixedChild
			this.windowsbuttons = new global::iMetaGui.WindowsButtons ();
			this.windowsbuttons.WidthRequest = 95;
			this.windowsbuttons.HeightRequest = 18;
			this.windowsbuttons.Events = ((global::Gdk.EventMask)(256));
			this.windowsbuttons.Name = "windowsbuttons";
			this.fixedRight.Add (this.windowsbuttons);
			global::Gtk.Fixed.FixedChild w14 = ((global::Gtk.Fixed.FixedChild)(this.fixedRight [this.windowsbuttons]));
			w14.X = 240;
			this.hboxHeader.Add (this.fixedRight);
			global::Gtk.Box.BoxChild w15 = ((global::Gtk.Box.BoxChild)(this.hboxHeader [this.fixedRight]));
			w15.Position = 2;
			w15.Expand = false;
			this.eventboxHeader.Add (this.hboxHeader);
			this.Add (this.eventboxHeader);
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.lblStatusSpacer.Hide ();
			this.Hide ();
		}
	}
}
