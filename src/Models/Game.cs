using System.ComponentModel.DataAnnotations;

namespace login_game.Models
{
    public class Game
    {
        [Key]
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
    }
}
