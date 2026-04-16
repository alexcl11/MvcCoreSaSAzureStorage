using Azure.Data.Tables;
using MvcCoreSaSAzureStorage.Models;
using Newtonsoft.Json.Linq;
using NuGet.Common;

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

        private async Task<string> GetTokenAsync(string curso)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "token/" + curso;
                client.BaseAddress = new Uri(this.urlApi);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    string datos = await response.Content.ReadAsStringAsync();
                    JObject keys = JObject.Parse(datos);
                    string token = keys.GetValue("token").ToString();
                    return token;
                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<List<Alumno>> GetAlumnosAsync(string curso)
        {
            string token = await this.GetTokenAsync(curso);
            // CONVERTIMOS A URI EL STRING
            Uri uriToken = new Uri(token);
            // SIMPLEMENTE ACCEDEMOS AL RECURSO (TABLA DE AZURE STORAGE) MEDIANTE EL TOKEN
            this.tableAlumnos = new TableClient(uriToken);
            List<Alumno> alumnos = new List<Alumno>();
            var query = this.tableAlumnos.QueryAsync<Alumno>(filter: "");
            await foreach ( var item in query)
            {
                alumnos.Add(item);
            }
            return alumnos;
        }

        public async Task CreateAlumnoAsync(int idAlumno, string nombre, string apellidos, int nota)
        {
            string curso = "EN PROCESO";
            string token = await this.GetTokenAsync(curso);
            Alumno alumno = new Alumno();
            alumno.IdAlumno = idAlumno;
            alumno.Nombre = nombre;
            alumno.Curso = curso;
            alumno.Apellidos = apellidos;
            alumno.Nota = nota;

            Uri uriToken = new Uri(token);
            this.tableAlumnos = new TableClient(uriToken);
            await this.tableAlumnos.AddEntityAsync<Alumno>(alumno);

        }
    }
}
