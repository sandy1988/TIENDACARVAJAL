using Newtonsoft.Json;
using SandyStore.Models;
using SandyStore.Negocio;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TiendaCarvajal.Models;

namespace SandyStore.Controllers
{
    [Authorize(Roles ="Administrador")]
    public class ProductosController : Controller
    {
        private static string UrlWS = ConfigurationManager.AppSettings["urlservices"];
        private static string TokenAutorizacion = ConfigurationManager.AppSettings["token"];
        ProductosNegocio ProductosNegocio = new ProductosNegocio();

        // GET: Productos
        public ActionResult Index()
        {
            try
            {
                return View(ProductosNegocio.ListaProductos());
            }            
            catch(Exception ex)
            {
                return View("Error");
            }
        }

        // GET: Productos/Create
        public ActionResult Create()
        {
            try
            {
                ViewBag.DepID = new SelectList(ProductosNegocio.ListaDepartamentos(), "DepID", "DepDescripcion");
                return View();
            }
            catch(Exception ex)
            {
                return View("Error");
            }            
        }

        // POST: Productos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DepID,ProNombre,ProDescripcion,ProValor,ProRutaImagen")] Productos productos, HttpPostedFileBase ProRutaImagen)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string[] formato = { ".png", ".jpg", ".jpeg", ".gif" };

                    if (formato.Contains(Path.GetExtension(ProRutaImagen.FileName)))
                    {
                        int TamanoImagen = ProRutaImagen.ContentLength;
                        if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + @"ImagenesProductos\" + productos.ProNombre + @"\") == false)
                            Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory.ToString() + @"ImagenesProductos\" + productos.ProNombre + @"\");

                        var DirectorioImagen = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"ImagenesProductos\" + productos.ProNombre + @"\" + Path.GetFileName(ProRutaImagen.FileName);
                        if (TamanoImagen > 2097152) //2MB
                        {
                            ModelState.AddModelError("Imagen", "El tamaño de los archivos seleccionados es superior a 2MB. Debe eliminar o reducir el tamaño de la imagen");
                        }
                        else
                        {
                            ProRutaImagen.SaveAs(DirectorioImagen);
                            productos.ProRutaImagen = ProRutaImagen.FileName;
                            HttpClient client = new HttpClient();
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenAutorizacion);
                            string UrlWSTiendaCarvajal = UrlWS + "Productos/";
                            client.BaseAddress = new Uri(UrlWSTiendaCarvajal);
                            string json = JsonConvert.SerializeObject(productos);
                            StringContent queryString = new StringContent(json, Encoding.Unicode, "application/json");
                            HttpResponseMessage response = client.PostAsync(UrlWSTiendaCarvajal, queryString).ContinueWith(task => task.Result).Result;
                            if (response.IsSuccessStatusCode)
                            {
                                var httpResponseResult = response.Content.ReadAsStringAsync().ContinueWith(task => task.Result).Result;
                                productos = JsonConvert.DeserializeObject<Productos>(httpResponseResult.ToString());
                            }
                            return RedirectToAction("Index");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("ProRutaImagen", "Solo se permiten archivos con el formato .png, .jpg, .jpeg, .gif");
                    }
                }
                ViewBag.DepID = new SelectList(ProductosNegocio.ListaDepartamentos(), "DepID", "DepDescripcion", productos.DepID);
                return View(productos);
            }
            catch(Exception ex)
            {
                return View("Error");
            }
        }

        // GET: Productos/Edit/5
        public ActionResult Edit(int? id)
        {
            try
            {
                Productos productos = new Productos();
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenAutorizacion);
                string UrlWSTiendaCarvajal = UrlWS + "Productos/" + id;
                client.BaseAddress = new Uri(UrlWSTiendaCarvajal);
                HttpResponseMessage response = client.GetAsync(UrlWSTiendaCarvajal).ContinueWith(task => task.Result).Result;
                if (response.IsSuccessStatusCode)
                {
                    var httpResponseResult = response.Content.ReadAsStringAsync().ContinueWith(task => task.Result).Result;
                    productos = JsonConvert.DeserializeObject<Productos>(httpResponseResult.ToString());
                }
                if (productos == null)
                {
                    return HttpNotFound();
                }
                ViewBag.DepID = new SelectList(ProductosNegocio.ListaDepartamentos(), "DepID", "DepDescripcion", productos.DepID);
                ViewBag.RutaAnterior = productos.ProRutaImagen;
                return View(productos);
            }
            catch(Exception ex)
            {
                return View("Error");
            }
        }

        // POST: Productos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProID,DepID,ProNombre,ProDescripcion,ProValor,ProRutaImagen")] Productos productos, HttpPostedFileBase ProRutaImagen, FormCollection form)
        {
            try
            {
                if(ProRutaImagen == null)
                {
                    productos.ProRutaImagen = form["RutaAnterior"];
                }
                if (ModelState.IsValid)
                {
                    string[] formato = { ".png", ".jpg", ".jpeg", ".gif" };

                    if (formato.Contains(Path.GetExtension(ProRutaImagen.FileName)))
                    {
                        int TamanoImagen = ProRutaImagen.ContentLength;
                        if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + @"ImagenesProductos\" + productos.ProNombre + @"\") == false)
                            Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory.ToString() + @"ImagenesProductos\" + productos.ProNombre + @"\");

                        var DirectorioImagen = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"ImagenesProductos\" + productos.ProNombre + @"\" + Path.GetFileName(ProRutaImagen.FileName);
                        if (TamanoImagen > 2097152) //2MB
                        {
                            ModelState.AddModelError("Imagen", "El tamaño de los archivos seleccionados es superior a 2MB. Debe eliminar o reducir el tamaño de la imagen");
                        }
                        else
                        {
                            ProRutaImagen.SaveAs(DirectorioImagen);
                            productos.ProRutaImagen = ProRutaImagen.FileName;
                            HttpClient client = new HttpClient();
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenAutorizacion);
                            string UrlWSTiendaCarvajal = UrlWS + "Productos/" + productos.ProID;
                            client.BaseAddress = new Uri(UrlWSTiendaCarvajal);
                            string json = JsonConvert.SerializeObject(productos);
                            StringContent queryString = new StringContent(json, Encoding.Unicode, "application/json");
                            HttpResponseMessage response = client.PutAsync(UrlWSTiendaCarvajal, queryString).ContinueWith(task => task.Result).Result;
                            if (response.IsSuccessStatusCode)
                            {
                                var httpResponseResult = response.Content.ReadAsStringAsync().ContinueWith(task => task.Result).Result;
                                productos = JsonConvert.DeserializeObject<Productos>(httpResponseResult.ToString());
                            }
                            return RedirectToAction("Index");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("ProRutaImagen", "Solo se permiten archivos con el formato .png, .jpg, .jpeg, .gif");
                    }
                }
                ViewBag.DepID = new SelectList(ProductosNegocio.ListaDepartamentos(), "DepID", "DepDescripcion", productos.DepID);
                return View(productos);
            }
            catch(Exception ex)
            {
                return View("Error");
            }
        }

        // GET: Productos/Delete/5
        public ActionResult Delete(int? id)
        {
            try
            {
                Productos productos = new Productos();
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenAutorizacion);
                string UrlWSTiendaCarvajal = UrlWS + "Productos/" + id;
                client.BaseAddress = new Uri(UrlWSTiendaCarvajal);
                HttpResponseMessage response = client.GetAsync(UrlWSTiendaCarvajal).ContinueWith(task => task.Result).Result;
                if (response.IsSuccessStatusCode)
                {
                    var httpResponseResult = response.Content.ReadAsStringAsync().ContinueWith(task => task.Result).Result;
                    productos = JsonConvert.DeserializeObject<Productos>(httpResponseResult.ToString());
                }
                if (productos == null)
                {
                    return HttpNotFound();
                }
                return View(productos);
            }
            catch(Exception ex)
            {
                return View("Error");
            }
        }

        // POST: Productos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Productos productos = new Productos();
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenAutorizacion);
                string UrlWSTiendaCarvajal = UrlWS + "Productos/" + id;
                client.BaseAddress = new Uri(UrlWSTiendaCarvajal);
                HttpResponseMessage response = client.DeleteAsync(UrlWSTiendaCarvajal).ContinueWith(task => task.Result).Result;
                if (response.IsSuccessStatusCode)
                {
                    var httpResponseResult = response.Content.ReadAsStringAsync().ContinueWith(task => task.Result).Result;
                    productos = JsonConvert.DeserializeObject<Productos>(httpResponseResult.ToString());
                }
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                return View("Error");
            }
        }
    }
}
