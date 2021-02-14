using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace ModelsLayer.Identity
{
    public class EditEventsModel
    {
        //public EditEventsModel()
        //{
        //    StartDate = DateTime.Now;
        //}
        [Required]
        public string EventName { get; set; }
        [Required]
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public int NumberOfAttenders { get; set; }
        public string UserId { get; set; }
        public int EventId { get; set; }
        public IFormFile image { get; set; }
        public string ImagePath { get; set; }
    }
}
