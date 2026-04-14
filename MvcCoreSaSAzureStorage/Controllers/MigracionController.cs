using Azure.Data.Tables;
using Microsoft.AspNetCore.Mvc;
using MvcCoreSaSAzureStorage.Helpers;
using MvcCoreSaSAzureStorage.Models;
using System.Threading.Tasks;

namespace MvcCoreSaSAzureStorage.Controllers
{
    public class MigracionController : Controller
    {
        private HelperReadXML helper;
        private IConfiguration config;
        public MigracionController(HelperReadXML helper, IConfiguration config)
        {
            this.helper = helper;
            this.config = config;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string accion)
        {
            // EN ESTE METODO LO QUE NECESITAMOS SON LAS KEYS DE AZURE STORAGE
            // ESTA FUNCIONALIDAD DEBERIA ESTAR EN OTRO PROYECTO
            string azureKey = this.config.GetValue<string>("AzureKeys:StorageAccount");
            TableServiceClient tableService = new TableServiceClient(azureKey);
            TableClient tableClient = tableService.GetTableClient("alumnos");
            await tableClient.CreateIfNotExistsAsync();
            List<Alumno> alumnos = this.helper.GetAlumnos();
            foreach (Alumno alumno in alumnos)
            {
                await tableClient.AddEntityAsync<Alumno>(alumno);
            }
            ViewData["MENSAJE"] = "Migración de alumnos completada";
            return View(alumnos);
        }
    }
}
