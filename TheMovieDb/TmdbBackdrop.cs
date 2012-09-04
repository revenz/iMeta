using System.Runtime.Serialization;

namespace TheMovieDb
{
    [DataContract(Name = "backdrop")]
    public class TmdbBackdrop
    {
        [DataMember(Name = "image")]
        public TmdbImageInfo TmdbImageInfo { get; set; }
    }
}
