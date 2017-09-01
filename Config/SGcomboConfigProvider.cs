using System;
using Microsoft.Extensions.Configuration;
using System.IO;

////////////////////////////////////////////////////////////////////////////
//	Copyright 2017 : Vladimir Novick    https://www.linkedin.com/in/vladimirnovick/  
//
//    NO WARRANTIES ARE EXTENDED. USE AT YOUR OWN RISK. 
//
// To contact the author with suggestions or comments, use  :vlad.novick@gmail.com
//
////////////////////////////////////////////////////////////////////////////

namespace SGcomboLib.Npgsql.Config
{
    public static  class SGcomboConfigProvider
    {



        public static SGcomboConfigValues ConfigSettings { get; set; }

        public static string GetConnectionString()
        {
            String connectionString = SGcomboConfigProvider.ConfigSettings.TablesConnectString;
            return connectionString;
        }


         static SGcomboConfigProvider()
        {
            GetConfiguration();
        }

        public static void GetConfiguration()
        {
            String baseDir = Directory.GetCurrentDirectory();


            IConfiguration configuration;
            if (File.Exists(Path.Combine(baseDir, "connectionString.json"))) {
                configuration = new ConfigurationBuilder()
                    .SetBasePath(baseDir)
                    .AddJsonFile("connectionString.json", optional: true)
                    .Build();
            } else
            {
                configuration = new ConfigurationBuilder()
                    .SetBasePath(baseDir)
                    .AddJsonFile("connectionString_debug.json", optional: true)
                    .Build();
            }

            ConfigSettings = configuration.GetSection("ConfigSettings").Get<SGcomboConfigValues>();
        }

    }
}
