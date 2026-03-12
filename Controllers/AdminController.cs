using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShadowFile.Data;
using ShadowFile.DTOs;
using ShadowFile.Models;

public class AdminController : Controller
{
    private readonly ShadowFileDbContext _context;

    public AdminController(ShadowFileDbContext context)
    {
        _context = context;
    }

    public IActionResult Dashboard()
    {
        return View();
    }
    
    public async Task<IActionResult> ManageAgents()
    {
        var agents = await _context.Users
            .Include(u => u.Role)
            .ToListAsync();

        return View(agents);
    }
    [HttpGet]
    public IActionResult CreateAgent()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateAgent(CreateUserDto dto)
    {
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);

        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = dto.Username,
            PasswordHash = hashedPassword,
            RoleId = dto.RoleId,
            AgentCode = dto.AgentCode,
            IsActive = true
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return RedirectToAction("ManageAgents");
    }
    //Edit Agent
    [HttpGet]
    public async Task<IActionResult> EditAgent(Guid id)
    {
        var agent = await _context.Users.FindAsync(id);

        return View(agent);
    }
    
    [HttpPost]
    public async Task<IActionResult> EditAgent(User model)
    {
        var agent = await _context.Users.FindAsync(model.Id);

        if (agent == null)
            return NotFound();

        agent.Username = model.Username;
        agent.AgentCode = model.AgentCode;
        agent.IsActive = model.IsActive;

        await _context.SaveChangesAsync();

        return RedirectToAction("ManageAgents");
    }
    //View
    public async Task<IActionResult> AgentDetails(Guid id)
    {
        var agent = await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Id == id);

        return View(agent);
    }
    //Delete Agent
    public async Task<IActionResult> DeleteAgent(Guid id)
    {
        var agent = await _context.Users.FindAsync(id);

        if(agent != null)
        {
            _context.Users.Remove(agent);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction("ManageAgents");
    }
    
}