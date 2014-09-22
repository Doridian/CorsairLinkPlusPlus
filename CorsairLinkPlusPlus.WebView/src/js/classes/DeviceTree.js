/**
 * CorsairLinkPlusPlus
 * Copyright (c) 2014, Mark Dietzer & Simon Schick, All rights reserved.
 *
 * CorsairLinkPlusPlus is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 3.0 of the License, or at your option any later version.
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

var p = DeviceTree.prototype;

p.buildSensor = function(rawData) {
	return SensorFactory.create(rawData);
}

p.buildController = function(rawData) {
	return ControllerFactory.create(rawData);
}

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
			if("Controller" in rawData)
				sensor.setController(this.buildController(rawData.Controller));
			return sensor;
		case "Cooler":
			return new Cooler(rawData);
		default:
			console.warn("Unhandled device type " + rawData.Type);
			break;
	}
}

p.buildDevices = function(rawTree, parent) {
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

p.getDevices = function() {
	return flattenTree(this.root);
}

return DeviceTree;