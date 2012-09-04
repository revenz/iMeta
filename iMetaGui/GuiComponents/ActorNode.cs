using System;
using System.Collections.Generic;
using Gtk;

namespace iMetaGui
{
		
	[TreeNode(ListOnly=true)]
	class ActorNode : Gtk.TreeNode
	{
		[Gtk.TreeNodeValue(Column=0)]
		public string Name{get;set;}
		[Gtk.TreeNodeValue(Column=1)]
		public string Role{get;set;}
		
		public ActorNode()
		{
			this.Name = "";
			this.Role = "";
		}
		
		public static KeyValuePair<string, string>[] GetActors(NodeStore Store)
		{
            List<KeyValuePair<string, string>> actors = new List<KeyValuePair<string,string>>();
			
			var enumerator = Store.GetEnumerator();
			while(enumerator.MoveNext())
			{
				var actor = (ActorNode)enumerator.Current;
				if(!String.IsNullOrWhiteSpace(actor.Name.Trim()))
					actors.Add(new KeyValuePair<string, string>(actor.Name, actor.Role));
			}
			return actors.ToArray();
		}
	}
		
}

