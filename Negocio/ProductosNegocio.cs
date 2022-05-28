using Newtonsoft.Json;
using SandyStore.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace SandyStore.Negocio
{
    public class ProductosNegocio
    {
        private static string UrlWS = ConfigurationManager.AppSettings["urlservices"];
        private static string TokenAutorizacion = ConfigurationManager.AppSettings["token"];
        public List<Departamentos> ListaDepartamentos()
        {
            List<Departamentos> ListaDepartamentos = new List<Departamentos>();
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenAutorizacion);
            string UrlWSTiendaCarvajal = UrlWS + "Departamentos/";
            client.BaseAddress = new Uri(UrlWSTiendaCarvajal);
            using (HttpResponseMessage response = client.GetAsync(UrlWSTiendaCarvajal).ContinueWith(task => task.Result).Result)
            {
                if (response.IsSuccessStatusCode)
                {
                    var httpResponseResult = response.Content.ReadAsStringAsync().ContinueWith(task => task.Result).Result;
                    ListaDepartamentos = JsonConvert.DeserializeObject<List<Departamentos>>(httpResponseResult.ToString());
                }
            }
            var DepartamentosList = ListaDepartamentos;
            return DepartamentosList.ToList();
        }

        internal List<Productos> ListaProductos()
        {
            List<Productos> ListaProductos = new List<Productos>();
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenAutorizacion);
            string UrlWSTiendaCarvajal = UrlWS + "Productos/";
            client.BaseAddress = new Uri(UrlWSTiendaCarvajal);
            using (HttpResponseMessage response = client.GetAsync(UrlWSTiendaCarvajal).ContinueWith(task => task.Result).Result)
            {
                if (response.IsSuccessStatusCode)
                {
                    var httpResponseResult = response.Content.ReadAsStringAsync().ContinueWith(task => task.Result).Result;
                    ListaProductos = JsonConvert.DeserializeObject<List<Productos>>(httpResponseResult.ToString());
                }
            }
            var ProductosList = ListaProductos;
            return ProductosList.ToList();
        }
    }
}