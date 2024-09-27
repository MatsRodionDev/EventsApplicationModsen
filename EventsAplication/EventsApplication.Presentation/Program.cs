using EventsApplication.Persistence.DI;
using EventsApplication.Presentation.Profiles;
using EventsApplication.Application.Common.DI;
using EventsApplication.Application.Common;
using Microsoft.AspNetCore.CookiePolicy;
using EventsApplication.Infrastructure.DI;
using EventsApplication.Presentation.Meddlewares;
using Microsoft.Extensions.FileProviders;
using FluentValidation;
using EventsAplication.Presentation.Validators;
using EventsApplication.Infrastructure.Services;
using EventsApplication.Presentation.Extensions;
using FluentValidation.AspNetCore;
using EventsApplication.Application.Common.Handlers;
using EventsApplication.Presentation.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));
builder.Services.Configure<EmailOptions>(builder.Configuration.GetSection(nameof(EmailOptions)));

builder.Services.AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters()
    .AddValidatorsFromAssemblyContaining<UpdateEventDtoValidator>()
    .AddValidatorsFromAssemblyContaining<CreateEventCommandValidator>();

builder.Services.AddControllers(options => options.Filters
    .Add(typeof(ValidationFilter)));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(ApiProfile));

builder.Services.RegisterPersistenceDapendencies(builder.Configuration);

builder.Services.RegisterInfrastructureLayerDependencies(builder.Configuration);

builder.Services.RegisterApplicationLayerDependencies(builder.Configuration);

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();
}

app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
    HttpOnly = HttpOnlyPolicy.Always,
    Secure = CookieSecurePolicy.Always
});

app.UseAuthentication();
app.UseAuthorization();

var webRootPath = builder.Environment.WebRootPath;

var path = Path.Combine(webRootPath, "Uploads");

if (!Directory.Exists(path))
{
    Directory.CreateDirectory(path);
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.WebRootPath,
        "Uploads")),
    RequestPath = string.Empty
});

app.MapControllers();

app.Run();
