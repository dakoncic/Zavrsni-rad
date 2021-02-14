using ModelsLayer.Identity;
using System;
using System.Collections.Generic;

namespace ModelsLayer.DatabaseEntities
{
    public class Event
    {
        public int Id { get; set; }
        public string EventName { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DateCreated { get; set; }
        public string ImagePath { get; set; }
        public int NumberOfAttenders { get; set; }
        public string UserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; } //veza s parentom
        public List<Going> Goings { get; set; } //veza s djetetom
    }
}
