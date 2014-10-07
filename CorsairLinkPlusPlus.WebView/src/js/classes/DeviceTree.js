/**
 * CorsairLinkPlusPlus
 * Copyright (c) 2014, Mark Dietzer & Simon Schick, All rights reserved.
 *
 * CorsairLinkPlusPlus is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 3.0 of the License, or (at your option) any later version.
 *
 * CorsairLinkPlusPlus is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public
 * License along with CorsairLinkPlusPlus.
 */

"use strict";

var Controller = require("Controller");
var Sensor = require("Devices/Sensor");

var Cooler = require("Devices/Cooler");
var Hub = require("Devices/Hub");
var PSU = require("Devices/PSU");
var RootDevice = require("Devices/RootDevice");
var VirtualHub = require("Devices/VirtualHub");

var ControllerFactory = require("ControllerFactory");
var SensorFactory = require("SensorFactory");

function DeviceTree(rawTree) {
	if (rawTree)
		this.buildDevices(rawTree, null);
}

var p = DeviceTree.prototype;

p.buildSensor = function(rawData) {
	return SensorFactory.create(rawData);
};

p.buildController = function(rawData) {
	return ControllerFactory.getInstance().create(rawData);
};

p.buildDevice = function (rawData) {
	switch(rawData.Type) {
		case "Root":
			return new RootDevice(rawData);
		case "Hub":
			return new Hub(rawData);
		case "VirtualHub":
			return new VirtualHub(rawData);
		case "PSU":
			return new PSU(rawData);
		case "Sensor":
			var sensor = this.buildSensor(rawData);
			if("Controller" in rawData) {
				var controller = this.buildController(rawData.Controller);
				sensor.setController(controller);
				controller.addDevice(sensor);
			}
			return sensor;
		case "Cooler":
			return new Cooler(rawData);
		default:
			console.warn("Unhandled device type " + rawData.Type);
			break;
	}
};

p.buildDevices = function(rawTree, parent) {
	var newDevice = this.buildDevice(rawTree);
	if(parent) {
		newDevice.setParent(parent);
		parent.addChild(newDevice);
	}
	
	if(!this.root)
		this.root = newDevice;
	for(var subTree of rawTree.Children)
		this.buildDevices(subTree, newDevice);
};

function traverseTree(treeNode, callback) {
	callback(treeNode);
	if(treeNode instanceof Hub)
		for(var subNode of treeNode.getChildren())
			traverseTree(subNode, callback);
};

function flattenTree(treeNode) {
	var out = [];
	traverseTree(treeNode, function(device) {
		out.push(device);
	});
	return out;
};

p.getDevices = function() {
	return flattenTree(this.root);
};

p.getRoot = function() {
	return this.root;
};

p.forEach = function(callback) {
	traverseTree(this.root, callback);
};

p.getDevicesByName = function(name) {
	var out = [];
	this.forEach(function(device) {
		if(device.getName() == name)
			out.push(device);
	});
	return out;
};

return DeviceTree;