using CorsairLinkPlusPlus.Common.Sensor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorsairLinkPlusPlus.Common.Controller
{
    public interface ITemperatureDependantController
    {
        void SetThermistor(IThermistor thermistor);
        IThermistor GetThermistor();
    }
}
