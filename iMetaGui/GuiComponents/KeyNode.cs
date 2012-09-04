using System;
using Gtk;
using iMetaLibrary;

namespace iMetaGui
{
	public class KeyNode:TreeNode
	{
		public string Key{get;set;}			
		public KeyNode(string Key){this.Key = Key;}
		
		public string Markup { get { return "<span size=\"large\" weight=\"bold\">{0}</span>".FormatStr(this.Key); } }
	}
}

