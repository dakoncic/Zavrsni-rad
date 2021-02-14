using DataAccessLayer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelsLayer.DatabaseEntities;
using Newtonsoft.Json;
using ModelsLayer.Identity;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;

namespace prbb.Controllers
{
    public class HomeController : Controller
    {
        private readonly IGuestAccess _GuestAccess;
        private readonly IClubAccess _clubAccess;
        private readonly IAdminAccess _adminAccess;
        private readonly IMapper _mapper;

        public HomeController(IGuestAccess GuestAccess, IClubAccess clubAccess, IAdminAccess adminAccess,
            IMapper mapper)
        {
            _GuestAccess = GuestAccess;
            _clubAccess = clubAccess;
            _adminAccess = adminAccess;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            if (User.IsInRole("Club"))
            {
                return View();
            }
            else if (User.IsInRole("Guest"))
            {
                return RedirectToAction("IndexGuest", "Home");
            }
            else if (User.IsInRole("Admin"))
            {
                return RedirectToAction("IndexAdmin", "Home");
            }
            else
            {
                return RedirectToAction("IndexVisitor", "Home");
            }
        }
        [Authorize(Roles = "Guest")]
        public IActionResult IndexGuest()
        {
            var EditorPickEvents = _adminAccess.GetAllEditorEvents();
            var TrendingEvents = _GuestAccess.GetTop5TrendingEvents();
            var IndexGuestModel = new IndexGuestModel
            {
                EditorEvents = EditorPickEvents,
                Top5TrendingEvents = TrendingEvents
            };

            return View(IndexGuestModel);
        }
        public IActionResult IndexVisitor()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> IndexAdmin()
        {


            var AllClubsAndGuests = new AllClubsAndGuestsModel();
            var AllClubsTask = await _GuestAccess.ClubList();
            var AllGuestsTask = await _adminAccess.GetAllGuests();

            var AllClubs = new List<ApplicationUser>();
            var AllGuests = new List<ApplicationUser>();
            foreach (var Club in AllClubsTask)
            {
                AllClubs.Add(Club);
            }

            foreach (var Guest in AllGuestsTask)
            {
                AllGuests.Add(Guest);
            }

            AllClubsAndGuests.AllClubs = AllClubs;
            AllClubsAndGuests.AllGuests = AllGuests;
            return View(AllClubsAndGuests);
        }

        [Authorize(Roles = "Club")]
        [HttpGet]
        public IActionResult MyClub(string id)
        {
            MyClubModel ClubModel = new MyClubModel();
            ApplicationUser UserModel = _clubAccess.GetMyClub(id);
            Edit EditTemp = _clubAccess.GetDescrById(id);
            List<Event> TempEventList = _clubAccess.GetEventsById(id);

            ClubModel.ClubName = UserModel.ClubName;
            ClubModel.Location = UserModel.Location;
            if (EditTemp != null)
            {
                ClubModel.Description = EditTemp.Description;
                ClubModel.ImagePath = EditTemp.ImagePath;
            }
            ClubModel.UserId = id;
            ClubModel.Events = TempEventList;


            return View(ClubModel);
        }

        [Authorize(Roles = "Club")]
        [HttpPost]
        public async Task<IActionResult> MyClub(string id, MyClubModel ToEdit)
        {
            if (ModelState.IsValid)
            {
                await _clubAccess.EditModelAsync(_mapper.Map<MyClubModelDTO>(ToEdit));
                return RedirectToAction("MyClub", new { id = id });
            }


            return View(ToEdit);
        }


        [Authorize(Roles = "Guest")]
        public async Task<IActionResult> ClubList()
        {
            var clublist = await _GuestAccess.ClubList();


            var TempUserFollowList = new List<UserFollow>();
            foreach (var item in clublist)
            {
                TempUserFollowList.Add(new UserFollow
                {
                    Id = item.Id,
                    ClubName = item.ClubName,
                    IsFollowed = _GuestAccess.Followpaircheck(item.Id, User.FindFirstValue(ClaimTypes.NameIdentifier))
                }
                    );
            }

            return View(TempUserFollowList);
        }


        [Authorize(Roles = "Guest")]
        public IActionResult ClubInfo(string id)
        {
            MyClubModel ClubModel = new MyClubModel();
            ApplicationUser UserModel = _clubAccess.GetMyClub(id);
            Edit EditTemp = _clubAccess.GetDescrById(id);
            List<UserGoing> TempEventList = new List<UserGoing>();
            var TempEvent = _clubAccess.GetEventsById(id);

            foreach (var item in TempEvent)
            {
                TempEventList.Add(new UserGoing
                {
                    EventName = item.EventName,
                    Id = item.Id,
                    IsGoing = _GuestAccess.Followgoingcheck(item.Id, User.FindFirstValue(ClaimTypes.NameIdentifier))
                }
                    );
            }

            ClubModel.ClubName = UserModel.ClubName;
            ClubModel.Location = UserModel.Location;
            ClubModel.NumberOfFollowers = UserModel.NumberOfFollowers;
            ClubModel.Description = EditTemp.Description;
            ClubModel.UserId = id;
            ClubModel.UserGoings = TempEventList;
            ClubModel.ImagePath = EditTemp.ImagePath;
            ClubModel.IsFollowed = _GuestAccess.Followpaircheck(id, User.FindFirstValue(ClaimTypes.NameIdentifier));

            return View(ClubModel);
        }

        [Authorize(Roles = "Club")]
        [HttpGet]
        public IActionResult EditEvents(int id, string UserId)
        {
            EditEventsModel model = new EditEventsModel();

            Event temp = _clubAccess.GetEventById(id);

            model.EventId = id;
            model.Description = temp.Description;
            model.EventName = temp.EventName;
            model.StartDate = temp.StartDate;
            model.UserId = UserId;
            model.ImagePath = temp.ImagePath;

            return View(model);
        }

        [Authorize(Roles = "Club")]
        [HttpPost]
        public async Task<IActionResult> EditEvents(int id, string UserId, EditEventsModel model)
        {
            if (ModelState.IsValid)
            {
                await _clubAccess.ChangeEventAsync(_mapper.Map<EditEventsModelDTO>(model));
                return RedirectToAction("MyClub", new { id = UserId });
            }

            return View(model);
        }

        [Authorize(Roles = "Club")]
        [HttpGet]
        public IActionResult CreateEvent(string UserId)
        {
            CreateEventModel model = new CreateEventModel
            {
                UserId = UserId
            };

            return View(model);
        }

        [Authorize(Roles = "Club")]
        [HttpPost]
        public async Task<IActionResult> CreateEvent(string UserId, CreateEventModel modelino)
        {
            if (ModelState.IsValid)
            {
                await _clubAccess.CreateNewEventAsync(_mapper.Map<CreateEventModelDTO>(modelino));
                return RedirectToAction("MyClub", new { id = UserId });
            }

            return View();
        }

        [Authorize(Roles = "Club")]
        public IActionResult DeleteEvent(int id, string UserId)
        {
            _clubAccess.DeleteEvent(id);

            return RedirectToAction("MyClub", new { id = UserId });
        }

        [Authorize(Roles = "Guest")]
        public IActionResult EventInfo(int id, string UserId)
        {
            Event temp = _clubAccess.GetEventById(id);
            CreateEventModel model = new CreateEventModel
            {
                Description = temp.Description,
                EventId = temp.Id,
                EventName = temp.EventName,
                StartDate = temp.StartDate,
                UserId = temp.UserId,
                ImagePath = temp.ImagePath,
                NumberOfAttenders = temp.NumberOfAttenders,
                IsGoing = _GuestAccess.Followgoingcheck(id, User.FindFirstValue(ClaimTypes.NameIdentifier))
            };

            return View(model);
        }

        [Authorize(Roles = "Guest")]
        [HttpPost]
        public void AddFollower(string clubid, string currentuserid)
        {
            _GuestAccess.AddOrRemoveFollower(clubid, currentuserid);
        }

        [Authorize(Roles = "Guest")]
        [HttpPost]
        public void AddGoing(int eventid, string currentuserid)
        {
            _GuestAccess.AddOrRemoveGoing(eventid, currentuserid);
        }

        [Authorize(Roles = "Guest")]
        public IActionResult AttendingEvents()
        {
            var EventList = _GuestAccess.GetAllEventsByUserId(User.FindFirstValue(ClaimTypes.NameIdentifier));
            List<EventListModel> TempEvent = new List<EventListModel>();

            foreach (var item in EventList)
            {
                TempEvent.Add(
                    new EventListModel
                    {
                        Description = item.Description,
                        EventName = item.EventName,
                        Id = item.Id,
                        StartDate = item.StartDate,
                        UserId = item.UserId, //ID kluba za event
                        IsGoing = _GuestAccess.Followgoingcheck(item.Id, User.FindFirstValue(ClaimTypes.NameIdentifier))
                    }
                    );
            }
            return View(TempEvent);
        }

        [Authorize(Roles = "Guest")]
        public IActionResult News()
        {
            var AllCreatedEvents = _GuestAccess.GetAllCreatedEventsFromFollowedClubs(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var AllChangedEvents = _GuestAccess.GetAllEditedEventsFromAttendedEvents(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var AllDeletedEvents = _GuestAccess.GetAllDeletedEventsFromAttendedEvents(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var AllEvents = new NewsEventModel
            {
                CreatedEvents = AllCreatedEvents,
                ChangedEvents = AllChangedEvents,
                DeletedEvents = AllDeletedEvents
            };

            return View(AllEvents);
        }


        //ADMIN PART
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteUser(string UserId)
        {
            _adminAccess.DeleteUser(UserId);

            return RedirectToAction("IndexAdmin");
        }
        [Authorize(Roles = "Admin")]
        public IActionResult EditorPick()
        {
            var Events = new AllEventsModel();
            var AllEvents = _adminAccess.GetAllEventsWithoutEditorEvents();
            var AllEditorEvents = _adminAccess.GetAllEditorEvents();

            Events.AllEvents = AllEvents;
            Events.AllEditorEvents = AllEditorEvents;
            return View(Events);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult AdminEventInfo(int id)
        {
            Event temp = _clubAccess.GetEventById(id);
            CreateEventModel model = new CreateEventModel
            {
                Description = temp.Description,
                EventId = temp.Id,
                EventName = temp.EventName,
                StartDate = temp.StartDate,
                UserId = temp.UserId,
                ImagePath = temp.ImagePath
            };

            return View(model);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult AddMyPick(int id)
        {
            _adminAccess.AddMyPick(id);

            return RedirectToAction("EditorPick");
        }
        [Authorize(Roles = "Admin")]
        public IActionResult RemoveMyPick(int id)
        {
            _adminAccess.RemoveMyPick(id);

            return RedirectToAction("EditorPick");
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }
        public async Task<IActionResult> ForgotPasswordPost(string email)
        {
            await _clubAccess.ResetPasswordAsync(email);

            return View();
        }

        public async Task<string> ReturnJSON()
        {
            var guests = await _adminAccess.GetAllGuests();
            var modifiedGuests = new List<AppUser>();
            foreach (var item in guests)
            {
                modifiedGuests.Add(new AppUser
                {
                    GuestName = item.GuestName,
                    GuestLastName = item.GuestLastName
                });
            }

            string strResultJson = JsonConvert.SerializeObject(modifiedGuests);

            return strResultJson;
        }
    }
    public class AppUser
    {
        public string GuestName;
        public string GuestLastName;
    }
}
