using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebdScadaBackend.Models
{
    public class ConfigItem : IConfigItem
    {
		private PointType registryType;
		private ushort numberOfRegisters;
		private ushort startAddress;
		private ushort decimalSeparatorPlace;
		private ushort minValue;
		private ushort maxValue;
		private ushort defaultValue;
		private string processingType;
		private string description;
		private int acquisitionInterval;
		private double scalingFactor;
		private double deviation;
		private double egu_max;
		private double egu_min;
		private ushort abnormalValue;
		private double highLimit;
		private double lowLimit;
		private int secondsPassedSinceLastPoll;

		public PointType RegistryType { get => registryType; set => registryType = value; }
		public ushort NumberOfRegisters { get => numberOfRegisters; set => numberOfRegisters = value; }
		public ushort StartAddress { get => startAddress; set => startAddress = value; }
		public ushort DecimalSeparatorPlace { get => decimalSeparatorPlace; set => decimalSeparatorPlace = value; }
		public ushort MinValue { get => minValue; set => minValue = value; }
		public ushort MaxValue { get => maxValue; set => maxValue = value; }
		public ushort DefaultValue { get => defaultValue; set => defaultValue = value; }
		public string ProcessingType { get => processingType; set => processingType = value; }
		public string Description { get => description; set => description = value; }
		public int AcquisitionInterval { get => acquisitionInterval; set => acquisitionInterval = value; }
		public double ScaleFactor { get => scalingFactor; set => scalingFactor = value; }
		public double Deviation { get => deviation; set => deviation = value; }
		public double EGU_Max { get => egu_max; set => egu_max = value; }
		public double EGU_Min { get => egu_min; set => egu_min = value; }
		public ushort AbnormalValue { get => abnormalValue; set => abnormalValue = value; }
		public double HighLimit { get => highLimit; set => highLimit = value; }
		public double LowLimit { get => lowLimit; set => lowLimit = value; }
		public int SecondsPassedSinceLastPoll { get => secondsPassedSinceLastPoll; set => secondsPassedSinceLastPoll = value; }
	}
}
