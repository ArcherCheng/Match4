using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatchApi.Helper
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseCorsCustom(this IApplicationBuilder application)
        {
            application.UseCors(cors =>
            //cors.WithOrigins("http://localhost:4200")
            cors.AllowAnyOrigin()
            .AllowCredentials()
            .AllowAnyHeader()
            .AllowAnyMethod());
        }

        public static void UseExceptionCustom(this IApplicationBuilder application, IHostingEnvironment environment)
        {
            if (environment.IsDevelopment())
            {
                application.UseDatabaseErrorPage();
                application.UseDeveloperExceptionPage();
            }
            application.UseMiddleware<ExceptionMiddleware>();
        }

        public static void UseHstsCustom(this IApplicationBuilder application, IHostingEnvironment environment)
        {
            if (!environment.IsDevelopment())
            {
                application.UseHsts();
            }
        }

        public static void UseSpaCustom(this IApplicationBuilder application, IHostingEnvironment environment)
        {
            application.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";
                if (environment.IsDevelopment())
                {
                    //spa.UseAngularCliServer(server);
                    //spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }

        public static void UseSwaggerCustomer(this IApplicationBuilder application)
        {
            application.UseSwagger();
            application.UseSwaggerUI(cfg => cfg.SwaggerEndpoint("/swagger/api/swagger.json", "API"));
        }
    }
}
