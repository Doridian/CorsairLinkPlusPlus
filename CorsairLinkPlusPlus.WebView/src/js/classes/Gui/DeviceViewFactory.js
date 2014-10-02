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
var SensorView = require("classes/Gui/Views/Devices/Sensor");

var Fan = require("classes/Devices/Sensors/Fan");
var FanView = require("classes/Gui/Views/Devices/Sensors/Fan");

var LED = require("classes/Devices/Sensors/LED");
var LEDView = require("classes/Gui/Views/Devices/Sensors/LED");

function DeviceViewFactory() {
	this.deviceMap = new Map();
}

util.makeSingletonGetter(DeviceViewFactory);

var p = DeviceViewFactory.prototype;

p.getByDevice = function(device) {
	var ret;
	try {
		ret = this.deviceMap.get(device);
		if(!ret) {
			var constructor = require("classes/Gui/Views/Devices/" + device.constructor.name);
			ret = new constructor(device);
		}
		return ret;
	} catch(e) {
		console.error("Could not find view for device " + device.getName());
		console.log(e);
		var constructor
		if(device instanceof Fan)
			constructor = FanView;
		else if(device instanceof LED)
			constructor = LEDView;
		else if(device instanceof Sensor)
			constructor = SensorView;
		else
			constructor = require("classes/Gui/Views/DeviceView");
		ret = new constructor(device);
	}
	this.deviceMap.set(device, ret);
	return ret;
};

return DeviceViewFactory;