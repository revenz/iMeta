using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace iMetaGui.GuiComponents
{
	public class MetaNodeItem:Gtk.TreeNode
	{
		public MetaNodeItem (string Title, string Subtitle, System.Drawing.Image Poster, object ObjectModel)
		{
			this.Title = Title;
			this.Subtitle = Subtitle;
			this.ObjectModel = ObjectModel;
			UpdatePoster(Poster);
		}
		
		public object ObjectModel{get;set;}
		
		public string Title{get;set;}
		
		public string Subtitle{get;set;}
		
		public System.Drawing.Image Icon{get; private set;}
		
		public void UpdatePoster(System.Drawing.Image Poster)
		{
			if(this.Icon != null)
			{
				this.Icon.Dispose();
				this.Icon = null;
			}
			if(Poster != null)
			{
				this.Icon = iMetaLibrary.Helpers.ImageHelper.CreateThumbnail(Poster, 100, 140, iMetaLibrary.Helpers.ImageHelper.ImageResizeMode.Stretch);
			}
		}
		
		private static System.Drawing.Font font = System.Drawing.SystemFonts.DefaultFont;
		private static System.Drawing.Font subFont = new System.Drawing.Font(font.FontFamily, font.Size * .9f, font.Style);
		private static System.Drawing.Brush brush = new System.Drawing.SolidBrush(System.Drawing.SystemColors.WindowText);
		private static System.Drawing.Brush subbrush = new System.Drawing.SolidBrush(System.Drawing.SystemColors.GrayText);
		private static System.Drawing.StringFormat formatting = new System.Drawing.StringFormat() { Alignment = System.Drawing.StringAlignment.Center};
		
		public Gdk.Pixbuf GenerateDisplayIcon()
		{
			Bitmap img = new Bitmap(100, 185);
			using(Graphics g = Graphics.FromImage(img))
			{
				g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;					
				g.Clear(System.Drawing.Color.White);
				//g.DrawImage(this.Icon ?? Images.NoPosterSmallImage, 0, 2);
				using(Bitmap fluffy = FluffyPoster())
					g.DrawImage(fluffy, 0, 2);
				
				g.DrawString(this.Title, font, brush, new System.Drawing.RectangleF(2, 145, 98, 28), formatting);
				int yPos = g.MeasureString(this.Title, font).Width > 98 ? 170 : 157;
				g.DrawString(this.Subtitle, subFont, subbrush, new System.Drawing.RectangleF(2, yPos, 98, 15), formatting);
			}			
			return iMetaGui.GuiHelper.ImageToPixbuf(img);
		}
		
		
		#region fluffy poster corners http://danbystrom.se/2008/08/24/soft-edged-images-in-gdi/		
		
		Bitmap FluffyPoster()
		{
			Bitmap bmpFluffy = new Bitmap( this.Icon ?? Images.NoPosterSmallImage );
			Rectangle r = new Rectangle( Point.Empty, bmpFluffy.Size );
			
			using ( Bitmap bmpMask = new Bitmap( r.Width, r.Height ) ){
				using ( Graphics g = Graphics.FromImage( bmpMask ) )
				{
					using ( GraphicsPath path = createRoundRect( r.X, r.Y, r.Width, r.Height, Math.Min( r.Width, r.Height ) / 5 ) )
					{
						// these two float[] control how much fluff is applied
						using ( Brush brush = createFluffyBrush(path, new float[] { 0.0f, 0.05f, 0.05f, 1.0f }, new float[] { 0.0f, 0.9f, 1.0f, 1.0f }  ) )
						{
							g.FillRectangle( Brushes.Black, r );
							g.SmoothingMode = SmoothingMode.HighQuality;
							g.FillPath( brush, path );
							transferOneARGBChannelFromOneBitmapToAnother(
								bmpMask,
								bmpFluffy,
								ChannelARGB.Blue,
								ChannelARGB.Alpha );
						}
					}
				}
			}
			// bmpFluffy is now powered up and ready to be used
			return bmpFluffy;
		}
		
		static public GraphicsPath createRoundRect( int x, int y, int width, int height, int radius )
		{
		    GraphicsPath gp = new GraphicsPath();
		
		    if (radius == 0)
		        gp.AddRectangle( new Rectangle( x, y, width, height ) );
		    else
		    {
		        gp.AddLine( x + radius, y, x + width - radius, y );
		        gp.AddArc( x + width - radius, y, radius, radius, 270, 90 );
		        gp.AddLine( x + width, y + radius, x + width, y + height - radius );
		        gp.AddArc( x + width - radius, y + height - radius, radius, radius, 0, 90 );
		        gp.AddLine( x + width - radius, y + height, x + radius, y + height );
		        gp.AddArc( x, y + height - radius, radius, radius, 90, 90 );
		        gp.AddLine( x, y + height - radius, x, y + radius );
		        gp.AddArc( x, y, radius, radius, 180, 90 );
		        gp.CloseFigure();
		    }
		    return gp;
		}
		public static Brush createFluffyBrush(GraphicsPath gp, float[] blendPositions, float[] blendFactors )
		{
			PathGradientBrush pgb = new PathGradientBrush( gp );
			Blend blend = new Blend();
			blend.Positions = blendPositions;
			blend.Factors = blendFactors;
			pgb.Blend = blend;
			pgb.CenterColor = Color.White;
			pgb.SurroundColors = new Color[] { Color.Black };
			return pgb;
		}
		public enum ChannelARGB
		{
			Blue = 0,
			Green = 1,
			Red = 2,
			Alpha = 3
		}
		
		public static void transferOneARGBChannelFromOneBitmapToAnother(Bitmap source, Bitmap dest, ChannelARGB sourceChannel, ChannelARGB destChannel )
		{
			if ( source.Size!=dest.Size )
				throw new ArgumentException();
			Rectangle r = new Rectangle( Point.Empty, source.Size );
			BitmapData bdSrc = source.LockBits( r, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb );
			BitmapData bdDst = dest.LockBits( r, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb );
			unsafe
			{
				byte* bpSrc = (byte*)bdSrc.Scan0.ToPointer();
				byte* bpDst = (byte*)bdDst.Scan0.ToPointer();
				bpSrc += (int)sourceChannel;
				bpDst += (int)destChannel;
				for ( int i = r.Height * r.Width; i > 0; i-- )
				{
					*bpDst = *bpSrc;
					bpSrc += 4;
					bpDst += 4;
				}
			}
			source.UnlockBits( bdSrc );
			dest.UnlockBits( bdDst );
		}


		#endregion
	}
}

