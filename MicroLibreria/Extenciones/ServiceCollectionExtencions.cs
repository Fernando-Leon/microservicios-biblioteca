using Uttt.Micro.Service.Persistencia;
using MediatR;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Uttt.Micro.Service.Aplication;
using Uttt.Micro.Service.Services;

namespace Uttt.Micro.Service.Extenciones
{
    public static class ServiceCollectionExtencions
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers()
                .AddFluentValidation(cfg => cfg.RegisterValidatorsFromAssemblyContaining<Nuevo>());

            services.AddDbContext<ContextoLibreria>(options =>
            {
                options.UseMySQL(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddMediatR(typeof(Nuevo.Manejador).Assembly);
            services.AddAutoMapper(typeof(Consulta.Manejador));

            // Registrar HttpClient y AuthValidationService
            services.AddHttpClient<IAuthValidationService, AuthValidationService>();

            return services;
        }
    }
}
