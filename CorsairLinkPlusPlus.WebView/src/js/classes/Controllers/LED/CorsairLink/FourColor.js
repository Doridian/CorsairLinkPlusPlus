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

var util = require("libraries/util");

var LED = require("classes/Controllers/LED");
var InvalidArgumentError = require("classes/InvalidArgumentError");
var Color = require("classes/Color");

function FourColor(rawData) {
	if(rawData.length != 4)
		throw new InvalidArgumentError("List must contain exactly 4 values");
	LED.apply(this, arguments);
	var old = this.value;
	this.value = [];
	old.forEach(function(rawColor) {
		this.value.push(new Color(rawColor.R, rawColor.G, rawColor.B));
	});
}
var p = inherit(FourColor, LED);

p.getValue = function() {
	return util.arrayCopy(this.value);
}

p.setValue = function(value) {
	this.value = util.arrayCopy(value);
}

return FourColor;