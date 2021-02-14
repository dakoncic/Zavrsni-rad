using ModelsLayer.Identity;

namespace ModelsLayer.DatabaseEntities
{
    public class Following
    {
        public string Id { get; set; } //CLUB ID ->error, Id je unikatan!
        public string ClubId { get; set; }
        public int NumberOfFollowers { get; set; }
        public string UserId { get; set; } //CURRENT USER WHICH FOLLOWS
        public ApplicationUser ApplicationUser { get; set; }
    }
}
