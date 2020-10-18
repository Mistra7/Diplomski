using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebdScadaBackend.Models
{
    internal class DigitalOutput : DigitalBase
    {
        public DigitalOutput(IConfigItem c, int i) : base(c, i) { }
    }
}
