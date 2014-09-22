var Hub = require("classes/Devices/Hub");

function PSU(rawDevice) {
	Hub.apply(this, arguments);
}
inherit(PSU, Hub);

return PSU;