using ClassroomData.Entities;
using ClassroomMVC_59.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
        
        var user = new User()   
        { 
            FirstName = createUserDto.FirstName,
            LastName = createUserDto.LastName,
            PhoneNumber = createUserDto.PhoneNumber,
            UserName = createUserDto.Username
        };
        
        var result = await _userManager.CreateAsync(user, createUserDto.Password);

        if (!ModelState.IsValid)
        {
            return View(createUserDto);
        }
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

        if (result.Succeeded)
        {
            return RedirectToAction("Index", "Home");
        }
        else
        {
            ModelState.AddModelError("", "Неправильный логин и (или) пароль");
        }
        return View(signInDto);

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
