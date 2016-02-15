define(["jquery", "chartViewer.umd", "exports"], function ($, Charting, exports) {    
    exports.Show = function (plots, container) {
        var plotMap = {};
        for (var i = 0; i < plots.length; i++) {
            var pi = plots[i];
            var props = $.extend(true, {}, pi.properties);
            props["kind"] = pi.kind;
            props["displayName"] = pi.displayName;
            props["titles"] = pi.titles;
            plotMap[i] = props;
        }
        Charting.ChartViewer.show(container, plotMap);
    };
});