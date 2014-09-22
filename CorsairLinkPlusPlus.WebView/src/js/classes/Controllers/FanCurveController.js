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

var FanController = require("classes/Controllers/FanController");
var ControlCurve = require("classes/ControlCurve");

function FanCurveController(rawData) {
	FanController.apply(this, arguments);
	var points = rawData.Value.Points;
	var curve = new ControlCurve();
	points.forEach(function(point) {
		curve.add(point);
	});
	this.curve = curve;
}
inherit(FanCurveController, FanController);

FanCurveController.prototype.getCurve = function() {
	return this.curve;
}

return FanCurveController;