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

var LED = require("Controllers/LED");
var ControlCurve = require("ControlCurve");

function LEDTemperature(rawData) {
	LED.apply(this, arguments);
	var points = this.value.Points;
	var curve = new ControlCurve();
	for(var point of points)
		curve.add({
			x: point.X,
			y: new Color(point.Y.R, point.Y.G, point.Y.B)
		});
	this.value = curve;
}
var p = inherit(LEDTemperature, LED);

return LEDTemperature;