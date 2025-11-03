using Application.DependencyInjection;
using Infrastructure.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;


var builder = WebApplication.CreateBuilder(args);

var logger = builder.Logging.Services.BuildServiceProvider().GetService<ILogger<Program>>();
logger?.LogInformation("=== DEBUG: Entorno = {Env}, ConnectionString length = {Len}", builder.Environment.EnvironmentName, builder.Configuration.GetConnectionString("MinashDB")?.Length ?? 0);

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
logger?.LogInformation("=== DEBUG: JWT Key length = {KeyLen}, Issuer = {Issuer}, Duration = {Dur}",
    jwtSettings["Key"]?.Length ?? 0, jwtSettings["Issuer"], jwtSettings["DurationInMinutes"]);

if (string.IsNullOrEmpty(jwtSettings["Key"]))
{
    logger?.LogError("=== ERROR: JWT Key is NULL/EMPTY – check env vars!");
    throw new InvalidOperationException("JWT Key missing in config");  // Falla startup visible
}

var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);
logger?.LogInformation("=== DEBUG: JWT Key bytes = {BytesLen}", key.Length);


builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

var connectionString = builder.Configuration.GetConnectionString("MinashDB");
builder.Configuration.GetValue<string>("MercadoPago:AccessToken");

#region
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddScoped<Application.Authentication.JwtService>();
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
#endregion


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key),
    };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "https://minash-frontend.azurewebsites.net")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddAuthorization( options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin", "CEO"));
    options.AddPolicy("ClienteOrAdmin", policy => policy.RequireRole("Cliente", "Admin", "CEO"));
    options.AddPolicy("CEOPolicy", policy => policy.RequireRole("CEO"));
    options.AddPolicy("CEOOrAdmin", policy => policy.RequireRole("Admin", "CEO"));
});
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Minash API", Version = "v1" });
c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
{
    Name = "Authorization",
    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
    Scheme = "bearer",
    BearerFormat = "JWT",
    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
    Description = "Ingrese el token JWT con el prefijo 'Bearer' "
});
    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });

});


var app = builder.Build();

if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();



app.UseAuthentication();
app.UseAuthorization();



app.MapControllers();

app.Run();
