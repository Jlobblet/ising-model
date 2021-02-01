module IsingModel.TensorExtensions

open FSharpPlus
open FSharpPlus.Data
open Tensor

let tryRoll<'a> (offset: int64 list) (tensor: Tensor<'a>): Result<Tensor<'a>, string> =
    match (tensor |> Tensor.nDims) = (offset |> List.length) with
    | false -> Result.throw "oh no"
    | true ->
        Tensor.zerosLike tensor
        |> HostTensor.mapi
            (fun newCoords _ ->
                let oldCoords =
                    (newCoords |> List.ofArray, offset, tensor |> Tensor.shape)
                    |||> List.zip3
                    |> List.map (fun (c, o, s) -> (c - o + s) % s)

                Tensor.get tensor oldCoords)
        |> Result.result

let roll offset tensor =
    match tryRoll offset tensor with
    | Error e -> failwith e
    | Ok o -> o
