using INBS.API.AppStart;
using Infrastructure.DependencyInjection;
using Microsoft.Extensions.Logging.AzureAppServices; // Add this using directive

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddLogging(lb =>
{
    lb.AddConsole();
    lb.AddDebug();
    lb.AddAzureWebAppDiagnostics();
});

//builder.Logging.ClearProviders();
//builder.Logging.AddConsole(); // Đảm bảo log ra console

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddPresentation();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigins", policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

var app = builder.Build();
app.Use(async (context, next) =>
{
    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
    logger.LogInformation("🚀 Request: {Method} {Path}", context.Request.Method, context.Request.Path);
    await next();
});
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("AllowAnyOrigins");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();