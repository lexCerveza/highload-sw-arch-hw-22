module Program

open System
open BstModule
open BenchmarkDotNet.Attributes
open BenchmarkDotNet.Running
open BenchmarkDotNet.Jobs

type System.Random with
    member this.GetValues(minValue, maxValue, take) =
        Seq.initInfinite (fun _ -> this.Next(minValue, maxValue)) |> Seq.take take

[<MemoryDiagnoser>]
[<SimpleJob (RuntimeMoniker.Net50)>]
type BinarySearchTree () =
    let r = Random()
    let mutable values = []

    [<Params (100, 500, 1000)>] 
    member val public TreeSize = 0 with get, set

    [<GlobalSetup>]
    member self.GlobalSetupData() =
        values <- r.GetValues(1, self.TreeSize, self.TreeSize) |> List.ofSeq

    [<Benchmark>]
    member self.Search () = create values |> search self.TreeSize

[<EntryPoint>]
let main _ =
    BenchmarkRunner.Run<BinarySearchTree>() |> ignore
    0