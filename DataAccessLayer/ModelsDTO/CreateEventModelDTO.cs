using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace ModelsLayer.Identity
{
    public class CreateEventModelDTO
    {
        public string EventName { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public int NumberOfAttenders { get; set; }
        public string IsGoing { get; set; }
        public string UserId { get; set; }
        public int EventId { get; set; }
        public IFormFile image { get; set; }
        public string ImagePath { get; set; }
    }
}
