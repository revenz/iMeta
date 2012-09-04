using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace TheMovieDb
{
    public class TmdbGenresCompletedEventArgs : AsyncCompletedEventArgs
    {
        private readonly IEnumerable<TmdbGenre> _genres;
        public IEnumerable<TmdbGenre> Genres
        {
            get
            {
                RaiseExceptionIfNecessary();
                return _genres;
            }
        }

        public TmdbGenresCompletedEventArgs(
        IEnumerable<TmdbGenre> genres,
        Exception e,
        bool canceled,
        object state)
            : base(e, canceled, state)
        {
            _genres = genres;
        }
    }
}
