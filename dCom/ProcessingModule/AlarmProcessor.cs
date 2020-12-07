using Common;

namespace ProcessingModule
{
    /// <summary>
    /// Class containing logic for alarm processing.
    /// </summary>
    public class AlarmProcessor
	{
        /// <summary>
        /// Processes the alarm for analog point.
        /// </summary>
        /// <param name="eguValue">The EGU value of the point.</param>
        /// <param name="configItem">The configuration item.</param>
        /// <returns>The alarm indication.</returns>
		public AlarmType GetAlarmForAnalogPoint(double eguValue, IConfigItem configItem)
		{
			if(configItem.LowLimit != 0 && configItem.HighLimit != 0 && configItem.AbnormalValue != 0)
            {
                if (configItem.AbnormalValue > eguValue)
                    return AlarmType.ABNORMAL_VALUE;
                else if (configItem.HighLimit < eguValue)
                    return AlarmType.HIGH_ALARM;
                else if (configItem.LowLimit > eguValue)
                    return AlarmType.LOW_ALARM;
            }

            return AlarmType.NO_ALARM;
		}

        /// <summary>
        /// Processes the alarm for digital point.
        /// </summary>
        /// <param name="state">The digital point state</param>
        /// <param name="configItem">The configuration item.</param>
        /// <returns>The alarm indication.</returns>
		public AlarmType GetAlarmForDigitalPoint(ushort state, IConfigItem configItem)
		{
            if (state == configItem.AbnormalValue)
                return AlarmType.ABNORMAL_VALUE;
            return AlarmType.NO_ALARM;
        }
	}
}
