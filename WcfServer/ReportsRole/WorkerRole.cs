using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using WcfServer;
using System.IO;
using System.Runtime.InteropServices;
using ExcelLibrary.SpreadSheet;
using System.Globalization;

namespace ReportsRole
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);

        public override void Run()
        {
            Trace.TraceInformation("ReportsRole is running");

            try
            {
                this.RunAsync(this.cancellationTokenSource.Token).Wait();
            }
            finally
            {
                this.runCompleteEvent.Set();
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at https://go.microsoft.com/fwlink/?LinkId=166357.

            bool result = base.OnStart();

            Trace.TraceInformation("ReportsRole has been started");

            return result;
        }

        public override void OnStop()
        {
            Trace.TraceInformation("ReportsRole is stopping");

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();

            base.OnStop();

            Trace.TraceInformation("ReportsRole has stopped");
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            AzureQueue queue = new AzureQueue();
            DBHandler db = new linqDBHandler();
            AzureBlob blob = new AzureBlob();
            
            while (!cancellationToken.IsCancellationRequested)
            {
                var message = await queue.deleteMessageAsync();
                if (message == null)
                    continue;

                CultureInfo culture = CultureInfo.CreateSpecificCulture("fr-FR");
                ExcelBuilder builder = new ExcelBuilder();
                string[] splitted = message.Split(',');
                DateTime weekDate = DateTime.Parse(splitted[2] , culture);
                long userId , companyId;
                long.TryParse(splitted[0], out userId);
                long.TryParse(splitted[1], out companyId);
                List<Clock> clocks = db.getClocksOfComapny(userId, weekDate , companyId);
                List<string> columns = new List<string>();
                columns.Add("Company");
                columns.Add("Start Time");
                columns.Add("End Time");

                List<List<string>> rows = new List<List<string>>();
                foreach(Clock shift in clocks)
                {
                    List<string> row = new List<string>();
                    row.Add(shift.Shift.Company.name);
                    row.Add(shift.startTime.ToString());
                    row.Add(shift.endTime.ToString());

                    rows.Add(row);
                }

                FileInfo workBook = builder.write(columns, rows);

                string fileName = userId + "_" + companyId +"_" + weekDate.Ticks + ".xlsx";
                string url = await blob.uploadFileAsync(workBook.Name, fileName);
                workBook.Delete();

                WorkerReport workerReport = new WorkerReport();
                workerReport.workerId = userId;
                workerReport.url = fileName;
                workerReport.date = weekDate;
                workerReport.companyId = companyId;

                db.addWorkerReport(workerReport);

                List<long> usersId = new List<long>();
                usersId.Add(userId);
                await Notifications.Instance.Notify("Report for date : " + weekDate.ToString() + " is available", usersId);

            }
        }
    }
}
