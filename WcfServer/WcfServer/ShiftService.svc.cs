using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfServer
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ShiftService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ShiftService.svc or ShiftService.svc.cs at the Solution Explorer and start debugging.
    public class ShiftService : IShiftService
    {
        DBHandler handler = new linqDBHandler();
        public bool addShift(Shift newShift)
        {
            return handler.addShift(newShift);
        }

        public bool deleteShift(int shiftId)
        {
            return handler.deleteShift(shiftId);
        }

        public bool updateShift(Shift updatedShift)
        {
            return handler.updateShift(updatedShift);
        }
    }
}
