using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Writers;
using Test2;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
using (var scope = app.Services.CreateAsyncScope())
{
    var _logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    try
    {
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        await ApplicationSeedData.SeedData(userManager, roleManager);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "An error occurred while seeding the database");
    }
}
app.Run();
