var FanController = require("classes/Controllers/FanController");

function FanDefaultController(rawData) {
	FanController.apply(this, arguments);
}
inherit(FanDefaultController, FanController);

return FanDefaultController;