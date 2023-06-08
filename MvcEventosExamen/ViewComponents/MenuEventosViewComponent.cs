using Microsoft.AspNetCore.Mvc;
using MvcEventosExamen.Models;
using MvcEventosExamen.Services;

namespace MvcEventosExamen.ViewComponents
{
    public class MenuEventosViewComponent : ViewComponent
    {
        private ServiceAPIEventos service;

        public MenuEventosViewComponent(ServiceAPIEventos service)
        {
            this.service = service;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await this.service.GetCategoriasEventoAsync());
        }
    }
}
