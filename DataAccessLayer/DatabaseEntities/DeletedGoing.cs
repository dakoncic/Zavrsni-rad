using ModelsLayer.Identity;

namespace ModelsLayer.DatabaseEntities
{
    public class DeletedGoing 
    {
        public int Id { get; set; } //ovo ništa
        public int EventId { get; set; }
        public string UserId { get; set; }
        public DeletedEvent DeletedEvent { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
