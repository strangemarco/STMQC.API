using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
var builder = WebApplication.CreateBuilder(args);

// Configurar Autenticación JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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

builder.Services.AddControllers();

// Configurar OpenAPI 
builder.Services.AddOpenApi();

builder.Services.AddDbContext<SMTQC.API.Models.SmtqcDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        x => x.UseNetTopologySuite()));

// AutoMapper
builder.Services.AddAutoMapper(config => {
    config.AddProfile<SMTQC.API.Profiles.MappingProfile>();
});

// Services (RN)
builder.Services.AddScoped<SMTQC.API.Services.IProductorService, SMTQC.API.Services.ProductorService>();
builder.Services.AddScoped<SMTQC.API.Services.IAutorizacionService, SMTQC.API.Services.AutorizacionService>();
builder.Services.AddScoped<SMTQC.API.Services.ITrazabilidadService, SMTQC.API.Services.TrazabilidadService>();

// Background Services
builder.Services.AddHostedService<SMTQC.API.BackgroundServices.NasaFirmsBackgroundService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SMTQC.API.Models.SmtqcDbContext>();
    try
    {
        db.Database.ExecuteSqlRaw("UPDATE Autorizaciones SET Geometria.STSrid = 4326 WHERE Geometria IS NOT NULL AND Geometria.STSrid = 0");
    }
    catch { /* Ignore if table doesn't exist yet */ }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTheme(ScalarTheme.Default);
        options.WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
        options.WithClassicLayout();
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Redirigir la raíz a la documentación de Scalar
app.MapGet("/", context => {
    context.Response.Redirect("/scalar/v1");
    return Task.CompletedTask;
});

app.Run();
