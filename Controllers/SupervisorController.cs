using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ShadowFile.Controllers;

    public class SupervisorController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
    }