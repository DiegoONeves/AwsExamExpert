using Microsoft.Extensions.Configuration;

namespace AwsExamExpert
{
    public class BaseService
    {
        IConfiguration _config;
        public string ConnectionString { get; set; } = "";
        public string WhatsAppAdministrador { get; set; }
        public BaseService(IConfiguration config)
        {
            _config = config;
            ConnectionString = _config.GetConnectionString("DefaultConnection");
            WhatsAppAdministrador = _config.GetSection("WhatsAppAdministrador").Value;
        }

    }
}
