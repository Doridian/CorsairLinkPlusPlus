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

function DeviceViewFactory() {
	this.deviceMap = new Map();
}

util.makeSingletonGetter(DeviceViewFactory);

var p = DeviceViewFactory.prototype;

p.getByDevice = function(device) {
	var ret;
	try {
		console.log(this);
		ret = this.deviceMap.get(device);
		if(!ret) {
			var constructor = require("classes/Gui/Views/Devices/" + typeof controller);
			ret = new constructor(device);
		}
		return ret;
	} catch(e) {
		console.error("Could not find view for device " + device.getName());
		console.log(e);
		var constructor = require("classes/Gui/Views/DeviceView");
		ret = new constructor(device);
	}
	this.deviceMap.set(device, ret);
	return ret;
};

return DeviceViewFactory;