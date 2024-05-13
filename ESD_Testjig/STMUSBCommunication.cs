using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
//using  System.Management.ManagementObjectCollection;
using System.IO.Ports;
using System.IO;
using System.Threading;

namespace ESD_Testjig
{
    public static class STMUSBCommunication
    {
        public static System.IO.Ports.SerialPort serialPort;
        //static Stream _SafeBaseStream;

        public static string myPort = string.Empty;       
        static int lngHandle = 0;

        public static byte[] receivedbytes;
        public static int getDeviceCount()
        {
            int deviceCount = 0;
            string[] ports = SerialPort.GetPortNames();
            if(ports.Contains(myPort))
            {
                if(serialPort.IsOpen)
                {
                    deviceCount = ports.Length;
                }
                else
                {
                    bool checkOpen = OpenAlphaPort(myPort);
                    if(checkOpen)
                    {
                        deviceCount = 1;
                    }
                    else
                    {
                        deviceCount = -1;
                    }
                }                
            }
             return deviceCount;
        }
        public static int OpenUSB()
        {
            //1.Find all ports
            string alphaPort = AlphaPort();           
            myPort = alphaPort;
            if(serialPort!=null)
            {
               if(serialPort.IsOpen)
                {
                    lngHandle = 1;
                    return lngHandle;
                }
            }
            if (OpenAlphaPort(alphaPort))
            {
                lngHandle = 1;
            }
            else
            {
                lngHandle = -1;
            }          
            return lngHandle;
        }

        public static bool OpenAlphaPort(string portName)
        {
            bool ret = false;
            try
            {                
                using (serialPort = new SerialPort(portName, 115200, Parity.None,8,StopBits.One))
                {
                    if (!serialPort.IsOpen)
                    {
                        serialPort.Open();
                       // Thread.Sleep(500);
                        ret= true;
                    }                       
                }
                return ret;
            }
            catch (Exception ex)
            {
                //serialPort.Close();
                ex.ToString();
                return ret;
            }
        }       

        public static string AlphaPort()
        {
            string alphaPortDescription = "Prolific USB-to-Serial Comm Port";
            string alphaPortAnatherDescription = "USB Serial Port";
            string alphaPortSerial = "USB-SERIAL CH340 (COM14)";
            string alphaPort = string.Empty;
            ManagementObjectSearcher deviceList = new ManagementObjectSearcher("Select Name, Status from Win32_PnPEntity");

            ManagementObjectCollection deviceCollection = deviceList.Get();
            System.Management.ManagementObjectCollection.ManagementObjectEnumerator ourDevice = deviceCollection.GetEnumerator();
            
            while (ourDevice.MoveNext())
            {
                try
                {
                    ManagementBaseObject obj = ourDevice.Current;
                    PropertyDataCollection propertyCollection = obj.Properties;
                    QualifierDataCollection qulaifie = obj.Qualifiers;

                    string portName = obj.GetPropertyValue("name").ToString();
                    if (portName.Contains(alphaPortDescription)||portName.Contains(alphaPortAnatherDescription) || portName.Contains(alphaPortSerial))    //icroelectronics Virtual COM Port (COM8)")
                    {
                        string[] prts = portName.Split('(');
                        prts = prts[1].Split(')');
                        alphaPort = prts[0].ToString();                                             
                        return alphaPort;
                    }
                }
                catch (Exception ex)
                {
                    ex.ToString();
                    alphaPort = "";
                }
            }          
            return alphaPort;
        }        
    }
}
