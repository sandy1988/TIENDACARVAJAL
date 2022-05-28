using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SandyStore.Negocio;

namespace SandyStore.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            try
            {
                if (Request.IsAuthenticated)
                {
                    ProductosNegocio ProductosNegocio = new ProductosNegocio();
                    ViewBag.ListaProductos = ProductosNegocio.ListaProductos();
                }
                return View();
            }
            catch(Exception ex)
            {
                return View("Error");
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}