using CorsairLinkPlusPlus.Driver.Sensor;

namespace CorsairLinkPlusPlus.Driver.Controller.Fan
{
    public class CorsairFanDefaultController : CorsairTemperatureDependantControllerBase, CorsairFanController
    {
        public CorsairFanDefaultController()
        {

        }

        public CorsairFanDefaultController(CorsairThermistor thermistor)
            : base(thermistor)
        {

        }

        internal override void Apply(CorsairSensor sensor)
        {
            if(sensor is CorsairTemperatureControllableSensor)
                base.Apply(sensor);
        }

        internal override void Refresh(CorsairSensor sensor)
        {
            if (sensor is CorsairTemperatureControllableSensor)
                base.Refresh(sensor);
        }

        public byte GetFanModernControllerID()
        {
            return 0x06;
        }

        public virtual void AssignFrom(CorsairFan fan)
        {
            if (fan is CorsairTemperatureControllableSensor)
                SetThermistor(((CorsairTemperatureControllableSensor)fan).GetTemperatureSensor());
        }
    }
}
