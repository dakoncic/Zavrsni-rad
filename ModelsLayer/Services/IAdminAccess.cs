using ModelsLayer.DatabaseEntities;
using ModelsLayer.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer.Services
{
    public interface IAdminAccess
    {
        void AddMyPick(int EventId);
        void DeleteUser(string UserId);
        List<EditorPickEvent> GetAllEditorEvents();
        List<Event> GetAllEventsWithoutEditorEvents();
        Task<List<ApplicationUser>> GetAllGuests();
        void RemoveMyPick(int EventId);
    }
}