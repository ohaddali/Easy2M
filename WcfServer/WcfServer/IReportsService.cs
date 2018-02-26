using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WcfServer
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IReportsService" in both code and config file together.
    [ServiceContract]
    public interface IReportsService
    {
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json , BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        string getReportUrlByDate(long companyId, string date);

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json , BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        string getWorkerReportUrlByDate(long userId, string date);

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json , BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        bool exportWeeklyReportForWorker(long userId, long companyId , string date);

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        ClientWorkerReport[] getWorkerReports(long workerId, long companyId);

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        ClientWorkerReport[] getAdminReports(long companyId);
    }
}
