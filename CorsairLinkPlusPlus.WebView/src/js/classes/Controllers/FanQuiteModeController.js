var FanCurveController = require("classes/Controllers/FanCurveController");

function FanQuiteModeController(rawData) {
	FanCurveController.apply(this, arguments);
}
inherit(FanQuiteModeController, FanCurveController);

return FanQuiteModeController;