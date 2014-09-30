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

var DeviceTree = require("classes/DeviceTree");

api.path = "/api";

function recurseDeviceRequest(path) {
	return util.fetchJSON(path).then(function (data) {
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

api.executeOnDevice = function(path, method, params) {
	return util.fetchJSON(this.path + path, {
		Name: method,
		Params: params
	});
}

api.refreshDevice = function(path) {
	return this.executeOnDevice(path, "Refresh", {
		Volatile: true
	});
}

api.sendControllerUpdate = function(path, controller) {
	return this.executeOnDevice(path, "SetController", {
		Controller: controller.constructor.getFullClassName().replace("Controllers."),
		Params: controller.getData()
	});
}

api.fetchDeviceTree = function() {
	return recurseDeviceRequest(this.path).then(function(rawTree) {
		return new DeviceTree(rawTree);
	});
}

api.fetchDevice = function(devicePath) {
	return util.fetchJSON(this.path + "/" + devicePath);
}

function cleanupData(data) {
	data = data.result;
	delete data.ChildrenPaths;
	return data;
}

api.updateDevice = function(device) {
	return this.fetchDevice(device.getPath()).then(function(rawData) {
		device.setRawData(cleanupData(rawData));
	});
}

return api;
