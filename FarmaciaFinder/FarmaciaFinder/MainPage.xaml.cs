using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FarmaciaFinder
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void Button_IniciarS(object sender, EventArgs e)
        {
            Application.Current.MainPage = new LogI();
        }

        private void Button_Invitado(object sender, EventArgs e)
        {
            Application.Current.MainPage = new Home();
        }
        private void Button_Clicked_Registre(object sender, EventArgs e)
        {
            Application.Current.MainPage = new Home();
        }
    }
}
