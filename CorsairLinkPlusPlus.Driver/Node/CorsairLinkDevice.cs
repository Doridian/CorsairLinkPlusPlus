using CorsairLinkPlusPlus.Driver.Sensor;
using CorsairLinkPlusPlus.Driver.USB;
using System;
using System.Collections.Generic;

namespace CorsairLinkPlusPlus.Driver.Node
{
    public abstract class CorsairLinkDevice
    {
        internal static CorsairLinkDevice CreateNew(CorsairLinkUSBDevice usbDevice, byte channel, byte deviceType)
        {
            switch (deviceType)
            {
                case 0x5:
                    return new CorsairLinkDeviceModern(usbDevice, channel);
                case 0x3:
                    return new CorsairLinkDeviceAFP(usbDevice, channel);
                case 0x1:
                    return new CorsairLinkDevicePMBUS(usbDevice, channel);
                default:
                    return null;
            }
        }

        protected readonly CorsairLinkUSBDevice usbDevice;
        protected readonly byte channel;

        internal CorsairLinkDevice(CorsairLinkUSBDevice usbDevice, byte channel)
        {
            this.usbDevice = usbDevice;
            this.channel = channel;
        }

        internal byte ReadSingleByteRegister(byte register)
        {
            return usbDevice.ReadSingleByteRegister(register, channel);
        }

        internal byte[] ReadRegister(byte register, byte bytes)
        {
            return usbDevice.ReadRegister(register, channel, bytes);
        }

        internal void WriteSingleByteRegister(byte register, byte value)
        {
            usbDevice.WriteSingleByteRegister(register, channel, value);
        }

        internal void WriteRegister(byte register, byte[] bytes)
        {
            usbDevice.WriteRegister(register, channel, bytes);
        }

        public virtual string GetDeviceName()
        {
            throw new NotImplementedException();
        }
        public virtual int GetCoolerCount()
        {
            return 0;
        }

        internal virtual double GetCoolerRPM(int id)
        {
            return 0;
        }

        internal virtual double GetUsagePercent(int id)
        {
            return 0;
        }

        public virtual CorsairCooler GetCooler(int id)
        {
            if (id < 0 || id >= GetCoolerCount())
                return null;
            switch (GetCoolerType(id))
            {
                case "Fan":
                    return new CorsairFan(this, id);
                case "Pump":
                    return new CorsairPump(this, id);
            }
            return null;
        }

        public virtual CorsairThermistor GetTemperature(int id)
        {
            if (id < 0 || id >= GetTemperatureCount())
                return null;
            return new CorsairThermistor(this, id);
        }

        public virtual List<CorsairLinkDevice> GetSubDevices()
        {
            return new List<CorsairLinkDevice>();
        }

        internal virtual double GetTemperatureDegC(int id)
        {
            return 0;
        }

        public virtual string GetCoolerType(int id)
        {
            return "Fan";
        }

        public virtual int GetTemperatureCount()
        {
            return 0;
        }

        public virtual int GetLEDCount()
        {
            return 0;
        }

        public virtual List<CorsairSensor> GetSensors()
        {
            List<CorsairSensor> ret = new List<CorsairSensor>();

            for (int i = 0; i < GetCoolerCount(); i++)
            {
                CorsairSensor sensor = GetCooler(i);
                if(sensor.IsPresent())
                    ret.Add(sensor);
            }

            for (int i = 0; i < GetTemperatureCount(); i++)
            {
                CorsairSensor sensor = GetTemperature(i);
                if (sensor.IsPresent())
                    ret.Add(sensor);
            }

            return ret;
        }
    }
}
