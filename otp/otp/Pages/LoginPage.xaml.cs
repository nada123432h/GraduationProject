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
    public partial class LoginPage : ContentPage
    {
        public bool isPressed = false;
        public LoginPage()
        {
            InitializeComponent();

            //textinputlayout.FlowDirection = FlowDirection.RightToLeft;
            //textinputlayout.ContainerType = ContainerType.Outlined;
            //textinputlayout.Hint = "اسم";
            //textinputlayout.HelperText = "أدخل أسمك";
            //textinputlayout.InputView = new Entry();
            //pasEntry.IsPassword = false;
        }

       

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            if (isPressed == false)
            {
                emimg2.Source = new FontImageSource
                {
                    Glyph = "",
                    FontFamily = "FontIconSolid",
                    Color = Color.FromHex("#334257")
                };
                pasEntry.IsPassword = true;
                isPressed = true;

            }
            else
            {
                emimg2.Source = new FontImageSource
                {
                    Glyph = "",
                    FontFamily = "FontIconSolid",
                    Color = Color.FromHex("#334257")
                };
                pasEntry.IsPassword = false;
                isPressed = false;
            }

        }

        
    }
}