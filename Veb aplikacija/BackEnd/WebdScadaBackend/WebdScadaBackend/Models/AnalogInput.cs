﻿using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WCFContract;

namespace WebdScadaBackend.Models
{
    internal class AnalogInput : AnalogBase
    {
        public AnalogInput(PointData point) : base(point) { }

        public AnalogInput() { }
    }
}
