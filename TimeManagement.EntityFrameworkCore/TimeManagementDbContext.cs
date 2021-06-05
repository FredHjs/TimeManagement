using Microsoft.EntityFrameworkCore;
using TimeManagement.Entities;

namespace TimeManagement.EntityFrameworkCore
{
    public class TimeManagementDbContext : DbContext
    {
        public TimeManagementDbContext(DbContextOptions<TimeManagementDbContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB; Initial Catalog=TimeManagement");
        }

        public DbSet<Term> Terms { get; set; }

        public DbSet<Course> Courses { get; set; }

        public DbSet<Event> Events { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Term>(b =>
            {
                b.ToTable(nameof(Terms));

                b.HasKey(a => a.ID);
                b.HasIndex(a => a.TermNumber);

                b.Property(a => a.ID).HasMaxLength(36);
                b.Property(a => a.Name).HasMaxLength(36).IsRequired();
                b.Property(a => a.TermNumber).HasMaxLength(5).IsRequired();
                b.Property(a => a.StartTime).IsRequired();
                b.Property(a => a.EndTime).IsRequired();
            });

            builder.Entity<Course>(b =>
            {
                b.ToTable(nameof(Courses));

                b.HasKey(a => a.ID);
                b.HasOne(a => a.Term).WithMany(t => t.Courses).HasForeignKey(a => a.TermID).IsRequired();

                b.HasIndex(a => a.Name);

                b.Property(a => a.ID).HasMaxLength(36);
                b.Property(a => a.Name).HasMaxLength(20).IsRequired();
                b.Property(a => a.Discription).HasMaxLength(100);
                b.Property(a => a.Grade).HasMaxLength(10);
                b.Property(a => a.TermID).HasMaxLength(36).IsRequired();
            });

            builder.Entity<Event>(b =>
            {
                b.ToTable(nameof(Events));

                b.HasKey(a => a.ID);
                b.HasOne(a => a.Course).WithMany(c => c.Events).HasForeignKey(a => a.CourseID).IsRequired();

                b.Property(a => a.ID).HasMaxLength(36);
                b.Property(a => a.Name).HasMaxLength(50).IsRequired();
                b.Property(a => a.Description).HasMaxLength(100);
                b.Property(a => a.Time).IsRequired();
                //b.Property(a => a.RepeatDays);
                b.Property(a => a.CourseID).HasMaxLength(36).IsRequired();
            });
        }

    }
}
