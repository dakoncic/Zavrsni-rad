using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ModelsLayer.DatabaseEntities;
using ModelsLayer.Identity;

namespace DataAccessLayer.Services
{

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    //dodali smo ApplicationUser
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

            //servis trebamo napravit kad proširujemo bazu i koristimo negdje
        }

        public DbSet<Event> Events { get; set; }
        public DbSet<Following> Followings { get; set; }
        public DbSet<Going> Goings { get; set; }
        public DbSet<Edit> Edits { get; set; }
        public DbSet<ChangedEvent> ChangedEvents { get; set; }
        public DbSet<DeletedEvent> DeletedEvents { get; set; }
        public DbSet<DeletedGoing> DeletedGoings { get; set; }
        public DbSet<EditorPickEvent> EditorPickEvents { get; set; }

    }
}
