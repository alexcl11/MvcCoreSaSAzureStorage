using Azure.Data.Tables;
using Azure.Data.Tables.Sas;

namespace ApiTokenAzureStorage.Services
{
    public class ServiceSaSToken
    {
        private TableClient tablaAlumnos;
        public ServiceSaSToken(IConfiguration configuration)
        {
            string azureKeys = configuration.GetValue<string>("AzureKeys:StorageAccount");
            TableServiceClient tableService = new TableServiceClient(azureKeys);
            this.tablaAlumnos = tableService.GetTableClient("alumnos");
        }

        public string GenerateToken(string curso)
        {
            // NECESITAMOS LOS PERMISOS DE ACCESO
            TableSasPermissions permisos = TableSasPermissions.Read;
            // EL ACCESO A TOKEN ESTA DELIMITADO POR UN TIEMPO DETERMINADO
            TableSasBuilder builder = this.tablaAlumnos.GetSasBuilder(permisos, DateTime.UtcNow.AddMinutes(30));
            // EL ACCESO A LOS DATOS ES MEDIANTE ROWKEY O PARTITION KEY, SON STRING Y VAN DE FORMA ALFABETICA
            builder.PartitionKeyStart = curso;
            builder.PartitionKeyEnd = curso;
            // YA TENDREMOS EL TOKEN QUE ES UN ACCESO MEDIANTE URI
            Uri uriToken = this.tablaAlumnos.GenerateSasUri(builder);
            string token = uriToken.AbsoluteUri;
            return token;
        }
    }
}
