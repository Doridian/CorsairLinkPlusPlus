var FanController = require("classes/Controllers/FanController");

function FanFixedRPMController(rawData) {
	FanController.apply(this, arguments);
}
inherit(FanFixedRPMController, FanController);

return FanFixedRPMController;