using Microsoft.AspNetCore.Identity;
using ModelsLayer.DatabaseEntities;
using ModelsLayer.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer.Services
{
    public class GuestAccessMock : IGuestAccess
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public GuestAccessMock(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        //
        //TO DO...
        //
        public void AddOrRemoveFollower(string clubid, string currentuserid)
        {
            throw new NotImplementedException();
        }

        public void AddOrRemoveGoing(int eventid, string currentuserid)
        {
            throw new NotImplementedException();
        }

        public Task<List<ApplicationUser>> ClubList()
        {
            throw new NotImplementedException();
        }

        public string Followgoingcheck(int id, string currentuserid)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Following> FollowingsList()
        {
            throw new NotImplementedException();
        }

        public string Followpaircheck(string id, string currentuserid)
        {
            throw new NotImplementedException();
        }

        public List<Event> GetAllCreatedEventsFromFollowedClubs(string currentuserid)
        {
            throw new NotImplementedException();
        }

        public List<DeletedEvent> GetAllDeletedEventsFromAttendedEvents(string currentuserid)
        {
            throw new NotImplementedException();
        }

        public List<ChangedEvent> GetAllEditedEventsFromAttendedEvents(string currentuserid)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Event> GetAllEventsByUserId(string userid)
        {
            throw new NotImplementedException();
        }

        public List<Event> GetTop5TrendingEvents()
        {
            throw new NotImplementedException();
        }
    }
}
