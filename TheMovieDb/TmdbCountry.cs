using System.Runtime.Serialization;

namespace TheMovieDb
{
    [DataContract(Name="country")]
    public class TmdbCountry
    {
        [DataMember(Name="name")]
        public string Name { get; set; }

        [DataMember(Name="code")]
        public string Code { get; set; }

        [DataMember(Name="url")]
        public string Url { get; set; }
    }
}
