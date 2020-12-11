using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelManagementSystem.Models
{
    //Booking Status Class
    public class BookingStatus
    {
        //PK
        public int BookingStatusId { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }

        //Bookings associated with the Booking Status
        public ICollection<Booking> Bookings { get; set; }
    }
}
