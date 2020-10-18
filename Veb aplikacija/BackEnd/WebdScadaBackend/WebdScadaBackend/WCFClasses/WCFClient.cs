
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

        public List<IPoint> Connect()
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

        public IPoint ReadCommand(IPoint point)
        {
            throw new NotImplementedException();
        }

        public IPoint WriteCommand(IPoint point, short value)
        {
            throw new NotImplementedException();
        }
    }
}
