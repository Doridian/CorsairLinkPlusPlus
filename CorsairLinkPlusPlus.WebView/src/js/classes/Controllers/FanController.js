var Controller = require("classes/Controller");

function FanController(rawData) {
	Controller.apply(this, arguments);
}
inherit(FanController, Controller);

return FanController;