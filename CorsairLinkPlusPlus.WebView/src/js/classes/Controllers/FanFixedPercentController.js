var FanController = require("classes/Controllers/FanController");

function FanFixedPercentController(rawData) {
	FanController.apply(this, arguments);
}
inherit(FanFixedPercentController, FanController);

return FanFixedPercentController;