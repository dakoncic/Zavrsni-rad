using Microsoft.AspNetCore.Identity;
using ModelsLayer.DatabaseEntities;
using ModelsLayer.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace DataAccessLayer.Services
{
    public class ClubAccessDB : IClubAccess
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public ClubAccessDB(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        public ApplicationUser GetMyClub(string id)
        {
            ApplicationUser TempUser = new ApplicationUser();
            TempUser = _context.Users.FirstOrDefault(a => a.Id == id);

            return TempUser;
        }
        public Edit GetDescrById(string id)
        {
            Edit EditTemp = new Edit();
            EditTemp = _context.Edits.FirstOrDefault(a => a.UserId == id);
            if (EditTemp == null)
            {
                EditTemp = new Edit();
            }

            return EditTemp;
        }
        public async Task EditModelAsync(MyClubModelDTO ToEdit)
        {
            Edit NewEdit = new Edit();
            NewEdit = _context.Edits.FirstOrDefault(a => a.UserId == ToEdit.UserId);
            int i = 0;
            if (NewEdit == null)
            {
                i = 1;
                NewEdit = new Edit
                {
                    UserId = ToEdit.UserId
                };
            }
            NewEdit.Description = ToEdit.Description;

            //changing event image
            if (ToEdit.image != null && ToEdit.image.Length > 0)
            {
                var fileName = Path.GetFileName(ToEdit.image.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\UserImages", fileName);
                using (var fileSteam = new FileStream(filePath, FileMode.Create))
                {
                    await ToEdit.image.CopyToAsync(fileSteam);
                }

                NewEdit.ImagePath = fileName;
            }
            if (i == 1)
            {
                _context.Edits.Add(NewEdit);
            }

            _context.SaveChanges();
        }
        public async Task CreateNewEventAsync(CreateEventModelDTO NewEvent)
        {
            Event temp = new Event
            {
                Description = NewEvent.Description,
                EventName = NewEvent.EventName,
                UserId = NewEvent.UserId,
                StartDate = NewEvent.StartDate,
                DateCreated = DateTime.Now
            };

            //creating event image
            if (NewEvent.image != null && NewEvent.image.Length > 0)
            {
                var fileName = Path.GetFileName(NewEvent.image.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory()
                    , "wwwroot\\images\\EventImages", fileName);
                using (var fileSteam = new FileStream(filePath, FileMode.Create))
                {
                    await NewEvent.image.CopyToAsync(fileSteam);
                }

                temp.ImagePath = fileName;
            }

            _context.Events.Add(temp);

            _context.SaveChanges();


            SendingMailOnCreateNewEvent(NewEvent);
        }
        public Event GetEventById(int id)
        {
            Event temp = new Event();

            temp = _context.Events.FirstOrDefault(a => a.Id == id);

            return temp;
        }
        public async Task ChangeEventAsync(EditEventsModelDTO model)
        {
            Event temp = new Event();
            EditorPickEvent EditorTemp = new EditorPickEvent();
            ChangedEvent ChangedTemp = new ChangedEvent();


            temp = _context.Events.FirstOrDefault(a => a.Id == model.EventId);
            temp.Description = model.Description;
            temp.EventName = model.EventName;
            temp.StartDate = model.StartDate;

            //changing name at editors pick
            EditorTemp = _context.EditorPickEvents.FirstOrDefault(a => a.EventId == model.EventId);
            if (EditorTemp != null)
            {
                EditorTemp.EventName = model.EventName;
            }

            //changing at changes
            ChangingEventAtChangedTable(model);

            //changing event image
            if (model.image != null && model.image.Length > 0)
            {
                var fileName = Path.GetFileName(model.image.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\EventImages", fileName);
                using (var fileSteam = new FileStream(filePath, FileMode.Create))
                {
                    await model.image.CopyToAsync(fileSteam);
                }

                temp.ImagePath = fileName;
            }

            _context.SaveChanges();

            //sending mail

            SendingMailOnChangeEvent(model);
        }

        private void ChangingEventAtChangedTable(EditEventsModelDTO model)
        {
            var NewChangedEvent = new ChangedEvent();
            NewChangedEvent = _context.ChangedEvents.FirstOrDefault(a => a.EventId == model.EventId);
            int i = 0;
            if (NewChangedEvent == null)
            {
                i = 1;
                NewChangedEvent = new ChangedEvent();
            }

            NewChangedEvent.EventId = model.EventId;
            NewChangedEvent.UserId = model.UserId;
            NewChangedEvent.EventName = model.EventName;
            NewChangedEvent.DateChanged = DateTime.Now;
            if (i == 1)
            {
                _context.ChangedEvents.Add(NewChangedEvent);
            }
        }

        private void SendingMailOnChangeEvent(EditEventsModelDTO model)
        {
            var Attenders = _context.Goings.Where(a => a.EventId == model.EventId).ToList();
            if (Attenders.Count > 0)
            {
                var emails = new List<string>();
                var UserAttenders = new List<ApplicationUser>();
                var ClubName = _context.Users.FirstOrDefault(a => a.Id == model.UserId);
                string SubjectMessage = "Event details of '" + model.EventName + "' have been changed!";
                string BodyMessage = " For more info, please check new event details on our webpage!";

                foreach (var Attender in Attenders)
                {
                    UserAttenders.Add(_context.Users.FirstOrDefault(a => a.Id == Attender.UserId));
                }

                foreach (var Attender in UserAttenders)
                {
                    emails.Add(Attender.Email);
                }

                SendEmail(emails, SubjectMessage, BodyMessage, ClubName.ClubName);
            }
        }

        public void DeleteEvent(int id)
        {

            //first sending mail
            var Attenders = _context.Goings.Where(a => a.EventId == id).ToList();
            if (Attenders.Count > 0)
            {
                var UserAttenders = new List<ApplicationUser>();
                var emails = new List<string>();
                var EventName = _context.Events.FirstOrDefault(a => a.Id == id);
                var ClubName = _context.Users.FirstOrDefault(a => a.Id == EventName.UserId);
                string SubjectMessage = "Event '" + EventName.EventName + "' has been canceled!";
                string BodyMessage = "We are sorry to inform you that event has been canceled due to unexpected emergency." +
                    " Please consider visiting some of our other events.";

                foreach (var Attender in Attenders)
                {
                    UserAttenders.Add(_context.Users.FirstOrDefault(a => a.Id == Attender.UserId));
                }
                foreach (var Attender in UserAttenders)
                {
                    emails.Add(Attender.Email);
                }

                SendEmail(emails, SubjectMessage, BodyMessage, ClubName.ClubName);
            }
            UpdatingDeletedEventTable(id);
            UpdatingDeletedGoingTable(id);
            //then remove
            _context.Events.Remove(_context.Events.FirstOrDefault(a => a.Id == id));
            _context.SaveChanges();
        }

        private void UpdatingDeletedGoingTable(int id)
        {
            var AllGoingsFromClubToDelete = _context.Goings.Where(a => a.EventId == id).ToList();
            if (AllGoingsFromClubToDelete.Count > 0)
            {
                foreach (var Going in AllGoingsFromClubToDelete)
                {
                    var NewDeletedGoing = new DeletedGoing
                    {
                        EventId = Going.EventId,
                        UserId = Going.UserId
                    };
                    _context.DeletedGoings.Add(NewDeletedGoing);
                }
            }
        }

        private void UpdatingDeletedEventTable(int id)
        {
            var NewDeletedEventInfo = new DeletedEvent();

            var EventToDelete = _context.Events.FirstOrDefault(a => a.Id == id);
            NewDeletedEventInfo.EventName = EventToDelete.EventName;
            NewDeletedEventInfo.UserId = EventToDelete.UserId;
            NewDeletedEventInfo.EventId = EventToDelete.Id;
            NewDeletedEventInfo.DateDeleted = DateTime.Now;
            _context.DeletedEvents.Add(NewDeletedEventInfo);
        }
        public List<Event> GetEventsById(string id)
        {
            return _context.Events.Where(a => a.UserId == id).ToList();
        }
        public async Task ResetPasswordAsync(string email)
        {
            var ThisUser = new ApplicationUser();
            ThisUser = _context.Users.FirstOrDefault(a => a.Email == email);
            var NewPassword = NewRandomNumber();
            await _userManager.RemovePasswordAsync(ThisUser);
            await _userManager.AddPasswordAsync(ThisUser, NewPassword);
            _context.SaveChanges();
            SendEmailForgotPassword(email, NewPassword);
        }
        public string NewRandomNumber()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[8];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var finalString = new string(stringChars);
            return finalString;
        }



        private void SendingMailOnCreateNewEvent(CreateEventModelDTO NewEvent)
        {
            var ClubFollowers = _context.Followings.Where(a => a.ClubId == NewEvent.UserId).ToList();
            if (ClubFollowers.Count > 0)
            {
                List<ApplicationUser> AccountFollowers = new List<ApplicationUser>();
                var emails = new List<string>();
                ApplicationUser ClubName = _context.Users.FirstOrDefault(a => a.Id == NewEvent.UserId);
                string NewEventMessage = NewEvent.EventName + " at " + ClubName.ClubName + "! " + NewEvent.StartDate;

                foreach (var Follower in ClubFollowers)
                {
                    AccountFollowers.Add(_context.Users.FirstOrDefault(a => a.Id == Follower.UserId));
                }

                foreach (var Follower in AccountFollowers)
                {
                    emails.Add(Follower.Email);
                }

                SendEmail(emails, NewEventMessage, NewEvent.Description, ClubName.ClubName);
            }
        }

        public void SendEmail(List<string> emails, string subject, string body, string ClubName)
        {
            SmtpClient client = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("beatdropazure@gmail.com", "a1b2c3d4+"),
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network
            };

            foreach (var email in emails)
            {
                MailMessage mail = new MailMessage
                {
                    From = new MailAddress("beatdropazure@gmail.com", ClubName)
                };
                mail.To.Add(new MailAddress(email));
                mail.Subject = subject;
                mail.IsBodyHtml = true;
                mail.Body = body;

                client.Send(mail);
            }
        }
        public void SendEmailForgotPassword(string email, string body)
        {
            SmtpClient client = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("beatdropazure@gmail.com", "a1b2c3d4+"),
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network
            };

            MailMessage mail = new MailMessage
            {
                From = new MailAddress("beatdropazure@gmail.com", "a1b2c3d4+")
            };
            mail.To.Add(new MailAddress(email));
            mail.Subject = "New password";
            mail.IsBodyHtml = true;
            mail.Body = "Your new password is: " + body;

            client.Send(mail);
        }

    }
}
