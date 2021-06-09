using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using FarmaciaFinder.ModelsDto;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FarmaciaFinder.Droid
{
    class Service : Services
    {
        public async Task<List<Comuna>> listarComunas(int region)
        {
            List<Comuna> comunas = new List<Comuna>();
            using (var client = new HttpClient())
            {
                try
                {
                    string url = "https://apis.digital.gob.cl/dpa/regiones/"+region+"/provincias";
                    var resp = await client.GetAsync(url);
                    string json = resp.ToString();
                    comunas = JsonConvert.DeserializeObject<List<Comuna>>(resp.ToString());
                }
                catch (Exception e)
                {
                    throw e;
                }

            }
            return comunas;
        }

        #region Listar region 
        public List<Regiones> listarRegion()
        {
            List<Regiones> list = new List<Regiones>();
            using (var client = new HttpClient())
            {
                try
                {
                    string url = "https://apis.digital.gob.cl/dpa/regiones";
                    var resp = client.GetAsync(url);
                    string json = resp.ToString();
                    list = JsonConvert.DeserializeObject<List<Regiones>>(resp.ToString());
                    return list;
                }
                catch (Exception e)
                {
                    throw e;
                }

            } //await Task.Run(()=> new List<Regiones>(list));
        }
        #endregion

        #region consumir api minsal
        public async Task<List<Farmace>> ListarFarmacia()
        {
            List<Farmace> list = new List<Farmace>();
            using (var client = new HttpClient())
            {
                try
                {
                    string url = "https://farmanet.minsal.cl/index.php/ws/getLocalesTurnos";
                    var resp = await client.GetAsync(url);
                    if (resp.IsSuccessStatusCode)
                    {
                        list = JsonConvert.DeserializeObject<List<Farmace>>(resp.Content.ReadAsStringAsync().Result);
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }

            }
            return list;
        }
        #endregion


    }
}