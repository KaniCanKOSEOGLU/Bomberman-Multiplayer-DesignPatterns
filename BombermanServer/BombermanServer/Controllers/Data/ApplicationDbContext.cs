using Microsoft.EntityFrameworkCore;
using BombermanServer.Models;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<HighScore> HighScores { get; set; }
}