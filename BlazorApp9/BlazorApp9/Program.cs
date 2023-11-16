using BlazorApp9.Client.Pages;
using BlazorApp9.Components;
using BlazorApp9.Components.Account;
using BlazorApp9.Components.APIs;
using BlazorApp9.Data;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, PersistingRevalidatingAuthenticationStateProvider>();
// WEQ
builder.Services.AddScoped<BlazorApp9.Client.APIs.IAdminService, AdminService>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    //.AddGoogle(options =>
    //{
    //    options.ClientId = "";
    //    options.ClientSecret = "";
    //})
    .AddIdentityCookies();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Counter).Assembly);

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

// add admin role
using var scope = app.Services.CreateScope();
var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
var adminRole = await roleManager.FindByNameAsync("Admin");
if (adminRole == null)
{
    adminRole = new IdentityRole("Admin");
    await roleManager.CreateAsync(adminRole);
}

var adminUser = await userManager.FindByNameAsync("admin@email.com");
if (adminUser == null)
{
    adminUser = new ApplicationUser
    {
        UserName = "admin@email.com",
        Email = "admin@email.com",
        EmailConfirmed = true
    };

    await userManager.CreateAsync(adminUser, "Admin123!");
}

if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
{
    await userManager.AddToRoleAsync(adminUser, "Admin");
}

// add admin role to external google user kylesharplol@gmail.com
var googleUser = await userManager.FindByNameAsync("kylesharplol@gmail.com");
if (googleUser != null)
{
    if (!await userManager.IsInRoleAsync(googleUser, "Admin"))
    {
        await userManager.AddToRoleAsync(googleUser, "Admin");
    }
}

app.Run();
