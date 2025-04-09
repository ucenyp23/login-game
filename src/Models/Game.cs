namespace login_game.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
    }

    public class Questionnaire
    {
        public int Id { get; set; }
        public bool Active = false;
    }

    public class PersonalInfo
    {
        public int Id { get; set; }
        public string? Birthday { get; set; }
        public string? Gender { get; set; }
        public string? SecurityQuestion0 { get; set; }
        public string? SecurityQuestion1 { get; set; }
        public string? SecurityQuestion2 { get; set; }
    }
}
