using System;

namespace ModelsLayer.Identity
{
    public class EventListModelDTO
    {
        public string Description { get; set; }
        public string EventName { get; set; }
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public string UserId { get; set; }

        public string IsGoing { get; set; }
    }
}
