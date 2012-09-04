using System;

namespace iMetaGui
{
	public class GuiHelper
	{
		public GuiHelper ()
		{
		}
		
		public static Gdk.Pixbuf ImageToPixbuf(System.Drawing.Image Image)
		{
			using(System.IO.MemoryStream stream = new System.IO.MemoryStream())
			{
				Image.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
				stream.Position = 0;
				return new Gdk.Pixbuf(stream);
			}
		}
		
		public static Gdk.Pixbuf ImageToPixbufIcon(System.Drawing.Image Image, int Width = 85, int Height = 120)
		{
			using(Gdk.Pixbuf img = ImageToPixbuf(Image))
			{
				//Gdk.Pixbuf result = new Gdk.Pixbuf(Gdk.Colorspace.Rgb,  true, 24, Width, Height);
				//img.Scale(result, 0, 0, Width, Height, 0, 0, Width / Image.Width, Height / Image.Height, Gdk.InterpType.Nearest);
				return img.ScaleSimple( Width, Height, Gdk.InterpType.Bilinear);
			}
		}
		
		internal static string GetAccelPath (string key)
        {
            string path = "<MonoDevelop>/MainWindow/" + key;
            if (!Gtk.AccelMap.LookupEntry (path, new Gtk.AccelKey()) ) {
                string[] keys = key.Split ('|');
                Gdk.ModifierType mod = 0;
                uint ckey = 0;
                foreach (string keyp in keys) {
                    if (keyp == "Control") {
                            mod |= Gdk.ModifierType.ControlMask;
                    } else if (keyp == "Shift") {
                            mod |= Gdk.ModifierType.ShiftMask;
                    } else if (keyp == "Alt") {
                            mod |= Gdk.ModifierType.Mod1Mask;
                    } else {
                            ckey = Gdk.Keyval.FromName (keyp);
                    }
                }
                Gtk.AccelMap.AddEntry (path, ckey, mod);
            }
            return path;
        }
	}
}

