using Rg.Plugins.Popup.Extensions;
using Syncfusion.XForms.TextInputLayout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace otp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginProviderPage : ContentPage
    {
        public LoginProviderPage()
        {
            InitializeComponent();

           
        }

        public bool isPressed = true;

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            if (isPressed == false)
            {
                imgpass.Source = new FontImageSource
                {
                    Glyph = "",
                    FontFamily = "FontIconSolid",
                    Color = Color.FromHex("#334257")
                };
                PassProvider.IsPassword = true;
                isPressed = true;

            }
            else
            {
                imgpass.Source = new FontImageSource
                {
                    Glyph = "",
                    FontFamily = "FontIconSolid",
                    Color = Color.FromHex("#334257")
                };
                PassProvider.IsPassword = false;
                isPressed = false;
            }
        }

       
    }
}