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
using Microsoft.Office.Interop.Excel;
using System.IO;
using System.Runtime.InteropServices;

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
                ExcelBuilder builder = new ExcelBuilder();
                var message = await queue.deleteMessageAsync();
                if (message == null)
                    continue;
                string[] splitted = message.Split(',');
                DateTime weekDate = DateTime.Parse(splitted[1]);
                long userId;
                long.TryParse(splitted[0], out userId);
                List<Clock> clocks = db.getClocks(userId, weekDate);
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

                object misValue = System.Reflection.Missing.Value;
                Workbook workBook = builder.write(misValue,columns, rows);
                workBook.SaveAs("temp.xlsx");

                workBook.Close(true, misValue, misValue);
                builder.quit();

                Marshal.ReleaseComObject(workBook);
                Marshal.ReleaseComObject(builder.getApp());

                string path = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string fileName = userId + "_" + weekDate.Ticks + ".xlsx";
                string url = await blob.uploadFileAsync(path+"/temp.xlsx", fileName);
                File.Delete(path+"/temp.xlsx");

                WorkerReport workerReport = new WorkerReport();
                workerReport.workerId = userId;
                workerReport.url = fileName;
                workerReport.date = weekDate;

                db.addWorkerReport(workerReport);

                List<long> usersId = new List<long>();
                usersId.Add(userId);
                await Notifications.Instance.Notify("Report for date : " + weekDate.ToString() + " is available", usersId);

            }
        }
    }
}
