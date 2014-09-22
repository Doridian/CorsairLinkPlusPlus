var Sensor = require("classes/Devices/Sensor");

function LED(rawDevice) {
	Sensor.apply(this, arguments);
}
inherit(LED, Sensor);

return LED;