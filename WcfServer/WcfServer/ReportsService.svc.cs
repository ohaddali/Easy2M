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

        public bool exportWeeklyReportForWorker(long userId , string dateStr)
        {
            DateTime date = Convert.ToDateTime(dateStr);
            AzureQueue queue = new AzureQueue();
            string message = userId + "," + date.ToString();
            queue.sendMessage(message);

            return true;
        }

        public ClientWorkerReport[] GetAdminReports(long companyId)
        {
            Report[] reports = handler.getReportsOfAdmin(companyId);

            ClientWorkerReport[] clientReports = new ClientWorkerReport[reports.Length];

            for (int index = 0; index < reports.Length; index++)
            {
                Report report = reports[index];
                ClientWorkerReport clientReport = new ClientWorkerReport()
                {
                    reportId = report.reportId,
                    relatedId = companyId,
                    date = report.date.ToString(),
                    workerReport = false
                };

                clientReports[index] = clientReport;

            }

            return clientReports;
        }

        public string getReportUrlByDate(long companyId, string dateStr)
        {
            DateTime date = Convert.ToDateTime(dateStr);
            return handler.getReportByDate(companyId, date).url;
        }

        public ClientWorkerReport[] GetWorkerReports(long workerId)
        {
            WorkerReport [] reports = handler.getReportsOfWorker(workerId);

            ClientWorkerReport[] clientReports = new ClientWorkerReport[reports.Length];

            for(int index = 0; index < reports.Length; index++)
            {
                WorkerReport report = reports[index];
                ClientWorkerReport clientReport = new ClientWorkerReport()
                {
                    reportId = report.reportId,
                    relatedId = workerId,
                    date = report.date.ToString(),
                    workerReport = true
                };

                clientReports[index] = clientReport;

            }

            return clientReports;
        }

        public string getWorkerReportUrlByDate(long userId, string dateStr)
        {
            DateTime date = Convert.ToDateTime(dateStr);
            return handler.getWorkerReportByDate(userId, date).url;
        }
    }
}
