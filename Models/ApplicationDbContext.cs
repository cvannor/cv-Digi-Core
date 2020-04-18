using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
//using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cvDigiCore.Models
{
    public class ApplicationDbContext: IdentityDbContext
    {

        public DbSet<Profile> Profile { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Project> Project { get; set; }
        public DbSet<Images> Images { get; set; }
        public DbSet<Bridge> Bridges { get; set; }



        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bridge>()
        .HasKey(i => new { i.CategoryId, i.ProjectId});
            base.OnModelCreating(modelBuilder);

        }


    }
}
