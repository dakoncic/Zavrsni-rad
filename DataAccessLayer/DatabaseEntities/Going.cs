using ModelsLayer.Identity;

namespace ModelsLayer.DatabaseEntities
{
    public class Going
    {
        public int Id { get; set; } //ovo ništa
        public int EventId { get; set; }
        public string UserId { get; set; }
        public Event Event { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
