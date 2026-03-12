using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using ShadowFile.DTOs;
using ShadowFile.Interfaces;

namespace ShadowFile.Controllers
{
    public class AuthController : Controller
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: /Auth/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        
        public IActionResult SignUp()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> SignUp(CreateUserDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var result = await _userService.CreateUserAsync(dto);

            if (!result)
            {
                ModelState.AddModelError("", "Unable to create user");
                return View(dto);
            }

            return RedirectToAction("Login");
        }
        
        // FORGOT PASSWORD (GET)
        public IActionResult ForgotPassword()
        {
            return View();
        }

// FORGOT PASSWORD (POST)
        [HttpPost]
        public IActionResult ForgotPassword(string email, string agentCode)
        {
            // TODO: check in database if email + agentCode match

            if (email == "test@gmail.com" && agentCode == "1234") // temp logic
            {
                return RedirectToAction("ResetPassword");
            }

            ViewBag.Error = "Invalid Email or Agent Code";
            return View();
        }


// RESET PASSWORD (GET)
        public IActionResult ResetPassword()
        {
            return View();
        }

// RESET PASSWORD (POST)
        [HttpPost]
        public IActionResult ResetPassword(string newPassword, string confirmPassword)
        {
            if (newPassword != confirmPassword)
            {
                ViewBag.Error = "Passwords do not match";
                return View();
            }

            // TODO: Update password in database

            return RedirectToAction("Login");
        }

        // POST: /Auth/Login
        [HttpPost]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var user = await _userService.AuthenticateAsync(dto);

            if (user == null)
            {
                ViewBag.Error = "Invalid username or password";
                return View(dto);
            }

            // Save session
            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetString("Role", user.Role.RoleName);

            // Create claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role.RoleName),
                new Claim("AgentCode", user.AgentCode ?? "")
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));

            // ROLE BASED REDIRECT
            if (user.Role.RoleName == "Admin")
                return RedirectToAction("Dashboard", "Admin");

            if (user.Role.RoleName == "Detective")
                return RedirectToAction("Dashboard", "Detective");

            if (user.Role.RoleName == "Supervisor")
                return RedirectToAction("Dashboard", "Supervisor");

            return RedirectToAction("Login");
        }

        //Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            Response.Cookies.Delete(".AspNetCore.Session");

            return RedirectToAction("Login","Auth");
        }
        
    }
}