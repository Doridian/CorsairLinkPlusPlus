var LEDController = require("classes/Controllers/LEDController");

function LEDFourColorController(rawData) {
	LEDController.apply(this, arguments);
}
inherit(LEDFourColorController, LEDController);

return LEDFourColorController;