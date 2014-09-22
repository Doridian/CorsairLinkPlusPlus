var Device = require("classes/Device");

function Sensor(rawDevice) {
	Device.apply(this, arguments);
	this.type = rawDevice.SensorType;
	this.unit = rawDevice.Unit;
	this.value = rawDevice.Value;
}
inherit(Sensor, Device);

Sensor.prototype.getValue = function() {
	return this.value;
}

Sensor.prototype.getUnit = function() {
	return this.unit;
}

Sensor.prototype.setController = function(controller) {
	this.controller = controller;
}

return Sensor;
