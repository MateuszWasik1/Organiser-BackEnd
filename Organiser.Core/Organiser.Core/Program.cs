using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Organiser.Core.CQRS.Dispatcher;
using Organiser.Cores;
using Organiser.Cores.Context;
using Organiser.Cores.Entities;
using Organiser.Cores.Models.ViewModels;
using Organiser.Cores.Services;
using Organiser.Cores.Services.EmailSender;
using Organiser.CQRS.Abstraction.Commands;
using Organiser.CQRS.Abstraction.Queries;
using Organiser.CQRS.Resources.Categories.Handlers;
using Organiser.CQRS.Resources.Categories.Queries;
using System.Text;

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
builder.Services.AddScoped<IUserContext, UserContext>();
builder.Services.AddScoped<IDispatcher, Dispatcher>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

//CQRS
#region CQRS
builder.Services.AddScoped<IQueryHandler<GetCategoriesQuery, List<CategoriesViewModel>>, GetCategoriesQueryHandler>();


//builder.Services.AddScoped<IQueryHandler<GetCategoriesQuery, List<CategoriesViewModel>>, GetCategoriesQuery>();
//builder.Services.AddScoped<IQueryHandler<GetCategoriesQuery, List<CategoriesViewModel>>, GetCategoriesQuery>();

//builder.Services.Scan(selector =>
//{
//    selector.FromCallingAssembly()
//            .AddClasses(filter =>
//            {
//                filter.AssignableTo(typeof(IQueryHandler<,>));
//            })
//            .AsImplementedInterfaces()
//            .WithSingletonLifetime();
//});
//builder.Services.Scan(selector =>
//{
//    selector.FromCallingAssembly()
//            .AddClasses(filter =>
//            {
//                filter.AssignableTo(typeof(ICommandHandler<,>));
//            })
//            .AsImplementedInterfaces()
//            .WithSingletonLifetime();
//});

//void AddCommandQueryHandlers(this IServiceCollection services, Type handlerInterface)
//{
//    var handlers = typeof(ServiceExtensions).Assembly.GetTypes()
//        .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerInterface)
//    );

//    foreach (var handler in handlers)
//    {
//        services.AddScoped(handler.GetInterfaces().First(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerInterface), handler);
//    }
//}

//void ConfigureServices(IServiceCollection services)
//{
//    services.AddCommandQueryHandlers(typeof(ICommandHandler<>));
//    services.AddCommandQueryHandlers(typeof(IQueryHandler<,>));
//}
#endregion

//EmailSender
var emailSenderSettings = new EmailSenderSettings();
builder.Configuration.GetSection("EmailSender").Bind(emailSenderSettings);
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddSingleton(emailSenderSettings);

//Authentications
var authenticationSettings = new AuthenticationSettings();
builder.Configuration.GetSection("Authentication").Bind(authenticationSettings);

builder.Services.AddSingleton(authenticationSettings);

builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = "Bearer";
    option.DefaultScheme = "Bearer";
    option.DefaultChallengeScheme = "Bearer";
}).AddJwtBearer(configuration =>
{
    configuration.RequireHttpsMetadata = false;
    configuration.SaveToken = true;
    configuration.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = authenticationSettings.JWTIssuer,
        ValidAudience = authenticationSettings.JWTIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JWTKey)),
    };
});

//Building
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (!app.Environment.IsDevelopment())
{
    //app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.MapRazorPages();
app.UseAuthentication();
app.UseHttpsRedirection();

//app.UseAuthorization();
//app.UseAuthentication();

app.MapControllers();
app.UseStaticFiles();
app.UseRouting();
app.UseCors("OrganiserPolicy");
app.UseAuthorization();
app.Run();
app.Services.GetRequiredService<DataContext>().Database.EnsureCreated();