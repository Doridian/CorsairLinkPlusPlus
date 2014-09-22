function ControllerFactory() {
}

var controllerMap = {};

[
	"FanDefaultController",
	"FanBalancedModeController",
	"FanQuiteModeController",
	"FanPerformanceModeController",
	"FanCustomCurveController",
	"FanFixedRPMController",
	"FanFixedPercentController",

	"LEDSingleColorController",
	"LEDTwoColorController",
	"LEDFourColorController",
	"LEDTemperatureController"
].forEach(function(val) {
	controllerMap["CorsairLink." + val] = require("classes/Controllers/" + val);
});

ControllerFactory.create = function(rawData) {
	console.log(rawData);
	if(!controllerMap[rawData.Name]) {
		console.warn("Unhandled controller type " + rawData.Name);
		return new Controller();
	}
	return new controllerMap[rawData.Name](rawData);
}

return ControllerFactory