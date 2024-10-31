using Microsoft.AspNetCore.Builder;

namespace ApiCatalogo.AppServicesExtesions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseApplicationHandler (this IApplicationBuilder app, IWebHostEnvironment environment)
        {
            if (environment.IsDevelopment()) 
            { 
                app.UseDeveloperExceptionPage();
            }
            return app;
        }

        public static IApplicationBuilder UseAppCors(this ApplicationBuilder app) 
        {
            app.UseCors(p =>
            {
                p.AllowAnyOrigin();
                p.WithMethods("GET");
                p.AllowAnyHeader();
            });
            return app;
        }

        public static IApplicationBuilder UseSwaggerMiddleware (this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c=> { });
            return app;
        }
    }
}
