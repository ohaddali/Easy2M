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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IAuthService
    {
        

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json , BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        bool register(string userName, string password , bool admin);

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        UserClient login(string userName , string password);

    }

}
