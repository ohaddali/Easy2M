using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using WcfServer.ClientModels;

namespace WcfServer
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IcompaniesService" in both code and config file together.
    [ServiceContract]
    public interface IcompaniesService
    {
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json , ResponseFormat = WebMessageFormat.Json , BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Company addCompany(Company newCompany);

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json , ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        bool updateCompany(Company updatedCompany);

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json , ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        bool deleteCompany(long id);

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json , BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        bool addWorkerToCompany(long workerId , long comapnyId , long roleId);

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json , BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        CompanyClient[] getWorkerCompanies(long workerId);

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        string NotifyWorkerToJoinCompany(string userPhone , long companyId , long roleId);

    }

}
