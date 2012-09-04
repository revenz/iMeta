using System;
using System.Collections.Generic;
using Gtk;
using iMetaLibrary;
using iMetaLibrary.Metadata;

namespace iMetaGui
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class MovieListGui : Gtk.Bin
	{
		TreeStore store;
		
		public delegate void ItemActivated_EventHandler(MovieMeta Meta);
		public event ItemActivated_EventHandler ItemActivated;
		
		public MovieListGui ()
		{
			this.Build ();
			store = new TreeStore(typeof(object));
			
			treeview.Model = store;
			treeview.Selection.Mode = SelectionMode.Multiple;
			treeview.BorderWidth = 0;
			treeview.RowActivated += HandleTreeviewRowActivated;
			
			string[] titles = new string[] { "Title", "Year", "Set", "Rating", "Filename"};
			for(int i=0; i < titles.Length ; i++)
			{
				if(i == 0)
				{
					TreeViewColumn col = new Gtk.TreeViewColumn (); 
					TreeItemCellRenderer cell = new TreeItemCellRenderer(); 
					col.PackStart(cell, true); 
					col.SortColumnId = i;
					col.Title = titles[i];
					col.MinWidth = 300;
					col.MaxWidth = 400;
					col.SortIndicator = false;
					col.SetCellDataFunc(cell, new Gtk.TreeCellDataFunc(RenderTreeCell)); 
					treeview.AppendColumn(col); 
				}
				else if(i == 3)
				{
					TreeViewColumn col = new TreeViewColumn();
					CellRendererPixbuf cell = new CellRendererPixbuf();
					col.PackStart(cell, true);
					col.SortColumnId = i;
					col.Title = titles[i];
					col.SortIndicator = false;
					col.SetCellDataFunc(cell, new TreeCellDataFunc(RenderTreeCell));
					treeview.AppendColumn(col);
				}
				else
				{
					TreeViewColumn col = new Gtk.TreeViewColumn (); 
					CellRendererText cell = new Gtk.CellRendererText(); 
					col.PackStart(cell, true); 
					col.SortColumnId = i;
					col.SortIndicator = false;
					if(i == 1)
						col.MinWidth = 80;
					else if(i == 2)
						col.MinWidth = 200;
					col.Title = titles[i];
					col.SetCellDataFunc(cell, new Gtk.TreeCellDataFunc(RenderTreeCell)); 
					treeview.AppendColumn(col); 
				}
			}
		}

		void HandleTreeviewRowActivated (object o, RowActivatedArgs args)
		{			
	        TreeIter iter;
	        store.GetIter(out iter, args.Path);
			MovieNode movienode = store.GetValue(iter, 0) as MovieNode;
			if(movienode == null)
				return;
			if(ItemActivated != null && movienode.Meta != null)
				ItemActivated(movienode.Meta);			
		}		
		
		void MetaUpdated()
		{
			treeview.QueueDraw();
		}		
		
		public void AddToStore(SortedDictionary<string, List<MovieMeta>> Items)
		{
			Gtk.Application.Invoke(delegate
            {
				foreach(string key in Items.Keys)
				{
					var iter = store.AppendValues(new KeyNode("{0} [{1} Movie{2}]".FormatStr(key, Items[key].Count, Items[key].Count == 1 ? "" : "s")));
					foreach(MovieMeta meta in Items[key])
					{
						store.AppendValues(iter, new MovieNode(meta));
						meta.MetaUpdated += MetaUpdated;
					}
				}
				treeview.ExpandAll();
			});
		}
		
		protected virtual void RenderTreeCell( Gtk.TreeViewColumn _column, Gtk.CellRenderer _cell, Gtk.TreeModel _model, Gtk.TreeIter _iter) 
		{ 	
			object o =  _model.GetValue(_iter, 0);
			MovieNode node = o as MovieNode;
			Gdk.Color background = new Gdk.Color(255,255,255);
			if(node != null && (_model.GetPath(_iter).Indices[1] % 2) == 0)
				background = new Gdk.Color(240, 240, 255);			
			if(_cell is Gtk.CellRendererText)
			{
				if(node != null)
				{
					switch(_column.SortColumnId)
					{
						case 0: (_cell as Gtk.CellRendererText).Text = node.Title; break;
						case 1: (_cell as Gtk.CellRendererText).Text = node.Year; break;
						case 2: (_cell as Gtk.CellRendererText).Text = node.Set; break;
						case 4: (_cell as Gtk.CellRendererText).Text = node.Filename; break;
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
					render.Text = node.Title;
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
						case 0: (_cell as Gtk.CellRendererPixbuf).Pixbuf = node.StatusIcon; break;
						case 3: (_cell as Gtk.CellRendererPixbuf).Pixbuf = node.RatingIcon; break;
					}
				}
				else
				{
					(_cell as Gtk.CellRendererPixbuf).Pixbuf = Images.Empty;
				}
				(_cell as Gtk.CellRendererPixbuf).CellBackgroundGdk = background;
			}
		} 
		
		public MovieMeta[] GetSelected()
		{
			List<MovieMeta> results = new List<MovieMeta>();
			TreeIter iter;
			TreePath[] paths = treeview.Selection.GetSelectedRows();
			foreach(TreePath path in paths)
			{
				store.GetIter(out iter, path);
				MovieNode node = store.GetValue(iter, 0) as MovieNode;
				if(node != null)
				{
					Logger.Log("Selected Movie: {0}", node.Filename);
					results.Add(node.Meta);
				}
			}
			return results.ToArray();
		}
		
		[TreeNode(ListOnly=true)]
		class MovieNode:TreeNode
		{
			public MovieMeta Meta{get;set;}
			
			public MovieNode(MovieMeta Meta)
			{
				this.Meta = Meta;
			}
					
			[Gtk.TreeNodeValue(Column=0)]
			public Gdk.Pixbuf StatusIcon
			{
				get
				{
					if(Meta == null) return Images.Empty;
					switch(Meta.CompletionLevel)
					{
						case iMetaLibrary.Metadata.MetaCompletionLevel.Full:return Images.StatusGreen; 					
						case iMetaLibrary.Metadata.MetaCompletionLevel.Partial:return Images.StatusOrange;
						case iMetaLibrary.Metadata.MetaCompletionLevel.Loading:return Images.StatusLoading;
					}
					return Images.StatusRed;
				}
			}
				
			[Gtk.TreeNodeValue(Column=1)]
			public string Title
			{
				get
				{
					if(Meta == null) return String.Empty;
					if(String.IsNullOrEmpty(Meta.Title))
						return new System.IO.FileInfo(Meta.Filename).Name; // this should never be called as this should be done in the moviemeta class
					return Meta.Title;
				}
			}
				
			[Gtk.TreeNodeValue(Column=2)]
			public string Year
			{
				get
				{
					if(Meta == null || Meta.ReleaseDate < new DateTime(1850, 1,1)) return String.Empty;
					return Meta.ReleaseDate.Year.ToString();
				}
			}
			
			[Gtk.TreeNodeValue(Column=3)]
			public string Set { get {return Meta == null || String.IsNullOrEmpty(Meta.Set) ? String.Empty : Meta.Set; } }
			
			[Gtk.TreeNodeValue(Column=4)]
			public Gdk.Pixbuf RatingIcon
			{
				get
				{
					if(Meta == null) return Images.Empty;
					if(Meta.Rating < .5) return Images.Stars00;
					if(Meta.Rating < 1.5) return Images.Stars05;
					if(Meta.Rating < 2.5) return Images.Stars10;
					if(Meta.Rating < 3.5) return Images.Stars15;
					if(Meta.Rating < 4.5) return Images.Stars20;
					if(Meta.Rating < 5.5) return Images.Stars25;
					if(Meta.Rating < 6.5) return Images.Stars30;
					if(Meta.Rating < 7.5) return Images.Stars35;
					if(Meta.Rating < 8.5) return Images.Stars40;
					if(Meta.Rating < 9.5) return Images.Stars45;
					return Images.Stars50;
				}
			}
			
			[Gtk.TreeNodeValue(Column=5)]
			public string Filename { get {return Meta == null ? String.Empty : Meta.Filename; } }
		}		
	}
}

