using System.Runtime.Serialization;

namespace TheMovieDb
{
    [DataContract(Name = "poster")]
    public class TmdbPoster
    {
        [DataMember(Name = "image")]
        public TmdbImageInfo TmdbImageInfo { get; set; }
    }
}
