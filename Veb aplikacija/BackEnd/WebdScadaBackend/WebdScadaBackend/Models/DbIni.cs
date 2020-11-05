using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Common;

namespace WebdScadaBackend.Models
{
    public static class DbIni
    {
        public static void Initialize(PointContext pointContext)
        {
            pointContext.Database.EnsureCreated();
        }
    }
}
