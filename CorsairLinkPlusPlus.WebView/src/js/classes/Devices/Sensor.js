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

var Device = require("Device");

var util = require("Util");

function Sensor(rawData) {
	Device.apply(this, arguments);
}

var p = inherit(Sensor, Device);

p.setRawData = function(rawData) {
	this.type = rawData.SensorType;
	this.unit = rawData.Unit;
	this.validControllerNames = rawData.ValidControllerNames || [];
	Device.prototype.setRawData.apply(this, arguments);
};

p.getValidControllerNames = function() {
	return util.arrayCopy(this.validControllerNames);
};

p.getValue = function() {
	return this.value;
};

p.getUnit = function() {
	return this.unit;
};

p.setController = function(controller) {
	this.controller = controller;
};

p.getController = function() {
	return this.controller;
};

return Sensor;
