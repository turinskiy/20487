using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueYonder.Entities.Enums;

namespace BlueYonder.Entities
{
    public class Trip
    {
        public int TripId { get; set; }

        public virtual int FlightScheduleID { get; set; }

        [ForeignKey("FlightScheduleID")]
        public FlightSchedule FlightInfo { get; set; }

        public FlightStatus Status { get; set; }

        public SeatClass Class { get; set; }
    }
}
