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

var FanController = require("Controllers/Fan");

function FixedPercent(rawData) {
	FanController.apply(this, arguments);
}
var p = inherit(FixedPercent, FanController);


p.setValue = function(val) {
	if(val < 0 || val > 1)
		throw new InvalidArgumentError("value must be in unit interval");
	this.value = val;
	return this.sendUpdate();
}

return FixedPercent;