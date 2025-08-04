using Microsoft.EntityFrameworkCore;
using Tienda.Microservicios.Autor.Api.Persistencia;
using MediatR;
using FluentValidation.AspNetCore;
using Tienda.Microservicios.Autor.Api.Aplication;
using Tienda.Microservicios.Autor.Api.Services;

namespace Tienda.Microservicios.Autor.Api.extensions
{
    public static class ServiceCollectionsExtensions
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Controladores + Validaciones
            services.AddControllers()
                .AddFluentValidation(cfg =>
                    cfg.RegisterValidatorsFromAssemblyContaining<Nuevo>());

            services.AddDbContext<ContextoAutor>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddMediatR(typeof(Nuevo.Manejador).Assembly);
            services.AddAutoMapper(typeof(Consulta.Manejador));

            // Registrar HttpClient y AuthValidationService
            services.AddHttpClient<IAuthValidationService, AuthValidationService>();

            return services;
        }
    }
}
