using INBS.API.AppStart;
using Infrastructure.DependencyInjection;


var builder = WebApplication.CreateBuilder(args);

//builder.Logging.ClearProviders();
//builder.Logging.AddConsole(); // Đảm bảo log ra console

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddPresentation();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigins", policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("AllowAnyOrigins");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();