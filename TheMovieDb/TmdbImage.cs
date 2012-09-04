using System.Runtime.Serialization;

namespace TheMovieDb
{
    [DataContract]
    public class TmdbImage
    {
        [DataMember(Name="image")]
        public TmdbImageInfo ImageInfo { get; set; }
    }
}
