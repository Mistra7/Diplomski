using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCFContract
{
    public class RegisterData
    {
        private AlarmType alarm;
        private PointType type;
        private ushort address;
        private ushort rawValue;
        private double eguValue;
        private DateTime timestamp;
        private DState state;
        private int pointId;

        public RegisterData() {}

        public ushort RawValue { get => rawValue; set => rawValue = value; }
        public double EguValue { get => eguValue; set => eguValue = value; }
        public DState State { get => state; set => state = value; }
        public ushort Address { get => address; set => address = value; }
        public PointType Type { get => type; set => type = value; }
        public DateTime Timestamp { get => timestamp; set => timestamp = value; }
        public AlarmType Alarm { get => alarm; set => alarm = value; }
        public int PointId { get => pointId; set => pointId = value; }
    }
}
