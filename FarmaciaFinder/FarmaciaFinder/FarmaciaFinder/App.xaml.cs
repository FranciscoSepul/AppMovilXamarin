using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FarmaciaFinder
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new ListarFarmacias();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
