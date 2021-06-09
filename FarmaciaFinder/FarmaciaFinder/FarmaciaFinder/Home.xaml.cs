
using FarmaciaFinder.Services;
using System;

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
        private async void Button_Comuna(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Comuna.Text))
            {
                DisplayAlert("Error", "Favor ingrese la comuna", "Volver");
            }
            ServicesApi service = new ServicesApi();
            var list =await service.ListarFarmacia();
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