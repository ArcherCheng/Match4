using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MatchApi.Helper;
using MatchApi.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MatchApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment hostingEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddDbContext<AppDbContext>(options =>
            //    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                    .AddJsonOptions(opt =>
                    {
                        opt.SerializerSettings.ReferenceLoopHandling =
                        Newtonsoft.Json.ReferenceLoopHandling.Ignore;  //避免無限參考循環問題
                    });

            services.AddCors();

            services.AddAuthentication(Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                                System.Text.Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
                            ValidateIssuer = false,
                            ValidateAudience = false
                        };
                    });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            ////https://github.com/aspnet/AspNetCore/blob/release/3.0/src/Security/samples/StaticFilesAuth/Startup.cs
            //services.AddAuthorization(options =>
            //{
            //    var basePath = System.IO.Path.Combine(webHostEnvirnment.ContentRootPath, "PrivateFiles");
            //    var usersPath = System.IO.Path.Combine(basePath, "Users");
            //    options.AddPolicy("files", builder =>
            //     {
            //         builder.RequireAuthenticatedUser().RequireAssertion(context =>
            //         {
            //             var userName = context.User.Identity.Name;
            //             userName = userName?.Split('@').FirstOrDefault();
            //             if (userName == null)
            //             {
            //                 return false;
            //             }
            //             if(context.Resource is Endpoint endpoint)
            //             {
            //                 var userPath = System.IO.Path.Combine(usersPath, userName);
            //                 var directory = endpoint.Metadata.GetMetadata<System.IO.DirectoryInfo>();
            //                 if (directory != null)
            //                 {
            //                     return string.Equals(directory.FullName, basePath, StringComparison.OrdinalIgnoreCase)
            //                     || string.Equals(directory.FullName, usersPath, StringComparison.OrdinalIgnoreCase)
            //                     || string.Equals(directory.FullName, userPath, StringComparison.OrdinalIgnoreCase)
            //                     || directory.FullName.StartsWith(usersPath + System.IO.Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase);
            //                 }
            //                 throw new InvalidOperationException($"Missing file system metadata");
            //             }
            //             throw new InvalidOperationException($"Unknown resource type '{context.Resource.GetType()}'");
            //         });
            //     });
            //});

            //seed data
            services.AddTransient<Data.Seed>();

            //以下為自行定義的外部服務
            Ioc.RegisterInheritedTypes(typeof(BaseService).Assembly, typeof(BaseService));

            //var configuration = new AutoMapper.MapperConfiguration(cfg =>
            //{
            //    cfg.AddProfile(new AutoMapperProfiles());
            //});
            //AutoMapper.IMapper mapper = configuration.CreateMapper();
            //services.AddSingleton(mapper);

            //services.AddScoped<IMemberService, MemberService>();
            services.AppServiceRegister();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, Data.Seed seeder)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(builder => {
                    builder.Run(async context => {
                        context.Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;

                        var error = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();
                        if (error != null)
                        {
                            context.Response.AddApplicationError(error.Error.Message);
                            await context.Response.WriteAsync(error.Error.Message);
                        }
                    });
                });
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            //seed test data
            //seeder.SeedMembers();
            //seeder.SeedMemberDetail();
            //seeder.SeedMemberCondition();
            //seeder.SeedLiker();

            //app.UseHttpsRedirection(); //angular 不能使用 Redirection
            app.UseAuthentication();
            app.UseStaticFiles();
            app.UseCors(x =>
                x.WithOrigins("http://localhost:4200")
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials());
            app.UseMvc();
        }
    }
}
