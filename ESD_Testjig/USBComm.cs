using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using System.Threading;

namespace ESD_Testjig
{
    static class USBComms
    {
        static int usbHandle = -1;
        static int devCount = 0;
        static System.Windows.Forms.StatusStrip ss;      
        delegate void tmrDel(string status);
        static tmrDel td = new tmrDel(setText);
        //static USBCommunication usbObj = new USBCommunication();
        //static STMUSBCommunication stmUsbObj = new STMUSBCommunication();
        //public static int totalPCBTestCaeses = 0;

        public static void InitialCheckUSB()
        {
            usbHandle = STMUSBCommunication.OpenUSB();
        }
        public static void StatusTmrNew(object state)
        {
            try
            {
                if (usbHandle < 0)
                {
                    usbHandle = STMUSBCommunication.OpenUSB();
                }
                //STMUSBCommunication.AlphaPort();
                devCount = STMUSBCommunication.getDeviceCount();
                if (devCount > 0)
                {
                    ss.Invoke(td, new object[] { "USB device connected" });                  
                    //STMUSBCommunication.CloseUSB();
                }
                else
                {
                    ss.Invoke(td, new object[] { "USB device not connected" });                   
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }        

        static void setText(string status)
        {
           ss.Items[0].Text = status;
        }

        static TimerCallback tc = new TimerCallback(StatusTmrNew);
        static Timer t = new Timer(tc, null, Timeout.Infinite, 1000);       

        /// <summary>
        /// 
        /// </summary>
        /// <param name="st"></param>
        public static void TimerStart(System.Windows.Forms.StatusStrip st)
        {
            ss = st;
            t.Change(10,500);
        }
        public static void TimerStart()
        {
            t.Change(10, 500);
        }
        public static void TimerStop()
        {
            t.Change(Timeout.Infinite, Timeout.Infinite);
        }
    }
}
