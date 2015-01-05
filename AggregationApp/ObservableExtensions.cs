using System;
using System.Collections.Generic;
using System.Reactive.Linq;

namespace AggregationApp
{
    public static class ObservableExtensions
    {
        public static IObservable<T> RunningMax<T>(this IObservable<T> source)
        {
            return source.Scan(MaxOf).Distinct();
        }

        private static T MaxOf<T>(T x, T y)
        {
            var comparer = Comparer<T>.Default;
            if (comparer.Compare(x, y) < 0)
                return y;
            return x;
        }
    }
}
