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

var util = require("Util");

function ControllerFactory() {
}

util.makeSingletonGetter(ControllerFactory);

var p = ControllerFactory.prototype;

p.create = function(rawData) {
	var constructor;
	try {
		constructor = require("Controllers/" + rawData.Name.replace(/\./g, "/"));
	} catch(e) {
		throw new Error("Unhandled controller type " + rawData.Name + " " + e.message);
	}
	return new constructor(rawData);
};

p.createByName = function(name) {
	var constructor;
	try {
		constructor = require("Controllers/" + name.replace(/\./g, "/"));
	} catch(e) {
		throw new Error("Unhandled controller type " + name + " " + e.message);
	}
	return new constructor();
}