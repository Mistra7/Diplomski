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
        private int dataBaseId;
        private int secondsPassedSinceLastPoll;

        public ConfigItem() : base() { }

        [Key]
        public int DataBaseId { get => dataBaseId; set => dataBaseId = value; }
        public int SecondsPassedSinceLastPoll { get => secondsPassedSinceLastPoll; set => secondsPassedSinceLastPoll = value; }
    }
}
