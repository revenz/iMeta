using System.Runtime.Serialization;

namespace TheMovieDb
{
    [DataContract(Name = "studio")]
    public class TmdbStudio
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "url")]
        public string Url { get; set; }

        [DataMember(Name = "id")]
        public int Id { get; set; }
    }
}
