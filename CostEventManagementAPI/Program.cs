using CostEventManegement.AuthModule.Services;
using CostEventManegement.DatabaseModule;
using CostEventManegement.EventModule.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var aaa = builder.Configuration.GetConnectionString("Database");
// Add services to the container.
IWebHostEnvironment environment = builder.Environment;

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IEventService, EventService>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidIssuer = builder.Configuration.GetSection("Tokens:Issuer").Value,
        ValidAudience = builder.Configuration.GetSection("Tokens:Issuer").Value,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Tokens:Key").Value))
    };
});

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("https://panseb98.github.io", "http://localhost:4200").AllowAnyMethod().AllowAnyHeader(); ;
                      });
});


string connectionString;

if (builder.Environment.IsProduction())
{
    string connectionUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

    var databaseUri = new Uri(connectionUrl);

    string db = databaseUri.LocalPath.TrimStart('/');
    string[] userInfo = databaseUri.UserInfo.Split(':', StringSplitOptions.RemoveEmptyEntries);

    connectionString = builder.Configuration.GetConnectionString("Database")
                .Replace("<userId>", userInfo[0])
                .Replace("<password>", userInfo[1])
                .Replace("<host>", databaseUri.Host)
                .Replace("<port>", databaseUri.Port.ToString())
                .Replace("<db>", db);
}
else
{
    connectionString = builder.Configuration.GetConnectionString("Database");
}
builder.Services.AddDbContext<ApiDbContext>(
    o => o.UseNpgsql(connectionString)
    );

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    
}
//app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseHttpsRedirection();
app.UseCors(MyAllowSpecificOrigins);
app.UseAuthorization();

app.MapControllers();

app.UseHttpsRedirection();
app.Run();
