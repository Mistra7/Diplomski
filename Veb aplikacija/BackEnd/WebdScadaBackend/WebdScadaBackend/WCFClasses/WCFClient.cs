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

        public List<RegisterData> DoAcquisiton(List<AcqusitionData> pointIds)
        {
            try
            {
                return factory.DoAcquisiton(pointIds);
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

        public RegisterData ReadCommand(int pointId)
        {
            try
            {
                return factory.ReadCommand(pointId);
            }
            catch
            {
                return null;
            }
        }

        public RegisterData WriteCommand(int pointId, ushort value)
        {
            try
            {
                return factory.WriteCommand(pointId, value);
            }
            catch
            {
                return null;
            }
        }
    }
}
