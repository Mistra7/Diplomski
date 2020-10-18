using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebdScadaBackend.Models
{
	internal abstract class BasePointItem : IPoint
    {
		protected PointType type;
		protected ushort address;
		private DateTime timestamp = DateTime.Now;
		private string name = string.Empty;
		private ushort rawValue;
		private double commandedValue;
		protected AlarmType alarm;
		protected IConfigItem configItem;
		protected int pointId;

		public BasePointItem(IConfigItem c, int i)
		{
			this.configItem = c;

			this.type = c.RegistryType;
			this.address = (ushort)(c.StartAddress + i);
			this.name = $"{configItem.Description} [{i}]";
			this.rawValue = configItem.DefaultValue;
			this.pointId = PointIdentifierHelper.GetNewPointId(new PointIdentifier(this.type, this.address));
		}

		public PointType Type { get => type; set => type = value; }
		public ushort Address { get => address; set => address = value; }
		public DateTime Timestamp { get => timestamp; set => timestamp = value; }
		public string Name { get => name; set => name = value; }
		public ushort RawValue { get => rawValue; set => rawValue = value; }
		public double CommandedValue { get => commandedValue; set => commandedValue = value; }
		public AlarmType Alarm { get => alarm; set => alarm = value; }
		public IConfigItem ConfigItem { get => configItem; set => configItem = value; }
		public int PointId { get => pointId; set => pointId = value; }
	}
}
