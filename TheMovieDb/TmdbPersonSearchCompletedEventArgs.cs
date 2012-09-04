using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace TheMovieDb
{
    public class TmdbPersonSearchCompletedEventArgs : AsyncCompletedEventArgs
    {
        private readonly IEnumerable<TmdbPerson> _people;
        public IEnumerable<TmdbPerson> People
        {
            get
            {
                RaiseExceptionIfNecessary();
                return _people;
            }
        }

        public TmdbPersonSearchCompletedEventArgs(
        IEnumerable<TmdbPerson> people,
        Exception e,
        bool canceled,
        object state)
            : base(e, canceled, state)
        {
            _people = people;
        }
    }
}
