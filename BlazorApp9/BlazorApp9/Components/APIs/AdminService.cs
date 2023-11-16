using BlazorApp9.Client;
using BlazorApp9.Data;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp9.Components.APIs;
// WEQ
public class AdminService : BlazorApp9.Client.APIs.IAdminService
{
    private readonly ApplicationDbContext _context;

    public AdminService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<UserInfo>> GetAllUsers()
    {
        List<UserInfo> users = new();
        foreach (var user in await _context.Users.ToListAsync())
        {
            var roles = await _context.UserRoles.Where(ur => ur.UserId == user.Id).Select(ur => ur.RoleId).ToListAsync();

            users.Add(new UserInfo
            {
                UserId = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Roles = roles,
                IsAdmin = roles.Contains("Admin"),
            });
        }

        return users;
    }

    public async Task<UserInfo> GetUser(string id)
    {
        var user = await _context.Users.FindAsync(id);
        var roles = await _context.UserRoles.Where(ur => ur.UserId == user.Id).Select(ur => ur.RoleId).ToListAsync();

        return new UserInfo
        {
            UserId = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            Roles = roles,
            IsAdmin = roles.Contains("Admin"),
        };
    }
}
