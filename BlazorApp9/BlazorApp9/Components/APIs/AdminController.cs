using BlazorApp9.Client;
using Microsoft.AspNetCore.Mvc;

namespace BlazorApp9.Components.APIs;

// WEQ
[Route("api/[controller]")]
[ApiController]
//[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly BlazorApp9.Client.APIs.IAdminService _adminService;

    public AdminController(BlazorApp9.Client.APIs.IAdminService adminService)
    {
        _adminService = adminService;
    }

    [HttpGet]
    public async Task<ActionResult<List<UserInfo>>> Get()
    {
        return Ok(await _adminService.GetAllUsers());
    }


    [HttpGet("GetAllUsers")]
    public async Task<ActionResult<List<UserInfo>>> GetAllUsers()
    {
        return Ok(await _adminService.GetAllUsers());
    }

    [HttpGet("GetUser/{id}")]
    public async Task<ActionResult<UserInfo>> GetUser(string id)
    {
        return Ok(await _adminService.GetUser(id));
    }
}

