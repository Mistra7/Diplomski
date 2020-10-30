using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WCFContract;

namespace WebdScadaBackend.Models
{
    internal class DigitalInput : DigitalBase
    {
        public DigitalInput(PointData point) : base(point) { }
        public DigitalInput() { }
    }
}
