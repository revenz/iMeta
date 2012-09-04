using System;
using Gtk;

namespace iMetaGui
{
	public class TreeItemCellRenderer : CellRenderer
	{
	    public override void GetSize (Widget widget, ref Gdk.Rectangle cell_area, out int x_offset, out int y_offset, out int width, out int height)
	    {
	        base.GetSize (widget, ref cell_area, out x_offset, out y_offset, out width, out height);
	
	        height = 20;
	    }
	
	    protected override void Render (Gdk.Drawable window, Widget widget, Gdk.Rectangle background_area, Gdk.Rectangle cell_area, Gdk.Rectangle expose_area, CellRendererState flags)
	    {
	        base.Render (window, widget, background_area, cell_area, expose_area, flags);
			int xPos = cell_area.X;
			if(this.Pixbuf != null){
				window.DrawPixbuf(widget.Style.MidGC( StateType.Normal), this.Pixbuf, 0, 0, xPos + 1, cell_area.Y + 1, 16, 16, Gdk.RgbDither.Normal, 0, 0);
				xPos += 20;
			}
	        using (var layout = new Pango.Layout(widget.PangoContext)) {
	            layout.Alignment = Pango.Alignment.Left;
	            layout.SetText(this.Text ?? "");
					
	            StateType state = flags.HasFlag(CellRendererState.Selected) ?
	                widget.IsFocus ? StateType.Selected : StateType.Active : StateType.Normal;
	
	            window.DrawLayout(widget.Style.TextGC(state), xPos, cell_area.Y + 2, layout);
	        }
	    }
		
		public Gdk.Pixbuf Pixbuf{get;set;}
		public string Text{get;set;}
	}
}

