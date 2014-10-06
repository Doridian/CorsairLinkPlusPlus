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

var ScalarSensorView = require("Gui/Views/Devices/Sensors/ScalarSensor");
var DeviceView = require("Gui/Views/DeviceView");

function Fan(device) {
	ScalarSensorView.apply(this, arguments);
}
var p = inherit(Fan, ScalarSensorView);

p.buildIndicator = function() {
	return {
		tag: "div",
		children: [
			{
				tag: "div",
				id: "spinner"
			}
		]
	}
};

p.update = function() {
	ScalarSensorView.prototype.update.apply(this);
	var spinner = this.dataFields.spinner;
	var rpm = this.device.getValue();
	if(this.interval)
		clearInterval(this.interval);
	spinner.rotation = spinner.rotation || 0;
	var updateRate = 30;
	this.interval = setInterval(function() {
		spinner.rotation += ((rpm / 60) / updateRate) * 0.05;
		spinner.style.transform = "rotate(" + spinner.rotation.toFixed(4) + "turn)";
	}, 1000 / updateRate);
	
	DeviceView.prototype.update.apply(this);
	this.setDataFieldText("value", this.device.getValue() + this.device.getUnit());
};

return Fan;