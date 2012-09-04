using System;
using System.Collections.Generic;
using Gtk;

namespace iMetaGui
{
	public partial class EpisodesEditorGui : Gtk.Dialog
	{
		iMetaLibrary.Metadata.TvFileMeta Meta;
        private bool UsingTbnFile = false;
		NodeStore store;
		private ITreeNode SelectedEpisodeNode;
		
		public EpisodesEditorGui (iMetaLibrary.Metadata.TvFileMeta Meta)
		{
			this.Build ();
			
			this.Meta = Meta;
						
			this.buttonOk.Clicked += HandleButtonOkhandleClicked;
			
            string tbnFile = Meta.Filename.Substring(0, Meta.Filename.LastIndexOf(".")+1) + "tbn";
            if (System.IO.File.Exists(tbnFile))
            {
				using(System.Drawing.Image image = System.Drawing.Image.FromFile(tbnFile))
				{
					imgThumbnail.Pixbuf = GuiHelper.ImageToPixbufIcon(image, 320, 240);
				}
                UsingTbnFile = true;
            }
            else
            {
                //picThumbnail.Image = Resources.noposter;
            }
			
			store = new NodeStore(typeof(TvEpisodeNode));

			//List<iMetaLibrary.Metadata.TvEpisodeMeta> episodes = new List<iMetaLibrary.Metadata.TvEpisodeMeta>();
			bool first = true;
            foreach (iMetaLibrary.Metadata.TvEpisodeMeta episode in Meta.Episodes)
			{				
                //episodes.Add(episode.Clone());
				iMetaLibrary.Metadata.TvEpisodeMeta epmeta = episode.Clone();
				if(first)
				{
					episodeEditor.Initialize(epmeta);
					first = false;
				}
				store.AddNode(new TvEpisodeNode(epmeta));
			}			nvEpisodeList.NodeStore = store;
			
			nvEpisodeList.AppendColumn("Episode", new Gtk.CellRendererText (), "text", 0);
			
			nvEpisodeList.NodeSelection.Changed += delegate(object o, System.EventArgs args) {				
				
                Gtk.NodeSelection selection = (Gtk.NodeSelection) o;
                TvEpisodeNode node = (TvEpisodeNode) selection.SelectedNode;
				// check selection hasnt changed
	            if (node == SelectedEpisodeNode)
	                return;
				// check if current data in episode editor is valid
	            if (!episodeEditor.IsValid())
	            {
					nvEpisodeList.NodeSelection.SelectNode(SelectedEpisodeNode);
	                return;
	            }
	            else // if valid saved the data to the meta object
	            {
	                episodeEditor.Save();
	                // this reassignment to itself forces the listbox to revalidate it and updates the text in the listbox if change, otherwise it doesnt update
	                //lstEpisodes.Items[SelectedEpisodeIndex] = lstEpisodes.Items[SelectedEpisodeIndex];
	            }
				// update selected epsiode index
	            SelectedEpisodeNode = node;
				// initialize the newly selected episode data
	            episodeEditor.Initialize(node.Meta);
			};
		}

		void HandleButtonOkhandleClicked (object sender, EventArgs e)
		{
            if (!episodeEditor.IsValid())
			{				
				iMetaLibrary.Logger.Log("e type: " + e.GetType());
                return;
			}
            episodeEditor.Save(); // save changes on active episode

            // save the nfo file...
            List<iMetaLibrary.Metadata.TvEpisodeMeta> episodes = new List<iMetaLibrary.Metadata.TvEpisodeMeta>();
			var enumerator = store.GetEnumerator();
			float rating = 0;
			while(enumerator.MoveNext())
			{
				var episode = ((TvEpisodeNode)enumerator.Current).Meta;
				episodes.Add(episode);
				rating += episode.Rating;
			}
            this.Meta.Episodes = episodes.ToArray();
            this.Meta.Save();
			
			if(episodes.Count > 0)
            	this.Meta.Rating = rating / episodes.Count;		
			
			this.DefaultResponse = ResponseType.Ok;
			this.Destroy();
		}
				
		[TreeNode(ListOnly=true)]
		class TvEpisodeNode:TreeNode
		{
			public iMetaLibrary.Metadata.TvEpisodeMeta Meta {get;set;}
			public TvEpisodeNode(iMetaLibrary.Metadata.TvEpisodeMeta Meta)
			{
				this.Meta = Meta;
			}
			
			[Gtk.TreeNodeValue(Column=0)]
			public string DisplayText
			{
				get
				{
					return this.Meta.Title;
				}
			}
			
			
		}
			
	}
}

