using Microsoft.EntityFrameworkCore;
using UserService.Data;
using UserService.Repository;
using UserService.Repository.Token;
using UserService.Services;
using UserService.Services.Token;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUserService, UserService.Services.UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<ITokenRepository, TokenRepository>();

var app = builder.Build();

app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.None,
    HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always,
    Secure = CookieSecurePolicy.Always
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// ------------ ДОБАВЬТЕ ЭТОТ БЛОК КОДА ------------
// Автоматическое применение миграций при старте
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        // Получаем контекст базы данных
        var dbContext = services.GetRequiredService<AppDbContext>();

        // Применяем все ожидающие миграции
        dbContext.Database.Migrate();
    }
    catch (Exception ex)
    {
        // Можно добавить логирование, если что-то пошло не так
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the database.");
    }
}
// ---------------------------------------------------


app.UseHttpsRedirection();

app.UseMetricServer();

app.UseHttpMetrics();

app.UseAuthorization();

app.MapControllers();

app.Run();
