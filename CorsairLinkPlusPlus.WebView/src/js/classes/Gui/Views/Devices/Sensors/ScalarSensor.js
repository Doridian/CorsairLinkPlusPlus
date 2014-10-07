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

var SensorView = require("Gui/Views/Devices/Sensor");
var DeviceView = require("Gui/Views/DeviceView");

var util = require("Util");

function ScalarSensor(device) {
	DeviceView.apply(this, arguments);
}
var p = inherit(ScalarSensor, SensorView);

p.buildGraph = function() {
	return {
		tag: "canvas",
		id: "graph",
		attributes: {
			width: 200,
			height: 100
		}
	}
};

p.postBuildElement = function() {
	var canvas = this.dataFields.graph;
	var graph = new Chart(canvas.getContext("2d")).Line({
		labels: util.arrayOf(20, ""),
		datasets: [
			{
				label: "Value",
				fillColor: "rgba(220,220,220,0.2)",
				strokeColor: "rgba(220,220,220,1)",
				data: util.arrayOf(20, 0)
			}
		]
	}, {
		animation: false,
		pointDot: false,
		showTooltips: false,
	});
	this.graph = graph;
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
		this.buildGraph(),
		util.makeElement("br"),
		{
			tag: "div",
			attributes: {
				className: "controller-container"
			}
		},
		this.buildUpdateButton(),
		util.makeElement("br"),
		this.buildControllerSelector()
	]
};

p.update = function() {
	DeviceView.prototype.update.apply(this);
	var value = this.device.getValue()
	this.setDataFieldText("value", value.toFixed(1) + this.device.getUnit());
	this.graph.removeData();
	this.graph.addData([value], "");
};

return ScalarSensor;