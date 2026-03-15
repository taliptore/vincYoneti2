using System.Text;
using CraneManagementSystem.Persistence;
using CraneManagementSystem.Infrastructure;
using CraneManagementSystem.API.Middleware;
using CraneManagementSystem.API.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// DbContext (Persistence)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Infrastructure (repositories, Auth, Token, Email)
builder.Services.AddInfrastructure(builder.Configuration);

// Authentication & Authorization
var jwtSecret = builder.Configuration["Jwt:Secret"] ?? throw new InvalidOperationException("Jwt:Secret is not set.");
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "CraneManagementSystem";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "CraneManagementSystem";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            ValidateIssuer = true,
            ValidateAudience = true,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("AdminOrMuhasebe", policy => policy.RequireRole("Admin", "Muhasebe"));
    options.AddPolicy("AdminOrOperator", policy => policy.RequireRole("Admin", "Operatör"));
    options.AddPolicy("RequireFirma", policy => policy.RequireRole("Firma"));
});

// CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(
                builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? new[] { "http://localhost:3000", "https://localhost:5001" })
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// Swagger with JWT Bearer
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Crane Management System API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Bearer. Örnek: \"Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// İlk çalıştırmada Admin kullanıcı yoksa oluştur (Seed)
await DataSeeder.EnsureAdminUserAsync(app.Services);
// Rol ve menü tabloları + varsayılan menüler (Admin rolüne atanır)
await DataSeeder.EnsureRolesAndMenusAsync(app.Services);

// Pipeline: Exception middleware en üstte
app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
// Ana sayfa: Swagger'a yönlendir (tarayıcıda sistemi kullanmak için)
app.MapGet("/", () => Results.Redirect("/swagger", permanent: false));

app.Run();

// Integration test için WebApplicationFactory erişimi
public partial class Program { }
