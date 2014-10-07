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

var ControllerViewFactory = require("Gui/ControllerViewFactory");

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

p.setController = function(controller, noUpdate) {
	this.device.setController(controller);
	if(!noUpdate)
		Api.getInstance().sendControllerUpdate(this.device, controller);
	util.removeChildren(this.dataFields.controllerContainer);
	var controllerView = ControllerViewFactory.getInstance().getByController(controller);
	
	var element = controllerView.getElement();
	this.dataFields.controllerContainer.appendChild(element);
};

p.postBuildElement = function() {
	var currentController = this.device.getController();
	this.setController(currentController, true);
};

p.buildControllerSelector = function() {
	var names = this.device.getValidControllerNames();
	if(names.length == 0)
		return;
	
	var currentControllerName = this.device.getController().constructor.name;

	var children = [];
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
				self.setController(ControllerFactory.getInstance().createByName(this.value));
			}
		}
	};
};

p.buildControllerContainer = function() {
	return {
		tag: "div",
		id: "controllerContainer",
		attributes: {
			className: "controller-container"
		}
	};
};
		

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
		this.buildControllerContainer(),
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