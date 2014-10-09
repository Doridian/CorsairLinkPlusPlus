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

var util = require("Util");
var DeviceTree = require("DeviceTree");
var Hub = require("Devices/Hub");

function Api() {
	this.path = "/api";
}

var p = Api.prototype;

util.makeSingletonGetter(Api);

p.recurseDeviceRequest = function(path) {
	var self = this;
	return util.fetchJSON(path).then(function (data) {
		var childPaths = data.result.ChildrenPaths;
		if (childPaths.length > 0) {
			delete data.result.ChildrenPaths;
			return Promise.all(childPaths.map(function (val) {
				return self.recurseDeviceRequest(self.path + val);
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
};

p.executeOnDevice = function(path, method, params) {
	return util.fetchJSON(this.path + path, {
		Name: method,
		Params: params
	});
};

p.refreshDevice = function(path) {
	return this.executeOnDevice(path, "Refresh", {
		Volatile: true
	});
};

p.sendControllerUpdate = function(device, controller) {
	return this.executeOnDevice(device.getPath(), "SetController", {
		Controller: controller.constructor.getFullClassName().replace("Controllers.", ""),
		Value: controller.getValueInternal()
	});
};

p.fetchDeviceTree = function() {
	return this.recurseDeviceRequest(this.path).then(function(rawTree) {
		return new DeviceTree(rawTree);
	});
};

p.fetchDevice = function(devicePath) {
	return util.fetchJSON(this.path + "/" + devicePath);
};

function cleanupData(data) {
	data = data.result;
	delete data.ChildrenPaths;
	return data;
};

p.updateDevice = function(device, recursive) {
	var promises = [];
	if(recursive && device instanceof Hub)
		device.getChildren().forEach(function(childDevice) {
			promises.push(this.updateDevice(childDevice, recursive));
		}, this);
	promises.push(this.fetchDevice(device.getPath()).then(function(rawData) {
		device.setRawData(cleanupData(rawData));
	}));
	return Promise.all(promises);
};

return Api;
