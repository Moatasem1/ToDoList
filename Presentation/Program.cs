using Application.Features.Tasks.Commands;
using Application.Services;
using Application.Services.Interfaces;
using DataTables.AspNet.AspNetCore;
using Domain.Common;
using Domain.Common.Interfaces;
using Domain.Entities.User;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure.Email;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation.Options;
using Presentation.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
 options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")
 ));

builder.Services.AddFluentValidationAutoValidation();
//builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<CreateTaskCommand>();

builder.Services.AddScoped(typeof(IReadOnlyRepository<>),typeof(ReadOnlyRespository<>));
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<CurrentUserService>();
builder.Services.AddSingleton<IBase64ByteConverter, Base64ByteConverter>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<PasswordHasher<User>>();
builder.Services.AddSingleton<Application.Services.IEmailService, SmtpEmailService>();
builder.Services.AddSingleton<EmailTemplateBuilder>();

builder.Services.AddHttpContextAccessor();
builder.Services.RegisterDataTables();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var useCaseAssembly = typeof(IUseCase).Assembly;
builder.Services.Scan(scan => scan
    .FromAssemblies(useCaseAssembly)
    .AddClasses(classes => classes.AssignableTo<IUseCase>())
    .AsSelf()
    .WithTransientLifetime());

var emailPassword = builder.Configuration["EmailPassword"];

builder.Services.Configure<EmailConfig>(options =>
{
    builder.Configuration.GetSection("EmailConfig").Bind(options);
    options.EmailPassword = emailPassword!;
});

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(e => e.Value?.Errors.Count > 0)
            .Select(e => Error.ValueInvalidWithMessage("Fluent Validation", e.Value!.Errors.First().ErrorMessage)).ToList();

        var result = ResponseEnvelope<bool>.Fail(errors);

        return new BadRequestObjectResult(result);
    };
});


builder.Services.AddAuthentication("CookieAuth")
    .AddCookie("CookieAuth", options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.Cookie.Name = "Todo.Cookie";
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
        options.SlidingExpiration = true;
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Lax;
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
