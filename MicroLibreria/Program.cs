using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;
using Uttt.Micro.Service.Extenciones;

var builder = WebApplication.CreateBuilder(args);

// Configuración de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Add services to the container.

// Obtener el secreto del archivo de configuracion
//var secretKey = builder.Configuration["ApiSettings:JwtOptions:Secret"];
//var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

//builder.Services.AddAuthentication(JwtBarerDefaults.AuthenticationScheme)
//    .AddJwtBarer(options =>
//    {
//        options.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidateIssuer = true,
//            ValidateAudience = true,
//            ValidateLifetime = true,
//            ValidateIssuerSigningKey = true,
//            ValidIssuer = builder.Configuration["ApiSettings:JwtOptions:Issuer"],
//            ValidAudience = builder.Configuration["ApiSettings:JwtOptions:Audience"],
//            IssuerSigningKey = key
//        };
//    });

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCustomServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//// Configurar AutoMapper
//builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

//// Configurar MediatR
//builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly));

// Configurar FluentValidation

// builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
// builder.Services.AddFluentValidationAutoValidation();

app.UseSwagger();
app.UseSwaggerUI();

// Habilitar CORS antes de MapControllers
app.UseCors("AllowAll");

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
