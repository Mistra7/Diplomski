using Common;
using dCom.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WCFContract;

namespace dCom.ViewModel
{
    public class WCFServer : IWCFContract
    {
        private static IStorage storage;
        private static IProcessingManager processingManager;
        private static IConfiguration configuration;
        private static ServiceHost host;

        public WCFServer() { }

        public WCFServer(IStorage stor, IProcessingManager manager, IConfiguration config)
        {
            storage = stor;
            processingManager = manager;
            configuration = config;
            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:9999/Server";
            host = new ServiceHost(typeof(WCFServer));
            host.AddServiceEndpoint(typeof(IWCFContract), binding, address);

            host.Open();
        }

        public List<PointData> GetPoints()
        {
            var ipoints = storage.GetAllPoints();
            var points = new List<PointData>();

            foreach (IPoint point in ipoints)
            {
                var basePoint = (BasePointItem)point;
                var newPoint = new PointData() { PointId = point.PointId, RawValue = point.RawValue, Address = basePoint.Address,
                                                Type = basePoint.Type, Alarm = basePoint.Alarm, CommandedValue = basePoint.CommandedValue,
                                                Name = basePoint.Name, Timestamp = basePoint.Timestamp};
                if(basePoint.Type == PointType.ANALOG_INPUT || basePoint.Type == PointType.ANALOG_OUTPUT)
                {
                    newPoint.EguValue = ((AnalogBase)basePoint).EguValue;
                    newPoint.MinValue = basePoint.ConfigItem.MinValue;
                    newPoint.MaxValue = basePoint.ConfigItem.MaxValue;
                }
                else
                {
                    newPoint.State = ((DigitalBase)basePoint).State;
                }

                points.Add(newPoint);
            }

            return points;
        }

        public RegisterData ReadCommand(PointIdentifier point)
        {
            var pointToRead = storage.GetPoint(point);

            try
            {
                processingManager.ExecuteReadCommand(pointToRead.ConfigItem, configuration.GetTransactionId(), configuration.UnitAddress, point.Address, 1);
                Thread.Sleep(100);
                pointToRead = storage.GetPoint(point);
                var returnValue = new RegisterData()
                {
                    Type = pointToRead.ConfigItem.RegistryType,
                    RawValue = pointToRead.RawValue,
                    EguValue = pointToRead.ConfigItem.RegistryType == PointType.ANALOG_INPUT || pointToRead.ConfigItem.RegistryType == PointType.ANALOG_OUTPUT ? ((AnalogBase)pointToRead).EguValue : 0,
                    State = pointToRead.ConfigItem.RegistryType == PointType.DIGITAL_INPUT || pointToRead.ConfigItem.RegistryType == PointType.DIGITAL_OUTPUT ? ((DigitalBase)pointToRead).State : 0 
                };

                return returnValue;
            }
            catch(Exception e)
            {
                return null;
            }
        }

        public RegisterData WriteCommand(PointIdentifier point, ushort value)
        {
            var pointToWrite = storage.GetPoint(point);

            try
            {
                processingManager.ExecuteWriteCommand(pointToWrite.ConfigItem, configuration.GetTransactionId(), configuration.UnitAddress, point.Address, (int)value);
                Thread.Sleep(100);
                pointToWrite = storage.GetPoint(point);
                var returnValue = new RegisterData()
                {
                    Type = pointToWrite.ConfigItem.RegistryType,
                    RawValue = pointToWrite.RawValue,
                    EguValue = pointToWrite.ConfigItem.RegistryType == PointType.ANALOG_INPUT || pointToWrite.ConfigItem.RegistryType == PointType.ANALOG_OUTPUT ? ((AnalogBase)pointToWrite).EguValue : 0,
                    State = pointToWrite.ConfigItem.RegistryType == PointType.DIGITAL_INPUT || pointToWrite.ConfigItem.RegistryType == PointType.DIGITAL_OUTPUT ? ((DigitalBase)pointToWrite).State : 0
                };

                return returnValue;
            }
            catch
            {
                return null;
            }
        }

        public void CloseConnection()
        {
            host.Close();
        }

        public List<ConfigItemData> GetConfigItems()
        {
            try
            {
                var configItems = configuration.GetConfigurationItems();
                var returnValue = new List<ConfigItemData>();
                configItems.ForEach(ci => returnValue.Add(new ConfigItemData()
                {
                    AcquisitionInterval = ci.AcquisitionInterval,
                    NumberOfRegisters = ci.NumberOfRegisters,
                    RegistryType = ci.RegistryType,
                    StartAddress = ci.StartAddress
                }));

                return returnValue;
            }
            catch
            {
                return null;
            }

        }

        public bool? AcqusitionCommand(List<PointIdentifier> points)
        {
            try
            {
                var configItems = configuration.GetConfigurationItems();
                var readPointIdentifiers = new List<PointIdentifier>();

                foreach (PointIdentifier point in points)
                {
                    var configItem = configItems.Find(ci => ci.StartAddress == point.Address && ci.RegistryType == point.PointType);
                    processingManager.ExecuteReadCommand(configItem, configuration.GetTransactionId(), configuration.UnitAddress, point.Address, configItem.NumberOfRegisters);
                }

                return true;
            }
            catch
            {
                return false;
            } 
        }

        public List<RegisterData> AcqusitionResult(List<PointIdentifier> points)
        {
            try
            {
                var configItems = configuration.GetConfigurationItems();
                var returnValue = new List<RegisterData>();
                foreach (PointIdentifier point in points)
                {
                    var configItem = configItems.Find(ci => ci.StartAddress == point.Address && ci.RegistryType == point.PointType);
                    for (int i = 0; i < configItem.NumberOfRegisters; i++)
                    {
                        var p = storage.GetPoint(new PointIdentifier()
                        { Address = (ushort)(configItem.StartAddress + i), PointType = configItem.RegistryType }) as BasePointItem;

                        returnValue.Add(
                        new RegisterData()
                        {
                            Address = p.Address,
                            RawValue = p.RawValue,
                            Type = p.Type,
                            EguValue = p.Type == PointType.ANALOG_INPUT || p.Type == PointType.ANALOG_OUTPUT ? ((AnalogBase)p).EguValue : 0,
                            State = p.Type == PointType.DIGITAL_INPUT || p.Type == PointType.DIGITAL_OUTPUT ? ((DigitalBase)p).State : 0
                        });
                    }
                }

                return returnValue;
            }
            catch
            {
                return null;
            }
            
        }
    }
}
