using Microsoft.AspNetCore.Mvc;
using MvcEventosExamen.Helpers;
using MvcEventosExamen.Models;
using MvcEventosExamen.Services;

namespace MvcEventosExamen.Controllers
{
    public class EventosController : Controller
    {
        private ServiceAPIEventos service;

        public EventosController(ServiceAPIEventos service)
        {
            this.service = service;
        }

        public async Task<IActionResult> Index()
        {
            string bucketName = await HelperSecretManager.GetSecretAsync("BucketUrlExamen");
            ViewData["BUCKETURL"] = bucketName;

            return View(await this.service.GetEventosAsync());
        }

        public async Task<IActionResult> EventosCategoria(int idcategoria)
        {
            string bucketName = await HelperSecretManager.GetSecretAsync("BucketUrlExamen");
            ViewData["BUCKETURL"] = bucketName;

            return View(await this.service.GetEventosXCategoriaAsync(idcategoria));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Evento evento, IFormFile archivo)
        {
            using (Stream stream = archivo.OpenReadStream())
            {
                evento.Imagen = archivo.FileName;
                await this.service.InsertEvento(evento, stream);
            }

            return RedirectToAction("Index");
        }
    }
}
