using ApiProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ApiProject.Infrastructure.Context;

public class ApiProjectDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    public ApiProjectDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_configuration.GetConnectionString("WebApplicationDatabase"), o =>
        {
            o.MigrationsHistoryTable("__EFMigrationsHistory", "server_project_domain");
        });
    }
    public DbSet<User> Users { get; set; }
    public DbSet<ClassRoom> Classrooms { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(_configuration.GetConnectionString("ServerProjectSchema"));

        modelBuilder.Entity<User>().UseTptMappingStrategy();
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
        });
    }


}
