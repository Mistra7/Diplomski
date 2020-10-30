using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WCFContract;

namespace WebdScadaBackend.Models
{
    internal abstract class DigitalBase : BasePointItem
    {
        private DState state;

        public DigitalBase(PointData pointData) : base(pointData) 
        {
            this.State = pointData.State;
        }
        public DigitalBase() { }

        public DState State { get => state; set => state = value; }
    }
}
