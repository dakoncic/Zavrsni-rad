using Microsoft.AspNetCore.Identity;
using ModelsLayer.DatabaseEntities;
using ModelsLayer.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLayer.Services
{
    public class AdminAcessMock : IAdminAccess
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminAcessMock(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            ChangedEvents= new List<ChangedEvent>();
            DeletedEvents= new List<DeletedEvent>();
            DeletedGoings= new List<DeletedGoing>();
            Edits= new List<Edit>();
            EditorPickEvents= new List<EditorPickEvent>();
            Events= new List<Event>();
            Followings= new List<Following>();
            Goings= new List<Going>();
        }

        public void AddMyPick(int EventId)
        {
            if (EditorPickEvents.Count < 5)
            {
                var EventToAdd = Events.FirstOrDefault(a => a.Id == EventId);
                var NewEditorEvent = new EditorPickEvent
                {
                    Id = EditorPickEvents.Max(x => x.Id) + 1,
                    EventId = EventToAdd.Id,
                    EventName = EventToAdd.EventName,
                    UserId = EventToAdd.UserId,
                    NumberOfAttenders = EventToAdd.NumberOfAttenders
                };

                EditorPickEvents.Add(NewEditorEvent);
            }
        }

        public void DeleteUser(string UserId)
        {
            throw new NotImplementedException();
        }

        public List<EditorPickEvent> GetAllEditorEvents()
        {
            throw new NotImplementedException();
        }

        public List<Event> GetAllEventsWithoutEditorEvents()
        {
            throw new NotImplementedException();
        }

        public Task<List<ApplicationUser>> GetAllGuests()
        {
            throw new NotImplementedException();
        }

        public void RemoveMyPick(int EventId)
        {
            throw new NotImplementedException();
        }

        List<ChangedEvent> ChangedEvents;
        List<DeletedEvent> DeletedEvents;
        List<DeletedGoing> DeletedGoings;
        List<Edit> Edits;
        List<EditorPickEvent> EditorPickEvents;
        List<Event> Events;
        List<Following> Followings;
        List<Going> Goings;
    }
}
