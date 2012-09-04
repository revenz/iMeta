using System;
using Gtk;

namespace iMetaGui
{
    public class MessageBox
    {
        public static void Show(string Msg)
        {
            MessageDialog md = new MessageDialog (null, DialogFlags.Modal, MessageType.Info, ButtonsType.Ok, Msg);
            md.Run ();
            md.Destroy();
        }
    }
}

