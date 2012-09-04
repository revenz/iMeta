using System;
using System.Collections.Generic;
using Gtk;
using iMetaLibrary;

namespace iMetaGui
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class EpisodeEditorGui : Gtk.Bin
	{
		iMetaLibrary.Metadata.TvEpisodeMeta EpisodeMeta;
		
		NodeStore store;
		
		public EpisodeEditorGui ()
		{
			this.Build ();	
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
		}
		
		public void Initialize(iMetaLibrary.Metadata.TvEpisodeMeta EpisodeMeta)
		{
			this.EpisodeMeta = EpisodeMeta;		
			
			store.Clear();
			
			ratingwidget1.Value = (int)EpisodeMeta.Rating;
			
            txtEpisodeName.Text = EpisodeMeta.Title ?? "";
            txtStudio.Text = EpisodeMeta.Studio ?? "";
            txtPlot.Buffer.Text = EpisodeMeta.Plot ?? "";
            txtMpaa.Text = EpisodeMeta.Mpaa;
            txtWriters.Text = String.Join(", ", EpisodeMeta.Writers ?? new string[] { });
            txtDirectors.Text = String.Join(", ", EpisodeMeta.Directors ?? new string[] { });
            //ctrlRating.CurrentRating = (int)EpisodeMeta.Rating;
            txtAired.Text  = EpisodeMeta.Aired < new DateTime(1850,1,1) ? "": EpisodeMeta.Aired.ToString("yyyy-MM-dd");
            txtPremiered.Text = EpisodeMeta.Premiered < new DateTime(1850,1,1) ? "" : EpisodeMeta.Premiered.ToString("yyyy-MM-dd");
            numEpisode.Value = EpisodeMeta.Episode.CheckRange((int)numEpisode.Adjustment.Lower, (int)numEpisode.Adjustment.Upper);
            numSeason.Value = EpisodeMeta.Season.CheckRange((int)numSeason.Adjustment.Lower, (int)numSeason.Adjustment.Upper);
            numDisplayEpisode.Value = EpisodeMeta.DisplayEpisode.CheckRange((int)numDisplayEpisode.Adjustment.Lower, (int)numDisplayEpisode.Adjustment.Upper);
            numDisplaySeason.Value = EpisodeMeta.DisplaySeason.CheckRange((int)numDisplaySeason.Adjustment.Lower, (int)numDisplaySeason.Adjustment.Upper);
            txtEpisodeStart.Text = new DateTime(new TimeSpan(0, 0, EpisodeMeta.EpBookmark).Ticks).ToString("HH:mm:ss");
			
			if(EpisodeMeta.Actors != null){
				foreach(var actor in EpisodeMeta.Actors)
					store.AddNode(new ActorNode() { Name = actor.Key, Role = actor.Value});
			}
		}
		
		public void Save()
		{
            EpisodeMeta.Title = txtEpisodeName.Text;
            EpisodeMeta.Studio = txtStudio.Text;
            EpisodeMeta.Plot = txtPlot.Buffer.Text;
            EpisodeMeta.Mpaa = txtMpaa.Text;
            EpisodeMeta.Writers = txtWriters.Text.ToStringArray();
            EpisodeMeta.Directors = txtDirectors.Text.ToStringArray();
            if(((int)EpisodeMeta.Rating) != ratingwidget1.Value) // check if the rating was changed
                EpisodeMeta.Rating = ratingwidget1.Value;
            if(!String.IsNullOrEmpty(txtAired.Text))
                EpisodeMeta.Aired = DateTime.ParseExact(txtAired.Text, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            if(!String.IsNullOrEmpty(txtPremiered.Text))
                EpisodeMeta.Premiered = DateTime.ParseExact(txtPremiered.Text, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            EpisodeMeta.Episode = (int)numEpisode.Value;
            EpisodeMeta.Season = (int)numSeason.Value;
            EpisodeMeta.DisplayEpisode = (int)numDisplayEpisode.Value;
            EpisodeMeta.DisplaySeason = (int)numDisplaySeason.Value;
            //EpisodeMeta.EpBookmark = (int)DateTime.ParseExact(mtbEpisodeStart.Text, "HH:mm:ss", CultureInfo.InvariantCulture).TimeOfDay.TotalSeconds;
			/*
            List<KeyValuePair<string, string>> actors = new List<KeyValuePair<string,string>>();
			
			var enumerator = store.GetEnumerator();
			while(enumerator.MoveNext())
			{
				var actor = (ActorNode)enumerator.Current;
				if(!String.IsNullOrWhiteSpace(actor.Name.Trim()))
					actors.Add(new KeyValuePair<string, string>(actor.Name, actor.Role));
			}
            EpisodeMeta.Actors = actors.ToArray();*/
			EpisodeMeta.Actors = ActorNode.GetActors(store);
		}
		
		public bool IsValid()
		{
			if(String.IsNullOrWhiteSpace(txtEpisodeName.Text))
			{
				// show error some how
				return false;
			}
			if(txtPremiered.Text.Trim() != "" && !System.Text.RegularExpressions.Regex.IsMatch(txtPremiered.Text, @"^[\d]{4}\-[\d]{1,2}-[\d]{1,2}$"))
				return false;
			if(txtAired.Text.Trim() != "" && !System.Text.RegularExpressions.Regex.IsMatch(txtAired.Text, @"^[\d]{4}\-[\d]{1,2}-[\d]{1,2}$"))
				return false;
				
			return true;
		}
	}
}

