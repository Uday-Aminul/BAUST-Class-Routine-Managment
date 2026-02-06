using System.Text;
using ClassroomManagement.Api.Data;
using ClassroomManagement.Api.Repositories;
using ClassScheduleManagement.Api.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using TeacherManagement.Api.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ClassroomManagementDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("ClassroomManagementConnectionString")
    ));
// builder.Services.AddDbContext<ClassroomManagementAuthDbContext>(options => options.UseSqlServer(
//     builder.Configuration.GetConnectionString("ClassroomManagementAuthConnectionString")
//     ));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo() { Title = "Class Routine Management", Version = "v1" });
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement(){
        {
            new OpenApiSecurityScheme(){
                Reference=new OpenApiReference(){
                    Type=ReferenceType.SecurityScheme,
                    Id=JwtBearerDefaults.AuthenticationScheme
                },
                Scheme="Oauth2",
                Name=JwtBearerDefaults.AuthenticationScheme,
                In=ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

builder.Services.AddScoped<IClassroomRepository, SQLClassroomRepository>();
builder.Services.AddScoped<ITeacherRepository, SQLTeacherRepository>();
builder.Services.AddScoped<IClassScheduleRepository, SQLClassScheduleRepository>();
builder.Services.AddScoped<ITokenRepository, TokenRepository>();

builder.Services.AddAutoMapper(typeof(Program));

// builder.Services.AddIdentityCore<IdentityUser>()
//     .AddRoles<IdentityRole>()
//     .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("ClassroomManagement")
//     .AddEntityFrameworkStores<ClassroomManagementAuthDbContext>()
//     .AddDefaultTokenProviders();

// builder.Services.Configure<IdentityOptions>(options =>
// {
//     options.User.RequireUniqueEmail = true;
//     options.Password.RequiredLength = 4;
//     options.Password.RequiredUniqueChars = 0;
//     options.Password.RequireUppercase = false;
//     options.Password.RequireLowercase = false;
//     options.Password.RequireDigit = false;
//     options.Password.RequireNonAlphanumeric = false;
// });

// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//     .AddJwtBearer(options =>
//     options.TokenValidationParameters = new TokenValidationParameters
//     {
//         ValidateIssuer = true,
//         ValidateAudience = true,
//         ValidateLifetime = true,
//         ValidateIssuerSigningKey = true,
//         ValidIssuer = builder.Configuration["Jwt:Issuer"],
//         ValidAudience = builder.Configuration["Jwt:Audience"],
//         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
//     });

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
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Classroom Management API v1");
        options.RoutePrefix = string.Empty; // makes Swagger UI the default page
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
