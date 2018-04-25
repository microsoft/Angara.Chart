module Angara.Chart.SerializationTests

open Angara.Charting
open Angara.Serialization
open NUnit.Framework

let lib = SerializerLibrary("Reinstate")
do Angara.Charting.Serializers.Register [lib]

[<Test>]
let ``Serialization of PlotInfo``() =
    let primitives = 
        [ "string", PlotPropertyValue.StringValue "hello, world!"
        ; "real", PlotPropertyValue.RealValue System.Math.PI
        ; "real Array", PlotPropertyValue.RealArray [| System.Math.PI |] ] |> Map.ofList
    let composite =  [ "composite", PlotPropertyValue.Composite primitives ] |> Map.ofList
    let plotInfo : Angara.Charting.PlotInfo = { Kind = "kind"; DisplayName = "display name"; Titles = Map.empty.Add("x", "y"); Properties = composite }
    let chartInfo = [ plotInfo ] |> Chart.ofList
    let infoSet = ArtefactSerializer.Serialize lib chartInfo
    let chartInfo2 = ArtefactSerializer.Deserialize lib infoSet
    Assert.AreEqual(chartInfo, chartInfo2)

[<Test>]
let ``Serialization of Axes and Layout`` () =
    let chartInfo = [] |> Chart.ofList |> Chart.setLayout ChartLayout.Lean |> Chart.setXAxis (Labelled ([| 1.0; 2.0; 1.5|], [| "A"; "B"; "C" |], 40.0) )|> Chart.setYAxis Numerical
    let infoSet = ArtefactSerializer.Serialize lib chartInfo
    let chartInfo2 = ArtefactSerializer.Deserialize lib infoSet
    Assert.AreEqual(chartInfo, chartInfo2)