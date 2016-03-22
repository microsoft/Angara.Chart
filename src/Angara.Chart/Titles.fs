module Angara.Titles

/// Defines names of heatmap axes
type HeatmapTitles = {x: string; y: string}

/// Defines names of marker chart axes
type MarkersTitles = {x: string; y: string; color: string; size: string }

/// Defines names of line chart axes
type LineTitles = {x: string; y: string }

/// Defines names of band chart axes
type BandTitles = { x: string; y1: string; y2: string } 

type Titles () =
    ///Allows to set titles for heatmap chart axes
    static member heatmap (?x_title: string, ?y_title: string) : HeatmapTitles =
        let x = match x_title with
                    | Some value -> value
                    | None -> "x"
        let y = match y_title with
                    | Some value -> value
                    | None -> "y"
        {HeatmapTitles.x = x; HeatmapTitles.y = y}

    ///Allows to set titles for line chart axes
    static member line (?x_title: string, ?y_title: string) : LineTitles = 
        let x = match x_title with
                    | Some value -> value
                    | None -> "x"
        let y = match y_title with
                    | Some value -> value
                    | None -> "y"
        {LineTitles.x = x; LineTitles.y = y}

    ///Allows to set titles for band chart axes
    static member band (?x_title: string, ?y1_title: string, ?y2_title: string) : BandTitles =
        let x = match x_title with
                    | Some value -> value
                    | None -> "x"
        let y1 = match y1_title with
                    | Some value -> value
                    | None -> "y1"
        let y2 = match y2_title with
                    | Some value -> value
                    | None -> "y2"
        {BandTitles.x = x; BandTitles.y1 = y1; BandTitles.y2 = y2}
    
    //Allows to set titles for marker chart axes and legend
    static member markers (?x_title: string, ?y_title: string, ?color: string, ?size: string) : MarkersTitles =
        let x = match x_title with
                    | Some value -> value
                    | None -> "x"
        let y = match y_title with
                    | Some value -> value
                    | None -> "y"
        let color = match color with
                    | Some value -> value
                    | None -> "color"
        let size = match size with
                    | Some value -> value
                    | None -> "size"
        {MarkersTitles.x = x; MarkersTitles.y = y; MarkersTitles.color = color; MarkersTitles.size = size}
