using System.Collections.Generic;

namespace ModelsLayer.Identity
{
    public class AllClubsAndGuestsModel
    {
        public List<ApplicationUser> AllClubs { get; set; }
        public List<ApplicationUser> AllGuests { get; set; }
    }
}
