using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfServer
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ShiftRequestService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ShiftRequestService.svc or ShiftRequestService.svc.cs at the Solution Explorer and start debugging.
    public class ShiftRequestService : IShiftRequestService
    {
        DBHandler handler = new linqDBHandler();

        public bool cancelRequest(ShiftRequest request)
        {
            return handler.cancelShiftRequest(request);
        }

        public bool requestShift(int week, int year, long workerId)
        {
            ShiftRequest shiftRequest = new ShiftRequest()
            {
                week = week,
                year = year,
                workerId = workerId
            };

            return handler.requestShift(shiftRequest);
        }
    }
}
