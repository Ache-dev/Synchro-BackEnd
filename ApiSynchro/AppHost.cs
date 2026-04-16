using ApiSynchro.Hubs;

namespace ApiSynchro
{
    /// <summary>
    /// Punto de entrada principal para la configuración y ejecución de la API Synchro.
    /// </summary>
    public class AppHost
    {
        /// <summary>
        /// Configura servicios, middlewares y endpoints HTTP/SignalR de la aplicación.
        /// </summary>
        /// <param name="args">Argumentos de línea de comandos.</param>
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.WebHost.UseUrls("http://localhost:5000");

            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();
            builder.Logging.AddDebug();
            builder.Logging.AddFilter("Microsoft.Hosting.Lifetime", LogLevel.None);

            builder.Services.AddSingleton<IRepository, Repository>();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddSignalR();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                    policy.WithOrigins("http://localhost:5173")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials());
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Synchro API v1");
                    options.RoutePrefix = string.Empty;
                });
            }

            app.UseCors("AllowAll");
            app.MapControllers();
            app.MapHub<ChatHub>("/chatHub");

            app.Lifetime.ApplicationStarted.Register(() =>
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("=================================================");
                Console.WriteLine("  Synchro API - Servicio iniciado correctamente  ");
                Console.WriteLine("=================================================");
                Console.ResetColor();
                Console.WriteLine("Entorno   : " + app.Environment.EnvironmentName);
                Console.WriteLine("URL       : http://localhost:5000");
                Console.WriteLine("Estado    : Listo para recibir solicitudes");
                Console.WriteLine();
            });

            app.Run();
        }
    }
}
