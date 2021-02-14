using Microsoft.AspNetCore.Http;
using ModelsLayer.DatabaseEntities;
using System.Collections.Generic;

namespace ModelsLayer.Identity
{
    public class MyClubModel
    {
        public string UserId { get; set; } //user id
        public string Description { get; set; }
        public string ClubName { get; set; }
        public string Location { get; set; }
        public int NumberOfFollowers { get; set; }
        public string IsFollowed { get; set; }
        public List<Event> Events { get; set; }
        public List<UserGoing> UserGoings { get; set; }

        public IFormFile image { get; set; }
        public string ImagePath { get; set; }
    }
}
