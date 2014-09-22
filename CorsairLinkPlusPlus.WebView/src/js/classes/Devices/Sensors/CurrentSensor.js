var Sensor = require("classes/Devices/Sensor");

function CurrentSensor(rawDevice) {
	Sensor.apply(this, arguments);
}
inherit(CurrentSensor, Sensor);

return CurrentSensor;