# Angara.Chart
An F# library that allows to define and display a chart as a collection of plots such as line, band, markers, heatmap. Supports visualization of uncertain values represented as quantiles.

Also, there is a similar TypeScript component [ChartViewer](https://github.com/predictionmachines/InteractiveDataDisplay/blob/master/ChartViewer.md).

## Samples gallery

Each sample is represented as an F# module containing function `samples: unit -> Chart list`. It builds
the list of sample charts, so that then all the charts can be rendered using [Angara.Html](https://github.com/Microsoft/Angara)
library to an html file:

```F#
module Program

open Angara.Charting

type SampleCharts = 
    { Lines: Chart list 
    ; Band: Chart list
    ; Markers: Chart list 
    ; Heatmap: Chart list }

[<EntryPoint>]
let main argv = 
    let samples = 
        { Lines = Line.samples() 
        ; Band = Band.samples()
        ; Markers = Markers.samples() 
        ; Heatmap = Heatmap.samples() }
    Angara.Html.Save "Angara.Chart.SampleGallery.html" samples    
    0 
```


### Loading data using Angara.Table

All the samples below will use the `Data` module to get sample data series. We use [Angara.Table](https://github.com/Microsoft/Angara.Table) library to read data from CSV file.

```F#
module Data

open Angara.Charting

let wheat = Table.ReadFile("wheat.csv")
let uwheat = Table.ReadFile("uwheat.csv")
let site = Table.ReadFile("site.csv")
let npz = Table.ReadFile("npz.csv")
let grid = Table.ReadFile("grid.csv")
let ugrid = Table.ReadFile("ugrid.csv")

let col colName = Tables.ToArray<float[]> colName
let quantiles prefix table = 
    { median = table |> col (prefix + "_median")
      lower68 = table |> col (prefix + "_lb68")
      upper68 = table |> col (prefix + "_ub68")
      lower95 = table |> col (prefix + "_lb95")
      upper95 = table |> col (prefix + "_ub95") }
```

### Line

```F#    
module Line

open Angara.Charting


let samples() =
    let t = Data.site |> Data.col "t"
    let p = Data.site |> Data.col "p"
    let p_uncertain = Data.npz |> Data.quantiles "p"

    [
        [ Plot.line(t, p) ] |> Chart.ofList

        [ Plot.line(Array.init 100 (fun i -> let x = float(i)/10.0 in x*x), stroke = "#7F7F7F", thickness = 3.0) ] |> Chart.ofList

        [ Plot.line(LineX.Values t, LineY.UncertainValues p_uncertain) ] |> Chart.ofList
    ]
```

### Markers

```F# 
module Markers

open Angara.Charting


let samples() =
    let lon = Data.wheat |> Data.col "Lon"
    let lat = Data.wheat |> Data.col "Lat"
    let wheat = Data.wheat |> Data.col "wheat"
    let wheat_uncertain = Data.uwheat |> Data.quantiles "w"
    [
        [ Plot.markers(lon, lat, displayName = "Lat/lon") ] |> Chart.ofList
        
        [ Plot.markers(lon, lat, 
            color = MarkersColor.Values wheat, colorPalette = "0=Red=Green=Yellow=Blue=10", 
            shape = MarkersShape.Circle, displayName = "Lat/lon/color")] |> Chart.ofList
        
        [ Plot.markers(lon, lat, 
            color = MarkersColor.Values wheat, colorPalette = "0=Red=Green=Yellow=Blue=10", 
            size = MarkersSize.Values wheat, sizeRange = (5.0, 25.0),
            shape = MarkersShape.Diamond, displayName = "Lat/lon/color/size")] |> Chart.ofList
        
        [ Plot.markers(lon, lat, 
            color = MarkersColor.UncertainValues wheat_uncertain,
            size = MarkersSize.Value 15.0,
            shape = MarkersShape.Circle, displayName = "uncertain color")] |> Chart.ofList
        
        [ Plot.markers(lon, lat, 
            color = MarkersColor.Values wheat_uncertain.median,
            size = MarkersSize.UncertainValues wheat_uncertain, sizeRange = (5.0, 25.0),
            displayName = "uncertain size")] |> Chart.ofList

        [ Plot.markers(MarkersX.Values lat, MarkersY.UncertainValues wheat_uncertain,
            displayName = "uncertain y")] |> Chart.ofList
    ]
```

### Band

```F#
module Band

open Angara.Charting


let samples() =
    let t = Data.site |> Data.col "t"
    let p_lb95 = Data.npz |> Data.col "p_lb95"
    let p_ub95 = Data.npz |> Data.col "p_ub95"

    [
        [ Plot.band(t, p_lb95, p_ub95) ] |> Chart.ofList
    ]

```

### Heatmap

```F#
module Heatmap

open Angara.Charting


let samples() =
    let lon = Data.grid |> Data.col "lon"
    let lat = Data.grid |> Data.col "lat"
    let value = Data.grid |> Data.col "value"

    let lon2 = Data.ugrid |> Data.col "lon"
    let lat2 = Data.ugrid |> Data.col "lat"
    let value_uncertain = Data.ugrid |> Data.quantiles "value"

    [
        [ Plot.heatmap(lon, lat, value) ] |> Chart.ofList

        [ Plot.heatmap(lon, lat, value, treatAs = HeatmapTreatAs.Discrete) ] |> Chart.ofList

        [ Plot.heatmap(lon2, lat2, HeatmapValues.TabularUncertainValues value_uncertain, colorPalette = "blue,white,yellow,orange") ] |> Chart.ofList
    ]
```

