define(["jquery", "idd.umd", "exports"], function ($, Charting, exports) {    
    exports.Show = function (chartInfo, container /*HTMLElement*/) {
        var idd = Charting.InteractiveDataDisplay;

        var plotMap = [];
        for (var i = 0; i < chartInfo.plots.length; i++) {
            var pi = chartInfo.plots[i];
            var props = $.extend(true, {}, pi.properties);
            props["kind"] = pi.kind;
            props["displayName"] = pi.displayName;
            props["titles"] = pi.titles;
            plotMap[i] = props;
        }

        if(chartInfo["layout"] && chartInfo["layout"] === "Lean")
        {
            $(container).css("overflow", "hidden");
            var navigationPanel = $("<div></div>").css("float", "right").width(65);
            var chartElement = $("<div data-idd-plot='chart'></div>").css("float", "none").width("auto").css("overflow", "hidden");
            $(container).append(navigationPanel); // the order of adding is important!
            $(container).append(chartElement);
            chartElement.height($(container).height());

            var chart = idd.asPlot(chartElement);
            var panel = new idd.NavigationPanel(chart, navigationPanel);  

            var viewState = new idd.PersistentViewState();            
            for (var i = 0; i < plotMap.length; i++) {
                var p = plotMap[i];
                var factory = idd.PlotRegistry[p.kind] ? idd.PlotRegistry[p.kind] : idd.PlotRegistry["fallback"];
                factory.draw(factory.initialize(p, viewState, chart), p);
            }
            
            var titlesX = getTitles("x", plotMap), titlesY = getTitles("y", plotMap);            
            if (titlesX != "") {
                var xAxisTitle = $(chart.addDiv('<div style="font-size: larger; text-align: center"></div>', "bottom"))
                xAxisTitle.text(titlesX)
            }
            if (titlesY != "") {                
                var yAxisTitle =
                        $(chart.addDiv('<div class="idd-verticalTitle" style="font-size: larger;"></div>', "left"));
                yAxisTitle.text(titlesY);
            }
        }
        else
        {
            Charting.InteractiveDataDisplay.show(container, plotMap);
        }
    };

    function getTitles(axis, plots){
        var title = "";
        var items = [];
        for (var i = 0; i < plots.length; i++) {
            var p = plots[i];
            if(p["titles"] && p["titles"][axis]){
                var x = p["titles"][axis];
                if(items.indexOf(x) < 0){
                    items.push(x);
                    if(items.length > 1) title += ", ";
                    title += x;
                }
            }
        }
        return title;
    }
});