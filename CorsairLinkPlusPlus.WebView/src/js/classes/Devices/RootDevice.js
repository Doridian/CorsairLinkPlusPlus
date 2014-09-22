var Hub = require("classes/Devices/Hub");

function RootDevice(rawDevice) {
	Hub.apply(this, arguments);
}

inherit(RootDevice, Hub);

return RootDevice;