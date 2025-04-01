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

            Game? existingUsers = _context.Games.FirstOrDefault(u => u.Username == name);

            if (string.IsNullOrEmpty(name))
                ViewData["chyba"] = "Username wasn't inputed.";
            else if (string.IsNullOrEmpty(password))
                ViewData["chyba"] = "Password wasn't inputed.";
            else if (password != password_control)
                ViewData["chyba"] = "Passwords don't match.";
            else if (existingUsers != null)
                ViewData["chyba"] = "User with this name alredy exists.";

            if (ViewData["chyba"]?.ToString() != "")
                return View();

            password = BCrypt.Net.BCrypt.HashPassword(password);

            Game newUser = new Game() { Username = name, Password = password };

            _context.Games.Add(newUser);
            _context.SaveChanges();

            return RedirectToAction("Login");
        }
    }
}
