using FarmaciaFinder.ModelsDto;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FarmaciaFinder.Services
{
    public class ServicesApi
    {
        public async Task<List<Comuna>> listarComunas(string region)
        {
            List<Comuna> comunas = new List<Comuna>();
            using (var client = new HttpClient())
            {
                try
                {
                    string url = "https://apis.digital.gob.cl/dpa/regiones/" + region + "/comunas";
                    var resp = await client.GetAsync(url);
                    comunas = JsonConvert.DeserializeObject<List<Comuna>>(resp.Content.ReadAsStringAsync().Result);
                }
                catch (Exception e)
                {
                    throw e;
                }

            }
            return comunas;
        }

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

        #region Listar regiones
        public List<string> listarRegion()
        {
            var RegionList = new List<string>();

            RegionList.Add("Tarapacá");
            RegionList.Add("Antofagasta");
            RegionList.Add("Atacama");
            RegionList.Add("Coquimbo");
            RegionList.Add("Valparaíso");
            RegionList.Add("O'Higgins");
            RegionList.Add("Maule");
            RegionList.Add("El Bío Bío");
            RegionList.Add("La Araucanía");
            RegionList.Add("Los Lagos");
            RegionList.Add("Magallanes");
            RegionList.Add("Metropolitana de Santiago");
            RegionList.Add("Los Ríos");
            RegionList.Add("Arica y Parinacota");
            RegionList.Add("Ñuble");
            return RegionList;
        }
        #endregion

    }
}
