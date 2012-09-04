using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TheMovieDb
{
    [CollectionDataContract]
    public class TmdbMovies : List<TmdbMovie>
    {
        public TmdbMovies() { }
        public TmdbMovies(IEnumerable<TmdbMovie> movies) : base(movies) { }
    }
}
