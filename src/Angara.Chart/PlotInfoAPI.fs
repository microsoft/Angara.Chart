namespace Angara.Charting

open System

/// Contains quantiles describing a series of continuous probability distributions. 
type QuantileArray = 
    { median : float []
      lower68 : float []
      upper68 : float []
      lower95 : float []
      upper95 : float [] }

/// Defines positions of markers on horizontal axis.
type MarkersX = 
    /// An array of real values.
    | Values    of float[]

/// Defines positions of markers on vertical axis.
/// It is either an exact values or uncertain values.
type MarkersY = 
    /// An array of real values.
    | Values            of float[]
    /// Values are uncertain and represented as array of quantiles.
    /// The markers will be rendered as box-and-whisker plots.
    | UncertainValues   of QuantileArray

/// Defines colors of markers.
/// It can be a single color,
/// or an array of real values bound to a color palette,
/// or an array of uncertain values.
type MarkersColor = 
    /// All markers are rendered using this color.
    | Value             of string
    /// Marker colors are bound to these values through color palette.
    | Values            of float[]
    /// Markers are rendered as 'bull-eyes' when combination of two colors indicates level of uncertainty.
    | UncertainValues   of QuantileArray

/// Defines sizes of markers. 
/// It can be a single value in screen pixels,
/// or an array of real values bound to a size range in pixels,
/// or an array of uncertain values.
type MarkersSize = 
    /// All markers are rendered of this size in pixels.
    | Value              of float
    /// Marker sizes are bound to these values through size range.
    | Values            of float[]
    /// Markers are rendered as 'petals' when shape of a marker indicates level of uncertainty.
    | UncertainValues   of QuantileArray

/// Defines minimal and maximal size in screen pixels.
type MarkersSizeRange = float * float

type MarkersShape = 
    | Box
    | Circle
    | Diamond
    | Cross
    | Triangle
    | Custom of string

/// Defines positions of data points on horizontal axis.
/// It can be an array of real values.
type LineX = 
    | Values            of float[]
    /// Indicates that positions are indices of the LineY array.
    | Indices

/// Defines positions of data points on vertical axis.
/// It is either an exact values or uncertain values.
type LineY =     
    /// An array of real values.
    | Values            of float[]
    /// Values are uncertain and represented as array of quantiles.
    /// Quantiles will be displayed as colored bands.
    | UncertainValues   of QuantileArray

/// Determines whether the data points must be ordered or not.
type LineTreatAs = 
    /// The data points are ordered prior to rendering by increasing 'x'.
    | Function
    /// The data points are rendered in the original order.
    | Trajectory


/// Defines positions of points on horizontal axis.
type HeatmapX = 
    /// Coordinates of grid points on horizontal axis.
    | Values   of float[]
    
/// Defines positions of points on vertical axis.
type HeatmapY = 
    /// Coordinates of grid points on vertical axis.
    | Values   of float[]

/// Defines the values to be displayed as colors.
type HeatmapValues = 
    /// Values are represented as 1d-array; and corresponding items of the series "x" and "y" are considered as pairs of coordinates of a grid point,
    /// i.e. x[i], y[i], and i-th element of this array together define a grid point.
    | TabularValues            of float[]
    /// Uncertian values are represented as 1d-array; and corresponding items of the series "x" and "y"  are considered as pairs of coordinates of a grid point,
    /// i.e. x[i], y[i], and i-th element of this array together define a grid point.
    | TabularUncertainValues   of QuantileArray

/// Determines whether color within each heatmap cell is interpolated or solid.
type HeatmapTreatAs =
    | Gradient
    | Discrete

[<AbstractClass; Sealed>]
type Plot private () = 
    static let markersType = "markers"
    static let lineType = "line"
    static let bandType = "band"
    static let heatmapType = "heatmap"
    static let defaultColorPalette = 
        "#a6611aff=0.09090909090909091=#ca8325ff=0.18181818181818182=#d8a145ff=0.2727272727272727=#ddba6fff=0.36363636363636364=#cfdb7dff=0.4545454545454546=#92d67eff=0.5454545454545454=#7fd1a1ff=0.6363636363636363=#6cccbdff=0.7272727272727273=#2cd2b8ff=0.8181818181818182=#11b098ff=0.9090909090909092=#018571ff"
    static let defaultSizeRange = 10.0, 30.0
    static let defaultSize = 8.0
    static let defaultShape = MarkersShape.Box
    static let defaultBorderColor = "black"
    static let defaultColor = "#606060"
    static let defaultStroke = "#1F497D"
    static let defaultThickness = 1.0
    static let defaultFill68 = "#1F497D"
    static let defaultFill95 = "#1F497D"
    static let defaultMarkersTitles = {x = None; y = None; color = None; size = None}
    static let defaultLineTitles = {LineTitles.x = None; LineTitles.y = None}
    static let defaultBandTitles = {x = None; y1 = None; y2 = None}
    static let defaultHeatmapTitles = {HeatmapTitles.x = None; HeatmapTitles.y = None; HeatmapTitles.value = None}
    
    static let quantilesToPropertyValue (quant: QuantileArray) : PlotPropertyValue =
        PlotPropertyValue.OfPairs 
            [ "median",  PlotPropertyValue.RealArray quant.median
            ; "lower68", PlotPropertyValue.RealArray quant.lower68
            ; "upper68", PlotPropertyValue.RealArray quant.upper68
            ; "lower95", PlotPropertyValue.RealArray quant.lower95
            ; "upper95", PlotPropertyValue.RealArray quant.upper95 ]
                
    static let shapeToPropertyValue = 
        function 
        | Box -> StringValue "box"
        | Circle -> StringValue "circle"
        | Diamond -> StringValue "diamond"
        | Cross -> StringValue "cross"
        | Triangle -> StringValue "triangle"
        | Custom s -> StringValue s


    /// Displays data as a collection of points, each having the value of one variable determining the position on the horizontal axis and the value of the other variable determining the position on the vertical axis. 
    static member markers (x : MarkersX, y : MarkersY, ?color : MarkersColor, ?colorPalette : string, 
                           ?size : MarkersSize, ?sizeRange : MarkersSizeRange, ?shape : MarkersShape, ?borderColor : string, ?displayName: string, ?titles: MarkersTitles) : PlotInfo = 
        let name = defaultArg displayName "markers"
        let colorPalette = defaultArg colorPalette defaultColorPalette
        let size = defaultArg size (MarkersSize.Value defaultSize)
        let sizeMin, sizeMax = defaultArg sizeRange defaultSizeRange
        let shape = defaultArg shape defaultShape
        let borderColor = defaultArg borderColor defaultBorderColor

        let x = 
            match x with
            | MarkersX.Values array -> PlotPropertyValue.RealArray array
        
        let y = 
            match y with
            | MarkersY.Values array -> PlotPropertyValue.RealArray array
            | MarkersY.UncertainValues array -> quantilesToPropertyValue array

        let color = 
            match color with
            | Some c -> 
                match c with
                | MarkersColor.Value c -> 
                    [ "color", PlotPropertyValue.StringValue c ]
                | MarkersColor.Values array -> 
                    [ "color", PlotPropertyValue.RealArray array
                    ; "colorPalette", StringValue colorPalette ]
                | MarkersColor.UncertainValues array -> 
                    [ "color", quantilesToPropertyValue array
                    ; "colorPalette", StringValue colorPalette ]
            | None -> 
                    [ "color", PlotPropertyValue.StringValue defaultColor ]
        
        let size = 
            match size with
            | MarkersSize.Value n -> 
                [ "size", PlotPropertyValue.RealValue n ]
            | MarkersSize.Values array -> 
                [ "size", PlotPropertyValue.RealArray array
                ; "sizeRange", PlotPropertyValue.OfPairs ["min", RealValue sizeMin; "max", RealValue sizeMax ] ] 
            | MarkersSize.UncertainValues array ->
                [ "size", quantilesToPropertyValue array
                ; "sizeRange", PlotPropertyValue.OfPairs ["min", RealValue sizeMin; "max", RealValue sizeMax ] ] 
        let titles =  match titles with
                      | Some value -> 
                            let t = match value.x with
                                    | Some x -> Map.empty<string, string>.Add("x", x)
                                    | None -> Map.empty<string, string>
                            let t = match value.y with
                                    | Some y -> t.Add("y", y)
                                    | None -> t
                            let t = match value.color with
                                    | Some color -> t.Add("color", color)
                                    | None -> t
                            let t = match value.size with
                                    | Some size -> t.Add("size", size)
                                    | None -> t
                            t
                      | None -> Map.empty
        { Kind = markersType
        ; DisplayName = name
        ; Titles = titles
        ; Properties = 
            Map.ofList ([ "x", x; "y", y; "shape", shapeToPropertyValue shape; "borderColor", PlotPropertyValue.StringValue borderColor ] @ color @ size) }

    /// Displays data as a collection of points, each having the value of one variable determining the position on the horizontal axis and the value of the other variable determining the position on the vertical axis. 
    static member markers (seriesX : float[], seriesY : float[], ?color : MarkersColor, 
                           ?colorPalette : string, ?size : MarkersSize, ?sizeRange : MarkersSizeRange, 
                           ?shape : MarkersShape, ?borderColor : string, ?displayName: string, ?titles: MarkersTitles) : PlotInfo = 
        let colorPalette = defaultArg colorPalette defaultColorPalette
        let size = defaultArg size (MarkersSize.Value defaultSize)
        let sizeRange = defaultArg sizeRange defaultSizeRange
        let shape = defaultArg shape defaultShape
        let borderColor = defaultArg borderColor defaultBorderColor
        let name = defaultArg displayName "markers"
        let titles = defaultArg titles defaultMarkersTitles
        match color with
        | Some(color) -> 
            Plot.markers (MarkersX.Values seriesX, MarkersY.Values seriesY, displayName = name, color = color, colorPalette = colorPalette, 
                 size = size, sizeRange = sizeRange, shape = shape, borderColor = borderColor, titles = titles)
        | None -> 
            Plot.markers (MarkersX.Values seriesX, MarkersY.Values seriesY, displayName = name, colorPalette = colorPalette, size = size, 
                 sizeRange = sizeRange, shape = shape, borderColor = borderColor, titles = titles)
    
    static member line (x : LineX, y : LineY, ?stroke : string, ?thickness : float, 
                        ?treatAs : LineTreatAs, ?fill68 : string, ?fill95 : string, ?displayName: string, ?titles: LineTitles) : PlotInfo = 
        let stroke = defaultArg stroke defaultStroke
        let thickness = defaultArg thickness defaultThickness
        let fill68 = defaultArg fill68 defaultFill68
        let fill95 = defaultArg fill95 defaultFill95
        let treatAs = defaultArg treatAs LineTreatAs.Function
        let x = 
            match x with
            | LineX.Values array -> [ "x", PlotPropertyValue.RealArray array ]
            | LineX.Indices -> []

        let y =
            match y with
            | LineY.Values array -> ["y", PlotPropertyValue.RealArray array ]
            | LineY.UncertainValues array -> [ "y", quantilesToPropertyValue array ]
        let name = defaultArg displayName "line"
        let props = 
            Map.ofList (
                x @ y @
                [ "stroke", PlotPropertyValue.StringValue stroke
                ; "thickness", PlotPropertyValue.RealValue thickness
                ; "fill_68", PlotPropertyValue.StringValue fill68
                ; "fill_95", PlotPropertyValue.StringValue fill95
                ; "treatAs", PlotPropertyValue.StringValue (match treatAs with LineTreatAs.Function -> "0" | LineTreatAs.Trajectory -> "1") ])
        let titles =  match titles with
                      | Some value -> 
                            let t = match value.x with
                                    | Some x -> Map.empty<string, string>.Add("x", x)
                                    | None -> Map.empty<string, string>
                            let t = match value.y with
                                    | Some y -> t.Add("y", y)
                                    | None -> t
                            t
                      | None -> Map.empty
        { DisplayName = name; Kind = lineType ; Titles = titles; Properties = props }
 
    static member line (seriesX : float[], seriesY : float[], ?stroke : string, ?thickness : float, 
                        ?treatAs : LineTreatAs, ?fill68 : string, ?fill95 : string, ?displayName: string, ?titles: LineTitles) : PlotInfo = 
        let stroke = defaultArg stroke defaultStroke
        let thickness = defaultArg thickness defaultThickness
        let fill68 = defaultArg fill68 defaultFill68
        let fill95 = defaultArg fill95 defaultFill95
        let treatAs = defaultArg treatAs LineTreatAs.Function
        let displayName = defaultArg displayName "line"
        let titles = defaultArg titles defaultLineTitles
        Plot.line(LineX.Values seriesX, LineY.Values seriesY, displayName = displayName, stroke = stroke, thickness = thickness, treatAs = treatAs, fill68 = fill68, fill95 = fill95, titles = titles)
        
    static member line (seriesY : float[], ?stroke : string, ?thickness : float, 
                        ?treatAs : LineTreatAs, ?fill68 : string, ?fill95 : string, ?displayName: string, ?titles: LineTitles) : PlotInfo = 
        let stroke = defaultArg stroke defaultStroke
        let thickness = defaultArg thickness defaultThickness
        let fill68 = defaultArg fill68 defaultFill68
        let fill95 = defaultArg fill95 defaultFill95
        let treatAs = defaultArg treatAs LineTreatAs.Function
        let displayName = defaultArg displayName "line"
        let titles = defaultArg titles defaultLineTitles
        Plot.line(LineX.Indices, LineY.Values seriesY, displayName = displayName, stroke = stroke, thickness = thickness, treatAs = treatAs, fill68 = fill68, fill95 = fill95, titles = titles)

    static member band(seriesX: float[], seriesY1: float[], seriesY2: float[], ?fill: string, ?displayName: string, ?titles: BandTitles) : PlotInfo =
        let name = defaultArg displayName "band"
        let fill = defaultArg fill defaultFill68
        let props = 
            [ "x", PlotPropertyValue.RealArray seriesX
            ; "y1", PlotPropertyValue.RealArray seriesY1
            ; "y2", PlotPropertyValue.RealArray seriesY2
            ; "fill", PlotPropertyValue.StringValue fill ] |> Map.ofList
        let titles =  match titles with
                      | Some value -> 
                            let t = match value.x with
                                    | Some x -> Map.empty<string, string>.Add("x", x)
                                    | None -> Map.empty<string, string>
                            let t = match value.y1 with
                                    | Some y1 -> t.Add("y1", y1)
                                    | None -> t
                            let t = match value.y2 with
                                    | Some y2 -> t.Add("y2", y2)
                                    | None -> t
                            t
                      | None -> Map.empty
        { DisplayName = name; Kind = bandType; Titles = titles; Properties = props }



    static member heatmap (x: float[], y: float[], values: HeatmapValues, ?colorPalette: string, ?treatAs: HeatmapTreatAs, ?displayName: string, ?titles: HeatmapTitles ) : PlotInfo =
        let colorPalette = defaultArg colorPalette defaultColorPalette
        let treat = defaultArg treatAs HeatmapTreatAs.Gradient
        let treatDataAs = 
            match treat with
            | Gradient -> "gradient"
            | Discrete -> "discrete"
        let x = [ "x", PlotPropertyValue.RealArray x ]
        let y = [ "y", PlotPropertyValue.RealArray y ]
        let values =
            match values with
            | HeatmapValues.TabularValues array -> [ "values", PlotPropertyValue.RealArray array ]
            | HeatmapValues.TabularUncertainValues array -> [ "values", quantilesToPropertyValue array ]
        let name = defaultArg displayName "heatmap"
        let titles =  match titles with
                      | Some value -> 
                            let t = match value.x with
                                    | Some x -> Map.empty<string, string>.Add("x", x)
                                    | None -> Map.empty<string, string>
                            let t = match value.y with
                                    | Some y -> t.Add("y", y)
                                    | None -> t
                            let t = match value.value with
                                    | Some value -> t.Add("value", value)
                                    | None -> t
                            t
                      | None -> Map.empty
        let props = Map.ofList (x @ y @ values @ [ "colorPalette", StringValue colorPalette ; "treatAs", StringValue treatDataAs ])
        { DisplayName = name; Kind = heatmapType; Titles = titles; Properties = props}

    static member heatmap (x: float[], y: float[], values: float[], ?colorPalette: string, ?treatAs: HeatmapTreatAs, ?displayName: string, ?titles: HeatmapTitles ) : PlotInfo =
        let colorPalette = defaultArg colorPalette defaultColorPalette
        let treat = defaultArg treatAs HeatmapTreatAs.Gradient   
        let name = defaultArg displayName "heatmap"    
        let titles  = defaultArg titles defaultHeatmapTitles
        Plot.heatmap(x, y, HeatmapValues.TabularValues values, colorPalette, treat, name, titles)