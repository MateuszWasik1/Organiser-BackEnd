using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Organiser.Core;
using Organiser.Core.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddIdentity<Users, IdentityRole>(config =>
{
    config.SignIn.RequireConfirmedEmail = true;
}).AddEntityFrameworkStores<DataContext>()
  .AddDefaultTokenProviders();

builder.Services.AddRazorPages();

builder.Services.AddCors(options => 
    options.AddPolicy(name: "OrganiserPolicy", 
    policy =>
    {
        policy.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader();
    })
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseCors("OrganiserPolicy");
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();

app.Run();

app.UseAuthorization();
app.UseAuthentication();

app.Services.GetRequiredService<DataContext>().Database.EnsureCreated();