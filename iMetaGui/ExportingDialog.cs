using System;
using iMetaLibrary;

namespace iMetaGui
{
	public partial class ExportingDialog : Gtk.Dialog
	{
		System.ComponentModel.BackgroundWorker bkgWorker;
		public ExportingDialog (iMetaLibrary.Scanners.MovieScanner MovieScanner, iMetaLibrary.Scanners.TvScanner TvScanner)
		{
			this.Build ();
			
			notebook1.ShowTabs = false;
			notebook1.ShowBorder = false;		
			bkgWorker = new System.ComponentModel.BackgroundWorker();
            bkgWorker.WorkerSupportsCancellation = true;
			bkgWorker.DoWork += HandleBkgWorkerDoWork;
			System.Timers.Timer pulsar = new System.Timers.Timer(50) { AutoReset = true};
			pulsar.Elapsed += delegate {
				pbarExporting.Pulse();
			};
			
			this.buttonCancel.Clicked += delegate(object sender, EventArgs e) 
			{
				bkgWorker.CancelAsync();	
				this.Destroy();
			};
			
			this.buttonOk.Clicked += delegate(object sender, EventArgs e) {
				if(notebook1.Page == 0)
				{
					string folder = filechooserbutton1.Filename;
					if(String.IsNullOrEmpty(folder) || !System.IO.Directory.Exists(folder)){
						MessageBox.Show("Invalid export location specfied.");
						return;
					}
					this.buttonOk.Visible = false;
					bkgWorker.RunWorkerAsync(new object[] { folder, MovieScanner, TvScanner}  );
					pulsar.Start();
					notebook1.Page = 1;
				}
				else
				{
					// final page.
					this.Destroy();
				}
			};
			
			bkgWorker.RunWorkerCompleted += delegate(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e) {
				pulsar.Stop();
				if(e.Result as bool? == true)
				{
					notebook1.Page = 2;
					this.buttonOk.Visible = true;
					this.buttonCancel.Visible = false;
				}
				else
				{
					notebook1.Page = 3;
					this.buttonOk.Visible = true;
					this.buttonCancel.Visible = false;
				}
			};
		}

		void HandleBkgWorkerDoWork (object sender, System.ComponentModel.DoWorkEventArgs e)
		{				
			try
			{	
				object[] parameters = e.Argument as object[];
				string exportdir = parameters[0] as string;
				iMetaLibrary.Scanners.MovieScanner MovieScanner = (iMetaLibrary.Scanners.MovieScanner)parameters[1];
				iMetaLibrary.Scanners.TvScanner TvScanner = (iMetaLibrary.Scanners.TvScanner)parameters[2];
				
	            string html = iMetaGui.ResourceReader.ReadResource("ExportTemplate.html");
	            foreach (System.Text.RegularExpressions.Match match in System.Text.RegularExpressions.Regex.Matches(html, "<!--@Resource\\(\"([^\"]*)\"\\)-->", System.Text.RegularExpressions.RegexOptions.Singleline))
	            {
	                string resource = iMetaGui.ResourceReader.ReadResource(match.Groups[1].Value);
					Logger.Log("Resouce match: " + match.Groups[1].Value);
	                html = html.Replace(match.Value, resource);
	            }
				if(bkgWorker.CancellationPending)
					return;
					
				html = html.Replace("<!--@Movies-->", MovieScanner.CreateHtmlElement(exportdir));
				if(bkgWorker.CancellationPending)
					return;
				html = html.Replace("<!--@TvShows-->", TvScanner.CreateHtmlElement(exportdir));
				if(bkgWorker.CancellationPending)
					return;
				System.IO.File.WriteAllText(System.IO.Path.Combine(exportdir, "imeta_catalog.html"), html);				
				e.Result = true;
			}
			catch(Exception ex)
			{
				Logger.Log(ex.Message + Environment.NewLine + ex.StackTrace);
				e.Result = false;
			}
		}
	}
}

