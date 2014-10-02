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

var SensorView = require("classes/Gui/Views/Devices/Sensor");

function LED(device) {
	SensorView.apply(this, arguments);
}
var p = inherit(LED, SensorView);

p.buildInternalElement = function() {
	return SensorView.prototype.buildInternalElement.apply(this, arguments).concat({
		tag: "div",
		attributes: {
			className: "sensor"
		},
		children: [
			{
				tag: "div",
				id: "indicator",
				attributes: {
					className: "color"
				}
			}
		]
	});
}

p.update = function() {
	SensorView.prototype.update.apply(this);
	var indicator = this.dataFields.indicator;
	var color = this.device.getValue();
	indicator.style.backgroundColor = "rgb(" + color.r + ", " + color.g + ", " + color.b + ")";
};

return LED;