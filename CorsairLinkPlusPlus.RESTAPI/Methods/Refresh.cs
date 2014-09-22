
namespace CorsairLinkPlusPlus.RESTAPI.Methods
{
    public class Refresh : BaseMethod
    {
        public override void Call()
        {
            bool volatileOnly = GetOptionalArgument<bool>("VolatileOnly", true);
            Device.Refresh(volatileOnly);
        }
    }
}
