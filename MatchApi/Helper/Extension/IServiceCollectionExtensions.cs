using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatchApi.Helper
{
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// 使用時機--例如在 service 中,直接叫用 AutoMapper
        /// var mapper = services.GetAppService<IMapper>();
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static T GetAppService<T>(this Microsoft.Extensions.DependencyInjection.IServiceCollection services)
        {
            return services.BuildServiceProvider().GetService<T>();
        }

        public static void AddAuthenticationCustom(this Microsoft.Extensions.DependencyInjection.IServiceCollection services)
        {
            //var jsonWebtoken = services.BuildServiceProvider().GetRequiredService<IJsonWebToken>();

            //void JwtBearer(JwtBearerOptions jwtBearer)
            //{
            //    jwtBearer.TokenValidationParameters = jsonWebtoken.TokenValidationParameters;
            //}

            string token = AppSettingsHelper.Configuration["AppSettings:Token"];
            services.AddAuthentication(Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                            System.Text.Encoding.ASCII.GetBytes(token)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
        }

        public static void AddMvcCustom(this Microsoft.Extensions.DependencyInjection.IServiceCollection services)
        {
            //void Mvc(MvcOptions mvc)
            //{
            //    mvc.Filters.Add(new AuthorizeFilter(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build()));
            //}
            void Json(Microsoft.AspNetCore.Mvc.MvcJsonOptions json)
            {
                json.SerializerSettings.ReferenceLoopHandling =
                    Newtonsoft.Json.ReferenceLoopHandling.Ignore;  //避免無限參考循環問題
            }
            //services.AddMvc(Mvc).AddJsonOptions(Json);
            services.AddMvc().AddJsonOptions(Json);
        }

        public static void AddResponseCompressionCustom(this IServiceCollection services)
        {
            services.AddResponseCompression(options =>
            {
                options.Providers.Add<Microsoft.AspNetCore.ResponseCompression.BrotliCompressionProvider>();
                options.EnableForHttps = true;
            });
        }

        public static void AddSpaStaticFilesCustom(this IServiceCollection services)
        {
            services.AddSpaStaticFiles(spa => spa.RootPath = "ClientApp/dist");
        }

        public static void AddSwaggerCustom(this IServiceCollection services)
        {
            services.AddSwaggerGen(cfg => cfg.SwaggerDoc("api", new Swashbuckle.AspNetCore.Swagger.Info()));
        }

    }
}
