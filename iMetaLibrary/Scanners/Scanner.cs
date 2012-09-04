using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.ComponentModel;
using iMetaLibrary.Metadata;

namespace iMetaLibrary.Scanners
{
    public abstract class Scanner
    {
        public Scanner()
        {
            this.Items = new Dictionary<string, Meta>();
        }

        private Thread ScannerThread;
        public void Scan(params string[] Folders)
        {
			Logger.Log("Scanning Folders: " + String.Join(";", Folders));
            if (Folders.Length == 0)
            {
                Stop();
                // master scan, use scanner thread
                ScannerThread = new Thread(new ParameterizedThreadStart(Scan_Executing));
                ScannerThread.Start(null);
            }
            else
            {
                // small scan, ie files have changed, use background worker
                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += delegate(object sender, DoWorkEventArgs e)
                {
                    Scan_Executing(Folders);
                };
                worker.RunWorkerAsync();
            }
			this.IsRunning = true;
            TriggeredStarted();
        }
		
		public void RefreshFiles(Meta[] oMetaList)
		{
			//Logger.Log("Refreshing Files: " + String.Join(";", ());
			
            // small scan, ie files have changed, use background worker
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += delegate(object sender, DoWorkEventArgs e)
            {
                RefreshFiles_Executing(oMetaList);
            };
            worker.RunWorkerAsync();
            TriggeredStarted();
		}
		
        public void Stop()
        {
			Logger.Log("Stopping Scanning Thread");
            if (ScannerThread != null)
            {
                ScannerThread.Abort();
                ScannerThread.Join(30 * 1000);
                ScannerThread = null;
            }
			this.IsRunning = false;
        }

        protected abstract void Scan_Executing(object oFolders);
		protected abstract void RefreshFiles_Executing(object oFilenames);

        public delegate void Updated_EventHandler(Meta Item, int Result);
        public event Updated_EventHandler Updated;
        public delegate void ItemAdded_EventHandler(Meta Item);
        public event ItemAdded_EventHandler ItemAdded;
        public delegate void Completed_EventHandler();
        public event Completed_EventHandler Completed;
        public delegate void AllItemsFound_EventHandler(Meta[] NewItems);
        public event AllItemsFound_EventHandler AllItemsFound;
        public delegate void ScanningItem_EventHandler(Meta Item);
        public event ScanningItem_EventHandler ScanningItem;
        public delegate void Started_EventHandler();
        public event Started_EventHandler Started;

        public delegate void StatusUpdated_EventHandler(string StatusText);
        public event StatusUpdated_EventHandler StatusUpdated;
		
		public bool IsRunning { get; protected set; }

        protected void TriggeredStarted()
        {
			Logger.Log("Scanner: Scanner Started");
            if (Started != null)
                Started();
        }

        protected void TriggerUpdated(Meta Item, int Result)
        {
			Logger.Log("Scanner: Item Updated: " + Item.Filename);
            if (Updated != null)
                Updated(Item, Result);
			Item.TriggerMetaUpdated();
        }

        protected void TriggerCompleted()
        {
			Logger.Log("Scanner: Scan completed");
			this.IsRunning = false;
            if (Completed != null)
                Completed();
        }

        protected void TriggerAllItemsFound(Meta[] NewItems)
        {
			Logger.Log("Scanner: All Items Found, {0} items", NewItems.Length);
            if (AllItemsFound != null)
                AllItemsFound(NewItems);
        }

        protected void TriggerScanningItem(Meta Item)
        {
			Logger.Log("Scanner: Scanning Item: {0}", Item.Filename);
            if (ScanningItem != null)
                ScanningItem(Item);
        }

        protected void TriggerStatusUpdated(string StatusText)
        {
			Logger.Log("Scanner: Status Updated: {0}", StatusText);
            if (StatusUpdated != null)
                StatusUpdated(StatusText);
        }

        public Dictionary<string, Meta> Items { get; private set; }

        protected void AddItem(Meta Item)
        {
			if(this.Items.ContainsKey(Item.Filename))
				return;
            this.Items.Add(Item.Filename.ToLower(), Item);
            if (ItemAdded != null)
                ItemAdded(Item);
        }
		
		public abstract string CreateHtmlElement(string BaseDirectory);
		
		protected string CreateImageThumbnail(System.Drawing.Image Image, string Directory, int Width, int Height)
		{
			if(!System.IO.Directory.Exists(Directory))
				System.IO.Directory.CreateDirectory(Directory);
			string shortname = Guid.NewGuid().ToString() + ".png";
			string filename = System.IO.Path.Combine(Directory, shortname);			
			
			using(System.IO.MemoryStream stream = new System.IO.MemoryStream())
			{
				Image.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
				stream.Position = 0;
							
				using(Gdk.Pixbuf img = new Gdk.Pixbuf(stream))
				{
					using(Gdk.Pixbuf pixbuf = img.ScaleSimple( Width, Height, Gdk.InterpType.Bilinear))
					{
						pixbuf.Save(filename, "png"); // doc says "jpeg" is supported, but its not...				
						return shortname;
					}
				}
			}
		}
    }
}
