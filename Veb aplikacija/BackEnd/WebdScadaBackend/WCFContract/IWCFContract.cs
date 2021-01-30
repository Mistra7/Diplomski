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
        RegisterData ReadCommand(int pointId);
        [OperationContract]
        RegisterData WriteCommand(int pointId, ushort value);
        [OperationContract]
        List<RegisterData> DoAcquisiton(List<AcqusitionData> pointId);
    }
}
