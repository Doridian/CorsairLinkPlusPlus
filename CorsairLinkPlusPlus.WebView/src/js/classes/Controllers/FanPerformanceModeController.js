var FanCurveController = require("classes/Controllers/FanCurveController");

function FanPerformanceModeController(rawData) {
	FanCurveController.apply(this, arguments);
}
inherit(FanPerformanceModeController, FanCurveController);

return FanPerformanceModeController;