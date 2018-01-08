using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WcfServer
{
    interface DBHandler
    {
        bool register(string username, string password, bool admin);
        bool auth(string username, string password);

        bool addCompany(string companyName, long ownerID, string picUrl, string description);

    }
}
