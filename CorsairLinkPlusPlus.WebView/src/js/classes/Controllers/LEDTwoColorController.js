var LEDController = require("classes/Controllers/LEDController");

function LEDTwoColorController(rawData) {
	LEDController.apply(this, arguments);
}
inherit(LEDTwoColorController, LEDController);

return LEDTwoColorController;