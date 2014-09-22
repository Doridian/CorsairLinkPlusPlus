
using CorsairLinkPlusPlus.Common;
using CorsairLinkPlusPlus.Common.Controller;
using CorsairLinkPlusPlus.Common.Registry;
using CorsairLinkPlusPlus.Common.Sensor;
using CorsairLinkPlusPlus.Common.Utility;
using System.Collections.Generic;
namespace CorsairLinkPlusPlus.RESTAPI.Methods
{
    public class SetController : BaseMethod
    {
        public override void Call()
        {
            IController controller = ControllerRegistry.Get(GetRequiredArgument<string>("Controller")).New();

            //Set thermistors
            if(controller is ITemperatureDependantController)
                ((ITemperatureDependantController)controller).SetThermistor((IThermistor)RootDevice.FindDeviceByPath(GetRequiredArgument<string>("Thermistor")));

            //Set fixed values
            if (controller is IFixedNumberController)
                ((IFixedNumberController)controller).Value = GetRequiredArgument<double>("Value");

            if (controller is IFixedColorController)
                ((IFixedColorController)controller).Value = GetRequiredArgument<Color>("Value");

            if (controller is IFixedColorCycleController)
                ((IFixedColorCycleController)controller).Value = new List<Color>(GetRequiredArgument<IEnumerable<Color>>("Value")).ToArray();

            //Set curves
            if(controller is ICurveColorController)
                ((ICurveColorController)controller).Value = GetRequiredArgument<ControlCurve<double, Color>>("Value");

            if (controller is ICurveNumberController)
                ((ICurveNumberController)controller).Value = GetRequiredArgument<ControlCurve<double, double>>("Value");

            ((IControllableSensor)Device).Controller = controller;
        }

        public override bool IsDeviceValid()
        {
            return Device is IControllableSensor;
        }
    }
}
