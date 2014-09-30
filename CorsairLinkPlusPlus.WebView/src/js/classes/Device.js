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

function Device(rawData) {
	this.setRawData(rawData);
	this.listeners = [];
}

var p = Device.prototype;

p.setRawData = function(rawData) {
	this.name = rawData.Name;
	this.path = rawData.AbsolutePath;
	try {
		for(var idx in this.listeners)
			this.listeners[idx]();
	} catch(e) {
		console.log(e);
	}
}

p.addListener = function(listener) {
	this.listeners.push(listener);
}

p.getPath = function() {
	return this.path;
}

p.setParent = function(parent) {
	this.parent = parent;
}

p.getParent = function() {
	return this.parent;
}

p.getName = function() {
	return this.name;
}

return Device;