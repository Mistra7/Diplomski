using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WCFContract;

namespace WebdScadaBackend.Models
{
	public abstract class BasePointItem
    {
		
		protected PointType type;
		protected ushort address;
		private DateTime timestamp = DateTime.Now;
		private string name = string.Empty;
		private ushort rawValue;
		private double commandedValue;
		protected AlarmType alarm;
		protected int pointId;
		private int dataBaseId;
		private ushort minValue;
		private ushort maxValue;

		public BasePointItem() { }

		public BasePointItem(PointData point)
		{
			this.PointId = point.PointId;
			this.Type = point.Type;
			this.Address = point.Address;
			this.Timestamp = point.Timestamp;
			this.Name = point.Name;
			this.RawValue = point.RawValue;
			this.Alarm = point.Alarm;
			this.CommandedValue = point.CommandedValue;
			this.MinValue = point.MinValue;
			this.MaxValue = point.MaxValue;
		}

		public PointType Type { get => type; set => type = value; }
		public ushort Address { get => address; set => address = value; }
		public DateTime Timestamp { get => timestamp; set => timestamp = value; }
		public string Name { get => name; set => name = value; }
		public ushort RawValue { get => rawValue; set => rawValue = value; }
		public double CommandedValue { get => commandedValue; set => commandedValue = value; }
		public AlarmType Alarm { get => alarm; set => alarm = value; }
		public int PointId { get => pointId; set => pointId = value; }
		[Key]
		public int DataBaseId { get => dataBaseId; set => dataBaseId = value; }
		public ushort MinValue { get => minValue; set => minValue = value; }
		public ushort MaxValue { get => maxValue; set => maxValue = value; }
	}
}
