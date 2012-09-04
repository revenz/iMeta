using System;
using Gdk;
using Gtk;
using iMetaLibrary;

namespace iMetaGui
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class MenuButton : Gtk.Bin
	{		
		public MenuButton ()
		{
			this.Build ();
			eventbox2.EnterNotifyEvent += delegate {
				this.IsHovered = true;
				QueueDraw();
			};
			eventbox2.ExposeEvent += HandleEventbox2ExposeEvent;
			eventbox2.ButtonReleaseEvent += delegate {
				this.IsActive = false;
				QueueDraw();		
				if(this.Clicked != null)
					Clicked();
			};
			eventbox2.ButtonPressEvent += delegate(object o, ButtonPressEventArgs args) {
				if(args.Event.Button == 1)
				{
					this.IsActive = true;
					QueueDraw();
					args.RetVal = true;
				}
			};
		}
		
		Pixbuf _NormalPixbuf;
		private Pixbuf NormalPixbuf
		{
			get
			{
				if(_NormalPixbuf == null)
				{
					_NormalPixbuf = new Gdk.Pixbuf(null, "iMetaGui.Images.button_{0}.png".FormatStr(ButtonType.ToLower()));
				}
				return _NormalPixbuf;
			}
		}
		Pixbuf _ActivePixbuf;
		private Pixbuf ActivePixbuf
		{
			get
			{
				if(_ActivePixbuf == null)
				{
					_ActivePixbuf = new Gdk.Pixbuf(null, "iMetaGui.Images.button_{0}_pushed.png".FormatStr(ButtonType.ToLower()));
				}
				return _ActivePixbuf;
			}
		}

		void HandleEventbox2ExposeEvent (object o, Gtk.ExposeEventArgs args)
		{
			Gdk.GC button = new Gdk.GC(this.eventbox2.GdkWindow);
			Gdk.Pixmap pixmap_mask;
			Pixmap pixmap;	
        	Gdk.Pixbuf pixbuf =  null;
			if(this.IsActive)
			{
				pixbuf = ActivePixbuf;
			}
			else
			{	
				pixbuf = NormalPixbuf;
			}
            pixbuf.RenderPixmapAndMask( out pixmap, out pixmap_mask, 255); 
			
			button.Fill = Gdk.Fill.Tiled;
			button.Tile = pixmap;
			
			this.eventbox2.GdkWindow.DrawRectangle (button, true, 0, 0, eventbox2.WidthRequest, eventbox2.HeightRequest);			
		}
		
		public string ButtonType{get;set;}
		
		public bool IsActive{ get; set; }
		private bool IsHovered { get; set; }
		
		public delegate void Clicked_EventHandler();
		public event Clicked_EventHandler Clicked;
	}
}

