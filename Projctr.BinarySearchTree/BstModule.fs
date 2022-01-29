module BstModule

type Tree<'a> = 
  | Empty
  | Node of value: 'a * left: Tree<'a> * right: Tree<'a>

let rec insert newValue (tree : Tree<'a>) =
  match tree with
  | Empty -> Node (newValue, Empty, Empty)
  | Node (value, left, right) when newValue < value ->
    let left' = insert newValue left
    Node (value, left', right)
  | Node (value, left, right) when newValue > value ->
    let right' = insert newValue right
    Node (value, left, right')
  | _ -> tree

let rec findInOrderPredecessor (tree : Tree<'a>) =
  match tree with
  | Empty -> Empty
  | Node (_, _, Empty) -> tree
  | Node (_, _, right) -> findInOrderPredecessor right 

let rec delete value (tree : Tree<'a>) =
  match tree with
  | Empty -> Empty
  | Node (value', left, right) when value < value' ->
    let left' = delete value left
    Node (value', left', right)
  | Node (value', left, right) when value > value' ->
    let right' = delete value right
    Node (value', left, right')
  | Node (_, Empty, Empty) ->
    Empty
  | Node (_, left, Empty) -> 
    left
  | Node (_, Empty, right) ->
    right
  | Node (_, left, right) ->
    let (Node(value', _, _)) = findInOrderPredecessor left
    let left' = delete value' left
    Node (value', left', right)

let search value (tree : Tree<'a>) =
  let rec loop v (tree : Tree<'a>) =
    match tree with
    | Node (value, _, right) when value < v -> loop v right
    | Node (value, left, _) when value > v -> loop v left
    | Node (value, _, _) when value = v -> Some value
    | _ -> None
  
  loop value tree

let create values = 
  let tree = Empty
  values |> Seq.iter (fun v -> insert v tree |> ignore)
  tree
