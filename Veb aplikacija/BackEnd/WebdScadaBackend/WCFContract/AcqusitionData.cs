using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCFContract
{
    public class AcqusitionData
    {
        private PointType pointType;
        private ushort startAddress;
        private ushort numberOfRegisters;

        public AcqusitionData() { }

        public AcqusitionData(PointType pointType, ushort startAddress, ushort numberOfRegisters)
        {
            PointType = pointType;
            StartAddress = startAddress;
            NumberOfRegisters = numberOfRegisters;
        }

        public PointType PointType { get => pointType; set => pointType = value; }
        public ushort StartAddress { get => startAddress; set => startAddress = value; }
        public ushort NumberOfRegisters { get => numberOfRegisters; set => numberOfRegisters = value; }
    }
}
