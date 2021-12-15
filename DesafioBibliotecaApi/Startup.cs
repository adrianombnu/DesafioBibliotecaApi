using DesafioBibliotecaApi.Repositorio;
using DesafioBibliotecaApi.Repository;
using DesafioBibliotecaApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesafioBibliotecaApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //
            ///////Sempre encodar a chave para não usar o texto puro
            var key = Encoding.ASCII.GetBytes(Configuration.GetValue<string>("Secret"));

            //
            ///////Aqui estou dizendo que será usado o esquema de autenticacao jwt
            services.AddAuthentication(options =>
            {
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            //
            ///////Aqui estou passando as configurações para o jwt
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });

            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

                });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DesafioBibliotecaApi", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Description = "Informe o token no formato Bearer: <token>"

                });
                c.AddSecurityRequirement(
                    new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Id = "Bearer",
                                    Type = ReferenceType.SecurityScheme
                                },

                            },
                            new string[]{}
                        }
                    });
            });

            //
            ///////Somente o user repository precisda ser singleton
            services.AddSingleton<UserRepository>();
            services.AddSingleton<AuthorRepository>();
            services.AddSingleton<ClientRepository>();
            services.AddSingleton<AdressRepository>();
            services.AddSingleton<BookRepository>();
            services.AddSingleton<ReservationRepository>();
            services.AddSingleton<WithdrawRepository>();

            services.AddTransient<LoginService>();
            services.AddTransient<CustumerService>();
            services.AddTransient<AdressService>();
            services.AddTransient<BookService>();
            services.AddTransient<ClientService>();
            services.AddTransient<EmployeeService>();
            services.AddTransient<AuthorService>();
            services.AddTransient<ReservationService>();
            services.AddTransient<WithdrawService>();
            services.AddTransient<JwtTokenService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DesafioBibliotecaApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
