using Microsoft.AspNetCore.Identity;
using ModelsLayer.DatabaseEntities;
using ModelsLayer.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLayer.Services
{

    public class GuestAccessDB : IGuestAccess
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public GuestAccessDB(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        public async Task<List<ApplicationUser>> ClubList()
        {
            var roles = await _userManager.GetUsersInRoleAsync("Club");
            var UsersClub = new List<ApplicationUser>();

            foreach (var user in roles)
            {
                UsersClub.Add(user);
            }
            return UsersClub;
        }
        public void AddOrRemoveFollower(string clubid, string currentuserid)
        {
            Following NewFollow = new Following();
            Following Temp = new Following();

            NewFollow.ClubId = clubid;
            NewFollow.UserId = currentuserid;

            Temp = _context.Followings.FirstOrDefault(a => a.ClubId == clubid && a.UserId == currentuserid);
            if (Temp != null) //ako postoji onda mičemo par
            {
                _context.Followings.Remove(Temp);
                _context.Users.FirstOrDefault(a => a.Id == clubid).NumberOfFollowers--;
            }
            else
            {
                _context.Followings.Add(NewFollow); //ako ne postoji onda dodajemo par
                _context.Users.FirstOrDefault(a => a.Id == clubid).NumberOfFollowers++;
            }


            _context.SaveChanges();
        }

        public void AddOrRemoveGoing(int eventid, string currentuserid)
        {
            Going NewGoing = new Going();
            Going Temp = new Going();

            NewGoing.EventId = eventid;
            NewGoing.UserId = currentuserid;

            Temp = _context.Goings.FirstOrDefault(a => a.EventId == eventid && a.UserId == currentuserid);
            var IfContainsEditorEvent = _context.EditorPickEvents.FirstOrDefault(a => a.EventId == eventid);
            if (Temp != null) //ako postoji onda mičemo par
            {
                _context.Goings.Remove(Temp);
                _context.Events.FirstOrDefault(a => a.Id == eventid).NumberOfAttenders--; //smanjujemo broj posjetitelja za 1

                if (IfContainsEditorEvent != null)
                {
                    IfContainsEditorEvent.NumberOfAttenders--;
                }

            }
            else
            {
                _context.Goings.Add(NewGoing); //ako ne postoji onda dodajemo par
                _context.Events.FirstOrDefault(a => a.Id == eventid).NumberOfAttenders++; //uvećavamo broj posjetitelja za 1
                if (IfContainsEditorEvent != null)
                {
                    IfContainsEditorEvent.NumberOfAttenders++;
                }
            }


            _context.SaveChanges();
        }

        public string Followpaircheck(string id, string currentuserid)
        {
            if (_context.Followings.FirstOrDefault(a => a.ClubId == id && a.UserId == currentuserid) != null)
            {
                return "true";
            }
            else
            {
                return "false";
            }
        }

        public string Followgoingcheck(int id, string currentuserid)
        {
            if (_context.Goings.FirstOrDefault(a => a.EventId == id && a.UserId == currentuserid) != null)
            {
                return "true";
            }
            else
            {
                return "false";
            }
        }

        public IEnumerable<Event> GetAllEventsByUserId(string userid)
        {
            List<Event> EventList = new List<Event>();
            var GoingsByIdList = _context.Goings.Where(a => a.UserId == userid).ToList();

            foreach (var item in GoingsByIdList)
            {
                EventList.Add(
                    _context.Events.FirstOrDefault(a => a.Id == item.EventId)
                    );
            }

            return EventList;
        }

        public List<Event> GetAllCreatedEventsFromFollowedClubs(string currentuserid)
        {
            var ClubsUser = _context.Followings.Where(a => a.UserId == currentuserid).ToList();
            var EventsFromClubsFollowed = new List<Event>();
            if (ClubsUser.Count > 0)
            {
                var ClubsFollowed = new List<ApplicationUser>();
                foreach (var Club in ClubsUser)
                {
                    ClubsFollowed.Add(_context.Users.FirstOrDefault(a => a.Id == Club.ClubId));
                }

                AllEventsFromEachClub(EventsFromClubsFollowed, ClubsFollowed);
            }
            return EventsFromClubsFollowed;
        }

        private void AllEventsFromEachClub(List<Event> EventsFromClubsFollowed, List<ApplicationUser> ClubsFollowed)
        {
            foreach (var Club in ClubsFollowed)
            {
                var ClubEvents = new List<Event>();
                ClubEvents = _context.Events.Where(a => a.UserId == Club.Id
                 && a.DateCreated.Year == DateTime.Now.Year
                 && a.DateCreated.Month == DateTime.Now.Month
                 && DateTime.Now.Day - a.DateCreated.Day < 2).ToList();
                if (ClubEvents != null)
                {
                    foreach (var Event in ClubEvents)
                    {
                        var TrimmedEvent = new Event
                        {
                            Id = Event.Id,
                            UserId = Event.UserId,
                            EventName = Event.EventName,
                            StartDate = Event.StartDate,
                            DateCreated = Event.DateCreated
                        };

                        if (TrimmedEvent != null)
                        {
                            EventsFromClubsFollowed.Add(TrimmedEvent);
                        }
                    }
                }
            }
        }

        public List<ChangedEvent> GetAllEditedEventsFromAttendedEvents(string currentuserid)
        {
            var EventsUser = _context.Goings.Where(a => a.UserId == currentuserid).ToList();
            var ChangedEventList = new List<ChangedEvent>();

            if (EventsUser.Count > 0)
            {
                foreach (var Event in EventsUser)
                {
                    var CurrentEvent = _context.ChangedEvents.FirstOrDefault(a => a.EventId == Event.EventId
                      && a.DateChanged.Year == DateTime.Now.Year
                      && a.DateChanged.Month == DateTime.Now.Month
                      && DateTime.Now.Day - a.DateChanged.Day < 2);

                    if (CurrentEvent != null)
                    {
                        ChangedEventList.Add(CurrentEvent);
                    }
                }
            }
            return ChangedEventList;
        }

        public List<DeletedEvent> GetAllDeletedEventsFromAttendedEvents(string currentuserid)
        {
            var UserGoings = _context.DeletedGoings.Where(a => a.UserId == currentuserid).ToList();
            var DeletedEvents = new List<DeletedEvent>();
            if (UserGoings.Count > 0)
            {
                foreach (var User in UserGoings)
                {
                    var CurrentDeletedEvent = _context.DeletedEvents.FirstOrDefault(a => a.EventId == User.EventId
                      && a.DateDeleted.Year == DateTime.Now.Year
                      && a.DateDeleted.Month == DateTime.Now.Month
                      && DateTime.Now.Day - a.DateDeleted.Day < 2);
                    if (CurrentDeletedEvent != null)
                    {
                        DeletedEvents.Add(CurrentDeletedEvent);
                    }
                }
            }
            return DeletedEvents;
        }
        public List<Event> GetTop5TrendingEvents()
        {
            var Events = _context.Events.OrderByDescending(a => a.NumberOfAttenders).Take(5).ToList();
            return Events;
        }
        //ADMIN PART


        public IEnumerable<Following> FollowingsList()
        {
            return _context.Followings.ToList();
        }
    }
}
