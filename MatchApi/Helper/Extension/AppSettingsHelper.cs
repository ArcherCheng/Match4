using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatchApi.Helper
{
    /// <summary>
    /// 調用
    /// string sqlString1 = AppSettingsHelper.Configuration.GetConnectionString("TestConnection");
    /// string sqlString2 = AppSettingsHelper.Configuration["Logging:LogLevel:Default"];
    /// </summary>
    public class AppSettingsHelper
    {
        public static Microsoft.Extensions.Configuration.IConfiguration Configuration { get; set; }

        static AppSettingsHelper()
        {
            Configuration = new ConfigurationBuilder()
                .Add(new Microsoft.Extensions.Configuration.Json.JsonConfigurationSource
                {
                    Path = "appsettings.json",
                    ReloadOnChange = true
                })
                .Build();
        }

        /// <summary>
        /// 直接在程式中調用
        /// </summary>
        //public Microsoft.Extensions.Configuration.IConfiguration Configuration { get; set; }
        //public void TestAppSettings()
        //{
        //    var builder = new ConfigurationBuilder()
        //       .SetBasePath(System.IO.Directory.GetCurrentDirectory())
        //       .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        //    Configuration = builder.Build();
        //    string sqlString2 = Configuration["Logging:LogLevel:Default"];
        //}

    }
}
