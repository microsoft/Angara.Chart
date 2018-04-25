using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.FSharp.Collections;
using c = Angara.Charting;
using Microsoft.FSharp.Core;

namespace Angara.Charting.CSharp
{
    internal static class Helpers
    {
        internal static FSharpOption<T> Opt<T>(T value) where T : class => value == null ? FSharpOption<T>.None : FSharpOption<T>.Some(value);
        internal static FSharpOption<T> Opt<T>(T? value) where T : struct => value.HasValue ? FSharpOption<T>.Some(value.Value) : FSharpOption<T>.None;
    }

    public static class ChartFactory
    {
        public static c.Chart FromPlots(IEnumerable<PlotInfo> plots, ChartLayout layout = ChartLayout.Chubby, Axis xAxis = null, Axis yAxis = null)
        {
            var chart = c.Chart.ofList(ListModule.OfSeq(plots));
            if (xAxis != null)
                chart = Chart.setXAxis(xAxis, chart);
            if (yAxis != null)
                chart = Chart.setYAxis(yAxis, chart);
            return Chart.setLayout(layout, chart);
        }
    }

    public static class TitlesFactory
    {
        ///Allows to set titles for heatmap chart axes
        public static HeatmapTitles HeatMap(string xTitle = null, string yTitle = null, string valueTitle = null) =>
           Titles.heatmap(Helpers.Opt(xTitle), Helpers.Opt(yTitle), Helpers.Opt(valueTitle));

        ///Allows to set titles for line chart axes
        public static LineTitles Line(string xTitle = null, string yTitle = null) =>
           Titles.line(Helpers.Opt(xTitle), Helpers.Opt(yTitle));

        ///Allows to set titles for band chart axes
        public static AreaTitles Area(string xTitle = null, string y1Title = null, string y2Title = null) =>
           Titles.area(Helpers.Opt(xTitle), Helpers.Opt(y1Title), Helpers.Opt(y2Title));

        //Allows to set titles for marker chart axes and legend
        public static MarkersTitles Markers(string xTitle = null, string yTitle = null, string color = null, string size = null) =>
           Titles.markers(Helpers.Opt(xTitle), Helpers.Opt(yTitle), Helpers.Opt(color), Helpers.Opt(size));
    }

    public static class PlotFactory
    {
        /// Displays data as a collection of points, each having the value of one variable determining the position on the horizontal axis and the value of the other variable determining the position on the vertical axis. 
        public static PlotInfo Markers(double[] seriesX, double[] seriesY, MarkersColor color = null,
            string colorPalette = null, MarkersSize size = null, MarkersSizePalette sizePalette = null,
            MarkersShape shape = null, string borderColor = null, string displayName = null, MarkersTitles titles = null) =>
            Plot.markers(seriesX, seriesY, Helpers.Opt(color), Helpers.Opt(colorPalette), Helpers.Opt(size), Helpers.Opt(sizePalette), Helpers.Opt(shape), Helpers.Opt(borderColor), Helpers.Opt(displayName), Helpers.Opt(titles));

        /// Displays data as a collection of points, each having the value of one variable determining the position on the horizontal axis and the value of the other variable determining the position on the vertical axis. QuantileArray for seriesY produces a box-and-whiskers plot.
        public static PlotInfo Markers(double[] seriesX, QuantileArray seriesY, MarkersColor color = null,
            string colorPalette = null, MarkersSize size = null, MarkersSizePalette sizePalette = null,
            MarkersShape shape = null, string borderColor = null, string displayName = null, MarkersTitles titles = null) =>
            Plot.markers(MarkersX.NewValues(seriesX), MarkersY.NewUncertainValues(seriesY), Helpers.Opt(color), Helpers.Opt(colorPalette), Helpers.Opt(size), Helpers.Opt(sizePalette), Helpers.Opt(shape), Helpers.Opt(borderColor), Helpers.Opt(displayName), Helpers.Opt(titles));
        
        public static PlotInfo Line(double[] seriesX, double[] seriesY, string stroke = null, double? thickness = null, LineTreatAs treatAs = null,
            string fill68 = null, string fill95 = null, string displayName = null, LineTitles titles = null) =>
            Plot.line(seriesX, seriesY, Helpers.Opt(stroke), Helpers.Opt(thickness), Helpers.Opt(treatAs), Helpers.Opt(fill68), Helpers.Opt(fill95), Helpers.Opt(displayName), Helpers.Opt(titles));

        public static PlotInfo Line(LineX x, LineY y, string stroke = null, double? thickness = null, LineTreatAs treatAs = null,
            string fill68 = null, string fill95 = null, string displayName = null, LineTitles titles = null) =>
            Plot.line(x, y, Helpers.Opt(stroke), Helpers.Opt(thickness), Helpers.Opt(treatAs), Helpers.Opt(fill68), Helpers.Opt(fill95), Helpers.Opt(displayName), Helpers.Opt(titles));
        
        public static PlotInfo Area(double[] seriesX, double[] seriesY1, double[] seriesY2, string fill = null, string displayName = null, AreaTitles titles = null) =>
            Plot.area(seriesX, seriesY1, seriesY2, Helpers.Opt(fill), Helpers.Opt(displayName), Helpers.Opt(titles));
        
        public static PlotInfo Heatmap(double[] x, double[] y, HeatmapValues values, string colorPalette = null, HeatmapTreatAs treatAs = null, string displayName = null, HeatmapTitles titles = null) =>
            Plot.heatmap(x, y, values, Helpers.Opt(colorPalette), Helpers.Opt(treatAs), Helpers.Opt(displayName), Helpers.Opt(titles));
    }
}
