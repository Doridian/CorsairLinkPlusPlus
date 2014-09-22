var LEDController = require("classes/Controllers/LEDController");

function LEDSingleColorController(rawData) {
	LEDController.apply(this, arguments);
}
inherit(LEDSingleColorController, LEDController);

return LEDSingleColorController;