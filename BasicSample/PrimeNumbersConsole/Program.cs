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

        static readonly Func<int, int, IEnumerable<int>> GetPrimeNumbers_Org = (minValue, maxValue) =>
            new[] { new List<int>() }
                .SelectMany(primes => Enumerable2.Range2(2, maxValue)
                    .Select(i => new { primes, i }))
                .Where(_ => _.primes
                    .All(p => _.i % p != 0))
                .Do(_ => _.primes.Add(_.i))
                .Select(_ => _.i)
                .SkipWhile(i => i < minValue);

        static readonly Func<int, int, IEnumerable<int>> GetPrimeNumbers = (minValue, maxValue) =>
            new[] { new List<int>() }
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
        public static IEnumerable<int> Range2(int minValue, int maxValue)
        {
            for (int i = minValue; i <= maxValue; i++)
            {
                yield return i;
            }
        }

        public static IEnumerable<T> Do<T>(this IEnumerable<T> source, Action<T> action)
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
