var Sensor = require("classes/Devices/Sensor");

function PowerSensor(rawDevice) {
	Sensor.apply(this, arguments);
}
inherit(PowerSensor, Sensor);

return PowerSensor;