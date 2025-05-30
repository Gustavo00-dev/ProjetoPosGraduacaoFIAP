using APIFCG.Infra.LogAPI;
using APIFCG.Infra.Repository;
using APIFCG.Service;

namespace APIFCG.Configuracao
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services)
        {
   
            services.AddHttpContextAccessor();
            services.AddCorrelationIdGenerator();
            services.AddTransient(typeof(BaseLogger<>));


            #region Services/Repository   
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IJogoRepository, JogoRepository>();
            #endregion

            #region Builder
            //services.AddScoped<IDadosClienteBuilder, DadosClienteBuilder>();
            #endregion


            return services;
        }
    }
}
