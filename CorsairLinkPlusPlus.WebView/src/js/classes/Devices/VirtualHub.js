var Hub = require("classes/Devices/Hub");

function VirtualHub(rawDevice) {
	Hub.apply(this, arguments);
}
inherit(VirtualHub, Hub);

return VirtualHub;