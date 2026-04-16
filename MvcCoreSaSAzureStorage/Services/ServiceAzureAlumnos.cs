using Azure.Data.Tables;

namespace MvcCoreSaSAzureStorage.Services
{
    public class ServiceAzureAlumnos
    {
        private TableClient tableAlumnos;
        private string urlApi;
        public ServiceAzureAlumnos(IConfiguration configuration)
        {
            this.urlApi = configuration.GetValue<string>("ApiUrls:ApiAzureToken");
        }
    }
}
