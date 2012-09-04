using System.Runtime.Serialization;

namespace TheMovieDb
{
    [DataContract(Name="genre")]
    public class TmdbGenre
    {
        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "url")]
        public string Url { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "id")]
        public int Id { get; set; }
    }
}
