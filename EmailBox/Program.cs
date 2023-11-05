using EmailBox_Application.Interfaces;
using EmailBox_Application.Services;
using EmailBox_Core_Web_App.HubService;
using EmailBox_Domain.TableEntities;
using EmailBox_Infrestructure.DataBaseContext;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllHeaders",
        builder =>
        {
            builder.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
        });

});
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer",new OpenApiSecurityScheme
    {
       Scheme="Bearer",
       BearerFormat="JWT",
       In = ParameterLocation.Header,
       Name = "Authorization",
       Description="Bearer Authentication with JWT Token",
       Type = SecuritySchemeType.Http
    });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement 
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                              Reference = new OpenApiReference
                              {
                                  Id = "Bearer",
                                  Type = ReferenceType.SecurityScheme
                              }
                            },
                            new List<string>()
                        }
                    });
             });
//Jwt /Authun / Auth
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options=> 
options.TokenValidationParameters = new TokenValidationParameters()
{
    ValidateActor = true,
    ValidateAudience = true,
    ValidateLifetime = true,
    ValidateIssuerSigningKey = true,
    ValidIssuer = builder.Configuration["Jwt:Issuer"],
    ValidAudience = builder.Configuration["Jwt:Audience"],
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))

});
builder.Services.AddAuthorization();

//signalR
builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
});

//Creating D I
builder.Services.AddTransient<IUserServices, UserServices>();
builder.Services.AddTransient<IConnectionServices, ConnectionServices>();
builder.Services.AddTransient<IPrivateEmailServices, PrivateEmailServices>();
builder.Services.AddTransient<IConfirmationCodeServices, ConfirmationCodeServices>();

//db
builder.Services.AddDbContext<EBContexts>(options => options.UseSqlServer("Server=.; Database=EmailBoxDb; Trusted_Connection=True; MultipleActiveResultSets=True;TrustServerCertificate=True"));
builder.Services.AddControllers();

// configuring Swagger
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();
app.UseCors("AllowAllHeaders");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.UseAuthentication();
app.MapControllers();
app.MapHub<MailHub>("/mailHub");
app.Run();
