using Library.API.Authorization;
using Library.API.Helpers;
using Library.DataAccess.Data;
using Library.DataAccess.Units;
using Library.Services.Services.Implementations;
using Library.Services.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// 1. Database Configuration
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyConnection")));

// 2. Unit of Work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// 3. Business Services
builder.Services.AddScoped<IBookManagementService, BookManagementService>();
builder.Services.AddScoped<IUserManagementService, UserManagementService>();
builder.Services.AddScoped<IFineService, FineService>();
builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IBorrowingService, BorrowingService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddSingleton<IAuthorizationHandler, ResourceOwnerOrAdminHandler>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ResourceOwnerOrAdmin", policy =>
        policy.Requirements.Add(new ResourceOwnerOrAdminRequirement()));
});
// add automapper
builder.Services.AddAutoMapper(typeof(MappingProfile));


// to ignore cycles of json responses
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler =
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // ===============================
    // 1) Define the JWT Bearer security scheme
    // ===============================
    //
    // This tells Swagger that our API uses JWT Bearer authentication
    // through the HTTP Authorization header.
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        // The name of the HTTP header where the token will be sent.
        Name = "Authorization",


        // Indicates this is an HTTP authentication scheme.
        Type = SecuritySchemeType.Http,


        // Specifies the authentication scheme name.
        // Must be exactly "Bearer" for JWT Bearer tokens.
        Scheme = "Bearer",


        // Optional metadata to describe the token format.
        BearerFormat = "JWT",


        // Specifies that the token is sent in the request header.
        In = ParameterLocation.Header,


        // Text shown in Swagger UI to guide the user.
        Description = "Enter: Bearer {your JWT token}"
    });


    // ===============================
    // 2) Require the Bearer scheme for secured endpoints
    // ===============================
    //
    // This tells Swagger that endpoints protected by [Authorize]
    // require the Bearer token defined above.
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                // Reference the previously defined "Bearer" security scheme.
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },


            // No scopes are required for JWT Bearer authentication.
            // This array is empty because JWT does not use OAuth scopes here.
            new string[] {}
        }
    });
});
// adding cors in order to allow only the origins of my projects to access the api
builder.Services.AddCors(options =>
{
    options.AddPolicy("StudentApiCorsPolicy", policy =>
    {
        policy
            .WithOrigins(
                "https://localhost:7217",
                "http://localhost:5215"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("StudentApiCorsPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers().RequireAuthorization(); // requires authentication for all the controllers in the system

app.Run();
