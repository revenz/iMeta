using System;
using System.ComponentModel;

namespace TheMovieDb
{
    public class TmdbPersonInfoCompletedEventArgs : AsyncCompletedEventArgs
    {
        private readonly TmdbPerson _person;
        public TmdbPerson Person
        {
            get
            {
                RaiseExceptionIfNecessary();
                return _person;
            }
        }

        public TmdbPersonInfoCompletedEventArgs(
        TmdbPerson person,
        Exception e,
        bool canceled,
        object state)
            : base(e, canceled, state)
        {
            _person = person;
        }
    }
}
