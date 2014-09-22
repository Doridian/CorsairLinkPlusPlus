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

var Device = require("classes/Device");

function Sensor(rawData) {
	Device.apply(this, arguments);
	this.type = rawData.SensorType;
	this.unit = rawData.Unit;
	this.value = rawData.Value;
	this.validControllerNames = rawData.ValidControllerNames;
}
inherit(Sensor, Device);

Sensor.prototype.getValue = function() {
	return this.value;
}

Sensor.prototype.getUnit = function() {
	return this.unit;
}

Sensor.prototype.setController = function(controller) {
	this.controller = controller;
}

Sensor.prototype.sendControllerUpdate = function(controller) {
	return api.sendControllerUpdate(this.getPath(), controller.fetchData());
}

return Sensor;
