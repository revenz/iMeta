using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TheMovieDb
{
    [DataContract]
    public class TmdbPerson
    {
        [DataMember(Name="score")]
        public string Score { get; set; }

        [DataMember(Name="popularity")]
        public string Popularity { get; set; }

        [DataMember(Name="name")]
        public string Name { get; set; }

        //[DataMember("known_as")]
        //public TmdbAlsoKnownAs AlsoKnownAs { get; set; }

        [DataMember(Name="url")]
        public string Url { get; set; }

        [DataMember(Name="id")]
        public int Id { get; set; }

        [DataMember(Name="biography")]
        public string Biography { get; set; }

        [DataMember(Name="known_movies")]
        public int KnownMovies { get; set; }

        [DataMember(Name="birthday")]
        public string BirthdayString { get; set; }

        public DateTime? Birthday
        {
            get
            {
                DateTime d;
                if (string.IsNullOrEmpty(BirthdayString) || !DateTime.TryParse(BirthdayString, out d))
                    return null;
                else
                    return d;
            }
        }

        [DataMember(Name="birthplace")]
        public string Birthplace { get; set; }

        [DataMember(Name = "profile")]
        public List<TmdbImage> Images { get; set; }

        [DataMember(Name = "filmography")]
        public List<TmdbPersonFilm> Filmography { get; set; }
    }
}
