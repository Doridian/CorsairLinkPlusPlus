var util = require("libraries/util");

function ControlCurve() {
	this.points = [];
}

ControlCurve.prototype.add = function(point) {
	this.points.push(point);
}

ControlCurve.prototype.getPoints = function(point) {
	return util.arrayCopy(this.points);
}

return ControlCurve;