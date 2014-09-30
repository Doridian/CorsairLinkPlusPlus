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

var util = require("libraries/util");

var Sensor = require("classes/Devices/Sensor");

function DeviceView(device) {
	this.device = device;
}

var p = DeviceView.prototype;

p.getDevice = function() {
	return this.device;
}

p.getElement = function() {
	if(!this.element) {
		this.buildElement();
		return this.element;
	}
	return this.element;
};

p.buildElement = function() {
	var treeData = util.makeElementTree({
		tag: "div",
		attributes: {
			className: "device"
		}, children: [
			{
				tag: "span",
				children: [
					util.makeText("Name: "),
					{
						tag: "span",
						attributes: {
							className: "name"
						},
						id: "name",
						children: [
							util.makeText()
						]
					}
				]
			},
			{
				tag: "br"
			},
			(this.device instanceof Sensor) && {
				tag: "span",
				children: [
					hildren: [
						util.makeText("Value: "),
						{
							tag: "span",
							attributes: {
								className: "value"
							},
							id: "name",
							children: [
								util.makeText()
							]
						}
					]
				]
			},
			{
				tag: "br"
			},
			(this.device instanceof Sensor) && {
				tag: "input",
				attributes: {
					type: "button",
					value: "update"
				},
				events: {
					click: function(event) {
					}
				},
				children: [
					util.makeText("Value: " + this.device.getValue() + " " + this.device.getUnit())
				]
			},
		]
	});
	this.element = treeData.node;
	this.dataFields = treeData.idMap;
};

return DeviceView;