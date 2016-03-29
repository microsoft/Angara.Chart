namespace Angara.Charting

type PlotProperties = Map<string, PlotPropertyValue>
and PlotPropertyValue = 
    | StringValue   of string
    | RealValue     of float
    | RealArray     of float[]
    | Composite     of PlotProperties
    static member OfPairs(props : (string * PlotPropertyValue) list) = Composite(Map.ofList props)

/// Defines a plot as a collection of properties.
type PlotInfo = 
    { Kind : string 
    ; DisplayName : string
    ; Titles : Map<string, string>
    ; Properties: PlotProperties }


/// Defines a chart as a collection plot definitions.
type Chart =
    { Plots: PlotInfo list }
    static member ofList plots : Chart = { Plots = plots }

/// Defines names of heatmap axes
type HeatmapTitles = {x: string option; y: string option; value: string option}

/// Defines names of marker chart axes
type MarkersTitles = {x: string option; y: string option; color: string option; size: string option}

/// Defines names of line chart axes
type LineTitles = {x: string option; y: string option }

/// Defines names of band chart axes
type BandTitles = { x: string option; y1: string option; y2: string option} 

type Titles () =
    ///Allows to set titles for heatmap chart axes
    static member heatmap (?x_title: string, ?y_title: string, ?value_title: string) : HeatmapTitles = {HeatmapTitles.x = x_title; HeatmapTitles.y = y_title; HeatmapTitles.value = value_title}
    ///Allows to set titles for line chart axes
    static member line (?x_title: string, ?y_title: string) : LineTitles = {LineTitles.x = x_title; LineTitles.y = y_title}
    ///Allows to set titles for band chart axes
    static member band (?x_title: string, ?y1_title: string, ?y2_title: string) : BandTitles = {BandTitles.x = x_title; BandTitles.y1 = y1_title; BandTitles.y2 = y2_title}
    //Allows to set titles for marker chart axes and legend
    static member markers (?x_title: string, ?y_title: string, ?color: string, ?size: string) : MarkersTitles = {MarkersTitles.x = x_title; MarkersTitles.y = y_title; MarkersTitles.color = color; MarkersTitles.size = size}
