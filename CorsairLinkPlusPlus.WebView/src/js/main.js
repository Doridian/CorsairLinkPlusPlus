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



var donePreload = {};
var loadCache = {};

function inherit(childClass, parentClass) {
	childClass.prototype = Object.create(parentClass.prototype);
	childClass.prototype.constructor = childClass;
	return childClass.prototype;
}

function require(what) {
	if(!donePreload[what])
		throw new Error("File " + what + " has not been loaded");
	if(loadCache[what])
		return loadCache[what];
	var ret;
	try {
		ret = donePreload[what]();
		var className = donePreload[what].path;
		if(className) {
			ret.className = className;
			ret.getFullClassName = function() {
				return className;
			}
		}
	} catch(e) {
		throw new Error("Failed to execute file:\n " + what + " " + e.message);
	}
	if(!ret)
		throw new Error("File " + what + " did not return anything");
	loadCache[what] = ret;
	return ret;
		
}

function xmlHttpPromise(url) {
	return new Promise(function (resolve, reject) {
		var req = new XMLHttpRequest();
		req.open("GET", url, true, "root", "root");
		req.addEventListener("readystatechange", function (event) {
			if (this.readyState === XMLHttpRequest.DONE && this.status === 200)
				resolve({
					contents: this.responseText,
					url: url
				});
			else if (this.status !== 200) {
				var err = new Error("Request failed");
				err.code = this.status;
				err.url = url;
				reject(err);
			}
		});
		req.timeout = 2000;
		req.addEventListener("timeout", function () {
			var err = new Error("Request timed out");
			err.url = url;
			reject(err);
		})
		req.send();
	})
}

var preload = [
	"classes/ControlCurve",
	"classes/Controller",
	"classes/Device",
	"classes/DeviceTree",
	"classes/Color",
	"classes/NotImplementedError",
	"classes/InvalidArgumentError",
	"classes/Controllers/Fan",
	"classes/Controllers/Fan/CorsairLink/BalancedMode",
	"classes/Controllers/Fan/CorsairLink/Curve",
	"classes/Controllers/Fan/CorsairLink/CustomCurve",
	"classes/Controllers/Fan/CorsairLink/Default",
	"classes/Controllers/Fan/CorsairLink/FixedPercent",
	"classes/Controllers/Fan/CorsairLink/FixedRPM",
	"classes/Controllers/Fan/CorsairLink/PerformanceMode",
	"classes/Controllers/Fan/CorsairLink/QuiteMode",
	"classes/Controllers/LED",
	"classes/Controllers/LED/CorsairLink/FourColor",
	"classes/Controllers/LED/CorsairLink/SingleColor",
	"classes/Controllers/LED/CorsairLink/Temperature",
	"classes/Controllers/LED/CorsairLink/TwoColor",
	"classes/Devices/Cooler",
	"classes/Devices/Hub",
	"classes/Devices/PSU",
	"classes/Devices/RootDevice",
	"classes/Devices/Sensor",
	"classes/Devices/VirtualHub",
	"classes/Devices/Sensors/ScalarSensor",
	"classes/Devices/Sensors/CurrentSensor",
	"classes/Devices/Sensors/EffiencySensor",
	"classes/Devices/Sensors/Fan",
	"classes/Devices/Sensors/LED",
	"classes/Devices/Sensors/PowerSensor",
	"classes/Devices/Sensors/Pump",
	"classes/Devices/Sensors/PWMFan",
	"classes/Devices/Sensors/Thermistor",
	"classes/Devices/Sensors/VoltageSensor",
	"classes/Gui/Views/Main/Plain",
	"classes/Gui/ControllerViewFactory",
	"classes/Gui/DeviceViewFactory",
	"classes/Gui/Views/DeviceView",
	"classes/Gui/Views/Devices/Sensor",
	"classes/Gui/Views/Devices/Sensors/ScalarSensor",
	"classes/Gui/Views/Devices/Sensors/Fan",
	"classes/Gui/Views/Devices/Sensors/LED",
	"classes/ControllerFactory",
	"classes/SensorFactory",
	"libraries/api",
	"libraries/util"
];

Promise.all(preload.map(function(val) {
	return xmlHttpPromise("js/" + val + ".js");
})).then(function(responseDataSets) {
	responseDataSets.forEach(function(responseData) {
		try {
			var loadFunc = new Function(responseData.contents);
			var trimmedPath = responseData.url.replace("js/", "").replace(".js", "");
			if(responseData.url.indexOf("classes") > -1)
				loadFunc.path = trimmedPath.replace(/\//g, ".").replace("classes.", "");
			donePreload[trimmedPath] = loadFunc;
		} catch(e) {
			throw Error("Could not compile file:\n " + responseData.url + ": " + e.message);
		}
	});
}).then(function() {
	//main file

	var api = require("libraries/api");
	
	var PlainView = require("classes/Gui/Views/Main/Plain")
	
	api.fetchDeviceTree().then(function(deviceTree) {
		try {
			var body = document.body;
			var mainView = new PlainView(deviceTree);
			body.appendChild(mainView.getElement());
		} catch(e) {
			console.error(e);
		}
	}, function(e) {
		console.error("Error while fetching devices", e);
	});
}, function(e) {
	console.error(e);
}).catch(function(e) {
	console.error(e);
});
