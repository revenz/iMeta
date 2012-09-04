using System;
using System.Collections.Generic;
using Gtk;
using Gdk;
using iMetaLibrary;
using iMetaLibrary.Metadata;

namespace iMetaGui
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class TvShowGui : Gtk.Bin
	{
		iMetaLibrary.Metadata.TvShowMeta TvShowMeta;
	 	iMetaLibrary.Metadata.TvFileMeta[] Episodes;
		
		TreeStore store;
		
		Pixbuf Poster;
		
		public TvShowGui ()
		{
			this.Build ();
			
			store = new TreeStore(typeof(object));
			treeview.Model = store;
			treeview.BorderWidth = 0;
			
			string[] titles = new string[] { "Episodes", "Title", "Rating", "Filename"};
			for(int i=0; i <titles.Length ; i++)
			{
				if( i == 0)
				{					
					TreeViewColumn col = new Gtk.TreeViewColumn (); 
					TreeItemCellRenderer cell = new TreeItemCellRenderer(); 
					col.PackStart(cell, true); 
					col.SortColumnId = i;
					col.Title = titles[i];
					col.MinWidth = 85;
					col.MaxWidth = 85;
					col.SetCellDataFunc(cell, new Gtk.TreeCellDataFunc(RenderTreeCell)); 
					treeview.AppendColumn(col);
				}
				else if(i == 2)
				{
					TreeViewColumn col = new TreeViewColumn();
					CellRendererPixbuf cell = new CellRendererPixbuf();
					col.PackStart(cell, true);
					col.SortColumnId = i;
					col.Title = titles[i];
					col.SetCellDataFunc(cell, new TreeCellDataFunc(RenderTreeCell));
					treeview.AppendColumn(col);
				}
				else
				{
					TreeViewColumn col = new Gtk.TreeViewColumn (); 
					CellRendererText cell = new Gtk.CellRendererText(); 
					col.PackStart(cell, true); 
					col.SortColumnId = i;
					/*
					if(i == 1)
						col.MinWidth = 80;
					else if(i == 2)
						col.MinWidth = 200;
						*/
					col.Title = titles[i];
					col.SetCellDataFunc(cell, new Gtk.TreeCellDataFunc(RenderTreeCell)); 
					treeview.AppendColumn(col); 
				}
			}
			
			treeview.RowActivated += HandleNodeview1RowActivated;
			
			this.ExposeEvent += delegate(object o, ExposeEventArgs args) {
				//ExposeIt();
			};
		}
		
		protected override bool OnExposeEvent (EventExpose evnt)
		{
			ExposeIt();
			return base.OnExposeEvent (evnt);
		}
		
		void ExposeIt()
		{
			if(!this.Visible)
				return;
			
			Gdk.GC gc = new Gdk.GC(this.GdkWindow);			
			int w, h;
			this.GdkWindow.GetSize(out w, out h);
			/*
			Gdk.Pixmap pixmap_mask;
			Pixmap pixmap;	
			Gdk.Pixbuf pixbuf = Images.LeftPaneBackground;
	        pixbuf.RenderPixmapAndMask( out pixmap, out pixmap_mask, 255); 
			gc.Fill = Gdk.Fill.Tiled;
			gc.Tile = pixmap;
			this.GdkWindow.DrawRectangle (gc, true, 0, 0, 260, h);	
			*/
			int posterW, posterH, posterX;
			if(Poster != null)
			{
				posterW = Poster.Width;
				posterH = Poster.Height;
            	//Poster.RenderPixmapAndMask(out pixmap, out pixmap_mask, 255); 
			}
			else
			{
				posterW = Images.NoPosterPixbuf.Width;
				posterH = Images.NoPosterPixbuf.Height;
            	//Images.NoPosterPixbuf.RenderPixmapAndMask(out pixmap, out pixmap_mask, 255); 
			}
			posterX = (260 - posterW) / 2;
			using(Cairo.Context cr =  Gdk.CairoHelper.Create(this.GdkWindow))
			{
				cr.SetSourceRGB(.87, .894, .9215);
				cr.Rectangle(0, 0, 259, h);
				cr.Fill();
				cr.Rectangle(posterX - 2, 98, posterW + 4, posterH + 4);
				cr.SetSourceRGB(.7, .7, .7);
				cr.Fill();
				cr.Rectangle(258, 0, 1, h);
				cr.Fill();
			}
			this.GdkWindow.DrawPixbuf(gc, Poster ?? Images.NoPosterPixbuf, 0, 0, posterX, 100, posterW, posterH, RgbDither.Normal, 0, 0);
			
			image1.HeightRequest = posterH + 20;
		}

		void HandleNodeview1RowActivated (object o, RowActivatedArgs args)
		{
			TreeIter iter;
			store.GetIter(out iter, args.Path);
			GuiComponents.TvEpisodeNodeItem node = store.GetValue(iter, 0) as GuiComponents.TvEpisodeNodeItem;
			if(node == null)
				return; // must be a season parent node
			using(EpisodesEditorGui editor = new EpisodesEditorGui(node.Meta))
			{
				editor.Run();
				editor.Destroy();
			}
		}
		public void InitializeShow(iMetaLibrary.Metadata.TvShowMeta TvShowMeta, iMetaLibrary.Metadata.TvFileMeta[] Episodes)
		{	
			/*
			var enumerator = store.GetEnumerator();
			while(enumerator.MoveNext())
			{
				((GuiComponents.TvEpisodeNodeItem)enumerator.Current).Meta.MetaUpdated -= MetaUpdated;
			}
			*/
			/*
			store.Foreach(delegate(TreeModel modelOuter, TreePath pathOuter, TreeIter iterOuter)
			             {
							TreeIter childIter;
							modelOuter.IterChildren(out childIter);
							modelOuter.Foreach(delegate(TreeModel model, TreePath path, TreeIter iter)
				            {
								GuiComponents.TvEpisodeNodeItem node = model.GetValue(iter, 0) as GuiComponents.TvEpisodeNodeItem;
								if(node != null)
									node.Meta.MetaUpdated -= MetaUpdated;
								return true;
							});
							return true;
						 });
						 */
			store.Clear();
			string showtitle = TvShowMeta == null ? Episodes[0].ShowTitle : TvShowMeta.Title;
			this.TvShowMeta = TvShowMeta;
			this.Episodes = Episodes;
			//lblShowName.Markup = "<span font=\"18\" weight=\"bold\">{0}</span> <span size=\"small\"> [{1} Episode{2}]</span>".FormatStr(showtitle.HtmlEncode(), Episodes.Length, Episodes.Length == 1 ? "" : "s");
			
			if(Poster != null)
			{
				Poster.Dispose();
				Poster = null;
			}
			
			System.Drawing.Image poster = Episodes[0].GetPoster();
			if(poster != null)
			{
				Poster = GuiHelper.ImageToPixbufIcon(poster, 230, 320);
				poster.Dispose();
				poster = null;
			}
			SortedList<int, List<iMetaLibrary.Metadata.TvFileMeta>> seasons = new SortedList<int, List<iMetaLibrary.Metadata.TvFileMeta>>();
			foreach(iMetaLibrary.Metadata.TvFileMeta file in Episodes)
			{
				if(!seasons.ContainsKey(file.Season))
					seasons.Add(file.Season, new List<iMetaLibrary.Metadata.TvFileMeta>());
				seasons[file.Season].Add(file);
			}
			foreach(int season in seasons.Keys)
			{
				var iter = store.AppendValues(new KeyNode(season == 0 ? "Specials" : "Season {0}".FormatStr(season)));
				foreach(iMetaLibrary.Metadata.TvFileMeta meta in seasons[season])
				{
					store.AppendValues(iter, new GuiComponents.TvEpisodeNodeItem(meta));
					meta.MetaUpdated += MetaUpdated;
				}
			}
			treeview.ExpandAll();
			//txtShowInfoTvdbId.Buffer.Text = TvShowMeta == null ? "" : TvShowMeta.Id.ToString();
			//txtShowInfoRuntime.Buffer.Text = TvShowMeta == null ? "" : TvShowMeta.Runtime.ToString();
			txtTitle.Text = showtitle;
			txtMpaa.Text = TvShowMeta == null ? "" : TvShowMeta.Mpaa;
			txtPremiered.Text = TvShowMeta == null ? "" : TvShowMeta.Premiered;
			txtStudio.Text = TvShowMeta == null ? "" : TvShowMeta.Studio;
			txtGenres.Text = TvShowMeta == null ? "" : String.Join(", ", TvShowMeta.Genres);
			txtPlot.Buffer.Text = TvShowMeta == null ? "" : TvShowMeta.Plot;	
			switch(TvShowMeta == null ? "" : TvShowMeta.Status)
			{
				// Continuing - Series currently still in production this is generally the status that should be set during the initial run of a season
				case "Continuing": cboStatus.Active = 0; break;
				// Ended - For a series which has finished it's run and is not producing any new episodes.
				case "Ended ": cboStatus.Active = 1; break;
				// On Hiatus - For series where the current season has ended and it's return status is unknown.
				case "On Hiatus": cboStatus.Active = 2; break;
				// Other - Not commonly used, potentially for series that are in production and have yet to air at all.
				default: cboStatus.Active = 3; break;
			}
			this.QueueDraw();
		}
		
		void MetaUpdated()
		{
			treeview.QueueDraw();
		}		
		
		protected virtual void RenderTreeCell( Gtk.TreeViewColumn _column, Gtk.CellRenderer _cell, Gtk.TreeModel _model, Gtk.TreeIter _iter) 
		{ 					
			object o = _model.GetValue(_iter, 0);
			GuiComponents.TvEpisodeNodeItem node = o as GuiComponents.TvEpisodeNodeItem;
			Gdk.Color background = new Gdk.Color(255, 255, 255);
			if(node != null && (_model.GetPath(_iter).Indices[1] % 2) == 0)
				background = new Gdk.Color(240, 240, 255);
			if(_cell is Gtk.CellRendererText)
			{
				if(node != null)
				{
					switch(_column.SortColumnId)
					{
						case 1: (_cell as Gtk.CellRendererText).Text = node.EpisodeNames; break;
						case 3: (_cell as Gtk.CellRendererText).Text = node.Filename; break;
					}
				}
				else if(o is KeyNode && _column.SortColumnId == 0)
				{
					(_cell as Gtk.CellRendererText).Markup = ((KeyNode)o).Markup;
				}
				else 
					(_cell as Gtk.CellRendererText).Text = "";
				(_cell as Gtk.CellRendererText).BackgroundGdk = background;
			}
			else if(_cell is TreeItemCellRenderer)
			{
				TreeItemCellRenderer render = (TreeItemCellRenderer)_cell;
				render.CellBackgroundGdk = background;
				render.Pixbuf = null;
				if(node != null)
				{
					render.Text = node.EpisodeNumbersString;
					render.Pixbuf = node.StatusIcon;
				}
				else
					render.Text = ((KeyNode)o).Key;
			}
			else
			{
				if(node != null)
				{
					switch(_column.SortColumnId)
					{
						case 2: (_cell as Gtk.CellRendererPixbuf).Pixbuf = node.RatingIcon; break;
					}
				}
				else
				{
					(_cell as Gtk.CellRendererPixbuf).Pixbuf = Images.Empty;
				}
				(_cell as Gtk.CellRendererPixbuf).CellBackgroundGdk = background;
			}
		} 
	}	
}
