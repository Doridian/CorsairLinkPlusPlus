var FanCurveController = require("classes/Controllers/FanCurveController");

function FanCustomCurveController(rawData) {
	FanCurveController.apply(this, arguments);
}
inherit(FanCustomCurveController, FanCurveController);

return FanCustomCurveController;