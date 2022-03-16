using Microsoft.EntityFrameworkCore;
using MusicPlayerOnline.Api.Entities;

namespace MusicPlayerOnline.Api.DbContext
{
    public sealed class DataContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DbSet<UserEntity> Users { get; set; } = null!;
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
    }
}
