module IsingModel.TensorExtensions

open FSharpPlus
open FSharpPlus.Data
open Tensor

let tryRoll<'a> (offset: int64 list) (tensor: Tensor<'a>): Result<Tensor<'a>, string> =
    match (tensor |> Tensor.nDims) = (offset |> List.length) with
    | false -> Result.throw "oh no"
    | true ->
        let shape = tensor |> Tensor.shape
        let aShape = Array.ofList shape

        HostTensor.init shape (fun newCoords ->
            let oldCoords =
                (newCoords, offset, aShape)
                |||> Array.zip3
                |> Array.map (fun (c, o, s) -> (c - o + s) % s)
                |> List.ofArray

            Tensor.get tensor oldCoords)
        |> Result.result

let roll offset tensor =
    match tryRoll offset tensor with
    | Error e -> failwith e
    | Ok o -> o
