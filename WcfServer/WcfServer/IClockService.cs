using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WcfServer
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IClockService" in both code and config file together.
    [ServiceContract]
    public interface IClockService
    {
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json , BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        long enter(long workerId, long shiftId, string time); // (format dd/mm/yyyy hh:mm:ss)

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        long enterByWorker(long workerId, long comapnyId, string time); // (format dd/mm/yyyy hh:mm:ss)

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json , BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        bool exit(long enterId, string endTime);

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json ,  BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        bool update(long enterId, string startTime, string endTime);
    }
}
