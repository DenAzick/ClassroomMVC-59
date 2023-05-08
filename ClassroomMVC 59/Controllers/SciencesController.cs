using ClassroomData.Context;
using ClassroomMVC_59.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClassroomData.Entities;
using ClassroomMVC_59.Models;

namespace ClassroomMVC_59.Controllers;

[Authorize]
public class SciencesController : Controller
{
    private readonly AppDbContext _context;
    private readonly UserProvider _userProvider;

    public SciencesController(UserProvider userProvider, AppDbContext context)
    {
        _userProvider = userProvider;
        _context = context;
    }


    public async Task<IActionResult> Index(Guid schoolId, string? name = null, bool orderByUsers = false)
    {
        ViewBag.SchoolId = schoolId;
        ViewBag.OrderByUsers = orderByUsers;

        var query = _context.Sciences
            .Include(s => s.UserSciences);

        if (name != null)
        {
            query = query.Where(s => s.Name.Contains(name))
                .Include(s => s.UserSciences);
        }

        if (orderByUsers)
        {
            var sciences = await query
                .Where(s => s.SchoolId == schoolId)
                .OrderByDescending(s => s.UserSciences.Count).ToListAsync();
            return View(sciences);
        }
        return View(await query.Where(s => s.SchoolId == schoolId).ToListAsync());
    }

    public async Task<IActionResult> GetById(Guid scienceId)
    {
        var science = await _context.Sciences
            .Include(s => s.School)
            .Include(s => s.UserSciences)
            .ThenInclude(s => s.User)
            .FirstOrDefaultAsync(s => s.Id == scienceId);

        return View(science);
    }


    [HttpGet]
    public IActionResult Create(Guid schoolId)
    {
        ViewBag.SchoolId = schoolId;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Guid schoolId, [FromForm] CreateScienceDto createScienceDto)
    {
        if (!ModelState.IsValid)
        {
            return View(createScienceDto);
        }

        if (await _context.Sciences.AnyAsync(s => s.Name == createScienceDto.Name))
        {
            ModelState.AddModelError("Name", "Science name exists in system.");
            return View();
        }

        var school = new Science()
        {
            Name = createScienceDto.Name,
            Description = createScienceDto.Description,
            SchoolId = schoolId
        };


        school.UserSciences = new List<UserScience>
        {
            new UserScience()
            {
                UserId = _userProvider.UserId,
                Type = EUserScience.Teacher
            }
        };

        _context.Sciences.Add(school);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index", new { schoolId = schoolId });
    }

    public IActionResult SendJoinScienceRequest(Guid scienceId)
    {
        ViewBag.ScienceId = scienceId;
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> SendJoinScienceRequest(Guid scienceId, [FromForm] CreateJoinScienceRequestDto requestDto)
    {
        var toUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == requestDto.Username);

        var isExistsPreviewJoinRequest =
            await _context.JoinScienceRequests
                .AnyAsync(r => r.ToUserId == toUser.Id && r.ScienceId == scienceId);

        // science ga qoshilish oldin suralgan bolsa yana suramaslik kerak
        // agar oldin saqlangan bolsa qayta saqlamaslik kerak
        if (isExistsPreviewJoinRequest)
        {
            return RedirectToAction("GetById", new { scienceId });
        }

        var isExistsInScience =
            await _context.UserSciences
                .AnyAsync(u => u.UserId == toUser.Id && u.ScienceId == scienceId);
        // science ga qoshilgan userga yana qoshilish surovini junatmaslik kerak
        if (isExistsInScience)
        {
            return RedirectToAction("GetById", new { scienceId });
        }

        var userId = _userProvider.UserId;

        var joinRequest = new JoinScienceRequest()
        {
            FromUserId = userId,
            ScienceId = scienceId,
            ToUserId = toUser.Id,
            IsJoined = false
        };

        _context.JoinScienceRequests.Add(joinRequest);
        await _context.SaveChangesAsync();

        return RedirectToAction("GetById", new { scienceId });
    }


    public async Task<IActionResult> JoinScience(Guid joinRequestId, bool isJoin)
    {
        var joinRequst = await _context.JoinScienceRequests
            .FirstOrDefaultAsync(r => r.Id == joinRequestId && r.ToUserId == _userProvider.UserId);

        if (isJoin)
        {
            var userScience = new UserScience()
            {
                ScienceId = joinRequst.ScienceId,
                UserId = joinRequst.ToUserId,
                Type = EUserScience.Student
            };

            joinRequst.IsJoined = true;
            _context.UserSciences.Add(userScience);
        }
        else
        {
            _context.JoinScienceRequests.Remove(joinRequst);
        }

        await _context.SaveChangesAsync();

        return RedirectToAction("Profile", "Users");
    }
}
