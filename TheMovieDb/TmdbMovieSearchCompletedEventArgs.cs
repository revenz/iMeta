using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace TheMovieDb
{
    public class TmdbMovieSearchCompletedEventArgs : AsyncCompletedEventArgs
    {
        private readonly IEnumerable<TmdbMovie> _movies;
        public IEnumerable<TmdbMovie> Movies
        {
            get
            {
                RaiseExceptionIfNecessary();
                return _movies;
            }
        }

        public TmdbMovieSearchCompletedEventArgs(
        IEnumerable<TmdbMovie> movies,
        Exception e,
        bool canceled,
        object state)
            : base(e, canceled, state)
        {
            _movies = movies;
        }
    }
}
