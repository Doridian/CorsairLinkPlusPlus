var LEDController = require("classes/Controllers/LEDController");

function LEDTemperatureController(rawData) {
	LEDController.apply(this, arguments);
}
inherit(LEDTemperatureController, LEDController);

return LEDTemperatureController;