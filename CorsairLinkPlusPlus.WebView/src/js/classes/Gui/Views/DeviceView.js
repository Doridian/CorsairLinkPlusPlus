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

var View = require("Gui/View");

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

var p = inherit(DeviceView, View);

p.getDevice = function() {
	return this.device;
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
				api.updateDevice(self.device, true);
			}
		}
	}
};

p.injectClassName = function() {
	return "";
};

p.buildElement = function() {
	var isHub = this.device instanceof Hub;
	return util.makeElementTree({
		tag: "div",
		attributes: {
			className: "device" + (isHub ? " hub " : " ") + this.injectClassName()
		}, 
		children: this.buildInner()
	});
};

p.buildInner = function() {
	return [
		this.buildNameText(),
		util.makeElement("br"),
		this.buildUpdateButton()
	]
};

p.update = function() {
	this.setDataFieldText("name", this.device.getName());
};

return DeviceView;