using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelManagementSystem.Models
{
    //Room Type Class
    public class RoomType
    {
        //PK
        public int RoomTypeId { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }

        //Rooms associated with the Room Type Class
        public ICollection<Room> Rooms { get; set; }
    }
}
