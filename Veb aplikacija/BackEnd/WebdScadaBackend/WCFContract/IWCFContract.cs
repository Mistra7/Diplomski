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
        List<PointData> Connect();
        [OperationContract]
        PointData ReadCommand(PointIdentifier point);
        [OperationContract]
        PointData WriteCommand(PointIdentifier point, short value);
        
    }
}
