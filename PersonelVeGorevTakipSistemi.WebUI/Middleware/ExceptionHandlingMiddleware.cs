using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using PersonelVeGorevTakipSistemi.DataAccess;
using Log = PersonelVeGorevTakipSistemi.Core.Entities.Log;

namespace PersonelVeGorevTakipSistemi.WebUI.Middleware
{
    // Sistem genelinde firlatilan tüm beklenmedik hatalari yakalayan ve veritabanina loglayan ara yazilim
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Istegi siradaki middleware bileşenine aktar
                await _next(context);
            }
            catch (Exception ex)
            {
                // Istek esnasinda bir hata olusursa yakala ve logla
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            try
            {
                // Scoped olan AppDbContext nesnesini istek servis saglayicisindan (RequestServices) talep ediyoruz
                var dbContext = context.RequestServices.GetRequiredService<AppDbContext>();

                var log = new Log
                {
                    LogLevel = "Hata (Error)",
                    Message = exception.Message,
                    Exception = exception.ToString(),
                    TimeStamp = DateTime.Now
                };

                // Hatayi veritabanina kaydediyoruz
                dbContext.Logs.Add(log);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                // Veritabanina loglama sirasinda hata olusursa sonsuz donguye girmemek icin yoksayiyoruz
            }

            // Kullaniciyi dost canlısı genel hata sayfasina yonlendiriyoruz
            context.Response.Redirect("/Home/Error");
        }
    }
}
