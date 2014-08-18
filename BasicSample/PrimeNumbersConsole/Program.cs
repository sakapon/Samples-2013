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
            var result = GetPrimeNumbers(1000000000000, 1000000001000).ToArray();
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

        static readonly Func<long, long, IEnumerable<long>> GetPrimeNumbers_Alpha = (minValue, maxValue) =>
            new[] { new List<long>() }
                .SelectMany(primes => Enumerable2.Range2(2, maxValue)
                    .Select(i => new { primes, i, root_i = (long)Math.Sqrt(i) }))
                .Where(_ => _.primes
                    .TakeWhile(p => p <= _.root_i)
                    .All(p => _.i % p != 0))
                .Do(_ => _.primes.Add(_.i))
                .Select(_ => _.i)
                .SkipWhile(i => i < minValue);

        static readonly Func<long, long, IEnumerable<long>> GetPrimeNumbers = (minValue, maxValue) =>
            new[]
            {
                new
                {
                    primes = new List<long>(),
                    min = Math.Max(minValue, 2),
                    max = Math.Max(maxValue, 0),
                    root_max = maxValue >= 0 ? (long)Math.Sqrt(maxValue) : 0,
                }
            }
                .SelectMany(_ => Enumerable2.Range2(2, Math.Min(_.root_max, _.min - 1))
                    .Concat(Enumerable2.Range2(_.min, _.max))
                    .Select(i => new { _.primes, i, root_i = (long)Math.Sqrt(i) }))
                .Where(_ => _.primes
                    .TakeWhile(p => p <= _.root_i)
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
