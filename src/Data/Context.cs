using Microsoft.EntityFrameworkCore;
using login_game.Models;

namespace login_game.Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options)
            : base(options)
        { }

        public DbSet<Game> Users { get; set; }
    }
}