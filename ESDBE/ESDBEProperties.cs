using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESDBE
{
    public class ESDBEProperties
    {
        
        public int PropUserID
        { get; set; }

        public string PropSerialNo
        { get; set; }

        public int PropPCBTypeID
        { get; set; }

        public string PropPCBType
        { get; set; }

        public int PropTestCaseID
        { get; set; }

        public string PropStatus
        { get; set; }

        public string PropFrameToSend
        { get; set; }

        public string PropResponseFrame
        { get; set; }

        public int PropCreatedBy
        { get; set; }

        public string PropIsActive
        { get; set; }
        public string PropCurrentDateTime
        { get; set; }
        public DateTime PropCreatedDate
        { get; set; }
        public string PropComment
        { get; set; }
        public string PropTestType
        { get; set; }
        public int ProductTypeID
        { get; set; }
    }
}
