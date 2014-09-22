var Fan = require("classes/Devices/Sensors/Fan");
var PWMFan = require("classes/Devices/Sensors/PWMFan");
var Thermistor = require("classes/Devices/Sensors/Thermistor");
var LED = require("classes/Devices/Sensors/LED");
var CurrentSensor = require("classes/Devices/Sensors/CurrentSensor");
var VoltageSensor = require("classes/Devices/Sensors/VoltageSensor");
var PowerSensor = require("classes/Devices/Sensors/PowerSensor");
var EffiencySensor = require("classes/Devices/Sensors/EffiencySensor");
var Pump = require("classes/Devices/Sensors/Pump");

function SensorFactory() {
}

SensorFactory.create = function(rawData) {
	switch(rawData.SensorType) {
		case "Fan":
			return new Fan(rawData);
		case "PWMFan":
			return new PWMFan(rawData);
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
}

return SensorFactory;