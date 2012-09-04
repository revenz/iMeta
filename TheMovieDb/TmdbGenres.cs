using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TheMovieDb
{
    [CollectionDataContract]
    public class TmdbGenres : List<TmdbGenre>
    {
        public TmdbGenres() { }
        public TmdbGenres(IEnumerable<TmdbGenre> genres) : base(genres) { }
    }
}
