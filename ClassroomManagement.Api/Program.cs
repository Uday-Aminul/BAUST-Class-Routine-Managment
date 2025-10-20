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

builder.Services.AddAutoMapper(typeof(Program));

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

//builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
/*if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}*/

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "TrackNStock API v1");
        options.RoutePrefix = string.Empty; // makes Swagger UI the default page
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
