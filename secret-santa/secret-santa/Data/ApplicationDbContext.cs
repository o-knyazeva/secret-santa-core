using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SecretSanta.Models;
using SecretSanta.Models.Entities;

namespace SecretSanta.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            builder.Entity<Participant>(entity =>
            {
                entity.HasOne(d => d.Group)
                    .WithMany(p => p.Participants)
                    .HasForeignKey(d => d.GroupId);
            });

            builder.Entity<Participant>(entity =>
            {
                entity.HasOne(d => d.User)
                    .WithMany(p => p.Participants)
                    .HasForeignKey(d => d.UserId);
            });
        }

        public DbSet<Group> Groups { get; set; }
        public DbSet<Participant> Participants{ get; set; }
        public DbSet<DrawRule> ParDrawRules { get; set; }
    }
}
