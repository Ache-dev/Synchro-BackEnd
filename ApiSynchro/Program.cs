using Microsoft.Data.SqlClient;
using Query.Implementations;
using Query.Interfaces;
using Repository.Implements;
using Repository.Interfaces;
using System.Data;

namespace ApiSynchro
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

            builder.Services.AddScoped<IDbConnection>(sp =>
            {
                string connectionString = builder.Configuration.GetConnectionString("sql");
                SqlConnection conection = new SqlConnection(connectionString);
                return conection;
            });

            builder.Services.AddTransient<IUsuarioQueries, UsuarioQueries>();
            builder.Services.AddTransient<IUsuarioRepository, UsuarioRepository>();
            builder.Services.AddTransient<IIntencionBusquedaQueries, IntencionBusquedaQueries>();
            builder.Services.AddTransient<ISesionQueries, SesionQueries>();
            builder.Services.AddTransient<IPreguntaEncuestaQueries, PreguntaEncuestaQueries>();
            builder.Services.AddTransient<IRespuestaEncuestaQueries, RespuestaEncuestaQueries>();
            builder.Services.AddTransient<IMatchQueries, MatchQueries>();
            builder.Services.AddTransient<IMensajeQueries, MensajeQueries>();

            builder.Services.AddTransient<IIntencionBusquedaRepository, IntencionBusquedaRepository>();
            builder.Services.AddTransient<ISesionRepository, SesionRepository>();
            builder.Services.AddTransient<IPreguntaEncuestaRepository, PreguntaEncuestaRepository>();
            builder.Services.AddTransient<IRespuestaEncuestaRepository, RespuestaEncuestaRepository>();
            builder.Services.AddTransient<IMatchRepository, MatchRepository>();
            builder.Services.AddTransient<IMensajeRepository, MensajeRepository>();

            string ruta = Path.Combine(AppContext.BaseDirectory, "api.xml");

            builder.Services.AddSwaggerGen(
                opt =>
                {
                    opt.IncludeXmlComments(ruta);
                }
                );

            builder.Services.AddSignalR();

            builder.Services.AddCors(
                opt =>
                {
                    opt.AddDefaultPolicy(policy =>
                    {
                        policy.AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                        .WithOrigins("http://127.0.0.1:5500");
                    });
                }
                );

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors();

            app.MapGet("/", () => "Servidor Signal escuhando");

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
