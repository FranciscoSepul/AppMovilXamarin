using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FarmaciaFinder
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LogI : ContentPage
    {
        public LogI()
        {
            InitializeComponent();
        }

        private void Button_Log(object sender, EventArgs e)
        {
            
            if (Connectivity.NetworkAccess != Xamarin.Essentials.NetworkAccess.Internet)
            {
                UserDialogs.Instance.HideLoading();
                DisplayAlert("Error", "Sin conexión a ethernet", "Volver");
            }
            else
            {
                if (string.IsNullOrEmpty(Username.Text))
                {
                    DisplayAlert("Error", "Favor ingrese su correo electrónico", "Volver");
                }
                else
                {
                    string user = Username.Text.Trim();
                    bool validatEmail = ValidateEmail(user);
                    if (validatEmail == false)
                    {
                        DisplayAlert("Error", "Favor ingrese un correo electrónico válido", "Volver");
                    }
                    else
                    {
                        string pass = Password.Text;
                        if (string.IsNullOrEmpty(pass))
                        {
                            DisplayAlert("Error", "Favor ingrese clave", "Volver");
                        }
                    }

                }
            }

        }
        private void Button_RecuperatePass(object sender, EventArgs e)
        {

        }

        #region Validacion de email
        public Boolean ValidateEmail(string email)
        {
            string expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
            if (Regex.IsMatch(email.Trim(), expresion))
            {
                if (Regex.Replace(email, expresion, string.Empty).Length == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        #endregion
    }
}