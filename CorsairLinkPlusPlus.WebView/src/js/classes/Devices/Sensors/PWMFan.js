var Fan = require("classes/Devices/Sensors/Fan");

function PWMFan(rawDevice) {
	Fan.apply(this, arguments);
}
inherit(PWMFan, Fan);

return PWMFan;