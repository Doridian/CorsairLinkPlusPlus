var Sensor = require("classes/Devices/Sensor");

function Pump(rawDevice) {
	Sensor.apply(this, arguments);
}
inherit(Pump, Sensor);

return Pump;