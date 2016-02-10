module Angara.Charting.Serializers

open Angara.Serialization
open Angara.Charting.Serialization

// Registers proper serializers in given libraries.
let Register(libraries: SerializerLibrary seq) =
    for lib in libraries do
        match lib.Name with
        | "Reinstate" 
        | "Html" ->        
            lib.Register(ChartSerializer())
            lib.Register(PlotInfoSerializer())
        | _ -> () // nothing to register

