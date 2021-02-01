// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System
open FSharpPlus
open FSharpPlus.Data
open IsingModel
open Tensor
open XPlot.Plotly

let runSimulationWithConstants constants =
    Functions.initSpins constants
    |> Reader.bind (Metropolis.metropolis2d Constants.constants)
    |> Reader.run
    <| Random(1)



[<EntryPoint>]
let main argv =
    printfn "Will run %i iterations" Constants.constants.numberIterations

    //    seq {
//        for B in -2.0 .. 0.5 .. 2.0 do
//            for kBt in 0.0 .. 0.4 .. 2.269 -> B, kBt
//    }
    [ Constants.constants.H, Constants.constants.kT ]
    |> Seq.map (fun (B, kBt) ->
        printfn $"Simulating %f{B} %f{kBt}"

        { Constants.constants with
              H = B
              kT = kBt }
        |> runSimulationWithConstants
        |> (fun (_, _, m) -> m)
        |> Chart.Line
        |> Chart.WithXTitle "Iteration Number"
        |> Chart.WithYTitle "Average magnetisation"
        |> Chart.WithTitle $"$B = %f{B}, k_BT = %f{kBt}$")
    |> Chart.ShowAll

    //    HostTensor.identity<int64> 3L .* HostTensor.identity<int64> 3L |> printfn "%A"

    0 // return an integer exit code
