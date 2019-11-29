using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatchApi.Service
{
    public static class IServiceCollectionRegister
    {
        public static IServiceCollection AppServiceRegister(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IHomeService, HomeService>();
            services.AddScoped<IMemberService, MemberService>();
            return services;
        }
    }
}
