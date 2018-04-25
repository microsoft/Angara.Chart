namespace Angara.Charting.Serialization

open Angara.Charting
open Angara.Serialization

module internal Helpers = 
    let rec SerializePlotProperties(pps : PlotProperties) =
        pps |> Seq.fold(fun (map : InfoSet) p -> map.AddInfoSet(p.Key, SerializePlotPropertyValue p.Value)) InfoSet.EmptyMap

    and SerializePlotPropertyValue(ppv : PlotPropertyValue) =
        match ppv with
        | StringValue s -> InfoSet.String s
        | RealValue v -> InfoSet.Double v
        | RealArray arr -> InfoSet.DoubleArray arr
        | Composite pp -> SerializePlotProperties pp

    let SerializePlotInfo(pi : PlotInfo) = InfoSet.EmptyMap.AddString("kind", pi.Kind).AddString("displayName", pi.DisplayName).AddInfoSet("titles", InfoSet.ofPairs(pi.Titles |> Map.map(fun _ y -> InfoSet.String y) |> Map.toSeq)).AddInfoSet("properties", SerializePlotProperties(pi.Properties))

    let rec DeserializePlotProperties(infoSet : InfoSet) : PlotProperties = 
        infoSet.ToMap() |> Map.map(fun _ i -> DeserializePlotPropertyValue i)

    and DeserializePlotPropertyValue(infoSet : InfoSet) =
        match infoSet with
        | InfoSet.String s -> PlotPropertyValue.StringValue s
        | InfoSet.Double v -> PlotPropertyValue.RealValue v
        | InfoSet.DoubleArray arr -> PlotPropertyValue.RealArray (arr |> Seq.toArray)
        | InfoSet.Map _ -> PlotPropertyValue.Composite (DeserializePlotProperties infoSet)
        | _ -> failwith "Unexpected type of the InfoSet instance"

    let DeserializePlotInfo(infoSet : InfoSet) = 
        let map = infoSet.ToMap()
        { DisplayName = map.["displayName"].ToStringValue()
        ; Kind = map.["kind"].ToStringValue()
        ; Titles = map.["titles"].ToMap() |> Map.map(fun _ y -> y.ToStringValue())
        ; Properties = DeserializePlotProperties(map.["properties"]) }

    let SerializeAxis (axis : Axis) : InfoSet = 
            match axis with
            | Numerical -> InfoSet.EmptyMap
            | Labelled (ticks, labels, angle) ->
                InfoSet.EmptyMap
                    .AddInfoSet("ticks", InfoSet.DoubleArray ticks)
                    .AddInfoSet("labels", InfoSet.StringArray labels)
                    .AddDouble("angle", angle)

    let DeserializeAxis (infoSet: InfoSet) : Axis =
            let map = infoSet.ToMap()
            if map.IsEmpty then Numerical else Labelled(map.["ticks"].ToDoubleArray(), map.["labels"].ToStringArray(), map.["angle"].ToDouble())

open Helpers

type internal PlotInfoSerializer () = 
    interface ISerializer<PlotInfo> with
        member x.TypeId = "PlotInfo"
        member x.Serialize _ (plot : PlotInfo) : InfoSet = SerializePlotInfo plot
        member x.Deserialize _ (infoSet : InfoSet) : PlotInfo = DeserializePlotInfo infoSet

type internal AxisSerializer () =
    interface ISerializer<Axis> with
        member x.TypeId = "Axis"
        member x.Serialize _ (axis : Axis) : InfoSet = SerializeAxis axis
        member x.Deserialize _ (infoSet: InfoSet) : Axis = DeserializeAxis infoSet


type internal ChartSerializer () = 
    interface ISerializer<Chart> with
        member x.TypeId = "Chart"
        member x.Serialize _ (chart : Chart) : InfoSet =
            InfoSet.EmptyMap
                .AddString("layout", chart.Layout.ToString())
                .AddInfoSet("xAxis", SerializeAxis chart.XAxis)
                .AddInfoSet("yAxis", SerializeAxis chart.YAxis)
                .AddInfoSet("plots", InfoSet.Seq(chart.Plots |> List.map SerializePlotInfo))
        member x.Deserialize _ (infoSet : InfoSet) : Chart = 
            let map = infoSet.ToMap()
            let xAxis = DeserializeAxis map.["xAxis"]
            let yAxis = DeserializeAxis map.["yAxis"]
            let layout = System.Enum.Parse(typeof<ChartLayout>, map.["layout"].ToStringValue()) :?> ChartLayout
            map.["plots"].ToSeq() |> Seq.map DeserializePlotInfo |> Seq.toList |> Chart.ofList |> Chart.setXAxis xAxis |> Chart.setYAxis yAxis |> Chart.setLayout layout

