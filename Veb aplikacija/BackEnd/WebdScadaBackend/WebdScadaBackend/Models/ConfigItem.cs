using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WCFContract;

namespace WebdScadaBackend.Models
{
    public class ConfigItem : ConfigItemData
    {
        private static int numberOfConfigs = 0;
        private int id;
        private int secondsPassedSinceLastPoll;

        public ConfigItem() : base() 
        {
            id = numberOfConfigs++;
        }

        

        public int Id { get => id; set => id = value; }
        public int SecondsPassedSinceLastPoll { get => secondsPassedSinceLastPoll; set => secondsPassedSinceLastPoll = value; }
    }
}
