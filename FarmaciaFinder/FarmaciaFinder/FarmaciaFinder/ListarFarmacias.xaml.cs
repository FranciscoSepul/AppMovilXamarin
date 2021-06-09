using FarmaciaFinder.ModelsDto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net.Http;
using Acr.UserDialogs;
using Newtonsoft.Json;

namespace FarmaciaFinder
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListarFarmacias : ContentPage
    {
        private Picker PickerFarmaciaComuna;
        private Picker PickerFarmaciaNombre;
        private Picker PickerFarmaciaRegion;
        public ListarFarmacias()
        {
            InitializeComponent();

            this.PickerFarmaciaComuna = new Picker
            {
                Title = "     Comuna     ",
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Margin = new Thickness(0, 70, 0, 0)

            };
            this.PickerFarmaciaNombre = new Picker
            {
                Title = "    Nombre    ",
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Margin = new Thickness(0, 50, 0, 0)
            };
            this.PickerFarmaciaRegion = new Picker
            {
                Title = "      Region    ",
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Margin = new Thickness(0, 50, 0, 0)
            };
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
            PickerFarmaciaRegion.ItemsSource = RegionList;

            #region valida cambio de picker
            this.PickerFarmaciaRegion.SelectedIndexChanged += new EventHandler(this.LoadPicketComuna);
            #endregion
            this.Content = new StackLayout
            {
                Children =
                {
                    this.PickerFarmaciaRegion,
                    this.PickerFarmaciaComuna,
                    this.PickerFarmaciaNombre
                }
            };

        }
        private async void LoadPicketComuna(object sender, EventArgs e)
        {
            try
            {
                string NamePickeRegion = this.PickerFarmaciaRegion.SelectedItem.ToString();
                #region regiones a numero
                int region=0;
                switch (NamePickeRegion)
                {
                    case "Tarapacá":
                        region = 1;
                        break;
                    case "Antofagasta":
                        region = 2;
                        break;
                    case "Atacama":
                        region = 3;
                        break;
                    case "Coquimbo":
                        region = 4;
                        break;
                    case "Valparaíso":
                        region = 5;
                        break;
                    case "O'Higgins":
                        region = 6;
                        break;
                    case "Maule":
                        region = 7;
                        break;
                    case "El Bío Bío":
                        region = 8;
                        break;
                    case "La Araucanía":
                        region = 9;
                        break;
                    case "Los Lagos":
                        region = 10;
                        break;
                    case "Aysén":
                        region = 11;
                        break;
                    case "Magallanes":
                        region = 12;
                        break;
                    case "Metropolitana de Santiago":
                        region = 13;
                        break;
                    case "Los Ríos":
                        region = 14;
                        break;
                    case "Arica y Parinacota":
                        region = 15;
                        break;
                    case "Ñuble":
                        region = 16;
                        break;
                }
                #endregion 
                List<Comuna> comunas =await listarComunas(region);
                foreach (var item in comunas)
                {
                    this.PickerFarmaciaComuna.Items.Add(item.comunas);
                }
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                DisplayAlert("Error", "No fue posible conectarse con el origen de los datos", "Volver");
            }
        }

        public async Task<List<Comuna>> listarComunas(int region)
        {
            List<Comuna> comunas = new List<Comuna>();
            using (var client = new HttpClient())
            {
                try
                {
                    string url = "https://apis.digital.gob.cl/dpa/regiones/" + region + "/provincias";
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
    }

}