using CleanArchBoilerPlateAspNetCore.Core.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CleanArchBoilerPlateAspNetCore.Infra.Data.DbContexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Context> Contexts { get; set; }
        public DbSet<ContextRole> ContextRoles { get; set; }
        public DbSet<ContextRoleUser> ContextRoleUsers { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<FeatureAllocation> FeatureAllocations { get; set; }
        public DbSet<FeatureGroup> FeatureGroups { get; set; }
        public DbSet<FeatureValue> FeatureValues { get; set; }
        public DbSet<Gate> Gates { get; set; }
        public DbSet<Matrix> Matrices { get; set; }
        public DbSet<MatrixColumn> MatrixColumns { get; set; }
        public DbSet<MatrixRow> MatrixRows { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Rule> Rules { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Modify default on delete behavior from "OnCascade" to "ClientNoAction", because it causes an error on SQL Server with the loop references that we got in the DB.
            modelBuilder.Entity<Rule>(r =>
            {
                r.HasOne(r => r.GateWhen)
                .WithOne()
                .OnDelete(DeleteBehavior.ClientNoAction);

                r.HasOne(r => r.GateThen)
                .WithOne()
                .OnDelete(DeleteBehavior.ClientNoAction);
            });

            modelBuilder.Entity<Gate>(g =>
            {
                g.HasOne(g => g.FeatureValue)
                .WithMany()
                .OnDelete(DeleteBehavior.ClientNoAction);
            });

            //Add the unique contraints on some properties.
            modelBuilder.Entity<Feature>().HasIndex(f => f.Name).IsUnique();

            modelBuilder.Entity<FeatureValue>().HasIndex(fv => fv.Name).IsUnique();

            modelBuilder.Entity<Tag>().HasIndex(t => t.Name).IsUnique();

            modelBuilder.Entity<FeatureGroup>().HasIndex(fg => fg.Name).IsUnique();

            modelBuilder.Entity<Context>().HasIndex(c => c.Name).IsUnique();

            modelBuilder.Entity<ContextRole>().HasIndex(cr => cr.Name).IsUnique();

            modelBuilder.Entity<Role>().HasIndex(r => r.Name).IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}
