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

    let SerializePlotInfo(pi : PlotInfo) = InfoSet.EmptyMap.AddString("kind", pi.Kind).AddString("displayName", pi.DisplayName).AddInfoSet("properties", SerializePlotProperties(pi.Properties))

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
        ; Properties = DeserializePlotProperties(map.["properties"]) }

open Helpers

type internal PlotInfoSerializer () = 
    interface ISerializer<PlotInfo> with
        member x.TypeId = "PlotInfo"
        member x.Serialize _ (plot : PlotInfo) : InfoSet = SerializePlotInfo plot
        member x.Deserialize _ (infoSet : InfoSet) : PlotInfo = DeserializePlotInfo infoSet

type internal ChartSerializer () = 
    interface ISerializer<Chart> with
        member x.TypeId = "Chart"
        member x.Serialize _ (chart : Chart) : InfoSet = InfoSet.Seq(chart.Plots |> List.map SerializePlotInfo)
        member x.Deserialize _ (infoSet : InfoSet) : Chart = 
            { Plots = infoSet.ToSeq() |> Seq.map DeserializePlotInfo |> Seq.toList }

