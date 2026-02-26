using Microsoft.EntityFrameworkCore;
using PharmacyManagementSystem.Data;
using PharmacyManagementSystem.Common;
using PharmacyManagementSystem.Services.Interfaces;
using PharmacyManagementSystem.Services.Implementations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using PharmacyManagementSystem.Services;

var builder = WebApplication.CreateBuilder(args);

// =============================
// SERVICES
// =============================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ✅ DATABASE
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ✅ HTTP CONTEXT ACCESSOR (must be before UserContextService)
builder.Services.AddHttpContextAccessor();

// ✅ DEPENDENCY INJECTION
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ResponseService>();
builder.Services.AddScoped<IMedicineService, MedicineService>();
builder.Services.AddScoped<ISupplierService, SupplierService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<ISalesService, SalesService>();
builder.Services.AddScoped<IUserContextService, UserContextService>();
builder.Services.AddScoped<IErrorLoggerService, ErrorLoggerService>();

// =============================
// JWT AUTHENTICATION
// =============================
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
});

builder.Services.AddAuthorization();

// =============================
var app = builder.Build();

// ✅ SWAGGER
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ✅ ORDER IS VERY IMPORTANT
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// =============================
// DATABASE CHECK
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        if (db.Database.CanConnect())
            Console.WriteLine("✅ Database connected successfully!");
        else
            Console.WriteLine("❌ Database connection failed!");
    }
    catch (Exception ex)
    {
        Console.WriteLine("❌ Database connection failed!");
        Console.WriteLine(ex);
    }
}

app.Run();
