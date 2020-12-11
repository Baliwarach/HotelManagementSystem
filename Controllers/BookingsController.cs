using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HotelManagementSystem.Data;
using HotelManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.SqlClient;

namespace HotelManagementSystem.Controllers
{
    //Booking Handler
    public class BookingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Bookings
        // Get Handler for Listing Bookings
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Bookings.Include(b => b.BookingStatus).Include(b => b.Customer).Include(b => b.Employee).Include(b => b.Room).Include(b=> b.Room.RoomType);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Bookings/Details/5
        // Get Handler for showing Details of a Bookings
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.BookingStatus)
                .Include(b => b.Customer)
                .Include(b => b.Employee)
                .Include(b => b.Room).Include(b=>b.Room.RoomType)
                .FirstOrDefaultAsync(m => m.BookingId == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // GET: Bookings/Create
        // Get Handler for Creating a new Booking
        [Authorize]
        public IActionResult Create()
        {
            ViewData["BookingStatusId"] = new SelectList(_context.BookingStatuses, "BookingStatusId", "Status");
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "FullName");
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "FullName");
            ViewData["RoomId"] = new SelectList(new List<Room>(), "RoomId", "RoomNumber");
            return View();
        }

        // POST: Bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // Post Handler for Creating a new Booking
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookingId,RoomId,CustomerId,EmployeeId,BookingStatusId,DateFrom,DateTo,ArrivalTime,CheckoutTime")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookingStatusId"] = new SelectList(_context.BookingStatuses, "BookingStatusId", "Status", booking.BookingStatusId);
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "FullName", booking.CustomerId);
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "FullName", booking.EmployeeId);

            // if user has provided to and from date, then we need to check for the rooms which are available during that time period
            // only available rooms during the time period specified should be available for booking
            if(booking.DateFrom != DateTime.MinValue && booking.DateTo != DateTime.MinValue &&
                (booking.DateFrom < booking.DateTo))
            {
                var query = (from x in _context.Rooms.Include(x=> x.RoomType)
                             join y in _context.Bookings on
                             new
                             {
                                 Key1 = x.RoomId,
                                 Key2 = true,
                                 Key3 = true
                             }
                             equals
                             new
                             {
                                 Key1 = y.RoomId,
                                 Key2 = y.DateFrom <= booking.DateTo,
                                 Key3 = y.DateTo >= booking.DateFrom
                             }
                             into result
                             from r in result.DefaultIfEmpty()
                             select new { x.RoomId, x.RoomNumber, r.BookingId, x.RoomDetail }
                             ).ToList().Where(x=>x.BookingId==0);

                ViewData["RoomId"] = new SelectList(query, "RoomId", "RoomDetail", booking.RoomId);
            }
            else
            {
                ViewData["RoomId"] = new SelectList(new List<Room>(), "RoomId", "RoomNumber");
            }

            return View(booking);
        }

        // GET: Bookings/Edit/5
        // Get Handler for Editing a Booking
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            ViewData["BookingStatusId"] = new SelectList(_context.BookingStatuses, "BookingStatusId", "Status", booking.BookingStatusId);
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "FullName", booking.CustomerId);
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "FullName", booking.EmployeeId);
            ViewData["RoomId"] = new SelectList(_context.Rooms.Include(x=>x.RoomType), "RoomId", "RoomDetail", booking.RoomId);
            return View(booking);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // Post Handler for Editing a Booking
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookingId,RoomId,CustomerId,EmployeeId,BookingStatusId,DateFrom,DateTo,ArrivalTime,CheckoutTime")] Booking booking)
        {
            if (id != booking.BookingId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.BookingId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookingStatusId"] = new SelectList(_context.BookingStatuses, "BookingStatusId", "Status", booking.BookingStatusId);
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "FullName", booking.CustomerId);
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "FullName", booking.EmployeeId);
            ViewData["RoomId"] = new SelectList(_context.Rooms.Include(x=>x.RoomType), "RoomId", "RoomDetail", booking.RoomId);
            return View(booking);
        }

        // GET: Bookings/Delete/5
        // Get Handler for Deleting a Booking
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.BookingStatus)
                .Include(b => b.Customer)
                .Include(b => b.Employee)
                .Include(b => b.Room).Include(b=>b.Room.RoomType)
                .FirstOrDefaultAsync(m => m.BookingId == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // POST: Bookings/Delete/5
        // Post Handler for Editing a Booking
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(int id)
        {
            return _context.Bookings.Any(e => e.BookingId == id);
        }
    }
}
