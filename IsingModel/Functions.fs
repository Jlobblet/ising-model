module IsingModel.Functions

open System
open FSharpPlus.Data
open Tensor

let initSpins (constants: Constants.Constants) =
    let numberDimensions = constants.numberDimensions
    let numberSpins = constants.numberSpins
    let probability = constants.initialSpinUpProbability

    let inner (random: Random) =
        let shape =
            match numberDimensions with
            | 1 -> [ 1L; numberSpins ]
            | 2 -> [ numberSpins; numberSpins ]
            // Otherwise, blow up
            | _ -> failwith "oh no"

        probability
        - HostTensor.randomUniform random (0.0, 1.0) shape
        |> Tensor.Sgn

    Reader inner

let calculateEnergy (constants: Constants.Constants) (spins: Tensor<float>) =
    let J = constants.J
    let mu = constants.mu
    let H = constants.H

    let neighbourSum =
        (spins |> TensorExtensions.roll [ 1L; 0L ])
        + (spins |> TensorExtensions.roll [ -1L; 0L ])
        + (spins |> TensorExtensions.roll [ 0L; 1L ])
        + (spins |> TensorExtensions.roll [ 0L; -1L ])

    (-0.5 * J * spins * neighbourSum - mu * H * spins)
    |> Tensor.sum
    |> fun s -> s / float (Tensor.nElems spins)

let calculateMagnetisation (spins: Tensor<float>) = Tensor.mean spins
