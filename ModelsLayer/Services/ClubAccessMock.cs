using Microsoft.AspNetCore.Identity;
using ModelsLayer.DatabaseEntities;
using ModelsLayer.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer.Services
{
    public class ClubAccessMock : IClubAccess
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ClubAccessMock(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        //
        //TO DO...
        //

        public Task ChangeEventAsync(EditEventsModelDTO model)
        {
            throw new NotImplementedException();
        }

        public Task CreateNewEventAsync(CreateEventModelDTO NewEvent)
        {
            throw new NotImplementedException();
        }

        public void DeleteEvent(int id)
        {
            throw new NotImplementedException();
        }

        public Task EditModelAsync(MyClubModelDTO ToEdit)
        {
            throw new NotImplementedException();
        }

        public Edit GetDescrById(string id)
        {
            throw new NotImplementedException();
        }

        public Event GetEventById(int id)
        {
            throw new NotImplementedException();
        }

        public List<Event> GetEventsById(string id)
        {
            throw new NotImplementedException();
        }

        public ApplicationUser GetMyClub(string id)
        {
            throw new NotImplementedException();
        }

        public string NewRandomNumber()
        {
            throw new NotImplementedException();
        }

        public Task ResetPasswordAsync(string email)
        {
            throw new NotImplementedException();
        }

        public void SendEmail(List<string> emails, string subject, string body, string ClubName)
        {
            throw new NotImplementedException();
        }

        public void SendEmailForgotPassword(string email, string body)
        {
            throw new NotImplementedException();
        }
    }
}
