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

var util = require("libraries/util")

function Hub(rawData) {
	Device.apply(this, arguments);
	this.children = [];
}
inherit(Hub, Device);

Hub.prototype.getChildren = function() {
	return util.arrayCopy(this.children);
}

Hub.prototype.addChild = function(device) {
	this.children.push(device);
}

Hub.prototype.getName = function() {
	return this.name;
}

return Hub;