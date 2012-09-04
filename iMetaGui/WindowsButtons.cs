using System;
using Gdk;
using Gtk;

namespace iMetaGui
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class WindowsButtons : Gtk.Bin
	{
		public WindowsButtons ()
		{
			this.Build ();
			this.eventMinimize.ExposeEvent += HandleEventMinimizehandleExposeEvent;
			this.eventMaximize.ExposeEvent += HandleEventMaximizehandleExposeEvent;
			this.eventClose.ExposeEvent += HandleEventClosehandleExposeEvent;
			
			this.eventMinimize.ButtonPressEvent += delegate(object o, ButtonPressEventArgs args) {				
				if(MinimizedClicked != null)
					MinimizedClicked();
				args.RetVal = true;
			};
			this.eventMaximize.ButtonPressEvent += delegate(object o, ButtonPressEventArgs args) {
				if(MaximinizedClicked != null)
					MaximinizedClicked();
				args.RetVal = true;
			};
			this.eventClose.ButtonPressEvent += delegate(object o, ButtonPressEventArgs args) {
				if(CloseClicked != null)
					CloseClicked();
				args.RetVal = true;
			};
			this.eventMinimize.EnterNotifyEvent += delegate {
				this.MinimizeHover = true;
				this.eventMinimize.QueueDraw();
			};
			this.eventMinimize.LeaveNotifyEvent += delegate {
				this.MinimizeHover = false;
				this.eventMinimize.QueueDraw();
			};
			this.eventMaximize.EnterNotifyEvent += delegate {
				this.MaximizeHover = true;
				this.eventMaximize.QueueDraw();
			};
			this.eventMaximize.LeaveNotifyEvent += delegate {
				this.MaximizeHover = false;
				this.eventMaximize.QueueDraw();
			};
			this.eventClose.EnterNotifyEvent += delegate {
				this.ExitHover = true;
				this.eventClose.QueueDraw();
			};
			this.eventClose.LeaveNotifyEvent += delegate {
				this.ExitHover = false;
				this.eventClose.QueueDraw();
			};
		}
		
		private bool MinimizeHover = false;
		private bool MaximizeHover = false;
		private bool ExitHover = false;
		
		public delegate void CloseClicked_EventHandler();
		public event CloseClicked_EventHandler CloseClicked;
		public delegate void MaximinizedClicked_EventHandler();
		public event MaximinizedClicked_EventHandler MaximinizedClicked;
		public delegate void MiniminizedClicked_EventHandler();
		public event MiniminizedClicked_EventHandler MinimizedClicked;

		void HandleEventMinimizehandleExposeEvent (object o, ExposeEventArgs args)
		{
			if(!this.Visible)
				return;
			Gdk.GC button = new Gdk.GC(eventMinimize.GdkWindow);
			Gdk.Pixmap pixmap_mask;
			Pixmap pixmap;	
        	Gdk.Pixbuf pixbuf =  null;
			if(this.MinimizeHover)
			{
				pixbuf = Images.Windows_MinimizeButton_Hover;
			}
			else
			{	
				pixbuf = Images.Windows_MinimizeButton;		
			}
            pixbuf.RenderPixmapAndMask( out pixmap, out pixmap_mask, 255); 
			
			button.Fill = Gdk.Fill.Tiled;
			button.Tile = pixmap;
			
			this.eventMinimize.GdkWindow.DrawRectangle (button, true, 0, 0, eventMinimize.WidthRequest, eventMinimize.HeightRequest);			
		}

		void HandleEventMaximizehandleExposeEvent (object o, ExposeEventArgs args)
		{
			if(!this.Visible)
				return;
			Gdk.GC button = new Gdk.GC(eventMaximize.GdkWindow);
			Gdk.Pixmap pixmap_mask;
			Pixmap pixmap;	
        	Gdk.Pixbuf pixbuf =  null;
			if(this.MaximizeHover)
			{
				pixbuf = Images.Windows_MaximizeButton_Hover;
			}
			else
			{	
				pixbuf = Images.Windows_MaximizeButton;		
			}
            pixbuf.RenderPixmapAndMask( out pixmap, out pixmap_mask, 255); 
			
			button.Fill = Gdk.Fill.Tiled;
			button.Tile = pixmap;
			
			this.eventMaximize.GdkWindow.DrawRectangle (button, true, 0, 0, eventMaximize.WidthRequest, eventMaximize.HeightRequest);			
		}

		void HandleEventClosehandleExposeEvent (object o, ExposeEventArgs args)
		{			
			if(!this.Visible)
				return;
			Gdk.GC button = new Gdk.GC(eventClose.GdkWindow);
			Gdk.Pixmap pixmap_mask;
			Pixmap pixmap;	
        	Gdk.Pixbuf pixbuf =  null;
			if(this.ExitHover)
			{
				pixbuf = Images.Windows_CloseButton_Hover;
			}
			else
			{	
				pixbuf = Images.Windows_CloseButton;		
			}
            pixbuf.RenderPixmapAndMask( out pixmap, out pixmap_mask, 255); 
			
			button.Fill = Gdk.Fill.Tiled;
			button.Tile = pixmap;
			
			this.eventClose.GdkWindow.DrawRectangle (button, true, 0, 0, eventClose.WidthRequest, eventClose.HeightRequest);			
		}		
	}
}

