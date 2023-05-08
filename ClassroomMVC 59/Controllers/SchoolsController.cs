using ClassroomData.Context;
using ClassroomData.Entities;
using ClassroomMVC_59.Helpers;
using ClassroomMVC_59.Models;
using ClassroomMVC_59.School;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClassroomMVC_59.Controllers;

[Authorize]
public class SchoolsController : Controller
{
    private readonly AppDbContext _context;
    private readonly UserProvider _userProvider;
    public SchoolsController(AppDbContext context, UserProvider userProvider)
    {
        _context = context;
        _userProvider = userProvider;
    }

    public async Task<IActionResult> Index()
    {
        var schools = await _context.Schools
            .Include(school => school.Users)
            .ToListAsync();

        return View(schools);
    }


    [HttpGet]
    public IActionResult CreateSchool()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateSchool([FromForm] CreateSchoolDto createSchoolDto)
    {
        if (!ModelState.IsValid)
        {
            return View(createSchoolDto);
        }

        var school = new ClassroomData.Entities.School()
        {
            Name = createSchoolDto.Name,
            Description = createSchoolDto.Description,
        };

        if (createSchoolDto.Photo != null)
        {
            school.PhotoUrlSchool = await FileHelper.SaveSchoolFile(createSchoolDto.Photo);
        }

       // var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));



        school.Users = new List<UserSchool>()
        {
            new UserSchool
            {
                UserId = _userProvider.UserId,
                Type = EUserSchool.Creator
            }
        };

        _context.Schools.Add(school);

        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> GetSchoolById(Guid id)
    {
        var school = await _context.Schools
            .Include(school => school.Users)
            .ThenInclude(users => users.User)
            .FirstOrDefaultAsync(x => x.Id == id);



        return View(school);
    }

    public async Task<IActionResult> JoinSchool(Guid id)
    {
        var school = await _context.Schools
            .Include(school => school.Users)
            .ThenInclude(users => users.User)
            .FirstOrDefaultAsync(s => s.Id == id);

        var userId = _userProvider.UserId;
        var isUserExistsInSchool = school.Users.Any(x => x.UserId == userId);
        if (!isUserExistsInSchool)
        {
            school.Users.Add(
                new UserSchool
                {
                    UserId = userId,
                    Type = EUserSchool.Student
                });
        }
        await _context.SaveChangesAsync();

        return RedirectToAction("GetSchoolById", new { id = school.Id});
    }

    public async Task<IActionResult> UpdateSchool(Guid id)
    {
        var school = await _context.Schools
            .FirstOrDefaultAsync(s => s.Id == id);

        ViewBag.Id = id;

        return View(new UpdateSchoolDto()
        {
            Name = school.Name,
            Description = school.Description,
            
        });
    }

    [HttpPost]
    public async Task<IActionResult> UpdateSchoolDto(Guid id,[FromForm] UpdateSchoolDto updateSchoolDto)
    {
        var school = await _context.Schools
            .FirstOrDefaultAsync(s => s.Id == id);

        school.Name = updateSchoolDto.Name;
        school.Description = updateSchoolDto.Description;

        if (updateSchoolDto.Photo != null)
        {
            school.PhotoUrlSchool = await FileHelper.SaveSchoolFile(updateSchoolDto.Photo);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction("GetSchoolById", new {id = school.Id});
    }


    public async Task<IActionResult> UpdateUserSchoolRole(Guid schoolId, Guid userId, EUserSchool role)
    {
       var userSchool = await _context.UserSchools
            .FirstOrDefaultAsync(u => u.UserId == userId && u.SchoolId == schoolId);

        if (userSchool.Type != EUserSchool.Creator || role != EUserSchool.Creator)
        {
            userSchool.Type = role;
        }


        await _context.SaveChangesAsync();
        return RedirectToAction("GetSchoolById" , new {id = schoolId });

    }

}
