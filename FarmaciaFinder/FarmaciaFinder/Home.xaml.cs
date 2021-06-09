using FarmaciaFinder.ModelsDto;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FarmaciaFinder
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Home : ContentPage
    {       
        public Home()
        {
            InitializeComponent();           
        }       

        #region Buscar por comuna
        private void Button_Comuna(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Comuna.Text))
            {
                DisplayAlert("Error", "Favor ingrese la comuna", "Volver");
            }
            Services service = DependencyService.Get<Services>();
            var list = service.ListarFarmacia();
            if (list != null)
            {
                DisplayAlert("Error", "Sin conexión a ethernet", "Volver");
            }
            else
            {
                DisplayAlert("Error", "Error Interno", "Volver");
            }
        }
        #endregion

        #region btn que trae todas las comunas
        private async void Button_Todo(object sender, EventArgs e)
        {
            Application.Current.MainPage = new ListarFarmacias();
        }
        #endregion
    }
}