using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WCFContract;

namespace WebdScadaBackend.Models
{
    internal abstract class AnalogBase : BasePointItem
    {
        private double eguValue;

        public AnalogBase(PointData point) : base(point) 
        {
            this.EguValue = point.EguValue;
        }

        public AnalogBase() { }

        public double EguValue { get => eguValue; set => eguValue = value; }
    }
}
