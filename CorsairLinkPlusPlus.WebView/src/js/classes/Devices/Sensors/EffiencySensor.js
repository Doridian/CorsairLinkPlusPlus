var Sensor = require("classes/Devices/Sensor");

function EffiencySensor(rawDevice) {
	Sensor.apply(this, arguments);
}
inherit(EffiencySensor, Sensor);

return EffiencySensor;