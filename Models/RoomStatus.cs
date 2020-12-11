using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelManagementSystem.Models
{
    //Room Status Class
    public class RoomStatus
    {
        //PK
        public int RoomStatusId { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }

        //Rooms associated with the Room Status Class
        public ICollection<Room> Rooms { get; set; }
    }
}
