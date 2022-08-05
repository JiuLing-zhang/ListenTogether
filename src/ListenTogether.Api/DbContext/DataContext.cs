using Microsoft.EntityFrameworkCore;
using ListenTogether.Api.Entities;

namespace ListenTogether.Api.DbContext;
public sealed class DataContext : Microsoft.EntityFrameworkCore.DbContext
{
    public DbSet<UserEntity> Users { get; set; } = null!;
    public DbSet<UserConfigEntity> UserConfigs { get; set; } = null!;
    public DbSet<MusicEntity> Musics { get; set; } = null!;
    public DbSet<MyFavoriteEntity> MyFavorites { get; set; } = null!;
    public DbSet<LogEntity> Logs { get; set; } = null!;
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

    }
}
