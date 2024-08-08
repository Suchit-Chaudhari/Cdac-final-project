using Microsoft.EntityFrameworkCore;
using CareerConnect.Models;

public class JobPortalContext : DbContext
{
    public JobPortalContext(DbContextOptions<JobPortalContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<JobSeeker> JobSeekers { get; set; }
    public DbSet<Employer> Employers { get; set; }
    public DbSet<Job> Jobs { get; set; }
    public DbSet<Application> Applications { get; set; }
    public DbSet<Project> Projects { get; set; } // Add DbSet for Projects

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<User>()
            .Property(u => u.UserId)
            .ValueGeneratedOnAdd(); // Ensure UserId is auto-generated

        modelBuilder.Entity<JobSeeker>()
            .HasOne(js => js.User)
            .WithOne(u => u.JobSeeker)
            .HasForeignKey<JobSeeker>(js => js.JobSeekerId); // Link via UserId

        modelBuilder.Entity<Employer>()
            .HasOne(e => e.User)
            .WithOne(u => u.Employer)
            .HasForeignKey<Employer>(e => e.EmployerId); // Link via UserId

        modelBuilder.Entity<Employer>()
            .HasMany(e => e.Jobs)
            .WithOne(j => j.Employer)
            .HasForeignKey(j => j.EmployerId);

        modelBuilder.Entity<Job>()
            .HasOne(j => j.Employer)
            .WithMany(e => e.Jobs)
            .HasForeignKey(j => j.EmployerId);

        modelBuilder.Entity<Application>()
            .HasOne(a => a.Job)
            .WithMany(j => j.Applications)
            .HasForeignKey(a => a.JobId);

        modelBuilder.Entity<Application>()
            .HasOne(a => a.JobSeeker)
            .WithMany(js => js.Applications)
            .HasForeignKey(a => a.JobSeekerId);

        // Configure the relationship between JobSeeker and Project
        modelBuilder.Entity<Project>()
            .HasOne(p => p.JobSeeker)
            .WithMany(js => js.Projects)
            .HasForeignKey(p => p.JobSeekerId);
    }
}
