using FarmaciaFinder.ModelsDto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net.Http;
using Acr.UserDialogs;
using Newtonsoft.Json;
using FarmaciaFinder.Services;

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
            UserDialogs.Instance.ShowLoading("Cargando");

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

            ServicesApi api = new ServicesApi();
            var RegionList = api.listarRegion();
            PickerFarmaciaRegion.ItemsSource = RegionList;

            #region valida cambio de picker
            this.PickerFarmaciaRegion.SelectedIndexChanged += new EventHandler(this.LoadPicketComuna);
            this.PickerFarmaciaComuna.SelectedIndexChanged += new EventHandler(this.LoadPicketSucursal);
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
            UserDialogs.Instance.HideLoading();
        }
        #region cargar picker con comunas por region
        private async void LoadPicketComuna(object sender, EventArgs e)
        {
            try
            {
                UserDialogs.Instance.ShowLoading("Cargando");
                this.PickerFarmaciaComuna.Items.Clear();
                this.PickerFarmaciaNombre.Items.Clear();
                string namePickeRegion = this.PickerFarmaciaRegion.SelectedItem.ToString();

                string region = RegionToNumber(namePickeRegion);
                if ("No se encontro la region".Equals(region))
                {
                    DisplayAlert("Error", "Favor ingrese una region", "Volver");
                }
                ServicesApi service = new ServicesApi();
                List<Comuna> comunas = await service.listarComunas(region);
                foreach (var item in comunas)
                {
                    this.PickerFarmaciaComuna.Items.Add(item.nombre);
                }
            }
            catch (Exception)
            {
                DisplayAlert("Error", "No fue posible conectarse con el origen de los datos", "Volver");
            }
            UserDialogs.Instance.HideLoading();
        }
        #endregion
        private async void LoadPicketSucursal(object sender, EventArgs e)
        {
            try
            {
                UserDialogs.Instance.ShowLoading("Cargando");
                string namePicketComuna = this.PickerFarmaciaComuna.SelectedItem.ToString().ToUpper();
                ServicesApi services = new ServicesApi();
                List<Farmace> response =await services.ListarFarmacia();
                List<Farmace> farmaciaDeLaComuna = new List<Farmace>();
                foreach (var item in response)
                {
                    if (namePicketComuna.Equals(item.comuna_nombre))
                    {
                        farmaciaDeLaComuna.Add(item);
                    }
                }
                foreach (var item in farmaciaDeLaComuna)
                {
                    this.PickerFarmaciaNombre.Items.Add(item.local_nombre);
                }
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", "No fue posible conectarse con el origen de los datos", "Volver");
            }
        }

            #region regiones a numero
            public string RegionToNumber(string NamePickeRegion)
        {

            string region="No se encontro la region";
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