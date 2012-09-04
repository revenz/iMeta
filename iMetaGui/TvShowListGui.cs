using System;
using System.Collections.Generic;
using Gtk;
using iMetaLibrary;

namespace iMetaGui
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class TvShowListGui : Gtk.Bin
	{
		private ListStore store;
		
		public TvShowListGui ()
		{
			this.Build ();
									
			store = new ListStore(typeof(string), typeof(Gdk.Pixbuf), typeof(string));	
			
			iconview1.Model = store;
			iconview1.SelectionMode = SelectionMode.Multiple;
			iconview1.MarkupColumn = 0;
			iconview1.PixbufColumn = 1;				 
			iconview1.ItemWidth = 160;
			iconview1.RowSpacing = 0;
			iconview1.ItemActivated += HandleIconview1ItemActivated; 
		}

		void HandleIconview1ItemActivated (object o, Gtk.ItemActivatedArgs args)
		{	
	        TreeIter iter;
	        store.GetIter(out iter, args.Path);
			string label  = store.GetValue(iter, 2) as string;
			if(String.IsNullOrEmpty(label))
				return;
			if(ItemActivated != null)
				ItemActivated(label);
		}
		
		public void Initialize()
		{
			
		}
		
		public void AddToStore(List<TvShowNode> Items)
		{
			Gtk.Application.Invoke(delegate
            {
				foreach(TvShowNode node in Items)
				{
					string markup = "<span>{0}</span>{1}<span size=\"small\">[{2} Episode{3}]</span>".FormatStr(node.Title.HtmlEncode(), Environment.NewLine, node.NumberOfEpisodes.ToString(), node.NumberOfEpisodes == 1 ? "" : "s");
					store.AppendValues(markup, node.Poster, node.Title);
				}
			});
		}
		
		public delegate void ItemActivated_EventHandler(string Label);
		public event ItemActivated_EventHandler ItemActivated;
		
	}
}

