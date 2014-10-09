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

var InvalidArgumentError = require("InvalidArgumentError");

function Color(r, g, b) {
	this.r = r;
	this.g = g;
	this.b = b;
}

var regex = /^#?([0-9A-Fa-f]{2})([0-9A-Fa-f]{2})([0-9A-Fa-f]{2})$/;

Color.fromHTMLHexString = function(str) {
	var matches = regex.exec(str);
	if(!matches)
		throw new InvalidArgumentError("invalid string");
	return new Color(parseInt(matches[1], 16), parseInt(matches[2], 16), parseInt(matches[3], 16));
}

var p = Color.prototype;

p.toString = function() {
	return this.r + ", " + this.g + ", " + this.b;
};

//TODO: get rid of me
function dec2hex(i) {
   return (i+0x10000).toString(16).substr(-2).toUpperCase();
}

p.toHTMLHexString = function(usePrefix) {
	return (usePrefix ? "#" : "") + dec2hex(this.r) + dec2hex(this.g) + dec2hex(this.b);
};