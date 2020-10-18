using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebdScadaBackend.Models
{
    internal abstract class DigitalBase : BasePointItem, IDigitalPoint
    {
        private DState state;

        public DigitalBase(IConfigItem c, int i) : base(c, i) { }

        public DState State { get => state; set => state = value; }
    }
}
