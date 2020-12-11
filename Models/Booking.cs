using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HotelManagementSystem.Models
{
    //Bookings Model class
    public class Booking
    {
        //PK
        public int BookingId { get; set; }
        [Required]
        public int RoomId { get; set; }             //FK Referring to Room Class
        [Required]
        public int CustomerId { get; set; }         //FK Referring to Customer Class
        [Required]
        public int EmployeeId { get; set; }         //FK Referring to Employee Class
        [Required]
        public int BookingStatusId { get; set; }    //FK Referring to BookingStatus Class
        [Required]
        public DateTime DateFrom { get; set; }
        [Required]
        public DateTime DateTo { get; set; }
        public DateTime? ArrivalTime { get; set; }
        public DateTime? CheckoutTime { get; set; }

        //Room associated with the Booking
        public Room Room { get; set; }

        //Customer associated with the Booking
        public Customer Customer { get; set; }

        //Employee associated with the Booking
        public Employee Employee { get; set; }

        //Booking Status associated with the Booking
        public BookingStatus BookingStatus { get; set; }
    }
}
