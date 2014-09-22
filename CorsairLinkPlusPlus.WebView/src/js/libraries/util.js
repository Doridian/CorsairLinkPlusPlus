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


var util = {};

util.arrayCopy = function(arr) {
	return arr.slice(0);
}

util.fetchResource = function(url, type, data) {
	return new Promise(function (resolve, reject) {
		var req = new XMLHttpRequest();
		req.open(data ? "POST" || "GET", url, true, "root", "root");
		if(type)
			req.responseType = type;
		req.addEventListener("readystatechange", function (event) {
			if (this.readyState === XMLHttpRequest.DONE && this.status === 200) {
				if(type)
					resolve(this.response);
				else
					resolve(this.responseText);
			} else if (this.status !== 200) {
				var err = new Error("Request failed");
				err.code = this.status;
				reject(err);
			}
		});
		req.timeout = 2000;
		req.addEventListener("timeout", function () {
			reject(new Error("Request timed out"));
		})
		req.send(data);
	});
}

util.fetchJSON = function(url, data) {
	return this.fetchResource(url, "json", data);
}

return util;