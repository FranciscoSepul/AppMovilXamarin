using FarmaciaFinder.ModelsDto;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Xaml;

namespace FarmaciaFinder
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Maps : ContentPage
    {

        public Maps(MapDto maps)
        {
            InitializeComponent();
            MapsUpdate(maps);
        }

        private async void MapsUpdate(MapDto maps)
        {
            map.InitialCameraUpdate = CameraUpdateFactory.NewCameraPosition(new CameraPosition(
                new Position(double.Parse(maps.latitud.ToString(), CultureInfo.InvariantCulture), Double.Parse(maps.longitud.ToString(), CultureInfo.InvariantCulture)),
                14d,
                20d,
                60d
                ));
         
            Pin pin = new Pin { Type = PinType.Place, Label = maps.placeName, Address = maps.adddres, Position = new Position(double.Parse(maps.latitud.ToString(), CultureInfo.InvariantCulture), Double.Parse(maps.longitud.ToString(), CultureInfo.InvariantCulture)) };
            map.Pins.Add(pin);
            
        }
    }
}