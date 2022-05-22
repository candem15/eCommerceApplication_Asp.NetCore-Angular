using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Persistence
{
    static class Configuration
    {
        static public string ConnectionStringDockerPg
        {
            get
            {
                ConfigurationManager configurationManager = new();
                configurationManager.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(),"../../Presentation/eCommerceAPI.WebAPI/"));
                configurationManager.AddJsonFile("appsettings.json");
                return configurationManager.GetConnectionString("DockerPostgreSQL");
            }
        }
    }
}
