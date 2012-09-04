using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TheMovieDb
{
    [CollectionDataContract]
    public class TmdbPersons : List<TmdbPerson>
    {
        public TmdbPersons() { }
        public TmdbPersons(IEnumerable<TmdbPerson> persons) : base(persons) { }
    }
}
