using System;
using System.Runtime.Serialization;

namespace TheMovieDb
{
    [DataContract]
    public class TmdbPersonFilm
    {
        [DataMember(Name="name")]
        public string Name { get; set; }

        [DataMember(Name="job")]
        public string Job { get; set; }

        [DataMember(Name="department")]
        public string Department { get; set; }

        [DataMember(Name="url")]
        public string Url { get; set; }

        [DataMember(Name="character")]
        public string Character { get; set; }

        [DataMember(Name="id")]
        public int Id { get; set; }

        [DataMember(Name = "cast_id")]
        public int CastId { get; set; }

        [DataMember(Name = "poster")]
        public string Poster { get; set; }

        [DataMember(Name="adult")]
        public bool Adult { get; set; }

        [DataMember(Name = "Release")]
        public string ReleaseString
        {
            get
            {
                return Release.HasValue ? Release.Value.ToString() : "";
            }
            set
            {
                DateTime d;
                if (DateTime.TryParse(value, out d))
                    Release = d;
                else
                    Release = null;
            }
        }

        public DateTime? Release
        {
            set;
            get;
        }
    }
}
