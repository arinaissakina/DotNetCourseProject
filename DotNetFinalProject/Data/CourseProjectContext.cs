using System.Text.RegularExpressions;
using DotNetFinalProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DotNetFinalProject.Data
{
    public class CourseProjectContext : IdentityDbContext<User>
    {
        public CourseProjectContext(DbContextOptions<CourseProjectContext> options) : base(options)
        {
        }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Specialty> Specialties { get; set; }
        public DbSet<User> AllUsers { get; set; }
        public DbSet<SpecialtyUser> SpecialityUsers { get; set; }
        public DbSet<ProjectMember> ProjectMembers { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            

            // One to Many
            
            modelBuilder
                .Entity<Project>()
                .HasOne(p => p.Owner)
                .WithMany(r => r.ProjectOwner);
            

            //Many to Many
            
            modelBuilder.Entity<ProjectMember>().HasKey(sc => new { sc.ProjectId, sc.MemberId });

            modelBuilder.Entity<ProjectMember>()
                .HasOne(sc => sc.Project)
                .WithMany(p => p.ProjectMembers)
                .HasForeignKey(sc => sc.ProjectId);

            modelBuilder.Entity<ProjectMember>()
                .HasOne(sc => sc.Member)
                .WithMany(p => p.ProjectMember)
                .HasForeignKey(sc => sc.MemberId);
            
            
            
            modelBuilder.Entity<SpecialtyUser>().HasKey(sc => new { sc.SpecialtyId, sc.UserId });

            modelBuilder.Entity<SpecialtyUser>()
                .HasOne(sc => sc.Specialty)
                .WithMany(p => p.SpecialityUsers)
                .HasForeignKey(sc => sc.SpecialtyId);

            modelBuilder.Entity<SpecialtyUser>()
                .HasOne(sc => sc.User)
                .WithMany(p => p.Specialties)
                .HasForeignKey(sc => sc.UserId);

        }

    }
}
        
