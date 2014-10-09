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

function View() {
}

var p = View.prototype;

p.getElement = function() {
	if(!this.element) {
		var buildNode = this.buildElement();
		this.dataFields = buildNode.idMap;
		this.element = buildNode.node;
		this.postBuildElement();
		this.update();
		return this.element;
	}
	return this.element;
};

p.postBuildElement = function() {
};

p.setDataFieldText = function(field, data) {
	if(!this.dataFields[field])
		throw new Error("View has no data field " + field);
	this.dataFields[field].firstChild.textContent = data;
};