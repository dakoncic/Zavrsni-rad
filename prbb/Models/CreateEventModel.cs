using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace ModelsLayer.Identity
{
    public class CreateEventModel
    {
        public CreateEventModel()
        {
            StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0);
        }

        [Required]
        public string EventName { get; set; }
        [Required]
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public int NumberOfAttenders { get; set; }
        public string IsGoing { get; set; }
        public string UserId { get; set; }
        public int EventId { get; set; }
        [Required]
        public IFormFile image { get; set; }
        public string ImagePath { get; set; }
    }
}
