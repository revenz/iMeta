using System;
using Gtk;

namespace iMetaGui
{
	class MainClass
	{
        [STAThread]
		public static void Main (string[] args)
		{			
			Application.Init ();
			MainWindow win = new MainWindow ();
			win.Show ();
			Application.Run ();
		}
	}
}
