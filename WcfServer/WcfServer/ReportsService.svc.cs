using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfServer
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ReportsService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ReportsService.svc or ReportsService.svc.cs at the Solution Explorer and start debugging.
    public class ReportsService : IReportsService
    {
        DBHandler handler = new linqDBHandler();

        public void exportWeeklyReportForWorker(long userId , DateTime date)
        {
            AzureQueue queue = new AzureQueue();
            string message = userId + "," + date.ToString();
            queue.sendMessage(message);
        }

        public string getReportUrlByDate(long companyId, DateTime date)
        {
            return handler.getReportByDate(companyId, date).url;
        }

        public string getWorkerReportUrlByDate(long userId, DateTime date)
        {
            return handler.getWorkerReportByDate(userId, date).url;
        }
    }
}
