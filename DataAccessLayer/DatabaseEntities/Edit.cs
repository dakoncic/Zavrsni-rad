using ModelsLayer.Identity;

namespace ModelsLayer.DatabaseEntities
{
    public class Edit
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public string UserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
