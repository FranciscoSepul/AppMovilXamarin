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
        #region listar comunas
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
        #endregion

        #region List farmacias de turno
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

        #region regiones a numero
        public string RegionToNumber(string NamePickeRegion)
        {

            string region = "No se encontro la region";
            switch (NamePickeRegion)
            {
                case "Tarapacá":
                    region = "01";
                    break;
                case "Antofagasta":
                    region = "02";
                    break;
                case "Atacama":
                    region = "03";
                    break;
                case "Coquimbo":
                    region = "04";
                    break;
                case "Valparaíso":
                    region = "05";
                    break;
                case "O'Higgins":
                    region = "06";
                    break;
                case "Maule":
                    region = "07";
                    break;
                case "El Bío Bío":
                    region = "08";
                    break;
                case "La Araucanía":
                    region = "09";
                    break;
                case "Los Lagos":
                    region = "10";
                    break;
                case "Aysén":
                    region = "11";
                    break;
                case "Magallanes":
                    region = "12";
                    break;
                case "Metropolitana de Santiago":
                    region = "13";
                    break;
                case "Los Ríos":
                    region = "14";
                    break;
                case "Arica y Parinacota":
                    region = "15";
                    break;
                case "Ñuble":
                    region = "16";
                    break;
            }
            return region;
        }
        #endregion
    }
}
