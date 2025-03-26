using login_game.Models;
using login_game.Data;
using Microsoft.AspNetCore.Mvc;

namespace login_game.Controllers
{
    public class GameController : Controller
    {
        private readonly Context _context;

        public GameController(Context context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(string name, string password, string password_control)
        {
            ViewData["chyba"] = "";

            if (name == null || name.Length == 0)
                ViewData["chyba"] += "Username wasn't inputed.";
            if (password == null || password.Length == 0)
                ViewData["chyba"] += "Password wasn't inputed.";
            if (password != password_control)
                ViewData["chyba"] += "Passwords don't match.";

            Game? existingUsers = _context.
                Game.FirstOrDefault(u => u.Username == name);

            if (existingUsers != null)
                ViewData["chyba"] = "Uživatel s tímto jménem již existuje.";

            if (ViewData["chyba"] != "")
                return View();

            password = BCrypt.Net.BCrypt.HashPassword(password);

            Game newUser = new Game() { Username = name, Password = password };

            _context.Games.Add(newUser);
            _context.SaveChanges();

            return RedirectToAction("Login");
        }
    }
}
