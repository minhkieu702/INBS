using INBS.API.AppStart;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using INBS.Domain.IRepository;
using INBS.Persistence.Repository;
using INBS.Persistence.Data;
using INBS.Application.Mappers;
using Microsoft.AspNetCore.OData;
using Microsoft.OData.ModelBuilder;
using INBS.Application.DTOs.Service;

var builder = WebApplication.CreateBuilder(args);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddPresentation(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigins", policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowAnyOrigins");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
