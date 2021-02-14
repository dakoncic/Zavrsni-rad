using Microsoft.AspNetCore.Identity;
using ModelsLayer.DatabaseEntities;
using System.Collections.Generic;

namespace ModelsLayer.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser() : base()
        {
        }

        public string GuestName { get; set; }
        public string GuestLastName { get; set; }
        public string ClubName { get; set; }
        public string Location { get; set; }
        public int NumberOfFollowers { get; set; }
        public List<Following> Followings { get; set; } //child
        public List<Event> Events { get; set; } //child
        public List<Going> Goings { get; set; } //child
        public List<Edit> Edits { get; set; } //child
    }
}
