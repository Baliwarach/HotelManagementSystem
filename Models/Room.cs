using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelManagementSystem.Models
{
    //Room Model Class
    public class Room
    {
        //PK
        public int RoomId { get; set; }
        public string RoomNumber { get; set; }
        public string Floor { get; set; }
        public int RoomTypeId { get; set; }         //FK Referring to RoomType Class
        public int RoomStatusId { get; set; }       //FK Referring to RoomStatus Class
        public string Description { get; set; }

        //RoomType associated with the Booking
        public RoomType RoomType { get; set; }

        //RoomStatus associated with the Booking
        public RoomStatus RoomStatus { get; set; }

        //Bookings associated with the Room Status
        public ICollection<Booking> Bookings { get; set; }

        //Custom property to show Room Type and Number of Room
        public string RoomDetail
        {
            get
            {
                return this.RoomNumber + "(" + this.RoomType.Type + ")";
            }
        }
    }
}
