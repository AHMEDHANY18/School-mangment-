using Microsoft.EntityFrameworkCore;

namespace shoolnew.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // DbSet properties for your entities
        public DbSet<student> Students { get; set; }
        public DbSet<teacher> Teachers { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<AssignmentSubmission> AssignmentSubmissions { get; set; }
        public DbSet<classes> Classes { get; set; }
        public DbSet<classe_recource> classe_Recources { get; set; }
        public DbSet<classStudent> ClassStudents { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<LoginUser> LoginUsers { get; set; }
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
     //   {
            // Configure additional model settings if needed
         //   base.OnModelCreating(modelBuilder);
       // }
    }
}