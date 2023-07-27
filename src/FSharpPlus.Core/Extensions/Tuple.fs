namespace FSharpPlus.Core

/// Additional operations on Tuple (,)
[<RequireQualifiedAccess>]
module Tuple2 =
    let inline mapItem1 ([<InlineIfLambda>]f) (x, y) = (f x, y)
    let inline mapItem2 ([<InlineIfLambda>]f) (x, y) = (x, f y)

/// Additional operations on Tuple (,,)
[<RequireQualifiedAccess>]
module Tuple3 =
    let inline mapItem1 ([<InlineIfLambda>]f) (x, y, z) = (f x, y, z)
    let inline mapItem2 ([<InlineIfLambda>]f) (x, y, z) = (x, f y, z)
    let inline mapItem3 ([<InlineIfLambda>]f) (x, y, z) = (x, y, f z)
