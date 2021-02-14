using System;

namespace ModelsLayer.DatabaseEntities
{
    public class ChangedEvent 
    {
        public int Id { get; set; }
        public string EventName { get; set; }
        public string UserId { get; set; }
        public int EventId { get; set; }
        public DateTime DateChanged { get; set; }
        public Event Event { get; set; } //veza s parentom
    }
}
