using System.Runtime.Serialization;

namespace TheMovieDb
{
    [DataContract(Name="image")]
    public class TmdbImageInfo
    {
        [DataMember(Name="type")]
        public string Type { get; set; }

        [DataMember(Name="size")]
        public string Size { get; set; }

        [DataMember(Name="url")]
        public string Url { get; set; }

        [DataMember(Name="id")]
        public string Id { get; set; }

        [DataMember(Name="width")]
        public int Width { get; set; }

        [DataMember(Name="height")]
        public int Height { get; set; }
    }
}
