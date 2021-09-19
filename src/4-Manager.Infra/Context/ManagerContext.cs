using Manager.Domain.Entities;
using Manager.Infra.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Manager.Infra.Context
{
  public class ManagerContext : DbContext
  {
    public ManagerContext()
    {
    }

    public ManagerContext(DbContextOptions<ManagerContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      const string uri = @"Server=localhost;Database=USER_MANAGER;User Id=SA;Password=xmm$tzb$J61;";
      optionsBuilder.UseSqlServer(uri);
    }
    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      builder.ApplyConfiguration(new UserMap());
    }
  }
}