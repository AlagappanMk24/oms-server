using API.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using API.Integrations;
using API.Logger;
using API.Utilities.Jwt;
using Newtonsoft.Json;
using API.Utilities.Email;
using API.Services.Interface;
using API.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
        .AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        });

// Microsoft.AspNetCore.Mvc.NewtonsoftJson package used for Advanced JSON handling features and
// better support for circular references,
builder.Services.AddAuthentication();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // Add JWT Authentication to Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' followed by a space and your JWT token."
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            Array.Empty<string>()
        }
    });
});

// Configures Entity Framework to use SQL Server
builder.Services.AddDbContext<AppDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("OMSConnection")
    ));

// Configures email settings
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

// Configures JWT settings
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

// Adds HttpContextAccessor
builder.Services.AddHttpContextAccessor();

// Add services to the container.
builder.Services.RegisterServices();

// Load JWT settings from configuration
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();

// Check if SecretKey is null or empty and generate a new one if necessary
if (string.IsNullOrEmpty(jwtSettings.SecretKey))
{
    var secretKey = SecretKeyGenerator.GenerateSecretKey();
    jwtSettings.SecretKey = secretKey;

    // Update appsettings.json with the new secret key
    var appSettingsFile = "appsettings.json";
    var json = File.ReadAllText(appSettingsFile);
    dynamic jsonObj = JsonConvert.DeserializeObject(json);

    jsonObj["JwtSettings"]["SecretKey"] = secretKey;
    string output = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
    File.WriteAllText(appSettingsFile, output);
}

// Register JWT settings as a singleton service
builder.Services.AddSingleton(jwtSettings);

// Register JwtService with the secret key from configuration
builder.Services.AddTransient<IJwtService>(provider => new JwtService(jwtSettings.SecretKey));

// Add CORS policy to allow requests from the specified origin
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMyOrigin",
        builder => builder.WithOrigins("http://localhost:3000")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials());
});

builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Add this line to serve static files
app.UseStaticFiles();

// Serve static files from wwwroot/uploads
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.WebRootPath, "uploads")),
    RequestPath = "/uploads"
});

// Apply CORS policy
app.UseCors("AllowMyOrigin");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

// Format the current date for log file naming
string formattedDate = DateTime.Now.ToString("MM-dd-yyyy");

// Retrieve base log path from configuration
string baseLogPath = builder.Configuration.GetValue<string>("Logging:LogFilePath");

// Combine base log path with formatted date to create log file path
string logFilePath = Path.Combine(baseLogPath, $"log-{formattedDate}.txt");

// Retrieve required services for logging
var loggerFactory = app.Services.GetRequiredService<ILoggerFactory>();
var httpContextAccessor = app.Services.GetRequiredService<IHttpContextAccessor>();

// Add custom file logger provider
loggerFactory.AddProvider(new CustomFileLoggerProvider(logFilePath, httpContextAccessor));

app.Run();
