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

var DeviceViewFactory = require("Gui/DeviceViewFactory");
var factory = DeviceViewFactory.getInstance();

var util = require("Util");
var api = require("Api").getInstance();

function Plain(deviceTree) {
	this.deviceTree = deviceTree;
	this.setUpdateInterval(2000);
}

var p = Plain.prototype;

p.setUpdateInterval = function(interval) {
	var self = this;
	if(this.interval)
		clearInterval(this.interval);
	this.interval = setInterval(function() {
		api.updateDevice(self.deviceTree.getRoot(), true).then(function() {
		});
	}, interval);
}

p.getElement = function() {
	var baseElement = document.createElement("div");
	var self = this;
	
	baseElement.appendChild(util.makeSelect(undefined, [
		{
			value: 1000,
		},
		{
			value: 1500,
		},
		{
			value: 2000,
			selected: true
		}
	],{
		change: function(event) {
			self.setUpdateInterval(parseInt(this.value));
		}
	}));

	this.deviceTree.forEach(function(device) {
		var parentDevice = device.getParent();
		var parent;
		if(parentDevice)
			parent = factory.getByDevice(parentDevice).getElement();
		else
			parent = baseElement;
		
		parent.appendChild(
			factory.getByDevice(device).getElement()
		);
	});
	
	return baseElement;
}

return Plain;