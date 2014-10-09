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
	"ControlCurve",
	"Controller",
	"Device",
	"DeviceTree",
	"Color",
	"NotImplementedError",
	"InvalidArgumentError",
	"Controllers/Fan",
	"Controllers/Fan/CorsairLink/BalancedMode",
	"Controllers/Fan/CorsairLink/Curve",
	"Controllers/Fan/CorsairLink/CustomCurve",
	"Controllers/Fan/CorsairLink/Default",
	"Controllers/Fan/CorsairLink/FixedPercent",
	"Controllers/Fan/CorsairLink/FixedRPM",
	"Controllers/Fan/CorsairLink/PerformanceMode",
	"Controllers/Fan/CorsairLink/QuiteMode",
	"Controllers/LED",
	"Controllers/LED/CorsairLink/FourColor",
	"Controllers/LED/CorsairLink/SingleColor",
	"Controllers/LED/CorsairLink/Temperature",
	"Controllers/LED/CorsairLink/TwoColor",
	"Devices/Cooler",
	"Devices/Hub",
	"Devices/PSU",
	"Devices/RootDevice",
	"Devices/Sensor",
	"Devices/VirtualHub",
	"Devices/Sensors/ScalarSensor",
	"Devices/Sensors/CurrentSensor",
	"Devices/Sensors/EffiencySensor",
	"Devices/Sensors/Fan",
	"Devices/Sensors/LED",
	"Devices/Sensors/PowerSensor",
	"Devices/Sensors/Pump",
	"Devices/Sensors/PWMFan",
	"Devices/Sensors/Thermistor",
	"Devices/Sensors/VoltageSensor",
	"Gui/View",
	"Gui/Views/Main/Plain",
	"Gui/ControllerViewFactory",
	"Gui/DeviceViewFactory",
	"Gui/Views/DeviceView",
	"Gui/Views/Devices/Sensor",
	"Gui/Views/Devices/Sensors/ScalarSensor",
	"Gui/Views/Devices/Sensors/Fan",
	"Gui/Views/Devices/Sensors/LED",
	"Gui/Views/Devices/Sensors/Pump",
	"Gui/Views/ControllerView",
	"Gui/Views/Controllers/LED/CorsairLink/SingleColor",
	"Gui/Views/Controllers/Fan/CorsairLink/Curve",
	"Gui/Views/Controllers/Fan/CorsairLink/CustomCurve",
	"ControllerFactory",
	"SensorFactory",
	"Api",
	"Util"
];

Promise.all(preload.map(function(val) {
	return xmlHttpPromise("js/classes/" + val + ".js");
})).then(function(responseDataSets) {
	responseDataSets.forEach(function(responseData) {
		try {
			var trimmedPath = responseData.url.replace("js/classes/", "").replace(".js", "");
			var className = trimmedPath.match(/[\w_]+$/)[0];
			var loadFunc = new Function(responseData.contents + "\r\n\r\n return " + className + ";");
			
			if(responseData.url.indexOf("classes") > -1)
				loadFunc.path = trimmedPath.replace(/\//g, ".").replace("classes.", "");
			donePreload[trimmedPath] = loadFunc;
		} catch(e) {
			throw Error("Could not compile file:\n " + responseData.url + ": " + e.message);
		}
	});
}).then(function() {
	//main file

	var api = require("Api").getInstance();
	
	var PlainView = require("Gui/Views/Main/Plain")
	
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
