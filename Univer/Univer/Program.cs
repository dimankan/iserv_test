using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Конфигурация
var configuration = builder.Configuration;

// Добавление сервисов в контейнер
builder.Services.AddControllers();

// Настройка Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Universities API",
        Version = "v1",
        Description = "API для работы с университетами мира"
    });
});

// Регистрация сервисов
builder.Services.AddScoped<IEtlService, EtlService>();
builder.Services.AddScoped<IUniversitiesService, UniversitiesService>();

// Настройка контекста БД
builder.Services.AddDbContext<UniversitiesContext>(options =>
{
    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
        npgsqlOptions =>
        {
            npgsqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorCodesToAdd: null);
        });
});

// Настройка HttpClient для ETL
builder.Services.AddHttpClient("universities", client =>
{
    client.BaseAddress = new Uri("http://universities.hipolabs.com/");
    client.Timeout = TimeSpan.FromSeconds(30);
});

var app = builder.Build();

// Применение миграций БД при запуске
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<UniversitiesContext>();
        context.Database.Migrate();
        Console.WriteLine("Database migrations applied successfully.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred while applying migrations: {ex.Message}");
    }
}

// Конфигурация конвейера HTTP запросов
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Universities API V1");
        c.RoutePrefix = string.Empty; // Делаем Swagger UI доступным по корневому URL
    });
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

// Health check endpoint
app.MapGet("/health", () => Results.Ok("API is healthy"));

app.MapControllers();

app.Run();