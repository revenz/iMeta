using System.Runtime.Serialization;

namespace TheMovieDb
{
    [DataContract]
    public class TmdbSpokenLanguage
    {
        [DataMember(Name = "code")]
        public string Code { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "native_name")]
        public string NativeName { get; set; }
    }
}
