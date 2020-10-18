using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebdScadaBackend.Models
{
    internal abstract class AnalogBase : BasePointItem, IAnalogPoint
    {
        private double eguValue;

        public AnalogBase(IConfigItem c, int i) : base(c, i) { }

        public double EguValue { get => eguValue; set => eguValue = value; }
    }
}
