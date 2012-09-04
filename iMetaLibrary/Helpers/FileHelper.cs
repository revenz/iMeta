// -----------------------------------------------------------------------
// <copyright file="FileHelper.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace iMetaLibrary.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Diagnostics;
    using System.IO;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class FileHelper
    {

        public static void OpenFileDirectoryInExplorer(string Filename)
        {
            Process.Start("explorer.exe", new FileInfo(Filename).Directory.FullName);
        }

        public static void OpenFile(string Filename)
        {
            Process.Start("explorer.exe", Filename);
        }

        public static void OpenLinkInBrowser(string Url)
        {
            // take from: http://devtoolshed.com/content/launch-url-default-browser-using-c
            try
            {
                System.Diagnostics.Process.Start(Url);
            }
            catch (Exception exc1)
            {
                // System.ComponentModel.Win32Exception is a known exception that occurs when Firefox is default browser.  
                // It actually opens the browser but STILL throws this exception so we can just ignore it.  If not this exception,
                // then attempt to open the URL in IE instead.
                if (exc1.GetType().ToString() != "System.ComponentModel.Win32Exception")
                {
                    // sometimes throws exception so we have to just ignore
                    // this is a common .NET bug that no one online really has a great reason for so now we just need to try to open
                    // the URL using IE if we can.
                    try
                    {
                        System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo("IExplore.exe", Url);
                        System.Diagnostics.Process.Start(startInfo);
                        startInfo = null;
                    }
                    catch (Exception)
                    {
                        // still nothing we can do so just show the error to the user here.
                    }
                }
            }
        }
    }
}
