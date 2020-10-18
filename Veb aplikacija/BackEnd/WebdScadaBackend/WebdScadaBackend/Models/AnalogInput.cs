using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebdScadaBackend.Models
{
    internal class AnalogInput : AnalogBase
    {
        public AnalogInput(IConfigItem c, int i) : base(c, i) { }
    }
}
