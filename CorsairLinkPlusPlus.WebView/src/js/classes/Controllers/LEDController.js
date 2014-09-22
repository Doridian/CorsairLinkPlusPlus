var Controller = require("classes/Controller");

function LEDController(rawData) {
	Controller.apply(this, arguments);
}
inherit(LEDController, Controller);

return LEDController;