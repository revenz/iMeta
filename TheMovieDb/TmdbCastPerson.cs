using System.Runtime.Serialization;

namespace TheMovieDb
{
    [DataContract(Name="person")]
    public class TmdbCastPerson
    {
        [DataMember(Name="name")]
        public string Name { get; set; }

        [DataMember(Name="job")]
        public string Job { get; set; }

        [DataMember(Name="url")]
        public string Url { get; set; }

        [DataMember(Name="character")]
        public string Character { get; set; }

        [DataMember(Name="id")]
        public int Id { get; set; }

        [DataMember(Name = "profile")]
        public string Profile { get; set; }

        [DataMember(Name="department")]
        public string Department { get; set; }

        [DataMember(Name = "order")]
        public int Order { get; set; }

        [DataMember(Name = "case_id")]
        public int CastId { get; set; }
    }
}
