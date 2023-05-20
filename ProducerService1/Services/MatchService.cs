using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace ProducerService1.Services
{
    public class MatchService
    {
        public readonly IConfiguration config;
        public MatchService(IConfiguration configuration)
        {
            this.config = configuration;
        }
    }
}
