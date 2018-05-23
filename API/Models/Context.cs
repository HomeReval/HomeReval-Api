using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class Context : DbContext
    {

        public Context(DbContextOptions<Context> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserGroup> UserGroups {get; set; }
        public DbSet<UserPhysio> UserPhysios { get; set; }
        public DbSet<UserExercise> UserExercises { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<ExercisePlanning> ExercisePlannings { get; set; }
        public DbSet<ExerciseResult> ExerciseResults { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserPhysio>()
                .HasKey(c => new { c.User_ID, c.Physio_ID});
        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<User>()
        //        .HasOne(p => p.UserGroup)
        //        .WithMany(b => b.Users)
        //        .HasForeignKey(p => p.UserGroup_ID);
        //}

    }
}
