module IsingModel.TensorExtensions

open FSharpPlus
open FSharpPlus.Data
open Tensor

let cartesianProduct seqs =
    Seq.foldBack (fun elem acc ->
        seq {
            for x in elem do
                for y in acc -> x :: y
        }) seqs (Seq.singleton [])

let tryRoll<'a> (offset: int64 []) (tensor: Tensor<'a>): Result<Tensor<'a>, string> =
    match (tensor |> Tensor.nDims) = (offset |> length) with
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

let neighbours coords s =
    [ -1L; 0L; 1L ]
    |> Seq.replicate (Tensor.nDims s)
    |> cartesianProduct
    |> Seq.filter (fun l -> l |> List.filter ((<>) 0L) |> List.length = 1)
    |> Seq.map (fun l ->
        List.zip3 coords l (Tensor.shape s)
        |> List.map (fun (c, o, s) -> (c - o + s) % s))
    |> Seq.map (Tensor.get s)

let allNeighbours s =
    s
    |> HostTensor.mapi (fun coords _ -> neighbours (coords |> List.ofArray) s)

//let roll2 axis offset (tensor: Tensor<'a>) =
//    tensor.GetSlice
