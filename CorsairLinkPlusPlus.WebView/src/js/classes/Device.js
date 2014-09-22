function Device(rawDevice) {
	this.name = rawDevice.Name;
	this.path = rawDevice.AbsolutePath;
}

Device.prototype.getPath = function() {
	return this.path;
}

Device.prototype.setParent = function(parent) {
	this.parent = parent;
}

return Device;