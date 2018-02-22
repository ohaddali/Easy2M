using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WcfServer
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IShiftService" in both code and config file together.
    [ServiceContract]
    public interface IShiftService
    {
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json , ResponseFormat = WebMessageFormat.Json , BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        bool addShift(Shift newShift);

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json , ResponseFormat = WebMessageFormat.Json , BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        bool deleteShift(int shiftId);

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json , ResponseFormat = WebMessageFormat.Json , BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        bool updateShift(Shift updatedShift);
    }
}
