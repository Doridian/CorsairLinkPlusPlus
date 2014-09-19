using CorsairLinkPlusPlus.Driver.CorsairLink.Sensor;

namespace CorsairLinkPlusPlus.Driver.CorsairLink.Controller.Fan
{
    public class FanDefaultController : TemperatureDependantControllerBase, FanController
    {
        public FanDefaultController() { }

        public FanDefaultController(Thermistor thermistor)
            : base(thermistor)
        {

        }

        internal override void Apply(Sensor.BaseSensorDevice sensor)
        {
            if(sensor is TemperatureControllableSensor)
                base.Apply(sensor);
        }

        internal override void Refresh(Sensor.BaseSensorDevice sensor)
        {
            if (sensor is TemperatureControllableSensor)
                base.Refresh(sensor);
        }

        public byte GetFanModernControllerID()
        {
            return 0x06;
        }

        public virtual void AssignFrom(Sensor.Fan fan)
        {
            if (fan is TemperatureControllableSensor)
                SetThermistor(((TemperatureControllableSensor)fan).GetTemperatureSensor());
        }
    }
}
