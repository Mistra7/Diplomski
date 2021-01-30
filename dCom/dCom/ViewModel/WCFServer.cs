using Common;
using dCom.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private static AutoResetEvent automationTrigger;

        public WCFServer() { }

        public WCFServer(IStorage stor, IProcessingManager manager, IConfiguration config, AutoResetEvent automationTri)
        {
            storage = stor;
            processingManager = manager;
            configuration = config;
            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:9999/Server";
            host = new ServiceHost(typeof(WCFServer));
            host.AddServiceEndpoint(typeof(IWCFContract), binding, address);
            automationTrigger = automationTri;

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
                                                Type = (WCFContract.PointType)basePoint.Type, Alarm = (WCFContract.AlarmType)basePoint.Alarm, CommandedValue = basePoint.CommandedValue,
                                                Name = basePoint.Name, Timestamp = basePoint.Timestamp};
                if(basePoint.Type == Common.PointType.ANALOG_INPUT || basePoint.Type == Common.PointType.ANALOG_OUTPUT)
                {
                    newPoint.EguValue = ((AnalogBase)basePoint).EguValue;
                    newPoint.MinValue = basePoint.ConfigItem.MinValue;
                    newPoint.MaxValue = basePoint.ConfigItem.MaxValue;
                }
                else
                {
                    newPoint.State = (WCFContract.DState)((DigitalBase)basePoint).State;
                }

                points.Add(newPoint);
            }

            return points;
        }

        public RegisterData ReadCommand(int point)
        {
            var pointToRead = storage.GetPoint(point) as BasePointItem;
            Transaction trans = null;

            try
            {
                ushort tranId = configuration.GetTransactionId();
                processingManager.Transactions.Add(trans = new Transaction(tranId, pointToRead.Address, false));

                processingManager.ExecuteReadCommand(pointToRead.ConfigItem, tranId , configuration.UnitAddress, pointToRead.Address, 1);

                while(!(trans = processingManager.Transactions.Find(t => t.TransactionId == tranId)).Finished)
                    Thread.Sleep(10);

                processingManager.Transactions.Remove(trans);

                pointToRead = storage.GetPoint(point) as BasePointItem;

                var returnValue = new RegisterData()
                {
                    PointId = pointToRead.PointId,
                    Alarm = (WCFContract.AlarmType)pointToRead.Alarm,
                    Address = pointToRead.Address,
                    Timestamp = pointToRead.Timestamp,
                    Type = (WCFContract.PointType)pointToRead.ConfigItem.RegistryType,
                    RawValue = pointToRead.RawValue,
                    EguValue = pointToRead.ConfigItem.RegistryType == Common.PointType.ANALOG_INPUT || pointToRead.ConfigItem.RegistryType == Common.PointType.ANALOG_OUTPUT ? ((AnalogBase)pointToRead).EguValue : 0,
                    State = pointToRead.ConfigItem.RegistryType == Common.PointType.DIGITAL_INPUT || pointToRead.ConfigItem.RegistryType == Common.PointType.DIGITAL_OUTPUT ? (WCFContract.DState)((DigitalBase)pointToRead).State : 0 
                };

                return returnValue;
            }
            catch
            {
                processingManager.Transactions.Remove(trans);
                return null;
            }
        }

        public RegisterData WriteCommand(int point, ushort value)
        {
            var pointToWrite = storage.GetPoint(point) as BasePointItem;
            Transaction trans = null;
            try
            {
                ushort tranId = configuration.GetTransactionId();
                processingManager.Transactions.Add(trans = new Transaction(tranId, pointToWrite.Address, false));
                processingManager.ExecuteWriteCommand(pointToWrite.ConfigItem, tranId, configuration.UnitAddress, pointToWrite.Address, (int)value);

                while (!(trans = processingManager.Transactions.Find(t => t.TransactionId == tranId)).Finished)
                    Thread.Sleep(10);

                processingManager.Transactions.Remove(trans);

                pointToWrite = storage.GetPoint(point) as BasePointItem;
                var returnValue = new RegisterData()
                {
                    PointId = pointToWrite.PointId,
                    Alarm = (WCFContract.AlarmType)pointToWrite.Alarm,
                    Timestamp = pointToWrite.Timestamp,
                    Type = (WCFContract.PointType)pointToWrite.ConfigItem.RegistryType,
                    RawValue = pointToWrite.RawValue,
                    EguValue = pointToWrite.ConfigItem.RegistryType == Common.PointType.ANALOG_INPUT || pointToWrite.ConfigItem.RegistryType == Common.PointType.ANALOG_OUTPUT ? ((AnalogBase)pointToWrite).EguValue : 0,
                    State = pointToWrite.ConfigItem.RegistryType == Common.PointType.DIGITAL_INPUT || pointToWrite.ConfigItem.RegistryType == Common.PointType.DIGITAL_OUTPUT ? (WCFContract.DState)((DigitalBase)pointToWrite).State : 0
                };
                return returnValue;
            }
            catch
            {
                processingManager.Transactions.Remove(trans);
                return null;
            }
        }

        public List<RegisterData> DoAcquisiton(List<AcqusitionData> pointIds)
        {
            try
            {
                var configItems = configuration.GetConfigurationItems();
                var readPointIdentifiers = new List<PointIdentifier>();
                var checkData = new Dictionary<ushort, ushort>();
                var transIds = new List<ushort>();
                var points = new List<PointIdentifier>();
                pointIds.ForEach(p => points.Add(new PointIdentifier((Common.PointType)p.PointType, p.StartAddress)));
                foreach (PointIdentifier point in points)
                {
                    var transId = configuration.GetTransactionId();
                    //transIds.Add(transId);
                    var configItem = configItems.Find(ci => ci.StartAddress == point.Address && ci.RegistryType == point.PointType);
                    for (int i = 0; i < configItem.NumberOfRegisters; i++)
                    {
                        checkData.Add((ushort)(configItem.StartAddress + i), transId);
                        processingManager.Transactions.Add(new Transaction(transId, (ushort)(configItem.StartAddress + i), false));
                        readPointIdentifiers.Add(new PointIdentifier(configItem.RegistryType, (ushort)(configItem.StartAddress + i)));
                    }
                    processingManager.ExecuteReadCommand(configItem, transId, configuration.UnitAddress, point.Address, configItem.NumberOfRegisters);
                }

                var counter = 0;

                while(true)
                {
                    readPointIdentifiers.ForEach(t =>
                    {
                        if (CheckIfTransactionFinished(checkData[t.Address], t.Address))
                            ++counter;
                    });

                    if (counter == checkData.Count)
                        break;
                    else
                        automationTrigger.WaitOne(10);
                }

                var values = storage.GetPoints(readPointIdentifiers).Cast<BasePointItem>().ToList();
                var returnValue = new List<RegisterData>();

                values.ForEach(p =>
                {
                    returnValue.Add(new RegisterData()
                    {
                        PointId = p.PointId,
                        Alarm = (WCFContract.AlarmType)p.Alarm,
                        Timestamp = p.Timestamp,
                        Address = p.Address,
                        RawValue = p.RawValue,
                        Type = (WCFContract.PointType)p.Type,
                        EguValue = p.Type == Common.PointType.ANALOG_INPUT || p.Type == Common.PointType.ANALOG_OUTPUT ? ((AnalogBase)p).EguValue : 0,
                        State = p.Type == Common.PointType.DIGITAL_INPUT || p.Type == Common.PointType.DIGITAL_OUTPUT ? (WCFContract.DState)((DigitalBase)p).State : 0
                    });
                });

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
                    RegistryType = (WCFContract.PointType)ci.RegistryType,
                    StartAddress = ci.StartAddress
                }));

                return returnValue;
            }
            catch
            {
                return null;
            }

        }

        private bool CheckIfTransactionFinished(ushort transId, ushort address)
        {
            Transaction t = null;
            if ((t = processingManager.Transactions.Find(tr => transId == tr.TransactionId && tr.Address == address)) == null)
                return false;

            if (t.Finished)
                processingManager.Transactions.Remove(t);

            return t.Finished;
        }
    }
}
