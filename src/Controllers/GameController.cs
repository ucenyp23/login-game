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

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Questionnaire()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Profile()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Win()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(string name, string password, string password_control)
        {
            ViewData["error"] = "";

            if (string.IsNullOrEmpty(name))
            {
                ViewData["error"] = "Username wasn't inputed.";
                return View();
            }
            
            if (string.IsNullOrEmpty(password))
            {
                ViewData["error"] = "Password wasn't inputed.";
                return View();
            }

            if (password != password_control)
            {
                ViewData["error"] = "Passwords don't match.";
                return View();
            }
            
            if (_context.Games.Any(u => u.Username == name))
            {
                ViewData["error"] = "User with this name alredy exists.";
                return View();
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            var newUser = new Game
            {
                Username = name,
                Password = hashedPassword
            };

            _context.Games.Add(newUser);
            _context.SaveChanges();

            return RedirectToAction("Login");
        }

        [HttpPost]
        public IActionResult Login(string name, string password)
        {
            ViewData["error"] = "";

            if (string.IsNullOrEmpty(name))
            {
                ViewData["error"] = "Username wasn't entered.";
                return View();
            }

            if (string.IsNullOrEmpty(password))
            {
                ViewData["error"] = "Password wasn't entered.";
                return View();
            }

            if (!_context.Games.Any(u => u.Username == name))
            {
                ViewData["error"] = "This user does not exist.";
                return View();
            }

            var user = _context.Games.First(u => u.Username == name);

            if (!BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                ViewData["error"] = "Invalid password.";
                return View();
            }

            HttpContext.Session.SetInt32("UserId", user.Id);

            return RedirectToAction("Questionnaire");
        }

        [HttpPost]
        public IActionResult Questionnaire(string birthday, string gender, string sec0, string sec1, string sec2)
        {
            if (HttpContext.Session.GetInt32("UserID") == null)
            {
                return RedirectToAction("Login");
            }

            ViewData["error"] = "";

            if (string.IsNullOrEmpty(birthday))
            {
                ViewData["error"] = "Date of birth wasn't entered.";
                return View();
            }

            if (string.IsNullOrEmpty(gender))
            {
                ViewData["error"] = "Gender wasn't entered.";
                return View();
            }

            if (string.IsNullOrEmpty(sec0))
            {
                ViewData["error"] = "Security question 1 wasn't entered.";
                return View();
            }

            if (string.IsNullOrEmpty(sec1))
            {
                ViewData["error"] = "Security question 2 wasn't entered.";
                return View();
            }

            if (string.IsNullOrEmpty(sec2))
            {
                ViewData["error"] = "Security question 3 wasn't entered.";
                return View();
            }

            HttpContext.Session.SetInt32("Questionnare", 1);

            return RedirectToAction("Profile");
        }

        [HttpPost]
        public IActionResult Profile(string button)
        {
            if (HttpContext.Session.GetInt32("UserID") == null)
            {
                return RedirectToAction("Login");
            }

            if (HttpContext.Session.GetInt32("Questionnare") == 1)
            {
                return RedirectToAction("Questionnare");
            }

            return RedirectToAction("Win");
        }
    }
}
