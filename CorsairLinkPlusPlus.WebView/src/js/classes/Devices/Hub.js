var Device = require("classes/Device");

var util = require("libraries/util")

function Hub(rawDevice) {
	Device.apply(this, arguments);
	this.children = [];
}
inherit(Hub, Device);

Hub.prototype.getChildren = function() {
	return util.arrayCopy(this.children);
}

Hub.prototype.addChild = function(device) {
	this.children.push(device);
}

Hub.prototype.getName = function() {
	return this.name;
}

return Hub;