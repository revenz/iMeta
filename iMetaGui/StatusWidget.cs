using System;
using Gdk;
using Gtk;
using iMetaLibrary;

namespace iMetaGui
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class StatusWidget : Gtk.Bin
	{
		private double PulsarLevel = 0;
		private bool PulsaringBackward = false;
		System.Timers.Timer pulsar;
		int fontsmallsize = 9;
		int fontlargesize = 12;
		
		public StatusWidget ()
		{
			this.Build ();
			this.Text = "";
			this.eventbox1.ExposeEvent += HandleEventbox1handleExposeEvent;
			
			if(PlatformDetection.IsMac){
				fontsmallsize = 11;
				fontlargesize = 16;
			}
			
			
			pulsar = new System.Timers.Timer(50);
			pulsar.AutoReset = true;
			pulsar.Elapsed += delegate {	
				if(!IsPulsing){
					pulsar.Stop();
					return;					
				}
				if(PulsaringBackward)
				{
					if(PulsarLevel == 0)
					{
						PulsaringBackward = false;
						PulsarLevel = .10;
					}
					else if( PulsarLevel - .10 < 0)
						PulsarLevel = 0;
					else
						PulsarLevel -= .10;
				}
				else
				{
					if(PulsarLevel == 1)
					{
						PulsaringBackward = true;
						PulsarLevel = .90;
					}
					else if( PulsarLevel + .10 > 1)
						PulsarLevel = 1;
					else
						PulsarLevel += .1;
				}
				this.QueueDraw();
			};
			pulsar.Start();
		}

		void HandleEventbox1handleExposeEvent (object o, Gtk.ExposeEventArgs args)
		{
			Gdk.GC gc = new Gdk.GC(this.eventbox1.GdkWindow);
			Gdk.Pixmap pixmap_mask;
			Pixmap pixmap;
			Gdk.Pixbuf pbBackground = Images.StatusWidget_Background;
			Gdk.Pixbuf pbLeft = Images.StatusWidget_Left;
			Gdk.Pixbuf pbRight = Images.StatusWidget_Right;
			
			int w, h;
            this.eventbox1.GdkWindow.GetSize(out w, out h); 
			
			pbBackground.RenderPixmapAndMask( out pixmap, out pixmap_mask, 255); 
			gc.Fill = Gdk.Fill.Tiled;
			gc.Tile = pixmap;
			this.eventbox1.GdkWindow.DrawRectangle (gc, true, pbLeft.Width, 0, this.WidthRequest - (pbLeft.Width + pbRight.Width), this.HeightRequest);						
			
			this.eventbox1.GdkWindow.DrawPixbuf(gc, pbLeft, 0, 0, 0, 0, pbLeft.Width, h, RgbDither.Normal, 0, 0);
			this.eventbox1.GdkWindow.DrawPixbuf(gc, pbRight, 0, 0, w - pbRight.Width, 0, pbRight.Width, h, RgbDither.Normal, 0, 0);
			
			Pango.Layout layout = new Pango.Layout(this.PangoContext);			
			//layout.Width = w - 40;
			layout.Width = Pango.Units.FromPixels(w - 40);
			layout.Alignment = Pango.Alignment.Center;
			layout.Ellipsize = Pango.EllipsizeMode.Middle;
			// draw pulsar
			if(IsPulsing)
			{
	        	layout.SetMarkup("<span color=\"white\" font=\"{1}\">{0}</span>".FormatStr(this.Text.HtmlEncode(), fontsmallsize));
				this.eventbox1.GdkWindow.DrawLayout(Style.WhiteGC, 20, 27, layout);
				
				int xoffset = 50;
				int yoffset = 10;
				int pheight = 10;
				int pulsarwidth = w - xoffset * 2;
				this.eventbox1.GdkWindow.DrawRectangle(Style.WhiteGC, false, xoffset, yoffset, pulsarwidth, pheight);
				this.eventbox1.GdkWindow.DrawRectangle(Style.WhiteGC, false, xoffset, yoffset, pulsarwidth, pheight);
				
				int barwidth = (w - xoffset * 2) / 10 + 2;
				this.eventbox1.GdkWindow.DrawRectangle(Style.WhiteGC, true, (int)(PulsarLevel * (pulsarwidth - barwidth)) + xoffset, yoffset, barwidth, pheight);
			}
			else
			{
				// draw text in center
	        	layout.SetMarkup("<span color=\"white\" font=\"{1}\">{0}</span>".FormatStr(this.Text.HtmlEncode(), fontlargesize));
				this.eventbox1.GdkWindow.DrawLayout(Style.WhiteGC, 20, 14, layout);
			}
		}
		
		public void SetStatusText(string Text)
		{
			this.Text = Text ?? "";
			this.eventbox1.QueueDraw();
		}
		
		private string Text{ get; set; }
		
		private bool _IsPulsing;
		public bool IsPulsing 
		{ 
			get { return _IsPulsing; }
			set
			{
				_IsPulsing = value;
				if(value)
					pulsar.Start();
			}
		}
	}
}

