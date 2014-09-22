var Controller = require("classes/Controller");
var Sensor = require("classes/Devices/Sensor");

var Cooler = require("classes/Devices/Cooler");
var Hub = require("classes/Devices/Hub");
var PSU = require("classes/Devices/PSU");
var RootDevice = require("classes/Devices/RootDevice");
var VirtualHub = require("classes/Devices/VirtualHub");

var ControllerFactory = require("classes/ControllerFactory");
var SensorFactory = require("classes/SensorFactory");

function DeviceTree(rawTree) {
	if (rawTree)
		this.buildDevices(rawTree, null);
}

DeviceTree.prototype.buildSensor = function(rawData) {
	return SensorFactory.create(rawData);
}

DeviceTree.prototype.buildController = function(rawData) {
	return ControllerFactory.create(rawData);
}

DeviceTree.prototype.buildDevice = function (rawDevice) {
	switch(rawDevice.Type) {
		case "Root":
			return new RootDevice(rawDevice);
		case "Hub":
			return new Hub(rawDevice);
		case "VirtualHub":
			return new VirtualHub(rawDevice);
		case "PSU":
			return new PSU(rawDevice);
		case "Sensor":
			var sensor = this.buildSensor(rawDevice);
			if("Controller" in rawDevice)
				sensor.setController(this.buildController(rawDevice.Controller));
			return sensor;
		case "Cooler":
			return new Cooler(rawDevice);
		default:
			console.warn("Unhandled device type " + rawDevice.Type);
			break;
	}
}

DeviceTree.prototype.buildDevices = function(rawTree, parent) {
	var newDevice = this.buildDevice(rawTree);
	if(parent) {
		newDevice.setParent(parent);
		parent.addChild(newDevice);
	}
	
	if(!this.root)
		this.root = newDevice;
	
	rawTree.Children.forEach(function(val) {
		this.buildDevices(val, newDevice);
	}, this);
	
}

function flattenTree(treeNode, out) {
	out = out || [];
	out.push(treeNode);
	if(treeNode instanceof Hub)
		treeNode.getChildren().forEach(function(subNode) {
			flattenTree(subNode, out);
		});
	return out;
}	

DeviceTree.prototype.getDevices = function() {
	return flattenTree(this.root);
}

return DeviceTree;