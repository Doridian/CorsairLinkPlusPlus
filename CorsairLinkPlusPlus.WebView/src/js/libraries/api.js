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


var api = {};

var util = require("libraries/util");



api.path = "/api";

function recurseDeviceRequest(path) {
	return util.xmlHTTPPromise(path).then(function (data) {
		var childPaths = data.result.ChildrenPaths;

		if (childPaths.length > 0) {
			delete data.result.ChildrenPaths;
			return Promise.all(childPaths.map(function (val) {
				return recurseDeviceRequest(api.path + val);
			})).then(function (objects) {
				data.result.Children = objects;
				return data.result;
			}, function (rejectValue) {
				if (rejectValue instanceof Error)
					throw rejectValue;
				var err = new Error("Failed to promise all");
				err.value = rejectValue;
				throw err;
			})
		}
		delete data.result.ChildrenPaths;
		data.result.Children = [];
		return data.result;
	}, function (code) {
		throw new Error("failed with code " + code);
	});
}

api.fetchAllDevices = function () {
	return recurseDeviceRequest(this.path).then(function(rawTree) {
		return rawTree;
	});
}

api.fetchDevice = function (devicePath) {
	return util.xmlHTTPPromise(this.path + "/" + devicePath);
}

return api;
