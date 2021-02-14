using ModelsLayer.Identity;
using System;

namespace ModelsLayer.DatabaseEntities
{
    public class DeletedEvent 
    {
        public int Id { get; set; }
        public string EventName { get; set; }
        public string UserId { get; set; }
        public int EventId { get; set; }
        public DateTime DateDeleted { get; set; }
        public ApplicationUser ApplicationUser { get; set; } //veza s parentom
    }
}