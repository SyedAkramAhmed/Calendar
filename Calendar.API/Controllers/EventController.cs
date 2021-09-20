using Calendar.API.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Calendar.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly EventDbContext _eventDbContext;
        public EventController(EventDbContext eventDbContext) { _eventDbContext = eventDbContext; }
        /// <summary>
        /// Get Calendar Events
        /// </summary>
        /// <returns>Calendar Events</returns>
        [HttpGet("GetEvents")]
        public IEnumerable<EventModel> GetEvents()
        {
            return _eventDbContext.Events.ToList();
        }
        /// <summary>
        /// Get Calendar Events By Start and End Date
        /// </summary>
        /// <returns>Filtered Calendar Events</returns>
        [HttpGet("GetEventsByRange/{startDate}/{endDate}")]
        public IActionResult GetEventsByRange([Required] DateTime StartDate, [Required] DateTime EndDate)
        {
            if (EndDate < StartDate) return BadRequest("End Date must be greater than Start Date");
            return Ok(_eventDbContext.Events.Where(e => e.StartDate < StartDate && e.EndDate >= EndDate).ToList().Select(e => e));
        }
        /// <summary>
        /// Get Calendar Event By Id
        /// </summary>
        /// <returns>Calendar Events</returns>
        [HttpGet("GetEventById/{id}")]
        public IActionResult GetEventById(int id)
        {
            EventModel eventDetails = new EventModel();
            eventDetails = _eventDbContext.Events.FirstOrDefault(x => x.Id == id);
            if (eventDetails == null) return NotFound();
            return Ok(eventDetails);
        }
        /// <summary>
        /// Insert Or Update Event
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("InsertOrUpdateEvent")]
        public IActionResult InsertOrUpdateEvent([FromBody] EventModel model)
        {
            EventModel eventDetails = new EventModel();
            if (!ModelState.IsValid) return BadRequest();
            eventDetails = _eventDbContext.Events.FirstOrDefault(x => x.Id == model.Id);
            if (eventDetails == null)
            {
                model.EndDate = model.AllDay ? new DateTime(model.StartDate.Year, model.StartDate.Month, model.StartDate.Day, 23, 59, 59, 999) : model.EndDate;
                _eventDbContext.Events.Add(model);
            }
            else
            {
                eventDetails.Title = model.Title;
                eventDetails.Description = model.Description;
                eventDetails.StartDate = model.StartDate;
                eventDetails.EndDate = model.AllDay ? new DateTime(model.StartDate.Year, model.StartDate.Month, model.StartDate.Day, 23, 59, 59, 999) : model.EndDate;
                eventDetails.RequiredAttendees = model.RequiredAttendees;
                eventDetails.OptionalAttendees = model.OptionalAttendees;
                eventDetails.AllDay = model.AllDay;
                eventDetails.EventPID = model.EventPID;
                eventDetails.RecurrenceType = model.RecurrenceType;
                eventDetails.EventLength = model.EventLength;
                if (!string.IsNullOrEmpty(eventDetails.RecurrenceType) && eventDetails.RecurrenceType != "none")
                {
                    _eventDbContext.Events.RemoveRange(_eventDbContext.Events.Where(ev => ev.EventPID == model.EventPID));
                }
            }
            _eventDbContext.SaveChanges();
            return eventDetails != null ? Ok(new { Message = "The event updated successfully." }) : Created("GetEventById", new { id = model.Id });
        }
        /// <summary>
        /// Delete Event By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("DeleteEvent/{id}")]
        public IActionResult DeleteEvent(int id)
        {
            var eventDetails = _eventDbContext.Events.FirstOrDefault(x => x.Id == id);
            if (eventDetails == null) return NotFound();
            if (!string.IsNullOrEmpty(eventDetails.RecurrenceType) && eventDetails.RecurrenceType != "none")
            {
                _eventDbContext.Events.RemoveRange(_eventDbContext.Events.Where(ev => ev.EventPID == id));
            }
            _eventDbContext.Events.Remove(eventDetails);
            _eventDbContext.SaveChanges();
            return Ok(new { Message = "The event deleted successfully." });
        }
    }
}
