using System;
using System.ComponentModel;

namespace TheMovieDb
{
    public class TmdbMovieInfoCompletedEventArgs : AsyncCompletedEventArgs
    {
        private readonly TmdbMovie _movie;
        public TmdbMovie Movie
        {
            get
            {
                RaiseExceptionIfNecessary();
                return _movie;
            }
        }

        public TmdbMovieInfoCompletedEventArgs(
        TmdbMovie movie,
        Exception e,
        bool canceled,
        object state)
            : base(e, canceled, state)
        {
            _movie = movie;
        }
    }
}
