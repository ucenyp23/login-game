using login_game.Models;
using Microsoft.EntityFrameworkCore;

namespace login_game.Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Questionnaire> Questionnaires { get; set; }
        public DbSet<PersonalInfo> PersonalInfos { get; set; }
    }
}