using System;
using Gtk;

namespace iMetaGui.GuiComponents
{
	[TreeNode(ListOnly=true)]
	public class TvEpisodeNodeItem : Gtk.TreeNode
	{
		public TvEpisodeNodeItem ()
		{
		}
		public TvEpisodeNodeItem (iMetaLibrary.Metadata.TvFileMeta Meta, bool Spacer = false)
		{
			this.Meta = Meta;
			this.Spacer = Spacer;
		}
		
		public bool Spacer{get;private set;}
		
		
		public iMetaLibrary.Metadata.TvFileMeta Meta { get; set; }
						
		[Gtk.TreeNodeValue(Column=0)]
		public Gdk.Pixbuf StatusIcon
		{
			get
			{
				if(Meta == null || Spacer) return Images.Empty;
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
		public string SeasonString
		{
			get
			{ 
				if(Meta == null || Spacer) return String.Empty;				
				return Meta.Season == 0 ? "Special" : Meta.Season.ToString();
			}
		}
		[Gtk.TreeNodeValue(Column=2)]
		public string EpisodeNumbersString
		{
			get
			{
				if(Meta == null  || Spacer) return String.Empty;
				if(Meta.EpisodeNumbers == null || Meta.EpisodeNumbers.Length == 0)
					return String.Empty;
				string[] epnumbers = new string[Meta.EpisodeNumbers.Length];
				for(int i =0;i< Meta.EpisodeNumbers.Length;i++)
					epnumbers[i] = Meta.EpisodeNumbers[i].ToString();
				return String.Join(", ", epnumbers);
			}
		}
		
		[Gtk.TreeNodeValue(Column=3)]
		public string EpisodeNames{ get { return this.Meta.EpisodeNames; }}
		
		[Gtk.TreeNodeValue(Column=4)]
		public Gdk.Pixbuf RatingIcon
		{
			get
			{
				if(Meta == null || Spacer) return Images.Empty;
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
		public string Filename
		{ 
			get 
			{
				if(this.Meta == null || Spacer) return String.Empty;
				return this.Meta.Filename; 
			}
		}
			
		
	}
}

