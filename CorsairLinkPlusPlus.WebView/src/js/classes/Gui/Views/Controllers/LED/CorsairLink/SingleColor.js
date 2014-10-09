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

var ControllerView = require("Gui/Views/ControllerView");

var util = require("Util");

var Color = require("Color");

function SingleColor(controller) {
	ControllerView.apply(this, arguments);
}

var p = inherit(SingleColor, ControllerView);

p.update = function() {
	this.dataFields.colorInput.value = this.controller.getValue().toHTMLHexString(true);
};

p.buildElement = function() {
	var self = this;
	return util.makeElementTree({
		tag: "input",
		id: "colorInput",
		attributes: {
			type: "color"
		},
		events: {
			change: function(event) {
				var color = Color.fromHTMLHexString(this.value);
				self.controller.setValue(color);
				self.sendUpdate();
			}
		}
	});
};