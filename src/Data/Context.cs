using login_game.Models;
using Microsoft.EntityFrameworkCore;

namespace login_game.Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options) { }

        public DbSet<Game> Games { get; set; }
    }
}