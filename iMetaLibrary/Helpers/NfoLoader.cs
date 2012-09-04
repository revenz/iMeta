using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Diagnostics;
using System.Reflection;
using System.Globalization;
using iMetaLibrary.Metadata;

namespace iMetaLibrary
{
    public class NfoLoader
    {
        public static bool Load<T>(T Obj, string NfoFile)
        {
			try
			{
	            XDocument doc = XDocument.Load(NfoFile);
	            XElement root = doc.Elements().FirstOrDefault();
	            return Load(Obj, root);
			}
			catch(Exception ex)
			{
				Logger.Log(ex.Message + Environment.NewLine  + ex.StackTrace);
				return false;	
			}
        }

        public static bool Load<T>(T Obj, XElement Root)
        {
            try
            {
                Dictionary<string, PropertyInfo> properties = new Dictionary<string, PropertyInfo>();
                foreach (PropertyInfo property in Obj.GetType().GetProperties())
                    properties.Add(property.Name.ToLower(), property);
                foreach (XElement element in Root.Elements())
                {
                    string propName = element.Name.LocalName;
                    if (!properties.ContainsKey(propName))
                    {
                        propName +="s"; // check for plurals
                        if(!properties.ContainsKey(propName))
                            continue; // no property found
                    }
                    PropertyInfo property = properties[propName];
                    if (!property.CanWrite)
                        continue;
                    if (property.PropertyType == typeof(string))
                        property.SetValue(Obj, element.Value, null);
                    #region numerics
                    else if (property.PropertyType == typeof(float))
                        property.SetValue(Obj, (float)ParseNumeric(element.Value, (float)property.GetValue(Obj, null)), null);
                    else if (property.PropertyType == typeof(int))
                        property.SetValue(Obj, (int)ParseNumeric(element.Value, (int)property.GetValue(Obj, null)), null);
                    else if (property.PropertyType == typeof(double))
                        property.SetValue(Obj, (double)ParseNumeric(element.Value, (double)property.GetValue(Obj, null)), null);
                    else if (property.PropertyType == typeof(short))
                        property.SetValue(Obj, (short)ParseNumeric(element.Value, (short)property.GetValue(Obj, null)), null);
                    #endregion
                    else if (property.PropertyType == typeof(bool))
                    {
                        bool val = false;
                        if (bool.TryParse(element.Value, out val))
                            property.SetValue(Obj, val, null);
                    }
                    else if (property.PropertyType == typeof(DateTime))
                    {
                        DateTime date = DateTime.MinValue;
                        if (DateTime.TryParseExact(element.Value, "yyyy-MM-dd", new CultureInfo("en-us"), DateTimeStyles.None, out date))
                            property.SetValue(Obj, date, null);
                    }
                    else if (property.PropertyType == typeof(string[]))
                    {
                        string[] list = property.GetValue(Obj, null) as string[] ?? new string[] { };
                        list = list.Union(new string[] { element.Value }).ToArray();
                        property.SetValue(Obj, list, null);
                    }
                    else if (property.PropertyType == typeof(KeyValuePair<string, string>[]) && element.HasElements)
                    {
                        XElement[] elements = element.Elements().ToArray();
                        string key = elements[0].Value;
                        string value = null;
                        if (elements.Length > 1)
                            value = elements[1].Value;

                        KeyValuePair<string, string>[] list = property.GetValue(Obj, null) as KeyValuePair<string, string>[] ?? new KeyValuePair<string, string>[] { };
                        list = list.Union(new KeyValuePair<string, string>[] { new KeyValuePair<string, string>(key, value) }).ToArray();
                        property.SetValue(Obj, list, null);
                    }
                    else if (property.PropertyType == typeof(VideoFileMeta))
                    {
                        VideoFileMeta vfm = VideoFileMeta.Load(element);
                        if (vfm != null)
                            property.SetValue(Obj, vfm, null);
                    }
                    else
                    {
                        Trace.WriteLine("Didnt find: " + propName);
                    }

                }
                return true;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                return false;
            }
        }

        private static double ParseNumeric(string Str, double Original)
        {
            try
            {
                return double.Parse(Str);
            }
            catch (Exception) { return Original; }
        }
    }
}
