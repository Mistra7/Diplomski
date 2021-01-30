using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WCFContract;

namespace WebdScadaBackend.Models
{
	public class PointItem
    {
		protected PointType type;
		protected ushort address;
		private DateTime timestamp = DateTime.Now;
		private string name = string.Empty;
		private ushort rawValue;
		private double commandedValue;
		protected AlarmType alarm;
		protected int pointId;
		private ushort minValue;
		private ushort maxValue;
		private double? eguValue;
		private DState? state;

		public PointItem() { }

		public PointItem(PointData point)
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
			this.EguValue = point.EguValue;
			this.State = point.State;
		}

		public PointType Type { get => type; set => type = value; }
		public ushort Address { get => address; set => address = value; }
		public DateTime Timestamp { get => timestamp; set => timestamp = value; }
		public string Name { get => name; set => name = value; }
		public ushort RawValue { get => rawValue; set => rawValue = value; }
		public double CommandedValue { get => commandedValue; set => commandedValue = value; }
		public AlarmType Alarm { get => alarm; set => alarm = value; }
		public int PointId { get => pointId; set => pointId = value; }
		public ushort MinValue { get => minValue; set => minValue = value; }
		public ushort MaxValue { get => maxValue; set => maxValue = value; }
		public double? EguValue { get => eguValue; set => eguValue = value; }
		public DState? State { get => state; set => state = value; }
	}
}
