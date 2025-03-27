using Microsoft.EntityFrameworkCore;

public class GameDbContext : DbContext
{
    public DbSet<BoardState> BoardStates { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite("Data Source=gameoflife.db");
    }
}

public class BoardState
{
    public Guid Id { get; set; }
    public string BoardJson { get; set; }
}