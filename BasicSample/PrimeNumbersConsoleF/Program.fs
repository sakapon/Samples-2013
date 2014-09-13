open System
open System.Collections.Generic
open System.Diagnostics

module Seq2 =
    let do2 action source =
        seq {
            for x in source do
                action(x)
                yield x
        }

let getPrimeNumbers_Org minValue maxValue =
    seq { Math.Max(minValue, 2L) .. maxValue }
    |> Seq.filter (fun i ->
        seq { 2L .. i - 1L }
        |> Seq.forall (fun j -> i % j <> 0L))

let getPrimeNumbers_Alpha minValue maxValue =
    new List<int64>()
    |> Seq.singleton
    |> Seq.collect (fun primes ->
        seq { 2L .. maxValue }
        |> Seq.map (fun i -> (primes, i)))
    |> Seq.filter (fun (primes, i) ->
        primes
        |> Seq.forall (fun p -> i % p <> 0L))
    |> Seq2.do2 (fun (primes, i) -> primes.Add(i))
    |> Seq.map snd
    |> Seq.skipWhile (fun i -> i < minValue)

let getPrimeNumbers_Beta minValue maxValue =
    new List<int64>()
    |> Seq.singleton
    |> Seq.collect (fun primes ->
        seq { 2L .. maxValue }
        |> Seq.map (fun i -> (primes, i, i |> float |> Math.Sqrt |> int64)))
    |> Seq.filter (fun (primes, i, root_i) ->
        primes
        |> Seq.takeWhile (fun p -> p <= root_i)
        |> Seq.forall (fun p -> i % p <> 0L))
    |> Seq2.do2 (fun (primes, i, _) -> primes.Add(i))
    |> Seq.map (fun (_, i, _) -> i)
    |> Seq.skipWhile (fun i -> i < minValue)

let getPrimeNumbers minValue maxValue =
    (
        new List<int64>(),
        Math.Max(minValue, 2L),
        Math.Max(maxValue, 0L),
        if maxValue >= 0L then (maxValue |> float |> Math.Sqrt |> int64) else 0L
    )
    |> Seq.singleton
    |> Seq.collect (fun (primes, min, max, root_max) ->
        (seq { 2L .. Math.Min(root_max, min - 1L) }, seq { min .. max })
        ||> Seq.append
        |> Seq.map (fun i -> (primes, i, i |> float |> Math.Sqrt |> int64)))
    |> Seq.filter (fun (primes, i, root_i) ->
        primes
        |> Seq.takeWhile (fun p -> p <= root_i)
        |> Seq.forall (fun p -> i % p <> 0L))
    |> Seq2.do2 (fun (primes, i, _) -> primes.Add(i))
    |> Seq.map (fun (_, i, _) -> i)
    |> Seq.skipWhile (fun i -> i < minValue)

[<EntryPoint>]
let main argv = 
    let stopwatch = Stopwatch.StartNew()
    let result = (1000000000000L, 1000000001000L) ||> getPrimeNumbers |> Seq.toArray
    stopwatch.Stop()

    result |> Seq.iter (printfn "%A")
    stopwatch.Elapsed.TotalSeconds |> printfn "%A seconds"
    0
