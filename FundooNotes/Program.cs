using BusinessLayer.Interface;
using BusinessLayer.Service;
using DataAcessLayer.Context;
using DataAcessLayer.Interface;
using DataAcessLayer.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// 🔹 Configure Swagger with JWT Authentication
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter Your Token'",
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],

        // 🔄 Flexible Key Validation Logic
        IssuerSigningKeyResolver = (token, securityToken, kid, parameters) =>
        {
            var keys = new List<SecurityKey>
            {
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])), 
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:EmailKey"]))  
            };
            return keys;
        },
        ClockSkew = TimeSpan.Zero
    };
});






builder.Services.AddAuthorization();


// Add Database Context
builder.Services.AddDbContext<UserDBContext>(options =>options.UseSqlServer(builder.Configuration.GetConnectionString("DBCS")));

//Dependancy Injection
builder.Services.AddScoped<IUserDataService, UserDataService>(); // Register first
builder.Services.AddScoped<IUserService, UserService>(); // Then register UserService
builder.Services.AddScoped<INotesDataService, NotesDataService>(); // Register the data service
builder.Services.AddScoped<INotesService, NotesService>(); // Register the business service
builder.Services.AddScoped<ILabelDataService, LabelDataService>(); // Register the data service
builder.Services.AddScoped<ILabelService, LabelService>();
builder.Services.AddScoped<IEmailDataService, EmailDataService>();





builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.WriteIndented = true;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAngularApp");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
