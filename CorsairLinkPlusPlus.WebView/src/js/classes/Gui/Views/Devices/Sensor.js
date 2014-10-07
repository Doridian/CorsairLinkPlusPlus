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

var DeviceView = require("Gui/Views/DeviceView");

var FixedPercentController = require("Controllers/Fan/CorsairLink/FixedPercent");
var Api = require("Api");

function Sensor(device) {
	DeviceView.apply(this, arguments);
}
var p = inherit(Sensor, DeviceView);

p.injectClassName = function() {
	return "sensor " + this.device.constructor.name.toLowerCase();
};

p.buildValueText = function() {
	return {
		tag: "span",
		children: [
			"Value: ",
			{
				tag: "span",
				attributes: {
					className: "value"
				},
				id: "value",
				children: [
					""
				]
			}
		]
	};
};

p.buildIndicator = function() {
	return {
		tag: "div", 
		attributes: {
			className: "indicator"
		}
	}
};

p.buildControllerSelector = function() {
	var names = this.device.getValidControllerNames();
	if(names.length == 0)
		return [];
	var children = [];

	var currentControllerName = this.device.getController().constructor.name;

	for(var name of names)
		children.push({
			tag: "option",
			attributes: {
				value: name,
				selected: util.endsWith(name, currentControllerName)
			},
			children: [
				name
			]
		});
	var self = this;
	return  {
		tag: "select",
		children: children,
		events: {
			change: function(event) {
				debugger;
				var currentController = self.device.getController();
				if(this.value == "Fan.CorsairLink.FixedPercent") {
					var controller = new FixedPercentController({Value: 0});
					self.device.setController(controller);
					Api.getInstance().sendControllerUpdate(self.device, controller);
				}
			}
		}
	};
}

p.buildInner = function() {
	var indicatorObject = this.buildIndicator();
	var attributes = indicatorObject.attributes;
	if(!attributes) {
		indicatorObject.attributes = {
			className: "indicator"
		};
	} else
		attributes.className = "indicator";
	 
	return [
		this.buildNameText(),
		util.makeElement("br"),
		this.buildValueText(),
		util.makeElement("br"),
		indicatorObject,
		util.makeElement("br"),
		this.buildUpdateButton(),
		util.makeElement("br"),
		this.buildControllerSelector()
	]
};

p.update = function() {
	DeviceView.prototype.update.apply(this);
	this.setDataFieldText("value", this.device.getValue());
};

return Sensor;