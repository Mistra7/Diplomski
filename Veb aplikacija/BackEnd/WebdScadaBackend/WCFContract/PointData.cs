using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WCFContract
{
    [DataContract]
    public class PointData
    {
        [DataMember]
        private int pointId;
        [DataMember]
        private ushort rawValue;
        [DataMember]
        private AlarmType alarm;
        [DataMember]
        private PointType type;
        [DataMember]
        private ushort address;
        [DataMember]
        private string name;
        [DataMember]
        private double commandedValue;
        [DataMember]
        private DateTime timestamp;
        [DataMember]
        private DState state;
        [DataMember]
        private double eguValue;
        [DataMember]
        private ushort minValue;
        [DataMember]
        private ushort maxValue;

        public PointData() { }

        public int PointId { get => pointId; set => pointId = value; }
        public ushort RawValue { get => rawValue; set => rawValue = value; }
        public AlarmType Alarm { get => alarm; set => alarm = value; }
        public PointType Type { get => type; set => type = value; }
        public ushort Address { get => address; set => address = value; }
        public string Name { get => name; set => name = value; }
        public double CommandedValue { get => commandedValue; set => commandedValue = value; }
        public DateTime Timestamp { get => timestamp; set => timestamp = value; }
        public DState State { get => state; set => state = value; }
        public double EguValue { get => eguValue; set => eguValue = value; }
        public ushort MinValue { get => minValue; set => minValue = value; }
        public ushort MaxValue { get => maxValue; set => maxValue = value; }
    }
}
