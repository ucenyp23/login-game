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

        private bool IsValidUser()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var userPassword = HttpContext.Session.GetString("UserPassword");

            if (userId == null) return false;

            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            return user != null && user.Password == userPassword;
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
            if (!IsValidUser()) return RedirectToAction("Login");

            var UserId = HttpContext.Session.GetInt32("UserId");
            if (_context.PersonalInfos.Any(u => u.UserId == UserId))
            {
                return RedirectToAction("Profile");
            }

            return View();
        }

        [HttpGet]
        public IActionResult Profile()
        {
            if (!IsValidUser()) return RedirectToAction("Login");

            return View();
        }

        public IActionResult Win()
        {
            if (!IsValidUser()) return RedirectToAction("Login");

            var random = new Random();
            int n = random.Next(0, 1000);

            ViewData["error"] = n.ToString();

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
            
            if (_context.Users.Any(u => u.Username == name))
            {
                ViewData["error"] = "User with this username alredy exists.";
                return View();
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            var newUser = new User
            {
                Username = name,
                Password = hashedPassword
            };

            _context.Users.Add(newUser);
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

            if (!_context.Users.Any(u => u.Username == name))
            {
                ViewData["error"] = "This username doesn't exist.";
                return View();
            }

            var User = _context.Users.First(u => u.Username == name);

            if (!BCrypt.Net.BCrypt.Verify(password, User.Password))
            {
                ViewData["error"] = "Password wasn't correct.";
                return View();
            }

            HttpContext.Session.SetInt32("UserId", User.Id);
            HttpContext.Session.SetString("UserPassword", User.Password ?? string.Empty);

            return RedirectToAction("Questionnaire");
        }

        [HttpPost]
        public IActionResult Questionnaire(string birthday, string gender, string sec0, string sec1, string sec2)
        {
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

            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login");
            }

            var personalInfo = new PersonalInfo
            {
                UserId = userId.Value,
                Birthday = birthday,
                Gender = gender,
                SecurityQuestion0 = sec0,
                SecurityQuestion1 = sec1,
                SecurityQuestion2 = sec2
            };

            _context.PersonalInfos.Add(personalInfo);
            _context.SaveChanges();

            return RedirectToAction("Profile");
        }

        [HttpPost]
        public IActionResult Profile(string button)
        {
            return RedirectToAction("Win");
        }
    }
}
