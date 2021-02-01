module IsingModel.Metropolis

open System
open FSharpPlus
open FSharpPlus.Data
open Tensor

let inline private (%%) a b = (a + b) % b

let cartesianProduct seqs =
    Seq.foldBack
        (fun elem acc ->
            seq {
                for x in elem do
                    for y in acc -> x :: y
            })
        seqs
        (Seq.singleton [])

let metropolis2d (constants: Constants.Constants) (spins: Tensor<float>) =
    let J = constants.J
    let mu = constants.mu
    let H = constants.H
    let kT = constants.kT

    let inner (random: Random) =
        let rec loop iterations (s: Tensor<float>) energies magnetisations =
            match iterations with
            | 0L -> s, energies, magnetisations
            | _ ->
                let coords =
                    s
                    |> Tensor.shape
                    |> List.map (fun e -> random.NextDouble() * float (e - 1L) |> round |> int64)

                let current = Tensor.get s coords

                let neighbours =
                    [ -1L; 0L; 1L ]
                    |> Seq.replicate (Tensor.nDims s)
                    |> cartesianProduct
                    |> Seq.filter (fun l -> l |> List.filter ((<>) 0L) |> List.length = 1)
                    |> Seq.map
                        (fun l ->
                            List.zip3 coords l (Tensor.shape s)
                            |> List.map (fun (c, o, s) -> (c - o + s) % s))
                    |> Seq.map (Tensor.get s)


                let dE =
                    2.0
                    * (J * current * (Seq.sum neighbours)
                       + mu * H * current)

                let probabilityFlip =
                    if dE <= 0.0 then 1.0 else exp (-dE / kT)

                if random.NextDouble() <= probabilityFlip then Tensor.set s coords -current

                loop
                    (iterations - 1L)
                    s
                    (Functions.calculateEnergy constants s :: energies)
                    (Functions.calculateMagnetisation s
                     :: magnetisations)

        loop constants.numberIterations spins [] []
        |> (fun (s, e, m) -> s, List.rev e, List.rev m)

    Reader inner
