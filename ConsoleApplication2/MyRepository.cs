using System;
using System.Reactive.Subjects;

namespace ConsoleApplication2
{
    public class MyRepository
    {
        private static readonly Lazy<MyRepository> _lazy = new Lazy<MyRepository>();

        private readonly ReplaySubject<string> _subjectOfStrings = new ReplaySubject<string>();

        public IObservable<string> StringsObservable
        {
            get { return _subjectOfStrings; }
        }

        public static MyRepository Instance
        {
            get { return _lazy.Value; }
        }

        public MyRepository()
        {
            for (var i = 0; i < 10; i++)
            {
                _subjectOfStrings.OnNext(i.ToString());
            }
        }

        public void Add(string s)
        {
            _subjectOfStrings.OnNext(s);
        }
    }
}
