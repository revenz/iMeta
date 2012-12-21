using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.IO;

namespace iMetaLibrary.Metadata
{
    public enum MetaCompletionLevel
    {
        None = 0,
        Loading = 1,
        Partial = 2,
        Full = 3
    }

    [Serializable]
    public abstract class Meta
    {
        public string Filename { get; set; }
        public float Rating { get; set; }
        public bool IsNull { get; set; }
        public bool ExistingNfoFile { get; set; }

        public Meta(string Filename)
        {
            this.Filename = Filename;
            this.CompletionLevel = MetaCompletionLevel.None;
            this.MetaId = Guid.NewGuid().ToString();

            if (!String.IsNullOrWhiteSpace(Filename))
            {
                ExistingNfoFile = File.Exists(Filename.Substring(0, Filename.LastIndexOf(".") + 1) + "nfo");
            }
        }

        public MetaCompletionLevel CompletionLevel { get; set; }

        public string MetaId { get; private set; }
		
		public delegate void MetaUpdated_EventHandler();
		public event MetaUpdated_EventHandler MetaUpdated;
		
		internal void TriggerMetaUpdated()
		{
			if(MetaUpdated != null)
				MetaUpdated();
				
		}		
    }
}
