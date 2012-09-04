using System;
using System.Collections.Generic;
using Gtk;
using iMetaLibrary;

namespace iMetaGui
{
	public partial class FolderListGui : Gtk.Dialog
	{
		public FolderListGui (string Title, string Description, string[] Folders)
		{
			this.Build ();
			
			this.Folders = Folders;
			this.label12.Text = Description;
			this.Title = Title;
			
			NodeStore store = new NodeStore(typeof(StringNode));
			foreach(string folder in Folders)
				store.AddNode(new StringNode(){ Value = folder});
			nodeview1.NodeStore = store;
			nodeview1.AppendColumn("Folder", new Gtk.CellRendererText (), "text", 0);
			
			buttonOk.Clicked += delegate {
				// get the folders
				List<string> nodes = new List<string>();				
				var enumerator = store.GetEnumerator();
				while(enumerator.MoveNext())
				{
					string folder = ((StringNode)enumerator.Current).Value;
					if(folder.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString()))
						folder = folder.Substring(0, folder.Length - 1);
					nodes.Add(folder);
				}
				this.Folders = nodes.ToArray();
				this.DialogResult = ResponseType.Ok;
				this.Destroy();
			};
			buttonCancel.Clicked += delegate{
				this.DialogResult = ResponseType.Cancel;
				this.Destroy();
			};
			
			btnAdd.Clicked += delegate {
				using(Gtk.FileChooserDialog fc = new Gtk.FileChooserDialog("Choose the Folder to scan.",
					      	                    						    this,
					        	                    					    FileChooserAction.SelectFolder,
					            	                					    "Cancel", ResponseType.Cancel,
					                	             				 	    "Open", ResponseType.Accept)){
					fc.LocalOnly = false;
					if (fc.Run() == (int)ResponseType.Accept) 
					{
						store.AddNode(new StringNode(){ Value = fc.Filename});
					}
					//Don't forget to call Destroy() or the FileChooserDialog window won't get closed.
					fc.Destroy();
				}
			};
			
			btnRemove.Clicked += delegate {
				foreach(var node in nodeview1.NodeSelection.SelectedNodes)
					store.RemoveNode(node);
			};
		}
		
		public ResponseType DialogResult{get;set;}
		
		public string[] Folders
		{
			get;set;
		}
	
		[TreeNode]
		class StringNode : Gtk.TreeNode
		{
			[TreeNodeValue(Column = 0)]
			public string Value{get;set;}
		}
	}
}

