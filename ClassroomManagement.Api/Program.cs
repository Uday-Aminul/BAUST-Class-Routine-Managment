using ClassroomManagement.Api.Data;
using ClassroomManagement.Api.Repositories;
using Microsoft.EntityFrameworkCore;
using TeacherManagement.Api.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ClassroomManagementDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("ClassroomManagementConnectionString")
    ));

builder.Services.AddControllers();

builder.Services.AddScoped<IClassroomRepository, SQLClassroomRepository>();
builder.Services.AddScoped<ITeacherRepository, SQLTeacherRepository>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
