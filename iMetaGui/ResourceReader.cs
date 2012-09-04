using System;
using System.Reflection;
using System.IO;

namespace iMetaGui
{
	public class ResourceReader
	{
        public static string ReadResource(string ResourceName)
        {
            try
            {
                using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("iMetaGui.Resources." + ResourceName))
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
    }
}

