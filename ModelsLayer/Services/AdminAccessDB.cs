using Microsoft.AspNetCore.Identity;
using ModelsLayer.DatabaseEntities;
using ModelsLayer.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLayer.Services
{
    public class AdminAccessDB : IAdminAccess
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public AdminAccessDB(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<List<ApplicationUser>> GetAllGuests()
        {
            var roles = await _userManager.GetUsersInRoleAsync("Guest");
            var UsersGuest = new List<ApplicationUser>();

            foreach (var user in roles)
            {
                UsersGuest.Add(user);
            }
            return UsersGuest;
        }
        public void DeleteUser(string UserId)
        {
            var eventsToRemove = _context.Events.Where(a => a.UserId == UserId).ToList();
            foreach (var Event in eventsToRemove)
            {
                _context.Events.Remove(Event);
            }
            _context.Users.Remove(_context.Users.FirstOrDefault(a => a.Id == UserId));
            _context.SaveChanges();
        }
        public List<Event> GetAllEventsWithoutEditorEvents()
        {
            var EventsWithoutEditorEvents = new List<Event>();
            var Events = _context.Events.ToList();
            foreach (var Event in Events)
            {
                int i = 0;
                foreach (var EditorEvent in GetAllEditorEvents())
                {
                    if (Event.Id == EditorEvent.EventId)
                    {
                        i++;
                        break;
                    }
                }
                if (i > 0)
                {
                    continue;
                }
                else
                {
                    EventsWithoutEditorEvents.Add(Event);
                }
            }

            return EventsWithoutEditorEvents;
        }
        public void AddMyPick(int EventId)
        {
            if (_context.EditorPickEvents.ToList().Count < 5)
            {
                var EventToAdd = _context.Events.FirstOrDefault(a => a.Id == EventId);
                var NewEditorEvent = new EditorPickEvent
                {
                    EventId = EventToAdd.Id,
                    EventName = EventToAdd.EventName,
                    UserId = EventToAdd.UserId,
                    NumberOfAttenders = EventToAdd.NumberOfAttenders
                };

                _context.EditorPickEvents.Add(NewEditorEvent);
                _context.SaveChanges();
            }
        }
        public List<EditorPickEvent> GetAllEditorEvents()
        {
            return _context.EditorPickEvents.ToList();
        }
        public void RemoveMyPick(int EventId)
        {
            _context.EditorPickEvents.Remove(_context.EditorPickEvents.FirstOrDefault(a => a.EventId == EventId));
            _context.SaveChanges();
        }


    }
}
