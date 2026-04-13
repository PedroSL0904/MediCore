using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace MediCore.Api.Middlewares
{
    // Usamos IExceptionHandler, la forma más nueva y pro de .NET 8 para manejar errores
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            // 1. Guardamos el error real en la consola de Windows para que TÚ lo puedas arreglar después
            _logger.LogError(exception, "💥 Error crítico capturado: {Message}", exception.Message);

            // 2. Preparamos un mensaje "amigable" y seguro para el usuario final (sin revelar código)
            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Error Interno del Servidor",
                Detail = "Ocurrió un problema inesperado. Nuestro equipo técnico ya fue notificado."
            };

            // 3. Se lo mandamos al Frontend en formato JSON estandarizado
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            // 4. Le decimos a .NET "Tranquilo, yo me encargo. No explotes."
            return true;
        }
    }
}