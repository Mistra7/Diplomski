using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCFContract
{
    public class RegisterData
    {
        private PointType type;
        private ushort address;
        private ushort rawValue;
        private double eguValue;
        private DState state;

        public RegisterData() {}

        public ushort RawValue { get => rawValue; set => rawValue = value; }
        public double EguValue { get => eguValue; set => eguValue = value; }
        public DState State { get => state; set => state = value; }
        public ushort Address { get => address; set => address = value; }
        public PointType Type { get => type; set => type = value; }
    }
}
