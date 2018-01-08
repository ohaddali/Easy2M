using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WcfServer
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IAuthService
    {

        [OperationContract]
        [WebInvoke(Method = "GET" , UriTemplate = "register/{userName}/{password}/{admin}") ]
        bool register(string userName, string password , string admin);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "login/{userName}/{password}")]
        bool login(string userName , string password);

        // TODO: Add your service operations here
    }

}
