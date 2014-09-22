var Sensor = require("classes/Devices/Sensor");

function Thermistor(rawDevice) {
	Sensor.apply(this, arguments);
}
inherit(Thermistor, Sensor);

return Thermistor;