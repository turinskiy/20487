using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueYonder.Entities
{
    public class Reservation
    {
        public int ReservationId { get; set; }

        public int TravelerId { get; set; }

        public DateTime ReservationDate { get; set; }

        public string ConfirmationCode { get; set; }

        public int DepartFlightScheduleID { get; set; }

        [ForeignKey("DepartFlightScheduleID")]
        public virtual Trip DepartureFlight { get; set; }

        public int? ReturnFlightScheduleID { get; set; }

        [ForeignKey("ReturnFlightScheduleID")]
        public virtual Trip ReturnFlight { get; set; }
    }
}
