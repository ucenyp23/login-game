namespace login_game.Models
{
    public class PersonalInfo
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string? Birthday { get; set; }
        public string? Gender { get; set; }
        public string? SecurityQuestion0 { get; set; }
        public string? SecurityQuestion1 { get; set; }
        public string? SecurityQuestion2 { get; set; }
    }
}
