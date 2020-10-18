using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WCFContract
{
    [ServiceContract]
    public interface IWCFContract
    {
        [OperationContract]
        List<IPoint> Connect();
        [OperationContract]
        IPoint ReadCommand(IPoint point);
        [OperationContract]
        IPoint WriteCommand(IPoint point, short value);
        
    }
}
