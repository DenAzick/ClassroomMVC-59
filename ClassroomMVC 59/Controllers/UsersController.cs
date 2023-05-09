using ClassroomData.Entities;
using ClassroomMVC_59.Models;
using ClassroomMVC_59.School;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ClassroomMVC_59.Controllers;

public class UsersController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public UsersController(UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }


    [HttpGet]
    public IActionResult SignUp()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SignUp([FromForm] CreateUserDto createUserDto)
    {
        if (!ModelState.IsValid)
        {
            return View(createUserDto);
        }

        var user = new User()
        {
            FirstName = createUserDto.FirstName,
            LastName = createUserDto.LastName,
            PhoneNumber = createUserDto.PhoneNumber,
            UserName = createUserDto.Username
        };
        if (createUserDto.Photo != null)
        {
            user.PhotoUrl = await FileHelper.SaveUserFile(createUserDto.Photo);
        }

        var result = await _userManager.CreateAsync(user, createUserDto.Password);

        if (!result.Succeeded)
        {
            ModelState.AddModelError("Username", result.Errors.First().Description);
            return View();
        }

        await _signInManager.SignInAsync(user, isPersistent: true);

        return RedirectToAction("Profile");
    }


    [HttpGet]
    public IActionResult SignIn()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SignIn(SignInDto signInDto)
    {
        var result = await _signInManager.PasswordSignInAsync(signInDto.Username, signInDto.Password, true, false);

        if (!result.Succeeded)
        {
            ModelState.AddModelError("Username", "Username or Password is incorrect.");
            return View();
        }

        return RedirectToAction("Profile");
    }



    public async Task<IActionResult> EditUser()
    {
        var user = await _userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));

        return View(new UpdateUserDto()
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Username = user.UserName,
            PhoneNumber = user.PhoneNumber
        });
    }

    [HttpPost]
    public async Task<IActionResult> EditUser(Guid id, UpdateUserDto updateUserDto)
    {
        var user = await _userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));

        user.FirstName = updateUserDto.FirstName;
        user.LastName = updateUserDto.LastName;
        user.PhoneNumber = updateUserDto.PhoneNumber;
        user.UserName = updateUserDto.Username;

        if (updateUserDto.Photo != null)
        {
            user.PhotoUrl = await FileHelper.SaveUserFile(updateUserDto.Photo);
        }
        await _userManager.UpdateAsync(user);

        return RedirectToAction("Profile", new { id = user.Id });
    }




    [Authorize]
    public async Task<IActionResult> Profile()
    {
        var user = await _userManager.GetUserAsync(User);
        return View(user);
    }



    public async Task<IActionResult> LogOut()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("SignIn", "Users");
    }






}
