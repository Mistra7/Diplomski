
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

        public List<PointData> Connect()
        {
            try
            {
                return factory.Connect();
            }
            catch(Exception e)
            {
                return null;
            }
        }

        public PointData ReadCommand(PointIdentifier point)
        {
            try
            {
                return factory.ReadCommand(point);
            }
            catch(Exception e)
            {
                return null;
            }
        }

        public PointData WriteCommand(PointIdentifier point, short value)
        {
            try
            {
                return factory.WriteCommand(point, value);
            }
            catch(Exception e)
            {
                return null;
            }
        }
    }
}
