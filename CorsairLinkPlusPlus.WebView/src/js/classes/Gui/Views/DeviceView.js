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

var util = require("Util");

var Sensor = require("Devices/Sensor");
var Hub = require("Devices/Hub");

var api = require("Api").getInstance();

function DeviceView(device) {
	this.device = device;
	var self = this;
	device.addListener(function() {
		self.update();
	});
}

var p = DeviceView.prototype;

p.getDevice = function() {
	return this.device;
};

p.getElement = function() {
	if(!this.element) {
		this.buildElement();
		this.update();
		return this.element;
	}
	return this.element;
};

p.buildNameText = function() {
	return {
		tag: "span",
		children: [
			"Name: ",
			{
				tag: "span",
				attributes: {
					className: "name"
				},
				id: "name",
				children: [
					""
				]
			}
		]
	}
};

p.buildUpdateButton = function() {
	return {
		tag: "input",
		attributes: {
			type: "button",
			value: "update"
		},
		events: {
			click: function(event) {
				api.updateDevice(self.device, true).then(function() {
					console.log("success");
				});
			}
		}
	}
};

p.injectClassName = function() {
	return "";
};

p.postBuildElement = function() {
};

p.buildElement = function() {
	var isHub = this.device instanceof Hub;
	var isSensor
	var treeData = util.makeElementTree({
		tag: "div",
		attributes: {
			className: "device" + (isHub ? " hub " : " ") + this.injectClassName()
		}, 
		children: this.buildInner()
	});
	this.element = treeData.node;
	this.dataFields = treeData.idMap;
	this.postBuildElement();
};

p.buildInner = function() {
	return [
		this.buildNameText(),
		util.makeElement("br"),
		this.buildUpdateButton()
	]
};

p.setDataFieldText = function(field, data) {
	if(!this.dataFields[field])
		throw new Error("View has no data field " + field);
	this.dataFields[field].firstChild.textContent = data;
};

p.update = function() {
	this.setDataFieldText("name", this.device.getName());
};

return DeviceView;