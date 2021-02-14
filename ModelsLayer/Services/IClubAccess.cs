using ModelsLayer.DatabaseEntities;
using ModelsLayer.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer.Services
{
    public interface IClubAccess
    {
        Task ChangeEventAsync(EditEventsModelDTO model);
        Task CreateNewEventAsync(CreateEventModelDTO NewEvent);
        void DeleteEvent(int id);
        Task EditModelAsync(MyClubModelDTO ToEdit);
        Edit GetDescrById(string id);
        Event GetEventById(int id);
        List<Event> GetEventsById(string id);
        ApplicationUser GetMyClub(string id);
        string NewRandomNumber();
        Task ResetPasswordAsync(string email);
        void SendEmail(List<string> emails, string subject, string body, string ClubName);
        void SendEmailForgotPassword(string email, string body);
    }
}