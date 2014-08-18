using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace PrimeNumbersConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var stopwatch = Stopwatch.StartNew();
            var result = GetPrimeNumbers(1000000, 1001000).ToArray();
            stopwatch.Stop();

            Array.ForEach(result, Console.WriteLine);
            Console.WriteLine("{0} seconds", stopwatch.Elapsed.TotalSeconds);
        }

        static readonly Func<long, long, IEnumerable<long>> GetPrimeNumbers_Org = (minValue, maxValue) =>
            new[] { new List<long>() }
                .SelectMany(primes => Enumerable2.Range2(2, maxValue)
                    .Select(i => new { primes, i }))
                .Where(_ => _.primes
                    .All(p => _.i % p != 0))
                .Do(_ => _.primes.Add(_.i))
                .Select(_ => _.i)
                .SkipWhile(i => i < minValue);

        static readonly Func<long, long, IEnumerable<long>> GetPrimeNumbers = (minValue, maxValue) =>
            new[] { new List<long>() }
                .SelectMany(primes => Enumerable2.Range2(2, maxValue)
                    .Select(i => new { primes, i, root = Math.Sqrt(i) }))
                .Where(_ => _.primes
                    .TakeWhile(p => p <= _.root)
                    .All(p => _.i % p != 0))
                .Do(_ => _.primes.Add(_.i))
                .Select(_ => _.i)
                .SkipWhile(i => i < minValue);
    }

    public static class Enumerable2
    {
        public static IEnumerable<long> Range2(long minValue, long maxValue)
        {
            for (var i = minValue; i <= maxValue; i++)
            {
                yield return i;
            }
        }

        public static IEnumerable<TSource> Do<TSource>(this IEnumerable<TSource> source, Action<TSource> action)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (action == null) throw new ArgumentNullException("action");

            foreach (var item in source)
            {
                action(item);
                yield return item;
            }
        }
    }
}
