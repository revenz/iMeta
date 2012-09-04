using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iMetaLibrary
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Replaces one or more format items in a specified string with the string representation of a specified object.
        /// </summary>
        /// <param name="Input">A composite format string.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <returns>A copy of format in which the format items have been replaced by the string representation of the corresponding objects in args.</returns>
        public static string FormatStr(this string Input, params object[] args)
        {
            return String.Format(Input, args);
        }

        /// <summary>
        /// Converts a string to a safe filename
        /// </summary>
        /// <param name="Filename">the input filename</param>
        /// <returns>a safe filename</returns>
        public static string ToSafeFilename(this string Filename)
        {
            Array.ForEach(System.IO.Path.GetInvalidFileNameChars(),
                  c => Filename = Filename.Replace(c.ToString(), String.Empty));
            return Filename;
        }

        /// <summary>
        /// Checks to see if the value is outside the specified range, and if it is adjusts it
        /// </summary>
        /// <param name="Value">the value to check</param>
        /// <param name="Minimum">the allowed minimum</param>
        /// <param name="Maximum">the allowed maximum</param>
        /// <returns>the value within the maximum and minimum</returns>
        public static int CheckRange(this int Value, int Minimum, int Maximum)
        {
            if (Value < Minimum)
                return Minimum;
            else if (Value > Maximum)
                return Maximum;
            return Value;
        }

        public static string[] ToStringArray(this string Input)
        {
            return (from s in Input.Split(new char[] { ',', ';', '|' }, StringSplitOptions.RemoveEmptyEntries) where s.Trim() != "" select s.Trim()).ToArray();
        }
		
		public static string HtmlEncode(this string Input)
		{
			return System.Web.HttpUtility.HtmlEncode(Input);
		}
    }
}
