using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfServer
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ClockService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ClockService.svc or ClockService.svc.cs at the Solution Explorer and start debugging.
    public class ClockService : IClockService
    {
        DBHandler handler = new linqDBHandler();
        public long enter(long workerId, long shiftId, string startTime)
        {
            Clock entrance = new Clock()
            {
                workerId = workerId,
                shiftId = shiftId,
                startTime = Convert.ToDateTime(startTime)
                
            };
            return handler.clockEnter(entrance);
        }

        public bool exit(long enterId , string endTime)
        {
            return handler.clockExit(enterId, Convert.ToDateTime(endTime));
        }


        public bool update(long enterId, string startTime, string endTime)
        {
            Clock clock = handler.getClock(enterId);
            if (startTime != null)
                clock.startTime = Convert.ToDateTime(startTime);
            if (endTime != null)
                clock.endTime = Convert.ToDateTime(endTime);

            return handler.updateClock(clock);
        }

        public long enterByWorker(long workerId, long companyId, string time)
        {
            DateTime startTime = Convert.ToDateTime(time);
            int dayInTheWeek = (int)startTime.DayOfWeek + 1;
            long roleId = handler.getRoleOfWorker(workerId, companyId);
            long shiftId = handler.getDefaultShiftId(roleId, dayInTheWeek);
            Clock entrance = new Clock()
            {
                workerId = workerId,
                shiftId = shiftId,
                startTime = startTime

            };
            return handler.clockEnter(entrance);
        }
    }
}
