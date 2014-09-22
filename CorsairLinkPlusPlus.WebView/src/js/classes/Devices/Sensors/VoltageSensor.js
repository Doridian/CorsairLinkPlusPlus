var Sensor = require("classes/Devices/Sensor");

function VoltageSensor(rawDevice) {
	Sensor.apply(this, arguments);
}
inherit(VoltageSensor, Sensor);

return VoltageSensor;