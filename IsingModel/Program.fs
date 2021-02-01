// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System
open FSharpPlus
open FSharpPlus.Data
open IsingModel
open Tensor
open XPlot.Plotly

[<EntryPoint>]
let main argv =
    printfn "Will run %i iterations" Constants.constants.numberIterations
    
    let finalState, energies, mags =
        Functions.initSpins Constants.constants
        |> Reader.bind (Metropolis.metropolis2d Constants.constants)
        |> Reader.run
        <| Random(1)
        
    printfn "Done"
        
    energies
    |> Chart.Line
    |> Chart.Show

    0 // return an integer exit code
