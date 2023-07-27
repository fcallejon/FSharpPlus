namespace FSharpPlus.Core

/// Additional operations on ValueTuple (,)
[<RequireQualifiedAccess>]
module ValueTuple2 =
    let inline mapItem1 ([<InlineIfLambda>]f) (struct (x, y)) = struct (f x, y)
    let inline mapItem2 ([<InlineIfLambda>]f) (struct (x, y)) = struct (x, f y)

/// Additional operations on ValueTuple (,,)
[<RequireQualifiedAccess>]
module ValueTuple3 =
    let inline mapItem1 ([<InlineIfLambda>]f) (struct (x, y, z)) = struct (f x, y, z)
    let inline mapItem2 ([<InlineIfLambda>]f) (struct (x, y, z)) = struct (x, f y, z)
    let inline mapItem3 ([<InlineIfLambda>]f) (struct (x, y, z)) = struct (x, y, f z)
