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
        List<PointData> GetPoints();
        [OperationContract]
        List<ConfigItemData> GetConfigItems();
        [OperationContract]
        RegisterData ReadCommand(PointIdentifier point);
        [OperationContract]
        RegisterData WriteCommand(PointIdentifier point, ushort value);
        [OperationContract]
        bool? AcqusitionCommand(List<PointIdentifier> points);
        [OperationContract]
        List<RegisterData> AcqusitionResult(List<PointIdentifier> points);

        
    }
}
