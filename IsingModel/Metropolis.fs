module IsingModel.Metropolis

open System
open FSharpPlus
open FSharpPlus.Data
open Tensor

let inline private (%%) a b = (a + b) % b

let metropolis2d (constants: Constants.Constants) (spins: Tensor<float>) =
    let J = constants.J
    let mu = constants.mu
    let H = constants.H
    let kT = constants.kT

    let adjacencyMatrix =
        let i =
            Tensor<float>.identity HostTensor.Dev spins.Shape.[0]

        (i |> TensorExtensions.roll [| 1L; 0L |])
        + (i |> TensorExtensions.roll [| -1L; 0L |])

    let calcEnergy s =
        let neighbourSum =
            adjacencyMatrix .* s + s .* adjacencyMatrix

        (-0.5 * J * spins * neighbourSum - mu * H * spins)
        |> Tensor.sum
        |> fun s -> s / float (Tensor.nElems spins)

    printfn "%A" adjacencyMatrix

    let inner (random: Random) =
        let rec loop iterations (s: Tensor<float>) energies magnetisations =
            match iterations with
            | 0L -> s, energies, magnetisations
            | _ ->
                let coords =
                    s
                    |> Tensor.shape
                    |> List.map (fun e ->
                        random.NextDouble() * float (e - 1L)
                        |> round
                        |> int64)

                let current = Tensor.get s coords

                let neighbours = TensorExtensions.neighbours coords s

                let dE =
                    2.0
                    * (J * current * (Seq.sum neighbours)
                       + mu * H * current)

                let probabilityFlip =
                    if dE <= 0.0 then 1.0 else exp (-dE / kT)

                if random.NextDouble() <= probabilityFlip
                then Tensor.set s coords -current

                loop
                    (iterations - 1L)
                    s
                    (calcEnergy s :: energies)
                    (Functions.calculateMagnetisation s
                     :: magnetisations)

        loop constants.numberIterations spins [] []
        |> (fun (s, e, m) -> s, List.rev e, List.rev m)

    Reader inner
