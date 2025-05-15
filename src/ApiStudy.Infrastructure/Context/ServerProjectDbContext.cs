using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ServerProject.Domain.Entities;

namespace ServerProject.Infrastructure.Context;

public class ServerProjectDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    public ServerProjectDbContext(IConfiguration configuration)
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
