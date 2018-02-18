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
using ReportsRole;
using Microsoft.Office.Interop.Excel;
using System.IO;
using Quartz;
using Quartz.Impl;

namespace ScheduledReportsRole
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);

        public override void Run()
        {
            Trace.TraceInformation("ScheduledReportsRole is running");

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
            ConfigureScheduler();
            Trace.TraceInformation("ScheduledReportsRole has been started");

            return result;
        }

        public override void OnStop()
        {
            Trace.TraceInformation("ScheduledReportsRole is stopping");

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();

            sched.Shutdown(false);
            sched = null;

            base.OnStop();

            Trace.TraceInformation("ScheduledReportsRole has stopped");
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                Trace.TraceInformation("Working");
                await Task.Delay(1000);
            }

        }

        IScheduler sched;

        private void ConfigureScheduler()
        {
            var schedFact = new StdSchedulerFactory();

            sched = schedFact.GetScheduler().Result;
            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Israel Standard Time");

            var weeklyJob = new JobDetailImpl("WeeklyJob", null, typeof(WeeklyJob));
            var weeklyCronScheduleBuilder = CronScheduleBuilder.WeeklyOnDayAndHourAndMinute(DayOfWeek.Sunday, 0, 0)
                                                          .InTimeZone(timeZoneInfo);
            var weeklyTrigger = TriggerBuilder.Create()
                                        .StartNow()
                                        .WithSchedule(weeklyCronScheduleBuilder)
                                        .Build();

            sched.ScheduleJob(weeklyJob, weeklyTrigger);

            var monthlyJob = new JobDetailImpl("MonthlyJob", null, typeof(MonthlyJob));
            var monthlyCronScheduleBuilder = CronScheduleBuilder.MonthlyOnDayAndHourAndMinute(1, 0, 0)
                                                          .InTimeZone(timeZoneInfo);
            var monthlyTrigger = TriggerBuilder.Create()
                                        .StartNow()
                                        .WithSchedule(monthlyCronScheduleBuilder)
                                        .Build();

            sched.ScheduleJob(monthlyJob, monthlyTrigger);
            sched.Start();
        }

        
    }
}
