using ModelsLayer.DatabaseEntities;
using ModelsLayer.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer.Services
{
    public interface IGuestAccess
    {
        void AddOrRemoveFollower(string clubid, string currentuserid);
        void AddOrRemoveGoing(int eventid, string currentuserid);
        Task<List<ApplicationUser>> ClubList();
        string Followgoingcheck(int id, string currentuserid);
        IEnumerable<Following> FollowingsList();
        string Followpaircheck(string id, string currentuserid);
        List<Event> GetAllCreatedEventsFromFollowedClubs(string currentuserid);
        List<DeletedEvent> GetAllDeletedEventsFromAttendedEvents(string currentuserid);
        List<ChangedEvent> GetAllEditedEventsFromAttendedEvents(string currentuserid);
        IEnumerable<Event> GetAllEventsByUserId(string userid);
        List<Event> GetTop5TrendingEvents();
    }
}