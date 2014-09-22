var Hub = require("classes/Devices/Hub");

function Cooler(rawDevice) {
	Hub.apply(this, arguments);
}
inherit(Cooler, Hub);

return Cooler;