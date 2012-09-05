
// This file has been generated by the GUI designer. Do not modify.
namespace iMetaGui
{
	public partial class MovieEditor
	{
		private global::Gtk.HBox hbox1;
		private global::Gtk.VBox vbox2;
		private global::Gtk.EventBox eventPoster;
		private global::iMetaGui.RatingWidget ratingwidget;
		private global::Gtk.Fixed fixed1;
		private global::Gtk.EventBox ebDetailsBackground;
		private global::Gtk.Table table2;
		private global::Gtk.ScrolledWindow GtkScrolledWindow;
		private global::Gtk.TextView txtPlot;
		private global::Gtk.ScrolledWindow GtkScrolledWindow1;
		private global::Gtk.NodeView nvActors;
		private global::Gtk.HBox hbox2;
		private global::Gtk.Button btnActorsAdd;
		private global::Gtk.Button btnActorsRemove;
		private global::Gtk.Label label1;
		private global::Gtk.Label label10;
		private global::Gtk.Label label11;
		private global::Gtk.Label label12;
		private global::Gtk.Label label13;
		private global::Gtk.Label label15;
		private global::Gtk.Label label16;
		private global::Gtk.Label label3;
		private global::Gtk.Label label4;
		private global::Gtk.Label label5;
		private global::Gtk.Label label6;
		private global::Gtk.Label label7;
		private global::Gtk.Label label8;
		private global::Gtk.Label label9;
		private global::Gtk.SpinButton numRuntime;
		private global::Gtk.Entry txtDirectors;
		private global::Gtk.Entry txtGenres;
		private global::Gtk.Entry txtImdbId;
		private global::Gtk.Entry txtMpaa;
		private global::Gtk.Entry txtReleaseDate;
		private global::Gtk.Entry txtSet;
		private global::Gtk.Entry txtSortTitle;
		private global::Gtk.Entry txtTagline;
		private global::Gtk.Entry txtTitle;
		private global::Gtk.Entry txtTrailer;
		private global::Gtk.Entry txtWriters;
		private global::Gtk.Button buttonCancel;
		private global::Gtk.Button buttonOk;
		
		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget iMetaGui.MovieEditor
			this.Name = "iMetaGui.MovieEditor";
			this.Title = global::Mono.Unix.Catalog.GetString ("Movie Editor");
			this.WindowPosition = ((global::Gtk.WindowPosition)(4));
			this.Modal = true;
			this.Resizable = false;
			this.Gravity = ((global::Gdk.Gravity)(5));
			// Internal child iMetaGui.MovieEditor.VBox
			global::Gtk.VBox w1 = this.VBox;
			w1.Name = "dialog1_VBox";
			w1.BorderWidth = ((uint)(2));
			// Container child dialog1_VBox.Gtk.Box+BoxChild
			this.hbox1 = new global::Gtk.HBox ();
			this.hbox1.Name = "hbox1";
			this.hbox1.Spacing = 6;
			// Container child hbox1.Gtk.Box+BoxChild
			this.vbox2 = new global::Gtk.VBox ();
			this.vbox2.HeightRequest = 320;
			this.vbox2.Name = "vbox2";
			this.vbox2.Spacing = 6;
			// Container child vbox2.Gtk.Box+BoxChild
			this.eventPoster = new global::Gtk.EventBox ();
			this.eventPoster.HeightRequest = 440;
			this.eventPoster.Name = "eventPoster";
			this.eventPoster.VisibleWindow = false;
			this.vbox2.Add (this.eventPoster);
			global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.vbox2 [this.eventPoster]));
			w2.Position = 0;
			// Container child vbox2.Gtk.Box+BoxChild
			this.ratingwidget = new global::iMetaGui.RatingWidget ();
			this.ratingwidget.HeightRequest = 15;
			this.ratingwidget.Events = ((global::Gdk.EventMask)(256));
			this.ratingwidget.Name = "ratingwidget";
			this.ratingwidget.Value = 0;
			this.vbox2.Add (this.ratingwidget);
			global::Gtk.Box.BoxChild w3 = ((global::Gtk.Box.BoxChild)(this.vbox2 [this.ratingwidget]));
			w3.Position = 1;
			w3.Expand = false;
			w3.Fill = false;
			// Container child vbox2.Gtk.Box+BoxChild
			this.fixed1 = new global::Gtk.Fixed ();
			this.fixed1.HeightRequest = 30;
			this.fixed1.Name = "fixed1";
			this.fixed1.HasWindow = false;
			this.vbox2.Add (this.fixed1);
			global::Gtk.Box.BoxChild w4 = ((global::Gtk.Box.BoxChild)(this.vbox2 [this.fixed1]));
			w4.Position = 2;
			w4.Expand = false;
			w4.Fill = false;
			this.hbox1.Add (this.vbox2);
			global::Gtk.Box.BoxChild w5 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.vbox2]));
			w5.Position = 0;
			w5.Expand = false;
			w5.Fill = false;
			// Container child hbox1.Gtk.Box+BoxChild
			this.ebDetailsBackground = new global::Gtk.EventBox ();
			this.ebDetailsBackground.Name = "ebDetailsBackground";
			// Container child ebDetailsBackground.Gtk.Container+ContainerChild
			this.table2 = new global::Gtk.Table (((uint)(13)), ((uint)(4)), false);
			this.table2.Name = "table2";
			this.table2.RowSpacing = ((uint)(6));
			this.table2.ColumnSpacing = ((uint)(6));
			// Container child table2.Gtk.Table+TableChild
			this.GtkScrolledWindow = new global::Gtk.ScrolledWindow ();
			this.GtkScrolledWindow.Name = "GtkScrolledWindow";
			this.GtkScrolledWindow.ShadowType = ((global::Gtk.ShadowType)(1));
			// Container child GtkScrolledWindow.Gtk.Container+ContainerChild
			this.txtPlot = new global::Gtk.TextView ();
			this.txtPlot.CanFocus = true;
			this.txtPlot.Name = "txtPlot";
			this.txtPlot.WrapMode = ((global::Gtk.WrapMode)(2));
			this.GtkScrolledWindow.Add (this.txtPlot);
			this.table2.Add (this.GtkScrolledWindow);
			global::Gtk.Table.TableChild w7 = ((global::Gtk.Table.TableChild)(this.table2 [this.GtkScrolledWindow]));
			w7.TopAttach = ((uint)(9));
			w7.BottomAttach = ((uint)(10));
			w7.LeftAttach = ((uint)(1));
			w7.RightAttach = ((uint)(4));
			w7.XOptions = ((global::Gtk.AttachOptions)(4));
			w7.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.GtkScrolledWindow1 = new global::Gtk.ScrolledWindow ();
			this.GtkScrolledWindow1.HeightRequest = 142;
			this.GtkScrolledWindow1.Name = "GtkScrolledWindow1";
			this.GtkScrolledWindow1.ShadowType = ((global::Gtk.ShadowType)(1));
			// Container child GtkScrolledWindow1.Gtk.Container+ContainerChild
			this.nvActors = new global::Gtk.NodeView ();
			this.nvActors.CanFocus = true;
			this.nvActors.Name = "nvActors";
			this.GtkScrolledWindow1.Add (this.nvActors);
			this.table2.Add (this.GtkScrolledWindow1);
			global::Gtk.Table.TableChild w9 = ((global::Gtk.Table.TableChild)(this.table2 [this.GtkScrolledWindow1]));
			w9.TopAttach = ((uint)(10));
			w9.BottomAttach = ((uint)(11));
			w9.LeftAttach = ((uint)(1));
			w9.RightAttach = ((uint)(4));
			w9.XOptions = ((global::Gtk.AttachOptions)(4));
			w9.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.hbox2 = new global::Gtk.HBox ();
			this.hbox2.Name = "hbox2";
			this.hbox2.Spacing = 6;
			// Container child hbox2.Gtk.Box+BoxChild
			this.btnActorsAdd = new global::Gtk.Button ();
			this.btnActorsAdd.CanFocus = true;
			this.btnActorsAdd.Name = "btnActorsAdd";
			this.btnActorsAdd.UseStock = true;
			this.btnActorsAdd.UseUnderline = true;
			this.btnActorsAdd.Label = "gtk-add";
			this.hbox2.Add (this.btnActorsAdd);
			global::Gtk.Box.BoxChild w10 = ((global::Gtk.Box.BoxChild)(this.hbox2 [this.btnActorsAdd]));
			w10.Position = 0;
			w10.Expand = false;
			w10.Fill = false;
			// Container child hbox2.Gtk.Box+BoxChild
			this.btnActorsRemove = new global::Gtk.Button ();
			this.btnActorsRemove.CanFocus = true;
			this.btnActorsRemove.Name = "btnActorsRemove";
			this.btnActorsRemove.UseStock = true;
			this.btnActorsRemove.UseUnderline = true;
			this.btnActorsRemove.Label = "gtk-remove";
			this.hbox2.Add (this.btnActorsRemove);
			global::Gtk.Box.BoxChild w11 = ((global::Gtk.Box.BoxChild)(this.hbox2 [this.btnActorsRemove]));
			w11.Position = 1;
			w11.Expand = false;
			w11.Fill = false;
			this.table2.Add (this.hbox2);
			global::Gtk.Table.TableChild w12 = ((global::Gtk.Table.TableChild)(this.table2 [this.hbox2]));
			w12.TopAttach = ((uint)(11));
			w12.BottomAttach = ((uint)(12));
			w12.LeftAttach = ((uint)(1));
			w12.RightAttach = ((uint)(2));
			w12.XOptions = ((global::Gtk.AttachOptions)(4));
			w12.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.label1 = new global::Gtk.Label ();
			this.label1.Name = "label1";
			this.label1.Xpad = 3;
			this.label1.Ypad = 4;
			this.label1.Xalign = 1F;
			this.label1.Yalign = 0F;
			this.label1.LabelProp = global::Mono.Unix.Catalog.GetString ("Title:");
			this.table2.Add (this.label1);
			global::Gtk.Table.TableChild w13 = ((global::Gtk.Table.TableChild)(this.table2 [this.label1]));
			w13.XOptions = ((global::Gtk.AttachOptions)(4));
			w13.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.label10 = new global::Gtk.Label ();
			this.label10.Name = "label10";
			this.label10.Xpad = 3;
			this.label10.Ypad = 4;
			this.label10.Xalign = 1F;
			this.label10.Yalign = 0F;
			this.label10.LabelProp = global::Mono.Unix.Catalog.GetString ("IMDB ID:");
			this.table2.Add (this.label10);
			global::Gtk.Table.TableChild w14 = ((global::Gtk.Table.TableChild)(this.table2 [this.label10]));
			w14.TopAttach = ((uint)(7));
			w14.BottomAttach = ((uint)(8));
			w14.XOptions = ((global::Gtk.AttachOptions)(4));
			w14.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.label11 = new global::Gtk.Label ();
			this.label11.Name = "label11";
			this.label11.Xpad = 3;
			this.label11.Ypad = 4;
			this.label11.Xalign = 1F;
			this.label11.Yalign = 0F;
			this.label11.LabelProp = global::Mono.Unix.Catalog.GetString ("Runtime:");
			this.table2.Add (this.label11);
			global::Gtk.Table.TableChild w15 = ((global::Gtk.Table.TableChild)(this.table2 [this.label11]));
			w15.TopAttach = ((uint)(8));
			w15.BottomAttach = ((uint)(9));
			w15.XOptions = ((global::Gtk.AttachOptions)(4));
			w15.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.label12 = new global::Gtk.Label ();
			this.label12.Name = "label12";
			this.label12.Xpad = 3;
			this.label12.Ypad = 4;
			this.label12.Xalign = 1F;
			this.label12.Yalign = 0F;
			this.label12.LabelProp = global::Mono.Unix.Catalog.GetString ("Plot:");
			this.table2.Add (this.label12);
			global::Gtk.Table.TableChild w16 = ((global::Gtk.Table.TableChild)(this.table2 [this.label12]));
			w16.TopAttach = ((uint)(9));
			w16.BottomAttach = ((uint)(10));
			w16.XOptions = ((global::Gtk.AttachOptions)(4));
			w16.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.label13 = new global::Gtk.Label ();
			this.label13.Name = "label13";
			this.label13.Xpad = 3;
			this.label13.Ypad = 4;
			this.label13.Xalign = 1F;
			this.label13.Yalign = 0F;
			this.label13.LabelProp = global::Mono.Unix.Catalog.GetString ("Cast:");
			this.table2.Add (this.label13);
			global::Gtk.Table.TableChild w17 = ((global::Gtk.Table.TableChild)(this.table2 [this.label13]));
			w17.TopAttach = ((uint)(10));
			w17.BottomAttach = ((uint)(11));
			w17.XOptions = ((global::Gtk.AttachOptions)(4));
			w17.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.label15 = new global::Gtk.Label ();
			this.label15.Name = "label15";
			this.label15.Xpad = 3;
			this.label15.Ypad = 4;
			this.label15.Xalign = 1F;
			this.label15.Yalign = 0F;
			this.label15.LabelProp = global::Mono.Unix.Catalog.GetString ("MPAA:");
			this.table2.Add (this.label15);
			global::Gtk.Table.TableChild w18 = ((global::Gtk.Table.TableChild)(this.table2 [this.label15]));
			w18.TopAttach = ((uint)(7));
			w18.BottomAttach = ((uint)(8));
			w18.LeftAttach = ((uint)(2));
			w18.RightAttach = ((uint)(3));
			w18.XOptions = ((global::Gtk.AttachOptions)(4));
			w18.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.label16 = new global::Gtk.Label ();
			this.label16.Name = "label16";
			this.label16.Xpad = 3;
			this.label16.Ypad = 4;
			this.label16.Xalign = 1F;
			this.label16.Yalign = 0F;
			this.label16.LabelProp = global::Mono.Unix.Catalog.GetString ("Release Date:");
			this.table2.Add (this.label16);
			global::Gtk.Table.TableChild w19 = ((global::Gtk.Table.TableChild)(this.table2 [this.label16]));
			w19.TopAttach = ((uint)(8));
			w19.BottomAttach = ((uint)(9));
			w19.LeftAttach = ((uint)(2));
			w19.RightAttach = ((uint)(3));
			w19.XOptions = ((global::Gtk.AttachOptions)(4));
			w19.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.label3 = new global::Gtk.Label ();
			this.label3.Name = "label3";
			this.label3.Xpad = 3;
			this.label3.Ypad = 4;
			this.label3.Xalign = 1F;
			this.label3.Yalign = 0F;
			this.label3.LabelProp = global::Mono.Unix.Catalog.GetString ("Sort Title:");
			this.table2.Add (this.label3);
			global::Gtk.Table.TableChild w20 = ((global::Gtk.Table.TableChild)(this.table2 [this.label3]));
			w20.TopAttach = ((uint)(1));
			w20.BottomAttach = ((uint)(2));
			w20.XOptions = ((global::Gtk.AttachOptions)(4));
			w20.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.label4 = new global::Gtk.Label ();
			this.label4.Name = "label4";
			this.label4.Xpad = 3;
			this.label4.Ypad = 4;
			this.label4.Xalign = 1F;
			this.label4.Yalign = 0F;
			this.label4.LabelProp = global::Mono.Unix.Catalog.GetString ("Set:");
			this.table2.Add (this.label4);
			global::Gtk.Table.TableChild w21 = ((global::Gtk.Table.TableChild)(this.table2 [this.label4]));
			w21.TopAttach = ((uint)(1));
			w21.BottomAttach = ((uint)(2));
			w21.LeftAttach = ((uint)(2));
			w21.RightAttach = ((uint)(3));
			w21.XOptions = ((global::Gtk.AttachOptions)(4));
			w21.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.label5 = new global::Gtk.Label ();
			this.label5.Name = "label5";
			this.label5.Xpad = 3;
			this.label5.Ypad = 4;
			this.label5.Xalign = 1F;
			this.label5.Yalign = 0F;
			this.label5.LabelProp = global::Mono.Unix.Catalog.GetString ("Tagline:");
			this.table2.Add (this.label5);
			global::Gtk.Table.TableChild w22 = ((global::Gtk.Table.TableChild)(this.table2 [this.label5]));
			w22.TopAttach = ((uint)(2));
			w22.BottomAttach = ((uint)(3));
			w22.XOptions = ((global::Gtk.AttachOptions)(4));
			w22.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.label6 = new global::Gtk.Label ();
			this.label6.Name = "label6";
			this.label6.Xpad = 3;
			this.label6.Ypad = 4;
			this.label6.Xalign = 1F;
			this.label6.Yalign = 0F;
			this.label6.LabelProp = global::Mono.Unix.Catalog.GetString ("Genres:");
			this.table2.Add (this.label6);
			global::Gtk.Table.TableChild w23 = ((global::Gtk.Table.TableChild)(this.table2 [this.label6]));
			w23.TopAttach = ((uint)(3));
			w23.BottomAttach = ((uint)(4));
			w23.XOptions = ((global::Gtk.AttachOptions)(4));
			w23.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.label7 = new global::Gtk.Label ();
			this.label7.Name = "label7";
			this.label7.Xpad = 3;
			this.label7.Ypad = 5;
			this.label7.Xalign = 1F;
			this.label7.Yalign = 0F;
			this.label7.LabelProp = global::Mono.Unix.Catalog.GetString ("Diretors:");
			this.table2.Add (this.label7);
			global::Gtk.Table.TableChild w24 = ((global::Gtk.Table.TableChild)(this.table2 [this.label7]));
			w24.TopAttach = ((uint)(4));
			w24.BottomAttach = ((uint)(5));
			w24.XOptions = ((global::Gtk.AttachOptions)(4));
			w24.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.label8 = new global::Gtk.Label ();
			this.label8.Name = "label8";
			this.label8.Xpad = 3;
			this.label8.Ypad = 4;
			this.label8.Xalign = 1F;
			this.label8.Yalign = 0F;
			this.label8.LabelProp = global::Mono.Unix.Catalog.GetString ("Writers:");
			this.table2.Add (this.label8);
			global::Gtk.Table.TableChild w25 = ((global::Gtk.Table.TableChild)(this.table2 [this.label8]));
			w25.TopAttach = ((uint)(5));
			w25.BottomAttach = ((uint)(6));
			w25.XOptions = ((global::Gtk.AttachOptions)(4));
			w25.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.label9 = new global::Gtk.Label ();
			this.label9.Name = "label9";
			this.label9.Xpad = 3;
			this.label9.Ypad = 4;
			this.label9.Xalign = 1F;
			this.label9.Yalign = 0F;
			this.label9.LabelProp = global::Mono.Unix.Catalog.GetString ("Trailer:");
			this.table2.Add (this.label9);
			global::Gtk.Table.TableChild w26 = ((global::Gtk.Table.TableChild)(this.table2 [this.label9]));
			w26.TopAttach = ((uint)(6));
			w26.BottomAttach = ((uint)(7));
			w26.XOptions = ((global::Gtk.AttachOptions)(4));
			w26.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.numRuntime = new global::Gtk.SpinButton (0D, 100D, 1D);
			this.numRuntime.CanFocus = true;
			this.numRuntime.Name = "numRuntime";
			this.numRuntime.Adjustment.PageIncrement = 10D;
			this.numRuntime.ClimbRate = 1D;
			this.numRuntime.Numeric = true;
			this.table2.Add (this.numRuntime);
			global::Gtk.Table.TableChild w27 = ((global::Gtk.Table.TableChild)(this.table2 [this.numRuntime]));
			w27.TopAttach = ((uint)(8));
			w27.BottomAttach = ((uint)(9));
			w27.LeftAttach = ((uint)(1));
			w27.RightAttach = ((uint)(2));
			w27.XOptions = ((global::Gtk.AttachOptions)(4));
			w27.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.txtDirectors = new global::Gtk.Entry ();
			this.txtDirectors.CanFocus = true;
			this.txtDirectors.Name = "txtDirectors";
			this.txtDirectors.IsEditable = true;
			this.txtDirectors.InvisibleChar = '●';
			this.table2.Add (this.txtDirectors);
			global::Gtk.Table.TableChild w28 = ((global::Gtk.Table.TableChild)(this.table2 [this.txtDirectors]));
			w28.TopAttach = ((uint)(4));
			w28.BottomAttach = ((uint)(5));
			w28.LeftAttach = ((uint)(1));
			w28.RightAttach = ((uint)(4));
			w28.XOptions = ((global::Gtk.AttachOptions)(4));
			w28.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.txtGenres = new global::Gtk.Entry ();
			this.txtGenres.CanFocus = true;
			this.txtGenres.Name = "txtGenres";
			this.txtGenres.IsEditable = true;
			this.txtGenres.InvisibleChar = '●';
			this.table2.Add (this.txtGenres);
			global::Gtk.Table.TableChild w29 = ((global::Gtk.Table.TableChild)(this.table2 [this.txtGenres]));
			w29.TopAttach = ((uint)(3));
			w29.BottomAttach = ((uint)(4));
			w29.LeftAttach = ((uint)(1));
			w29.RightAttach = ((uint)(4));
			w29.XOptions = ((global::Gtk.AttachOptions)(4));
			w29.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.txtImdbId = new global::Gtk.Entry ();
			this.txtImdbId.WidthRequest = 200;
			this.txtImdbId.CanFocus = true;
			this.txtImdbId.Name = "txtImdbId";
			this.txtImdbId.IsEditable = true;
			this.txtImdbId.InvisibleChar = '●';
			this.table2.Add (this.txtImdbId);
			global::Gtk.Table.TableChild w30 = ((global::Gtk.Table.TableChild)(this.table2 [this.txtImdbId]));
			w30.TopAttach = ((uint)(7));
			w30.BottomAttach = ((uint)(8));
			w30.LeftAttach = ((uint)(1));
			w30.RightAttach = ((uint)(2));
			w30.XOptions = ((global::Gtk.AttachOptions)(4));
			w30.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.txtMpaa = new global::Gtk.Entry ();
			this.txtMpaa.WidthRequest = 200;
			this.txtMpaa.CanFocus = true;
			this.txtMpaa.Name = "txtMpaa";
			this.txtMpaa.IsEditable = true;
			this.txtMpaa.InvisibleChar = '●';
			this.table2.Add (this.txtMpaa);
			global::Gtk.Table.TableChild w31 = ((global::Gtk.Table.TableChild)(this.table2 [this.txtMpaa]));
			w31.TopAttach = ((uint)(7));
			w31.BottomAttach = ((uint)(8));
			w31.LeftAttach = ((uint)(3));
			w31.RightAttach = ((uint)(4));
			w31.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.txtReleaseDate = new global::Gtk.Entry ();
			this.txtReleaseDate.WidthRequest = 200;
			this.txtReleaseDate.CanFocus = true;
			this.txtReleaseDate.Name = "txtReleaseDate";
			this.txtReleaseDate.IsEditable = true;
			this.txtReleaseDate.InvisibleChar = '●';
			this.table2.Add (this.txtReleaseDate);
			global::Gtk.Table.TableChild w32 = ((global::Gtk.Table.TableChild)(this.table2 [this.txtReleaseDate]));
			w32.TopAttach = ((uint)(8));
			w32.BottomAttach = ((uint)(9));
			w32.LeftAttach = ((uint)(3));
			w32.RightAttach = ((uint)(4));
			w32.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.txtSet = new global::Gtk.Entry ();
			this.txtSet.CanFocus = true;
			this.txtSet.Name = "txtSet";
			this.txtSet.IsEditable = true;
			this.txtSet.InvisibleChar = '●';
			this.table2.Add (this.txtSet);
			global::Gtk.Table.TableChild w33 = ((global::Gtk.Table.TableChild)(this.table2 [this.txtSet]));
			w33.TopAttach = ((uint)(1));
			w33.BottomAttach = ((uint)(2));
			w33.LeftAttach = ((uint)(3));
			w33.RightAttach = ((uint)(4));
			w33.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.txtSortTitle = new global::Gtk.Entry ();
			this.txtSortTitle.CanFocus = true;
			this.txtSortTitle.Name = "txtSortTitle";
			this.txtSortTitle.IsEditable = true;
			this.txtSortTitle.InvisibleChar = '●';
			this.table2.Add (this.txtSortTitle);
			global::Gtk.Table.TableChild w34 = ((global::Gtk.Table.TableChild)(this.table2 [this.txtSortTitle]));
			w34.TopAttach = ((uint)(1));
			w34.BottomAttach = ((uint)(2));
			w34.LeftAttach = ((uint)(1));
			w34.RightAttach = ((uint)(2));
			w34.XOptions = ((global::Gtk.AttachOptions)(4));
			w34.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.txtTagline = new global::Gtk.Entry ();
			this.txtTagline.CanFocus = true;
			this.txtTagline.Name = "txtTagline";
			this.txtTagline.IsEditable = true;
			this.txtTagline.InvisibleChar = '●';
			this.table2.Add (this.txtTagline);
			global::Gtk.Table.TableChild w35 = ((global::Gtk.Table.TableChild)(this.table2 [this.txtTagline]));
			w35.TopAttach = ((uint)(2));
			w35.BottomAttach = ((uint)(3));
			w35.LeftAttach = ((uint)(1));
			w35.RightAttach = ((uint)(4));
			w35.XOptions = ((global::Gtk.AttachOptions)(4));
			w35.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.txtTitle = new global::Gtk.Entry ();
			this.txtTitle.CanFocus = true;
			this.txtTitle.Name = "txtTitle";
			this.txtTitle.IsEditable = true;
			this.txtTitle.InvisibleChar = '●';
			this.table2.Add (this.txtTitle);
			global::Gtk.Table.TableChild w36 = ((global::Gtk.Table.TableChild)(this.table2 [this.txtTitle]));
			w36.LeftAttach = ((uint)(1));
			w36.RightAttach = ((uint)(4));
			w36.XOptions = ((global::Gtk.AttachOptions)(4));
			w36.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.txtTrailer = new global::Gtk.Entry ();
			this.txtTrailer.CanFocus = true;
			this.txtTrailer.Name = "txtTrailer";
			this.txtTrailer.IsEditable = true;
			this.txtTrailer.InvisibleChar = '●';
			this.table2.Add (this.txtTrailer);
			global::Gtk.Table.TableChild w37 = ((global::Gtk.Table.TableChild)(this.table2 [this.txtTrailer]));
			w37.TopAttach = ((uint)(6));
			w37.BottomAttach = ((uint)(7));
			w37.LeftAttach = ((uint)(1));
			w37.RightAttach = ((uint)(4));
			w37.XOptions = ((global::Gtk.AttachOptions)(4));
			w37.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.txtWriters = new global::Gtk.Entry ();
			this.txtWriters.CanFocus = true;
			this.txtWriters.Name = "txtWriters";
			this.txtWriters.IsEditable = true;
			this.txtWriters.InvisibleChar = '●';
			this.table2.Add (this.txtWriters);
			global::Gtk.Table.TableChild w38 = ((global::Gtk.Table.TableChild)(this.table2 [this.txtWriters]));
			w38.TopAttach = ((uint)(5));
			w38.BottomAttach = ((uint)(6));
			w38.LeftAttach = ((uint)(1));
			w38.RightAttach = ((uint)(4));
			w38.XOptions = ((global::Gtk.AttachOptions)(4));
			w38.YOptions = ((global::Gtk.AttachOptions)(4));
			this.ebDetailsBackground.Add (this.table2);
			this.hbox1.Add (this.ebDetailsBackground);
			global::Gtk.Box.BoxChild w40 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.ebDetailsBackground]));
			w40.Position = 1;
			w1.Add (this.hbox1);
			global::Gtk.Box.BoxChild w41 = ((global::Gtk.Box.BoxChild)(w1 [this.hbox1]));
			w41.Position = 0;
			// Internal child iMetaGui.MovieEditor.ActionArea
			global::Gtk.HButtonBox w42 = this.ActionArea;
			w42.Name = "dialog1_ActionArea";
			w42.Spacing = 10;
			w42.BorderWidth = ((uint)(5));
			w42.LayoutStyle = ((global::Gtk.ButtonBoxStyle)(4));
			// Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
			this.buttonCancel = new global::Gtk.Button ();
			this.buttonCancel.CanDefault = true;
			this.buttonCancel.CanFocus = true;
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.UseStock = true;
			this.buttonCancel.UseUnderline = true;
			this.buttonCancel.Label = "gtk-cancel";
			this.AddActionWidget (this.buttonCancel, -6);
			global::Gtk.ButtonBox.ButtonBoxChild w43 = ((global::Gtk.ButtonBox.ButtonBoxChild)(w42 [this.buttonCancel]));
			w43.Expand = false;
			w43.Fill = false;
			// Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
			this.buttonOk = new global::Gtk.Button ();
			this.buttonOk.CanDefault = true;
			this.buttonOk.CanFocus = true;
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.UseStock = true;
			this.buttonOk.UseUnderline = true;
			this.buttonOk.Label = "gtk-save";
			w42.Add (this.buttonOk);
			global::Gtk.ButtonBox.ButtonBoxChild w44 = ((global::Gtk.ButtonBox.ButtonBoxChild)(w42 [this.buttonOk]));
			w44.Position = 1;
			w44.Expand = false;
			w44.Fill = false;
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.DefaultWidth = 966;
			this.DefaultHeight = 637;
			this.Show ();
		}
	}
}
