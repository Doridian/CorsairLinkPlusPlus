var FanCurveController = require("classes/Controllers/FanCurveController");

function FanBalancedModeController(rawData) {
	FanCurveController.apply(this, arguments);
}
inherit(FanBalancedModeController, FanCurveController);

return FanBalancedModeController;