using APIFCG.Configuracao;
using APIFCG.Infra.Entity;
using APIFCG.Infra.Middleware;
using APIFCG.Infra.Model;
using APIFCG.Infra.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace APIFCG
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var configurations = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(c =>
            {
                // Configura Swagger UI
                c.SwaggerDoc("v1", new() { Title = "APIFCG", Version = "v1" });

                // Define esquema de segurança JWT
                c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "Insira o token JWT no formato: Bearer {seu token}"
                });

                // Exige JWT para endpoints protegidos
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
                        new string[] {}
                    }
                });
            });


            #region Configurações do banco de dados
            var connectionString = AzureMySqlConnectionHelper.BuildConnectionString(
                server: configurations.GetConnectionString("Server"),
                database: configurations.GetConnectionString("Database"),
                user: configurations.GetConnectionString("User"),
                password: configurations.GetConnectionString("Password")
            );
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            }, ServiceLifetime.Scoped);
            #endregion

            //Dependencies
            builder.Services.ResolveDependencies();


            #region Configura JWT
            var jwtSettings = builder.Configuration.GetSection("JwtSettings");
            builder.Services.Configure<JwtSettings>(jwtSettings);

            var secretKey = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]);

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(secretKey)
                };
            });
            #endregion

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();


            #region [Middler]
            app.UseCorrelationMiddleware();
            #endregion

            app.Run();
        }
    }
}
