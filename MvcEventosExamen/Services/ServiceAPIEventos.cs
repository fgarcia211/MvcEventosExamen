using MvcEventosExamen.Helpers;
using MvcEventosExamen.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace MvcEventosExamen.Services
{
    public class ServiceAPIEventos
    {
        private MediaTypeWithQualityHeaderValue Header;
        private ServiceS3Amazon serviceamazon;

        public ServiceAPIEventos(IConfiguration configuration, ServiceS3Amazon serviceamazon)
        {
            this.Header = new MediaTypeWithQualityHeaderValue("application/json");
            this.serviceamazon = serviceamazon;
        }

        private async Task<T> CallApiAsync<T>(string request)
        {
            string urlApi = await HelperSecretManager.GetSecretAsync("UrlApiExamen");

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                string url = urlApi + request;
                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    T data = await response.Content.ReadAsAsync<T>();
                    return data;
                }
                else
                {
                    return default(T);
                }
            }
        }

        public async Task InsertEvento(Evento evento, Stream stream)
        {
            string urlApi = await HelperSecretManager.GetSecretAsync("UrlApiExamen");

            await this.serviceamazon.UploadFileAsync(evento.Imagen, stream);

            using (HttpClient client = new HttpClient())
            {
                string request = "api/eventos";

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);

                string jsonEvento = JsonConvert.SerializeObject(evento);
                StringContent content = new StringContent(jsonEvento, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(urlApi + request, content);
            }
        }

        public async Task<List<Evento>> GetEventosAsync()
        {
            string request = "api/eventos/geteventos";
            return await this.CallApiAsync<List<Evento>>(request);
        }

        public async Task<List<Evento>> GetEventosXCategoriaAsync(int idcategoria)
        {
            string request = "api/eventos/GetEventosXCategoria/" + idcategoria;
            return await this.CallApiAsync<List<Evento>>(request);
        }

        public async Task<List<CategoriaEvento>> GetCategoriasEventoAsync()
        {
            string request = "api/eventos/GetCategoriasEvento";
            return await this.CallApiAsync<List<CategoriaEvento>>(request);
        }
    }
}
