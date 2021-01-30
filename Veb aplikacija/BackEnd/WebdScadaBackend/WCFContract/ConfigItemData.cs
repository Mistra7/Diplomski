using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCFContract
{
    public class ConfigItemData
    {
        private PointType registryType;
        private ushort numberOfRegisters;
        private ushort startAddress;
        private int acquisitionInterval;


        public ConfigItemData() { }

        public PointType RegistryType { get => registryType; set => registryType = value; }
        public ushort NumberOfRegisters { get => numberOfRegisters; set => numberOfRegisters = value; }
        public ushort StartAddress { get => startAddress; set => startAddress = value; }
        public int AcquisitionInterval { get => acquisitionInterval; set => acquisitionInterval = value; }
    }
}
