using ESDBE;
using ESD_Testjig;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using System.Threading;

namespace ESD_Testjig
{
    static class USBCommunication
    {
        delegate void tmrDelTest(string txt);
        readonly static tmrDelTest tdTest = new tmrDelTest(SetTextTest);
        readonly static ESD_TestjigProperties _objDal = new ESD_TestjigProperties();       
        readonly static ESDBEProperties _objBE = new ESDBEProperties();
        readonly static LogFile log = new LogFile();
        private static String Messages;
        static System.Windows.Forms.Label lbl;
        public static System.Windows.Forms.RadioButton rdPass;
        public static System.Windows.Forms.RadioButton rdFail;
        public static System.Windows.Forms.Label lblTestName;

        public static int testCaseId, LoginUserId, PcbTypeId ;
        public static string FrameToSend, ResponseToReceive, SerialNo, Pcbtype;
        public static bool showProductSelection = false;

        static void SetTextTest(string status)
        {
            lbl.Text = status;
            lbl.ForeColor = System.Drawing.Color.Red;

            rdFail.Enabled = false;
            rdPass.Enabled = false;
            lblTestName.Enabled = false;           

            TimerStopTest();
            //Save data 
            if (status == "Timeout")
                SaveTestCaseResult(testCaseId, "Timeout", FrameToSend, ResponseToReceive, "Hybrid", "");
            else
                SaveTestCaseResult(testCaseId, "Fail", FrameToSend, ResponseToReceive, "Hybrid", "");
        }

        // Save test case result
        public static void SaveTestCaseResult(int TestCaseID, string status, string FrameToSend, string ResponseToReceive, string TestType, string TestOutputValue)
        {
            try
            {
                _objBE.PropUserID = LoginUserId;
                _objBE.PropSerialNo = SerialNo;
                _objBE.PropPCBTypeID = PcbTypeId;
                _objBE.PropPCBType = Pcbtype;
                _objBE.PropTestCaseID = TestCaseID;
                _objBE.PropTestType = TestType;
                _objBE.PropStatus = status;
                _objBE.PropFrameToSend = FrameToSend;
                _objBE.PropResponseFrame = ResponseToReceive;
                _objBE.PropCreatedBy = LoginUserId;
                _objBE.PropComment = TestOutputValue;
                _objBE.ProductTypeID = GlobalInformation.ProductTypeID;
                string CurrentDateTime = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss tt");
                _objBE.PropCurrentDateTime = CurrentDateTime;
                Messages = _objDal.InsertTestCaseResult(_objBE);
            }
            catch (Exception ex)
            {
                log.CreateLog(ex.Message, ex.ToString(), "USBCommunication.cs");
            }
        }

        public static void StatusTmrNewTest(object state)
        {
            try
            {                
                if (timerCounter>90)
                {
                    lbl.Invoke(tdTest, new object[] { "Timeout" });
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            timerCounter++;
        }

        readonly static TimerCallback tcTest = new TimerCallback(StatusTmrNewTest);
        readonly static Timer tTest = new Timer(tcTest, null, Timeout.Infinite, 1000);
        static int timerCounter = 0;

        public static void TimerStartTest(System.Windows.Forms.Label lblTest)
        {
            timerCounter = 0;
            lbl = lblTest;
            tTest.Change(10, 1000);
        }

        public static void TimerStopTest()
        {
            timerCounter = 0;
            tTest.Change(Timeout.Infinite, Timeout.Infinite);
        }
    }
}
