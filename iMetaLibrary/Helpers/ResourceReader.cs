using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace iMetaLibrary.Helpers
{
    public class ResourceReader
    {
        public static string ReadResource(string ResourceName)
        {
            try
            {
                using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("iMeta." + ResourceName))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static string GetHtmlResource(string ResourceName)
        {
            return ReadResource("Html." + ResourceName);
        }
    }
}
