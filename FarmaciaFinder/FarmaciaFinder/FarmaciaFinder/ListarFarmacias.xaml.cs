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
        private Picker PickerFarmaciaRegion;
        private Label nombreF;
        private Label direccion;
        private Label horarioA;
        private Label horarioC;
        private Button btnBurcarEnMaps;
        private string Latitud;
        private string Longitud;
        private string direction;
        private string Name;
        ServicesApi api = new ServicesApi();

        public ListarFarmacias()
        {
            InitializeComponent();
            UserDialogs.Instance.ShowLoading("Cargando");
            var titulo = new Label { Text = "Bienvenido", TextDecorations = TextDecorations.Underline, FontSize = 60, HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center };

            this.nombreF = new Label
            {
                Text = "",
                TextDecorations = TextDecorations.Underline,
                FontSize = 25,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center
            };
            this.direccion = new Label
            {
                Text = "",
                TextDecorations = TextDecorations.Underline,
                FontSize = 20,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center
            };
            this.horarioA = new Label
            {
                Text = "",
                TextDecorations = TextDecorations.Underline,
                FontSize = 20,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center
            };
            this.horarioC = new Label
            {
                Text = "",
                TextDecorations = TextDecorations.Underline,
                FontSize = 20,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center
            };
            this.PickerFarmaciaComuna = new Picker
            {
                Title = "     Comuna     ",
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Margin = new Thickness(0, 70, 0, 0)
            };
            this.PickerFarmaciaRegion = new Picker
            {
                Title = "      Region    ",
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Margin = new Thickness(0, 50, 0, 0)
            };
            this.btnBurcarEnMaps = new Button
            {
                Text = "Buscar direccion en Maps",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                Margin = new Thickness(30, 10, 30, 0)

            };
            btnBurcarEnMaps.IsVisible = false;

            var RegionList = api.listarRegion();
            PickerFarmaciaRegion.ItemsSource = RegionList;

            #region valida cambio de picker
            this.PickerFarmaciaRegion.SelectedIndexChanged += new EventHandler(this.LoadPicketComuna);
            this.PickerFarmaciaComuna.SelectedIndexChanged += new EventHandler(this.LoadPicketSucursal);
            this.btnBurcarEnMaps.Clicked += new EventHandler(this.LoadMaps);
            #endregion

            this.Content = new StackLayout
            {
                Children =
                {
                    titulo,
                    this.PickerFarmaciaRegion,
                    this.PickerFarmaciaComuna,
                    this.nombreF,
                    this.direccion,
                    this.horarioA,
                    this.horarioC,
                    this.btnBurcarEnMaps
                }
            };
            UserDialogs.Instance.HideLoading();
        }

        #region Buscar direccion en maps
        private async void LoadMaps(object sender, EventArgs e)
        {
            MapDto map = new MapDto();
            map.adddres = this.direction;
            map.placeName = this.Name;
            map.latitud = this.Latitud;
            map.longitud = this.Longitud;

            Application.Current.MainPage = new Maps(map);
        }
        #endregion

        #region cargar picker con comunas por region
        private async void LoadPicketComuna(object sender, EventArgs e)
        {
            try
            {
                UserDialogs.Instance.ShowLoading("Cargando");
                this.PickerFarmaciaComuna.Items.Clear();
                string namePickeRegion = this.PickerFarmaciaRegion.SelectedItem.ToString();

                string region = api.RegionToNumber(namePickeRegion);
                if ("No se encontro la region".Equals(region))
                {
                    DisplayAlert("Error", "Favor ingrese una region", "Volver");
                }
                List<Comuna> comunas = await api.listarComunas(region);
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

        #region cargar label con informacion de sucursal
        private async void LoadPicketSucursal(object sender, EventArgs e)
        {
            try
            {
                UserDialogs.Instance.ShowLoading("Cargando");
                string namePicketComuna = this.PickerFarmaciaComuna.SelectedItem.ToString().ToUpper();
                List<Farmace> response = await api.ListarFarmacia();
                List<Farmace> farmaciaDeLaComuna = new List<Farmace>();
                foreach (var item in response)
                {
                    if (namePicketComuna.Equals(item.comuna_nombre))
                    {
                        farmaciaDeLaComuna.Add(item);
                    }
                }
                if (farmaciaDeLaComuna.Count > 0)
                {
                    foreach (var item in farmaciaDeLaComuna)
                    {
                        this.Latitud = item.local_lat;
                        this.Longitud = item.local_lng;
                        this.Name =item.local_nombre;
                        this.direction =item.local_direccion;
                        this.nombreF.Text = "Sucursal : " + item.local_nombre;
                        this.direccion.Text = "Direccion : " + item.local_direccion;
                        this.horarioA.Text = "Hora Apertura : " + item.funcionamiento_hora_apertura.ToString();
                        this.horarioC.Text = "Hora Cierre : " + item.funcionamiento_hora_cierre;
                        this.btnBurcarEnMaps.IsVisible = true;
                    }
                }
                else
                {
                    DisplayAlert("Error", "No Se encontraron farmacias de turno", "Volver");
                }
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception)
            {
                DisplayAlert("", "No fue posible conectarse con el origen de los datos", "Volver");
            }
        }
        #endregion

    }

}