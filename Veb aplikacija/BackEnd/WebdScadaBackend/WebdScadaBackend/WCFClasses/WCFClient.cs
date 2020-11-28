
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using WCFContract;

namespace WebdScadaBackend.WCFClasses
{
    public class WCFClient : ChannelFactory<IWCFContract>, IWCFContract, IDisposable
    {
        private static IWCFContract factory;
        public WCFClient(NetTcpBinding binding, EndpointAddress address) : base(binding, address)
        {
            factory = this.CreateChannel();
        }

        public List<RegisterData> DoAcquisiton(List<PointIdentifier> points)
        {
            try
            {
                return factory.DoAcquisiton(points);
            }
            catch
            {
                return null;
            }
        }

        public List<ConfigItemData> GetConfigItems()
        {
            try
            {
                return factory.GetConfigItems();
            }
            catch
            {
                return null;
            }
        }

        public List<PointData> GetPoints()
        {
            try
            {
                return factory.GetPoints();
            }
            catch
            {
                return null;
            }
        }

        public RegisterData ReadCommand(PointIdentifier point)
        {
            try
            {
                return factory.ReadCommand(point);
            }
            catch
            {
                return null;
            }
        }

        public RegisterData WriteCommand(PointIdentifier point, ushort value)
        {
            try
            {
                return factory.WriteCommand(point, value);
            }
            catch
            {
                return null;
            }
        }
    }
}
