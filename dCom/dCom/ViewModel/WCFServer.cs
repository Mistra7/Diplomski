using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
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

        public List<PointData> Connect()
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

        public PointData ReadCommand(PointIdentifier point)
        {
            var pointToRead = storage.GetPoints(new List<PointIdentifier>() { point })[0];

            try
            {
                processingManager.ExecuteReadCommand(pointToRead.ConfigItem, configuration.GetTransactionId(), configuration.UnitAddress, point.Address, 1);
            }
            catch(Exception e)
            {
                return null;
            }

            throw new NotImplementedException();
        }

        public PointData WriteCommand(PointIdentifier point, short value)
        {
            throw new NotImplementedException();
        }

        public void CloseConnection()
        {
            host.Close();
        }

    }
}
