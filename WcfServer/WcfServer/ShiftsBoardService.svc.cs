using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfServer
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ShiftsBoardService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ShiftsBoardService.svc or ShiftsBoardService.svc.cs at the Solution Explorer and start debugging.
    public class ShiftsBoardService : IShiftsBoardService

    {
        DBHandler handler = new linqDBHandler();

        public bool lookForReplace(ShiftsBoard shift)
        {
            shift.wantReplace = true;
            return handler.updateShift(shift);
        }

        public bool replace(ShiftsBoard shift, long workerId)
        {
            if (!shift.wantReplace)
                return false;
            shift.workerId = workerId;
            shift.wantReplace = false;
            return handler.updateShift(shift);
        }

        public bool setShift(long shiftId, int week, int year, long workerId)
        {
            ShiftsBoard shift = new ShiftsBoard()
            {
                shiftId = shiftId,
                week = week,
                year = year,
                workerId = workerId
            };

            return handler.setShift(shift);
        }

        public bool updateShift(long shiftId, int week, int year, long workerId)
        {
            
            ShiftsBoard shift = new ShiftsBoard()
            {
                shiftId = shiftId,
                week = week,
                year = year,
                workerId = workerId
            };

            return handler.updateShift(shift);
        }
    }
}
