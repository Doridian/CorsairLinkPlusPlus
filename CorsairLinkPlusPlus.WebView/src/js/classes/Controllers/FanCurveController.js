var FanController = require("classes/Controllers/FanController");
var ControlCurve = require("classes/ControlCurve");

function FanCurveController(rawData) {
	FanController.apply(this, arguments);
	var points = rawData.Value.Points;
	var curve = new ControlCurve();
	points.forEach(function(point) {
		curve.add(point);
	});
	this.curve = curve;
}
inherit(FanCurveController, FanController);

FanCurveController.prototype.getCurve = function() {
	return this.curve;
}

return FanCurveController;