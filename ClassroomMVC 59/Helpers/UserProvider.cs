﻿using System.Security.Claims;

namespace ClassroomMVC_59.Helpers;

public class UserProvider
{
    private readonly IHttpContextAccessor _contextAccessor;
    private ClaimsPrincipal User => _contextAccessor.HttpContext!.User;

    public UserProvider(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    private Guid? _userId;

    public Guid UserId =>
        _userId ??= Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
    
}
