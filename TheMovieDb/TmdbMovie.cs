using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TheMovieDb
{
    [DataContract(Name = "movie")]
    public class TmdbMovie
    {
        [DataMember(Name="popularity")]
        public string Popularity { get; set; }

        [DataMember(Name="translated")]
        public bool? Translated { get; set; }

        [DataMember(Name = "adult")]
        public bool Adult { get; set; }

        [DataMember(Name="language")]
        public string Language { get; set; }

        [DataMember(Name = "original_name")]
        public string OriginalName { get; set; }

        [DataMember(Name="name")]
        public string Name { get; set; }

        [DataMember(Name="alternative_name")]
        public string AlternativeName { get; set; }

        [DataMember(Name = "movie_type")]
        public string Type { get; set; }

        [DataMember(Name="id")]
        public int Id { get; set; }

        [DataMember(Name="imdb_id")]
        public string ImdbId { get; set; }

        [DataMember(Name="url")]
        public string Url { get; set; }

        [DataMember(Name = "votes")]
        public string Votes { get; set; }

        [DataMember(Name="rating")]
        public string Rating { get; set; }

        [DataMember(Name = "status")]
        public string Status { get; set; }

        [DataMember(Name = "tagline")]
        public string Tagline { get; set; }

        [DataMember(Name="certification")]
        public string Certification { get; set; }

        [DataMember(Name="overview")]
        public string Overview { get; set; }

        [DataMember(Name = "keywords")]
        public List<string> Keywords { get; set; }

        [DataMember(Name="released")]
        public string Released { get; set; }

        [DataMember(Name="runtime")]
        public string Runtime { get; set; }

        [DataMember(Name="budget")]
        public string Budget { get; set; }

        [DataMember(Name="revenue")]
        public string Revenue { get; set; }

        [DataMember(Name="homepage")]
        public string Homepage { get; set; }

        [DataMember(Name="trailer")]
        public string Trailer { get; set; }

        [DataMember(Name = "genres")]
        public List<TmdbGenre> Genres { get; set; }

        [DataMember(Name = "studios")]
        public List<TmdbStudio> Studios { get; set; }

        [DataMember(Name = "languages_spoken")]
        public List<TmdbSpokenLanguage> SpokenLanguages { get; set; }

        [DataMember(Name = "countries")]
        public List<TmdbCountry> Countries { get; set; }

        [DataMember(Name = "posters")]
        public List<TmdbImage> Posters { get; set; }

        [DataMember(Name = "backdrops")]
        public List<TmdbImage> Backdrops { get; set; }

        [DataMember(Name = "cast")]
        public List<TmdbCastPerson> Cast { get; set; }

        [DataMember(Name = "version")]
        public int Version { get; set; }

        [DataMember(Name="last_modified_at")]
        public string LastModifiedString
        {
            get
            {
                return LastModified.HasValue ? LastModified.Value.ToString() : "";
            }
            set
            {
                DateTime d;
                if (DateTime.TryParse(value, out d))
                    LastModified = d;
                else
                    LastModified = null;
            }
        }

        public DateTime? LastModified
        {
            set;
            get;
        }
    }
}
