using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var context = new ModuleDbContext(new DbContextOptionsBuilder<ModuleDbContext>()
               .UseNpgsql("Host=localhost;Port=5432;Database=sandBox")
               .Options);
            var users = context.Entities.Include(x => x.List).Select(x => new { x.Id, Names = x.List }).GroupBy(x => x.Id, (x, y) => new { x }).ToList();
        }
    }

    public class ModuleDbContext : DbContext
    {
        public ModuleDbContext(DbContextOptions<ModuleDbContext> options) : base(options)
        {
        }

        public DbSet<Entity> Entities { get; set; }
        public DbSet<ListEntity> ListEntities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ListEntity>().HasOne(x => x.Entity).WithMany(x => x.List);
        }
    }

    public class Entity
    {
        [Key]
        public int Id { get; set; }
        public List<ListEntity> List { get; set; }
    }
    public class ListEntity
    {
        [Key]
        public int Id { get; set; }
        public Entity Entity { get; set; }
    }
}
