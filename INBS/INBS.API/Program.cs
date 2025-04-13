using INBS.API.AppStart;
using INBS.Infrastructure.SignalR;
using Infrastructure.DependencyInjection;
using Microsoft.Extensions.Logging.AzureAppServices;
using Serilog;
using Serilog.Events; // Add this using directive

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information() // Chỉ log từ mức độ Information trở lên
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning) // Giảm log không cần thiết từ Microsoft
    .WriteTo.Console() // Ghi log ra console (hỗ trợ Azure Log Stream)
    .WriteTo.AzureApp() // Gửi log lên Azure Log Stream
    .CreateLogger();
builder.Host.UseSerilog();
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
    Log.Information("🚀 Request: {Method} {Path}", context.Request.Method, context.Request.Path);
    await next();
});

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("AllowAnyOrigins");

app.UseAuthentication();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<NotificationHub>("/notificationHub");
});

app.MapControllers();

app.Run();