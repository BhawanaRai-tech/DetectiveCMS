using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShadowFile.Data;

namespace ShadowFile.Controllers;

public class DetectiveController : Controller
{

    public IActionResult Dashboard()
    {
        return View();
    }
}