using System;
using System.Collections.Generic;
using Gtk;
using Gdk;
using iMetaLibrary;

namespace iMetaGui
{
	public partial class MovieEditor : Gtk.Dialog
	{
		iMetaLibrary.Metadata.MovieMeta Meta;
		
		NodeStore store;
		
		Pixbuf Poster;
		
		public MovieEditor (iMetaLibrary.Metadata.MovieMeta Meta)
		{
			this.Build ();
			this.Meta = Meta;
			#region setup the actors datagrid
			store = new NodeStore(typeof(ActorNode));
			
			CellRendererText nameRender = new CellRendererText(){ Editable = true, };
			nameRender.Edited += delegate(object o, EditedArgs args) {
				// update the text
				ActorNode node = (ActorNode)store.GetNode(new Gtk.TreePath(args.Path));
				node.Name = args.NewText;
			};
			nvActors.AppendColumn("Name", nameRender, "text", 0);
			CellRendererText roleRender = new CellRendererText(){ Editable = true, };
			roleRender.Edited += delegate(object o, EditedArgs args) {
				// update the text
				ActorNode node = (ActorNode)store.GetNode(new Gtk.TreePath(args.Path));
				node.Role = args.NewText;
			};
			nvActors.AppendColumn("Role", roleRender, "text", 1);
		
			nvActors.NodeStore = store;
			
			btnActorsRemove.Clicked += delegate
			{
				ActorNode node = nvActors.NodeSelection.SelectedNode as ActorNode;
				if(node != null)
					store.RemoveNode(node);
			};
			btnActorsAdd.Clicked += delegate {
				var node = new ActorNode();
				store.AddNode(node);
				nvActors.NodeSelection.SelectNode(node);
				int _n = 0;
				var enumerator = store.GetEnumerator();
				while(enumerator.MoveNext()) _n++;
				nvActors.ScrollToCell(new TreePath(new int[] {_n-1}), null, false, 0, 0);
			};
			#endregion	
						
			if(Meta.Actors != null){
				foreach(var actor in Meta.Actors)
					store.AddNode(new ActorNode() { Name = actor.Key, Role = actor.Value});
			}
			
			txtTitle.Text = Meta.Title ?? "";
			txtSortTitle.Text = Meta.SortTitle ?? "";
			txtSet.Text = Meta.Set ?? "";
			txtTagline.Text = Meta.TagLine ?? "";
			txtGenres.Text = String.Join(", ", Meta.Genres ?? new string[] {});
			txtDirectors.Text = String.Join(", ", Meta.Directors ?? new string[] {});
			txtWriters.Text = String.Join(", ", Meta.Writers ?? new string[] {});
			txtTrailer.Text = Meta.Trailer ?? "";
			txtImdbId.Text = Meta.Id ?? "";
			txtMpaa.Text = Meta.Mpaa ?? "";
			txtReleaseDate.Text = Meta.ReleaseDate > new DateTime(1850, 1,1) ? Meta.ReleaseDate.ToString("yyyy-MM-dd") : "";
			txtPlot.Buffer.Text = Meta.Plot ?? "";
			numRuntime.Value = Meta.Runtime / 60; // convert seconds to minutes
			ratingwidget.Value = (int)Meta.Rating;
			
			Poster = GuiHelper.ImageToPixbuf(Meta.LoadThumbnail(290, 1000));
			
			//ratingwidget.Visible  = false;
			
			buttonOk.Clicked += delegate {
				Save ();
			};

			eventPoster.ButtonPressEvent +=	delegate{
				BrowseForNewPoster();
			};
		}

		void BrowseForNewPoster ()
		{
            using (Gtk.FileChooserDialog fc = new Gtk.FileChooserDialog("Choose a new Poster.",
                                                                        this,
                                                                        FileChooserAction.Open,
                                                                        "Cancel", ResponseType.Cancel,
                                                                        "Open", ResponseType.Accept))
            {
                fc.LocalOnly = false;
				FileFilter filter = new FileFilter();
				filter.Name = "Images";
				filter.AddMimeType("image/png");
				filter.AddMimeType("image/jpeg");
				filter.AddMimeType("image/gif");
				filter.AddPattern("*.png");
				filter.AddPattern("*.jpg");
				filter.AddPattern("*.jpeg");
				filter.AddPattern("*.jpe");
				filter.AddPattern("*.gif");
				filter.AddPattern("*.tbn");
				fc.AddFilter(filter);
                if (fc.Run() == (int)ResponseType.Accept)
                {
                    try
                    {
						// get the new poster
						string newposter = new System.IO.FileInfo(fc.Filename).FullName;
						using(System.Drawing.Image poster = System.Drawing.Image.FromFile(newposter))
						{
							iMetaLibrary.Helpers.ImageHelper.SavePoster(poster, Meta.PosterFilename);
							if(Poster != null)
								Poster.Dispose();
							Poster = GuiHelper.ImageToPixbuf(poster);
							ExposeIt();
						}
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                //Don't forget to call Destroy() or the FileChooserDialog window won't get closed.
                fc.Destroy();
            }
		}


		void HandleActionAreahandleExposeEvent (object o, ExposeEventArgs args)
		{	
			return;
			int w, h;
			this.ActionArea.GdkWindow.GetSize(out w, out h);
			using(Cairo.Context cr =  Gdk.CairoHelper.Create(this.ActionArea.GdkWindow))
			{
				cr.Rectangle(0, h - 51, w, 1);
				cr.SetSourceRGB(.5, .5, .5);
				cr.Fill();
				
				cr.Rectangle(0, h - 50, w, 50);
				cr.SetSourceRGB(1, 1, 1);
				cr.Fill();
			}
		}
		
		protected override bool OnExposeEvent (Gdk.EventExpose evnt)
		{			
			// draw base last, so its elements are drawn ontop
			base.OnExposeEvent (evnt);
			ExposeIt();
			return true;
		}
		
		private void ExposeIt()
		{
			Gdk.GC gc = new Gdk.GC(this.GdkWindow);			
			int w, h;
			this.GdkWindow.GetSize(out w, out h);
			
			Gdk.Pixmap pixmap_mask;
			Pixmap pixmap;	
			Gdk.Pixbuf pixbuf = Images.DialogBackground;
	        pixbuf.RenderPixmapAndMask( out pixmap, out pixmap_mask, 255); 
			gc.Fill = Gdk.Fill.Tiled;
			gc.Tile = pixmap;
			this.GdkWindow.DrawRectangle (gc, true, 0, 0, Images.DialogBackground.Width, h - 41);	
			
			if(Poster != null)
			{
				Rectangle posterBounds = new Rectangle((Poster.Width - 272) / 2, 10, Poster.Width, Poster.Height);
				using(Cairo.Context cr =  Gdk.CairoHelper.Create(this.GdkWindow))
				{
					cr.SetSourceRGB(.7, .7, .7);
					cr.Rectangle(posterBounds.X - 2, posterBounds.Y - 2, posterBounds.Width + 4, posterBounds.Height + 4);
					cr.Fill();
				}
				this.GdkWindow.DrawPixbuf(gc, Poster, 0, 0, posterBounds.X, posterBounds.Y, posterBounds.Width, posterBounds.Height, RgbDither.Normal, 0, 0);
			}			
		}
		
		public void Save()
		{
			if(!IsValid())
			{
				return;
			}
			if(ratingwidget.Value != (int)Meta.Rating)
				Meta.Rating = ratingwidget.Value;
			if((int)numRuntime.Value != (int)(Meta.Runtime / 60))
				Meta.Runtime =(int) numRuntime.Value * 60;
				
			Meta.Title = txtTitle.Text;
			Meta.SortTitle = txtSortTitle.Text;
			Meta.Set = txtSet.Text;
			Meta.TagLine = txtTagline.Text;
			Meta.Genres = txtGenres.Text.ToStringArray();
			Meta.Directors = txtDirectors.Text.ToStringArray();
			Meta.Writers = txtWriters.Text.ToStringArray();
			Meta.Trailer = txtTrailer.Text;
			Meta.Id = txtImdbId.Text;
			Meta.Mpaa = txtMpaa.Text;
			Meta.ReleaseDate = !System.Text.RegularExpressions.Regex.IsMatch(txtReleaseDate.Text, @"^[\d]{4}\-[\d]{2}\-[\d]{2}$") ? DateTime.MinValue : DateTime.ParseExact(txtReleaseDate.Text, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
			Meta.Plot = txtPlot.Buffer.Text ?? "";
			
			Meta.Actors = ActorNode.GetActors(store);
			
			Meta.Save(false);
			
			Destroy();
		}
		
		public bool IsValid()
		{
			if(String.IsNullOrWhiteSpace(txtTitle.Text))
			{
				// show error some how
				return false;
			}
			if(txtReleaseDate.Text.Trim() == "" || !System.Text.RegularExpressions.Regex.IsMatch(txtReleaseDate.Text, @"^[\d]{4}\-[\d]{1,2}-[\d]{1,2}$"))
				return false;
				
			return true;
		}
	}
}

