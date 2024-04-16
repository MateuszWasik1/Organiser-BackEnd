using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Organiser.Core.CQRS.Dispatcher;
using Organiser.Core.CQRS.Resources.Accounts.Commands;
using Organiser.Core.CQRS.Resources.Accounts.Handlers;
using Organiser.Core.CQRS.Resources.Accounts.Queries;
using Organiser.Core.CQRS.Resources.Bugs.Bugs.Commands;
using Organiser.Core.CQRS.Resources.Bugs.Bugs.Handlers;
using Organiser.Core.CQRS.Resources.Bugs.Bugs.Queries;
using Organiser.Core.CQRS.Resources.Bugs.BugsNotes.Commands;
using Organiser.Core.CQRS.Resources.Bugs.BugsNotes.Handlers;
using Organiser.Core.CQRS.Resources.Bugs.BugsNotes.Queries;
using Organiser.Core.CQRS.Resources.Notes.Commands;
using Organiser.Core.CQRS.Resources.Notes.Handlers;
using Organiser.Core.CQRS.Resources.Notes.Queries;
using Organiser.Core.CQRS.Resources.Roles.Handlers;
using Organiser.Core.CQRS.Resources.Roles.Queries;
using Organiser.Core.CQRS.Resources.Savings.Commands;
using Organiser.Core.CQRS.Resources.Savings.Handlers;
using Organiser.Core.CQRS.Resources.Savings.Queries;
using Organiser.Core.CQRS.Resources.Stats.Handlers;
using Organiser.Core.CQRS.Resources.Stats.Queries;
using Organiser.Core.CQRS.Resources.Tasks.Tasks.Commands;
using Organiser.Core.CQRS.Resources.Tasks.Tasks.Handlers;
using Organiser.Core.CQRS.Resources.Tasks.Tasks.Queries;
using Organiser.Core.CQRS.Resources.Tasks.TasksNotes.Commands;
using Organiser.Core.CQRS.Resources.Tasks.TasksNotes.Handlers;
using Organiser.Core.CQRS.Resources.Tasks.TasksNotes.Queries;
using Organiser.Core.CQRS.Resources.Tasks.TasksSubTasks.Commands;
using Organiser.Core.CQRS.Resources.Tasks.TasksSubTasks.Handlers;
using Organiser.Core.CQRS.Resources.Tasks.TasksSubTasks.Queries;
using Organiser.Core.CQRS.Resources.User.Commands;
using Organiser.Core.CQRS.Resources.User.Handlers;
using Organiser.Core.CQRS.Resources.User.Queries;
using Organiser.Core.Models.ViewModels.BugsViewModels;
using Organiser.Core.Models.ViewModels.NotesViewModels;
using Organiser.Core.Models.ViewModels.TasksViewModels;
using Organiser.Cores;
using Organiser.Cores.Context;
using Organiser.Cores.Entities;
using Organiser.Cores.Models.ViewModels;
using Organiser.Cores.Models.ViewModels.CategoriesViewModel;
using Organiser.Cores.Models.ViewModels.SavingsViewModels;
using Organiser.Cores.Models.ViewModels.StatsViewModels;
using Organiser.Cores.Models.ViewModels.UserViewModels;
using Organiser.Cores.Services;
using Organiser.Cores.Services.EmailSender;
using Organiser.CQRS.Abstraction.Commands;
using Organiser.CQRS.Abstraction.Queries;
using Organiser.CQRS.Resources.Categories.Commands;
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
//Accounts
builder.Services.AddScoped<IQueryHandler<LoginQuery, string>, LoginQueryHandler>();

builder.Services.AddScoped<ICommandHandler<RegisterUserCommand>, RegisterUserCommandHandler>();

//Bugs
builder.Services.AddScoped<IQueryHandler<GetBugQuery, BugViewModel>, GetBugQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetBugsQuery, GetBugsViewModel>, GetBugsQueryHandler>();

builder.Services.AddScoped<ICommandHandler<SaveBugCommand>, SaveBugsCommandHandler>();
builder.Services.AddScoped<ICommandHandler<ChangeBugStatusCommand>, ChangeBugStatusCommandHandler>();

//BugsNotes
builder.Services.AddScoped<IQueryHandler<GetBugNotesQuery, GetBugsNotesViewModel>, GetBugNotesQueryHandler>();

builder.Services.AddScoped<ICommandHandler<SaveBugNoteCommand>, SaveBugNoteCommandHandler>();

//Categories
builder.Services.AddScoped<IQueryHandler<GetCategoryQuery, CategoryViewModel>, GetCategoryQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetCategoriesQuery, GetCategoriesViewModel>, GetCategoriesQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetCategoriesForFilterQuery, List<CategoriesForFiltersViewModel>>, GetCategoriesForFilterQueryHandler>();

builder.Services.AddScoped<ICommandHandler<AddCategoryCommand>, AddCategoryCommandHandler>();
builder.Services.AddScoped<ICommandHandler<UpdateCategoryCommand>, UpdateCategoryCommandHandler>();
builder.Services.AddScoped<ICommandHandler<DeleteCategoriesCommand>, DeleteCategoriesCommandHandler>();

//Roles
builder.Services.AddScoped<IQueryHandler<GetUserRolesQuery, RolesViewModel>, GetUserRolesQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetIsUserAdminQuery, bool>, GetIsUserAdminQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetIsUserSupportQuery, bool>, GetIsUserSupportQueryHandler>();

//Savings
builder.Services.AddScoped<IQueryHandler<GetSavingQuery, SavingViewModel>, GetSavingQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetSavingsQuery, GetSavingsViewModel>, GetSavingsQueryHandler>();

builder.Services.AddScoped<ICommandHandler<AddSavingCommand>, AddSavingCommandHandler>();
builder.Services.AddScoped<ICommandHandler<UpdateSavingCommand>, UpdateSavingCommandHandler>();
builder.Services.AddScoped<ICommandHandler<DeleteSavingCommand>, DeleteSavingCommandHandler>();

//Stats
builder.Services.AddScoped<IQueryHandler<GetSavingBarChartQuery, StatsBarChartViewModel>, GetSavingBarChartQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetMoneySpendedFromTaskBarChartQuery, StatsBarChartViewModel>, GetMoneySpendedFromTaskBarChartQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetMoneySpendedForCategoryBarChartQuery, StatsBarChartViewModel>, GetMoneySpendedForCategoryBarChartQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetNotesBarChartQuery, StatsBarChartViewModel>, GetNotesBarChartQueryHandler>();

//Tasks
builder.Services.AddScoped<IQueryHandler<GetTaskQuery, TaskViewModel>, GetTaskQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetTasksQuery, GetTasksViewModel>, GetTasksQueryHandler>();

builder.Services.AddScoped<ICommandHandler<AddTaskCommand>, AddTaskCommandHandler>();
builder.Services.AddScoped<ICommandHandler<UpdateTaskCommand>, UpdateTaskCommandHandler>();
builder.Services.AddScoped<ICommandHandler<DeleteTaskCommand>, DeleteTaskCommandHandler>();
builder.Services.AddScoped<ICommandHandler<DeleteTaskRelatedEntitiesCommand>, DeleteTaskRelatedEntitiesCommandHandler>();

//TaskNotes
builder.Services.AddScoped<IQueryHandler<GetTaskNoteQuery, GetTasksNotesViewModel>, GetTaskNoteQueryHandler>();

builder.Services.AddScoped<ICommandHandler<AddTaskNoteCommand>, AddTaskNoteCommandHandler>();
builder.Services.AddScoped<ICommandHandler<DeleteTaskNoteCommand>, DeleteTaskNoteCommandHandler>();

//TaskSubTasks
builder.Services.AddScoped<IQueryHandler<GetSubTasksQuery, GetTasksSubTasksViewModel>, GetSubTasksQueryHandler>();

builder.Services.AddScoped<ICommandHandler<AddTaskSubTaskCommand>, AddTaskSubTaskCommandHandler>();
builder.Services.AddScoped<ICommandHandler<ChangeTaskSubTaskStatusCommand>, ChangeTaskSubTaskStatusCommandHandler>();
builder.Services.AddScoped<ICommandHandler<DeleteTaskSubTaskCommand>, DeleteTaskSubTaskCommandHandler>();

//User
builder.Services.AddScoped<IQueryHandler<GetAllUsersQuery, GetUsersAdminViewModel>, GetAllUsersQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetUserByAdminQuery, UserAdminViewModel>, GetUserByAdminQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetUserQuery, UserViewModel>, GetUserQueryHandler>();

builder.Services.AddScoped<ICommandHandler<SaveUserCommand>, SaveUserCommandHandler>();
builder.Services.AddScoped<ICommandHandler<SaveUserByAdminCommand>, SaveUserByAdminCommandHandler>();
builder.Services.AddScoped<ICommandHandler<DeleteUserCommand>, DeleteUserCommandHandler>();

//Notes
builder.Services.AddScoped<IQueryHandler<GetNoteQuery, NotesViewModel>, GetNoteQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetNotesQuery, GetNotesViewModel>, GetNotesQueryHandler>();

builder.Services.AddScoped<ICommandHandler<AddNoteCommand>, AddNoteCommandHandler>();
builder.Services.AddScoped<ICommandHandler<UpdateNoteCommand>, UpdateNoteCommandHandler>();
builder.Services.AddScoped<ICommandHandler<DeleteNoteCommand>, DeleteNoteCommandHandler>();
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