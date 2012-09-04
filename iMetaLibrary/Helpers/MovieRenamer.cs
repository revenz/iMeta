using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using iMetaLibrary.Metadata;

namespace iMetaLibrary.Helpers
{
    class MovieRenamer
    {
        static MovieRenamer()
        {
            Movies = new Dictionary<string, MovieToRename>();
			
            RegexSets = new List<RegexSet>();
            XDocument doc = XDocument.Load("MovieRenames.xml");
            foreach (XElement element in doc.Element("movies").Elements())
            {
                string set = element.Attribute("name").Value.Trim();
                if (element.Attribute("match") != null)
                {
                    Regex rgx = new Regex(element.Attribute("match").Value.Trim(), RegexOptions.IgnoreCase);
                    RegexSet rgxSet = new RegexSet()
                    {
                        Expression = rgx,
                        Name = set
                    };
                    foreach (XElement eleReplacement in element.Elements("replacement"))
                    {
                        RegexReplacement replacement = new RegexReplacement();
                        replacement.Expression = new Regex(eleReplacement.Attribute("regex").Value, RegexOptions.IgnoreCase);
                        replacement.Value = eleReplacement.Attribute("value") != null ? eleReplacement.Attribute("value").Value : "";
                        rgxSet.Replacements.Add(replacement);
                    }
                    RegexSets.Add(rgxSet);
                }
                foreach (XElement ele in element.Elements("movie"))
                {
                    string name = ele.Attribute("name").Value.Trim();
                    string rename = ele.Attribute("rename") != null ? ele.Attribute("rename").Value.Trim() : null;
                    string year = ele.Attribute("year") != null ? ele.Attribute("year").Value.Trim() : "";
                    string sort = ele.Attribute("sort") != null ? ele.Attribute("sort").Value.Trim() : null;
                    Movies.Add(name.ToLower() + year, new MovieToRename() { OriginalName = name, Set = set, Renamed = rename, Year = year, Sort = sort });
                }            
            }
        }

        public static Dictionary<string, MovieToRename> Movies { get; private set; }

        //public static Dictionary<Regex, string> RegexSets { get; private set; }
        public static List<RegexSet> RegexSets { get; private set; }

        public static void Rename(MovieMeta Meta)
        {
            MovieToRename rename = Movies.ContainsKey(Meta.Title.ToLower() + Meta.Year) ? Movies[Meta.Title.ToLower() + Meta.Year] : Movies.ContainsKey(Meta.Title.ToLower()) ? Movies[Meta.Title.ToLower()] : null;
            if(rename == null && !String.IsNullOrWhiteSpace(Meta.OriginalTitle))
                rename = Movies.ContainsKey(Meta.OriginalTitle.ToLower() + Meta.Year) ? Movies[Meta.OriginalTitle.ToLower() + Meta.Year] : Movies.ContainsKey(Meta.OriginalTitle.ToLower()) ? Movies[Meta.OriginalTitle.ToLower()] : null;
            if (rename == null)
            {
                // look for wildcard set
                foreach (RegexSet regexSet in RegexSets)
                {
                    if (regexSet.Expression.IsMatch(Meta.Title) || (!String.IsNullOrWhiteSpace(Meta.OriginalTitle) && regexSet.Expression.IsMatch(Meta.OriginalTitle)))
                    {
                        Meta.Set = regexSet.Name;
                        // rename the title...
                        string title = Meta.Title;
                        foreach (RegexReplacement replacement in regexSet.Replacements)
                        {
                            if (replacement.Expression.IsMatch(title))
                                title = replacement.Expression.Replace(title, replacement.Value);
                        }
                        if (Meta.Title != title)
                        {
                            if (Meta.OriginalTitle == null)
                                Meta.OriginalTitle = Meta.Title;
                        }
                        Meta.Title = title;
                        return;
                    }
                }
                return;
            }
            Meta.Set = rename.Set;
            Meta.OriginalTitle = Meta.OriginalTitle ?? Meta.Title;
            if (!String.IsNullOrEmpty(rename.Sort))
                Meta.SortTitle = rename.Sort;
            if (!String.IsNullOrEmpty(rename.Renamed)) // this could be blank if we are just doing a rename to include it in a set
            {
                Meta.Title = rename.Renamed;
                if (String.IsNullOrEmpty(rename.Sort)) // only change sort to renamed if no sort given
                    Meta.SortTitle = rename.Renamed;

            }
        }
    }

    class MovieToRename
    {
        public string Set { get; set; }
        public string OriginalName { get; set; }
        public string Renamed { get; set; }
        public string Year { get; set; }
        public string Sort { get; set; }
    }

    class RegexSet
    {
        public string Name { get; set; }
        public Regex Expression { get; set; }
        public List<RegexReplacement> Replacements { get; set; }

        public RegexSet()
        {
            Replacements = new List<RegexReplacement>();
        }
    }

    class RegexReplacement
    {
        public Regex Expression { get; set; }
        public string Value { get; set; }
    }
}
