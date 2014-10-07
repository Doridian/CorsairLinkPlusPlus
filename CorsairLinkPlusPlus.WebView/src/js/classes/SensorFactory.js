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

var Fan = require("Devices/Sensors/Fan");
var PWMFan = require("Devices/Sensors/PWMFan");
var Thermistor = require("Devices/Sensors/Thermistor");
var LED = require("Devices/Sensors/LED");
var CurrentSensor = require("Devices/Sensors/CurrentSensor");
var VoltageSensor = require("Devices/Sensors/VoltageSensor");
var PowerSensor = require("Devices/Sensors/PowerSensor");
var EffiencySensor = require("Devices/Sensors/EffiencySensor");
var Pump = require("Devices/Sensors/Pump");

function SensorFactory() {
}

SensorFactory.create = function(rawData) {
	switch(rawData.SensorType) {
		case "Fan":
			if(rawData.PWM)
				return new PWMFan(rawData);
			return new Fan(rawData);
		case "Temperature":
			return new Thermistor(rawData);
		case "LED":
			return new LED(rawData);
		case "Current":
			return new CurrentSensor(rawData);
		case "Voltage":
			return new VoltageSensor(rawData);
		case "Power":
			return new PowerSensor(rawData);
		case "Efficiency":
			return new EffiencySensor(rawData);
		case "Pump":
			return new Pump(rawData);
		default:
			console.warn("Unhandled sensor type " + rawData.SensorType);
			return new Sensor(rawData);
	}
};

return SensorFactory;