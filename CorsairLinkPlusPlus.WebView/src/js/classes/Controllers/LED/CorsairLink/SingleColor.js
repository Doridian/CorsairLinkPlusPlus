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

var LED = require("Controllers/LED");
var Color = require("Color");

function SingleColor(rawData) {
	LED.apply(this, arguments);
	var rawColor = this.value;
	this.value = new Color(rawColor.R, rawColor.G, rawColor.B);
};
var p = inherit(SingleColor, LED);

p.setValue = function(value) {
	this.value = value;
};

p.getValueInternal = function() {
	return {
		R: this.value.r,
		G: this.value.g,
		B: this.value.b
	};
};