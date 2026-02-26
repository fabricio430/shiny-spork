using DevFreela.API.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace DevFreela.API.Database;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Skill>()
            .HasKey(s => s.Id);

        modelBuilder.Entity<User>()
            .HasKey(u => u.Id);

        modelBuilder.Entity<User>()
            .HasMany(u => u.Skills)
            .WithMany();

        modelBuilder.Entity<Project>()
            .HasKey(p => p.Id);

        modelBuilder.Entity<Project>()
            .HasMany(p => p.RequiredSkills)
            .WithMany();

        modelBuilder.Entity<Project>()
            .HasOne(p => p.Client)
            .WithMany()
            .HasForeignKey(p => p.ClientId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Project>()
            .HasOne(p => p.Freelancer)
            .WithMany()
            .HasForeignKey(p => p.FreelancerId)
            .OnDelete(DeleteBehavior.Restrict);

        base.OnModelCreating(modelBuilder);
    }
    public DbSet<Comment> Comments { get; internal set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Skill> Skills { get; set; }
    public DbSet<User> Users { get; set; }
}
