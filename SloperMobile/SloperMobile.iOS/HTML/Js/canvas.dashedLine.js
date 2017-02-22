//Canvas Dashed Line Function kindly provided by phrogz at:
//http://stackoverflow.com/questions/4576724/dotted-stroke-in-canvas
var CP = window.CanvasRenderingContext2D && CanvasRenderingContext2D.prototype;
if (CP && CP.lineTo) {
    CP.dashedLine = function (xIn, yIn, x2In, y2In, dashArray) {

        var x = parseInt(xIn);
        var y = parseInt(yIn);
        var x2 = parseInt(x2In);
        var y2 = parseInt(y2In);

        if (x2 - x == 0) { x2 += 1; } // fast fix for vertical lines.
        //TODO: need to split the drawing code at dx==0 and rewrite for verts, everything else works fine as-is.

        if (!dashArray) dashArray = [10, 5];
        var dashCount = dashArray.length;
        this.moveTo(x, y);
        var dx = (x2 - x), dy = (y2 - y);

        var slope = dy / dx;
        var distRemaining = Math.sqrt(dx * dx + dy * dy);
        var dashIndex = 0, draw = true;
        while (distRemaining >= 0.1) {
            var dashLength = dashArray[dashIndex++ % dashCount];
            if (dashLength > distRemaining) dashLength = distRemaining;
            var xStep = Math.sqrt(dashLength * dashLength / (1 + slope * slope));

            var signal = (x2 > x ? 1 : -1);

            x += xStep * signal;
            y += slope * xStep * signal;
            this[draw ? 'lineTo' : 'moveTo'](x, y);
            distRemaining -= dashLength;
            draw = !draw;
        }
    }

}
