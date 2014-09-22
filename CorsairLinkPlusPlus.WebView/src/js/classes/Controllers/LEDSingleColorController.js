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

var LEDController = require("classes/Controllers/LEDController");

function LEDSingleColorController(rawData) {
	LEDController.apply(this, arguments);
	var color = rawData.Value;
	this.color = new Color(color.r, color.g, color.b);
}
inherit(LEDSingleColorController, LEDController);

LEDSingleColorController.prototype.setColor = function(color)	{
	this.color = color;
}

LEDSingleColorController.prototype.getColor = function() {
	return this.color;
}

return LEDSingleColorController;