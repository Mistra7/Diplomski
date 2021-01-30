using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCFContract
{
    public enum DState : short
    {
        OFF = 0,
        ON = 1
    }

    public enum PointType : short
    {
        NO_TYPE = 0x00,
        DIGITAL_OUTPUT = 0x01,
        DIGITAL_INPUT = 0x02,
        ANALOG_INPUT = 0x03,
        ANALOG_OUTPUT = 0x04,
        HT_LONG = 0x05
    }

    public enum AlarmType : short
    {
        NO_TYPE = 0x00,
        NO_ALARM = 0x01,
        REASONABILITY_FAILURE = 0x02,
        LOW_ALARM = 0x03,
        HIGH_ALARM = 0x04,
        ABNORMAL_VALUE = 0x05,
    }


}
