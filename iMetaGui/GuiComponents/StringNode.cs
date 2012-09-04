using System;
using Gtk;

namespace iMetaGui
{
	
	[TreeNode]
	class StringNode : Gtk.TreeNode
	{
		[TreeNodeValue(Column = 0)]
		public string Value{get;set;}
	}
}

