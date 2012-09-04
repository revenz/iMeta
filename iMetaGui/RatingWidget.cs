using System;
using System.Collections.Generic;

namespace iMetaGui
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class RatingWidget : Gtk.Bin
	{
		Gtk.Image[] stars;
		Gtk.EventBox[] eventBoxes;
		
		public RatingWidget ()
		{
			this.Build ();
			
			this.stars = new Gtk.Image[] { image1, image2, image3, image4, image5, image6, image7, image8, image9, image10};
			this.eventBoxes = new Gtk.EventBox[] { eventbox1, eventbox2, eventbox3, eventbox4, eventbox5, eventbox6, eventbox7, eventbox8, eventbox9, eventbox10};
			for(int i=0; i< eventBoxes.Length; i++)
			{
				eventBoxes[i].ButtonPressEvent += Button_Pushed;				
			}
		}
		private void Button_Pushed(object o, Gtk.ButtonPressEventArgs args)
		{
			int index = int.Parse(System.Text.RegularExpressions.Regex.Match(((Gtk.EventBox)o).Name, @"[\d]+").Value);
			this.Value = index;
		}
		
		private int _Value = 0;
		public int Value
		{
			get { return _Value; }
			set
			{ 
				_Value = value;
				
				for(int j=0; j < value; j++)
					stars[j].Pixbuf = Images.Star32;
				for(int j=value;j<stars.Length;j++)
					stars[j].Pixbuf = Images.StarEmpty32;
			}
		}
	}
}

