using ClassroomData.Context;
using ClassroomData.Entities;

using ClassroomMVC_59.Models;
using ClassroomMVC_59.School;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ClassroomMVC_59.Controllers
{
    public class SchoolsController : Controller
    {
        private readonly AppDbContext _context;
        public SchoolsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult CreateSchool()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateSchool([FromForm] CreateSchoolDto createSchoolDto)
        {
            var school = new ClassroomData.Entities.School()
            {
                Name = createSchoolDto.Name,
                Description = createSchoolDto.Description,
            };

            if (createSchoolDto.Photo != null)
            {
                school.PhotoUrlSchool = await FileHelper.SaveSchoolFile(createSchoolDto.Photo);
            }

            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            school.Users = new List<UserSchool>()
            {
                new UserSchool
                {
                    UserId = userId,
                    Type = EUserSchool.Creator
                }
            };

            _context.Schools.Add(school);

            await _context.SaveChangesAsync();

            return RedirectToAction("Profile", "Users");
        }


    }
}
