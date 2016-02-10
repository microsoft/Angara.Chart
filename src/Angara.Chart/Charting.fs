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
    ; Properties: PlotProperties }


/// Defines a chart as a collection plot definitions.
type Chart =
    { Plots: PlotInfo list }
    static member ofList plots : Chart = { Plots = plots }