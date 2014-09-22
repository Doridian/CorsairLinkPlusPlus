var Sensor = require("classes/Devices/Sensor");

function Fan(rawDevice) {
	Sensor.apply(this, arguments);
}
inherit(Fan, Sensor);

return Fan;