using System;
using System.Collections.Generic;
using Gdk;
using Gtk;
using iMetaLibrary;

namespace iMetaGui
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class iButton : Gtk.Bin
	{
		public iButton ()
		{
			this.Build ();
						
			this.eventbox1.ExposeEvent += HandleExposeEvent;
			
			this.eventbox1.ButtonPressEvent += delegate(object o, ButtonPressEventArgs args) {				
				if(Clicked != null)
					Clicked();
				args.RetVal = true;
			};
			this.label1.Visible = false;
			
			if(PlatformDetection.IsWindows)
				yOffset = 2;
		}
		
		int yOffset = 4;
		
		void HandleExposeEvent (object o, Gtk.ExposeEventArgs args)
		{	
			Gdk.GC button = new Gdk.GC(this.eventbox1.GdkWindow);
			Gdk.Pixmap pixmap_mask;
			Pixmap pixmap;
			Gdk.Pixbuf pixbuf;
			
			if(!this.IsActive)
			{
				pixbuf = this.RoundLeft ? Images.iButtonLeft : Images.iButtonRight;
			}
			else
			{
				pixbuf = this.RoundLeft ? Images.iButtonLeftPushed : Images.iButtonRightPushed;
			}
            pixbuf.RenderPixmapAndMask( out pixmap, out pixmap_mask, 255); 
			button.Fill = Gdk.Fill.Tiled;
			button.Tile = pixmap;
			this.eventbox1.GdkWindow.DrawRectangle (button, true, 0, 0, this.WidthRequest, this.HeightRequest);						
			
			Pango.Layout layout = new Pango.Layout(this.PangoContext);			
        	layout.SetMarkup("<span color=\"{1}\" font=\"10.5\">{0}</span>".FormatStr(this.Text.HtmlEncode(), this.IsActive ? "white" : "black"));
			this.eventbox1.GdkWindow.DrawLayout(this.IsActive ? Style.WhiteGC : Style.BlackGC, this.RoundLeft ? 10 : 5, yOffset, layout);
		}
		
		public string Text { get; set; }
		
		public bool RoundRight { get; set; }
		public bool RoundLeft { get; set; }
	
		private bool _IsActive;
		public bool IsActive
		{ 
			get { return _IsActive;}
			set 
			{ 
				if(value != _IsActive)
				{
					_IsActive = value;
					QueueDraw(); 
				}
			}
		}
		
		public int Width { get; set; }
		public int Height { get; set; }
		
		public delegate void Clicked_EventHandler();
		public event Clicked_EventHandler Clicked;
	}
}

