module IsingModel.Constants

open System
open FSharpPlus

let inline private pow n x = pown x n

/// The number of dimensions of simulation
/// Must be 2
[<Literal>]
let numberDimensions = 2

/// The number of spins along one dimension of domain for simulation
/// Must be greater than 0
[<Literal>]
let numberSpins = 20L

[<Literal>]
let power = 2.0
/// The number of iterations of the Metropolis algorithm to simulate
let numberIterations =
    (2L |> pow 5)
    * (numberSpins |> pow numberDimensions)

let times = [ 1L .. numberIterations ]

/// The proportion of spins that start up
let initialUpSpinProbability = 0.5

/// Spin-spin interaction energy
/// J > 0 for ferromagnets
/// J = -1 for antiferromagnetism
/// |J| should be <= 1
let J = 1.0

/// Magnetic moment associated with each spin
let mu = 1.0

/// Curie temperature
let kTc =
    // 2.0 * J * numberDimensions
    4.0 * (abs J) / (Math.Acosh 3.0)

/// Temperature of simulation
let kT = 0.0

/// Magnetic field strength
let H = 1.0

/// Length scale used for smoothing
let smooth = 2

type Constants =
    { numberDimensions: int
      numberSpins: int64
      numberIterations: int64
      initialSpinUpProbability: float
      J: float
      mu: float
      kTc: float
      kT: float
      H: float
      smooth: int }

let constants =
    { numberDimensions = numberDimensions
      numberSpins = numberSpins
      numberIterations = numberIterations
      initialSpinUpProbability = initialUpSpinProbability
      J = J
      mu = mu
      kTc = kTc
      kT = kT
      H = H
      smooth = smooth }
