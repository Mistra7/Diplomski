using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebdScadaBackend.Models
{
    internal class DigitalInput : DigitalBase
    {
        public DigitalInput(IConfigItem c, int i) : base(c, i) { }
    }
}
