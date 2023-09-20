using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using IdentityUsers.Areas.Identity.Data;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("IdentityUsersContextConnection") ?? throw new InvalidOperationException("Connection string 'IdentityUsersContextConnection' not found.");

builder.Services.AddDbContext<IdentityUsersContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<IdentityUsersContext>()
                .AddDefaultUI()
                .AddDefaultTokenProviders();

builder.Services.AddRazorPages();

//keep the exising code
//Exising code has not been shown here to keep focus on just the new code being added
//You can refer to the complete code on GitHub  

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.MapRazorPages();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();;

app.UseAuthorization();

app.MapRazorPages();

app.Run();
