using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Organiser.Cores;
using Organiser.Cores.Context;
using Organiser.Cores.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddRazorPages();
builder.Services.AddControllers().AddNewtonsoftJson();

//mapper start
var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MappingProfile());
    mc.DestinationMemberNamingConvention = ExactMatchNamingConvention.Instance;
});

IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
//mapper end

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
    options.AddPolicy(name: "OrganiserPolicy",
    policy =>
    {
        policy.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader();
    })
);

builder.Services.AddIdentity<Users, IdentityRole>(config =>
{
    config.SignIn.RequireConfirmedEmail = true;
}).AddEntityFrameworkStores<DataContext>()
  .AddDefaultTokenProviders();

builder.Services.AddScoped<IDataBaseContext, DataBaseContext>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.MapRazorPages();

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();
app.UseStaticFiles();
app.UseRouting();
app.UseCors("OrganiserPolicy");
app.Run();
app.Services.GetRequiredService<DataContext>().Database.EnsureCreated();